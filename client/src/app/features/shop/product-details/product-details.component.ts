import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, NgIf } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';

import { ShopService } from '../../../core/services/shop.service';
import { CartService } from '../../../core/services/cart.service';
import { Product } from '../../../shared/models/product';

@Component({
  selector: 'app-product-details',
  standalone: true,
  // keep material + forms available for the template (ngModel, button etc.)
  imports: [ FormsModule, CurrencyPipe, MatButton, MatIcon, MatFormField, MatInput, MatLabel, MatDivider],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);

  product?: Product;

  // ---- cart-related view state (per Sir) ----
  quantityInCart = 0;   // what’s already in the cart for this product
  quantity = 1;         // what the user is editing in the input

  ngOnInit() {
    this.loadProduct();
  }

  private loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      console.error('Product ID is not provided in the route.');
      return;
    }

    this.shopService.getProduct(+id).subscribe({
      next: (product) => {
        this.product = product;
        this.updateQuantityInCart(); // <-- sync with cart after product loads
      },
      error: (error) => console.error('Error fetching product:', error),
    });
  }

  // compute how many of this product are already in the cart; set UI quantity accordingly
  updateQuantityInCart(): void {
    if (!this.product) {
      this.quantityInCart = 0;
      this.quantity = 1;
      return;
    }

    const existing = this.cartService.cart()?.items.find(
      x => x.productId === this.product!.id
    );

    this.quantityInCart = existing?.quantity ?? 0;
    this.quantity = this.quantityInCart || 1; // if none in cart, show 1 by default
  }

  getButtonText(): string {
    return this.quantityInCart > 0 ? 'Update Cart' : 'Add to Cart';
  }

  // apply the delta between desired quantity and what's in the cart
  updateCart(): void {
    if (!this.product) return;

    if (this.quantity > this.quantityInCart) {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemsToAdd;
      this.cartService.addItemToCart(this.product, itemsToAdd);
    } else if (this.quantity < this.quantityInCart) {
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.quantityInCart -= itemsToRemove;
      this.cartService.removeItemFromCart(this.product.id, itemsToRemove);
    } else {
      // no change; do nothing
      return;
    }
  }
}
