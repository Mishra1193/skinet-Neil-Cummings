import { Component, inject } from '@angular/core';
import { CartService } from '../../core/services/cart.service';
import { CartItemComponent } from './cart-item/cart-item.component';
import { OrderSummaryComponent } from "../../shared/components/order-summary/order-summary.component";

@Component({
  selector: 'app-cart',
  imports: [CartItemComponent, OrderSummaryComponent],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent {
  cartService = inject(CartService);

  /** + button: add one to this line item */
  onIncrease(productId: number): void {
    const cart = this.cartService.cart();
    if (!cart) return;

    const item = cart.items.find(x => x.productId === productId);
    if (!item) return;

    // minimal Product shape for addItemToCart (aligned with your CartService mapper)
    this.cartService.addItemToCart(
      {
        id: item.productId,
        name: item.productName,
        price: item.price,
        pictureUrl: item.pictureUrl,
        productBrand: item.brand,
        productType: item.type
      } as any,
      1
    );
  }

  /** - button: remove one from this line item (or remove line if qty goes to 0) */
  onDecrease(productId: number): void {
    this.cartService.removeItemFromCart(productId, 1);
  }

  /** 🗑️ delete: remove entire line item regardless of quantity */
  onRemoveLine(productId: number): void {
    this.cartService.removeItem(productId);
  }

  /** Clear Cart button in summary */
  onClearCart(): void {
    this.cartService.clearCart();
  }
}
