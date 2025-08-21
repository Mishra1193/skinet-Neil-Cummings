import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { nanoid } from 'nanoid';
import { environment } from '../../../environments/environment';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { Observable, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly storageKey = 'cart_id';
  private readonly baseUrl = environment.apiUrl;

  private _cart = signal<Cart | null>(null);

  cart = computed(() => this._cart());
  itemCount = computed(
    () => this._cart()?.items.reduce((sum, x) => sum + x.quantity, 0) ?? 0
  );

  // ▶️ NEW: totals (computed) — as in the transcript
  totals = computed(() => {
    const cart = this.cart(); // get current cart
    if (!cart) return null;

    const subtotal = cart.items.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );

    const shipping = 0;
    const discount = 0;
    const total = subtotal + shipping - discount;

    return { subtotal, shipping, discount, total: subtotal + shipping - discount };
  });

  constructor(private http: HttpClient) {}

  getCart(id: string): Observable<Cart> {
    return this.http.get<Cart>(`${this.baseUrl}cart?id=${id}`).pipe(
      map(cart => {
        this._cart.set(cart);
        return cart;
      })
    );
  }

  addItemToCart(product: Product, quantity = 1): void {
    const cart = this._cart() ?? this.createCart();

    const cartItem = this.mapProductToCartItem(product);
    this.addOrUpdateItem(cart.items, cartItem, quantity);

    // optimistic local update
    this._cart.set({ ...cart });

    // upsert to API
    this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: updated => this._cart.set(updated),
      error: err => console.error('Add to cart failed', err)
    });
  }

  /** Decrease quantity or remove an item from the cart (per transcript) */
removeItemFromCart(productId: number, quantity = 1): void {
  const cart = this._cart();
  if (!cart) return;

  const index = cart.items.findIndex(x => x.productId === productId);
  if (index === -1) return;

  const currentQty = cart.items[index].quantity;

  if (currentQty > quantity) {
    // reduce only
    cart.items[index].quantity -= quantity;
  } else {
    // remove the line item
    cart.items.splice(index, 1);
  }

  // if cart became empty → delete it from server + clear local state
  if (cart.items.length === 0) {
    this.deleteCart();
    return;
  }

  // otherwise, persist the updated cart (upsert) and update signal
  this._cart.set({ ...cart }); // optimistic update
  this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
    next: updated => this._cart.set(updated),
    error: err => console.error('Update cart failed', err)
  });
}

/** Delete the entire cart from Redis and clear local storage (per transcript) */
deleteCart(): void {
  const cart = this._cart();
  if (!cart) return;

  this.http.delete(this.baseUrl + 'cart?id=' + cart.id).subscribe({
    next: () => {
      localStorage.removeItem(this.storageKey);
      this._cart.set(null);
    },
    error: err => console.error('Delete cart failed', err)
  });
}


  /** 🆕 remove one product completely */
  removeItem(productId: number): void {
    const cart = this._cart();
    if (!cart) return;

    cart.items = cart.items.filter(x => x.productId !== productId);

    this._cart.set({ ...cart });

    this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: updated => this._cart.set(updated),
      error: err => console.error('Remove from cart failed', err)
    });
  }

  clearCart(): void {
    const c = this._cart();
    if (!c) return;
    c.items = [];
    this._cart.set({ ...c });
    this.http.post<Cart>(this.baseUrl + 'cart', c).subscribe({
      next: updated => this._cart.set(updated),
      error: err => console.error('Clear cart failed', err)
    });
  }

  setCart(cart: Cart | null): void {
    this._cart.set(cart);
  }

  get cartIdFromStorage(): string | null {
    return localStorage.getItem(this.storageKey);
  }

  // ------------ private helpers ------------

  private createCart(): Cart {
    const cart: Cart = { id: nanoid(), items: [] };
    localStorage.setItem(this.storageKey, cart.id);
    return cart;
  }

  private mapProductToCartItem(p: Product): CartItem {
    const toName = (v: any) =>
      v == null ? '' :
      typeof v === 'string' ? v :
      typeof v === 'number' ? String(v) :
      typeof v === 'object' && 'name' in v ? String((v as any).name) :
      String(v);

    const brandVal = (p as any).productBrand ?? (p as any).brand;
    const typeVal  = (p as any).productType  ?? (p as any).type;

    return {
      productId: p.id,
      productName: p.name,
      price: p.price,
      quantity: 0, // set later
      pictureUrl: p.pictureUrl,
      brand: toName(brandVal),
      type:  toName(typeVal),
    };
  }

  private addOrUpdateItem(items: CartItem[], item: CartItem, qty: number): void {
    const i = items.findIndex(x => x.productId === item.productId);
    if (i === -1) {
      item.quantity = qty;
      items.push(item);
    } else {
      items[i].quantity += qty;
    }
  }
}
export { Cart };
