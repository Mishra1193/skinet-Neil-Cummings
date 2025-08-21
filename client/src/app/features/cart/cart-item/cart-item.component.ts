import { Component, input, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { CartItem } from '../../../shared/models/cart';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [RouterLink, MatButtonModule, MatIcon, CurrencyPipe],
  templateUrl: './cart-item.component.html',
})
export class CartItemComponent {
  item = input.required<CartItem>();
  private cartService = inject(CartService);

  // ➕ add one (Sir: call addItemToCart with product shape; qty defaults to 1)
  increment(): void {
    const it = this.item();
    this.cartService.addItemToCart(
      {
        id: it.productId,
        name: it.productName,
        price: it.price,
        pictureUrl: it.pictureUrl,
        productBrand: it.brand,   // service mapper accepts productBrand|brand
        productType: it.type,     // service mapper accepts productType|type
      } as any
    );
  }

  // ➖ remove one (Sir: use removeItemFromCart; qty defaults to 1)
  decrement(): void {
    this.cartService.removeItemFromCart(this.item().productId);
  }

  // 🗑 remove the whole line (Sir: pass current qty to remove completely)
  remove(): void {
    const it = this.item();
    this.cartService.removeItemFromCart(it.productId, it.quantity);
  }
}
