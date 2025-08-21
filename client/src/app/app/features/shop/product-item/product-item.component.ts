import { Component, inject, Input } from '@angular/core';
import { Product } from '../../../../shared/models/product';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { CartService } from '../../../../core/services/cart.service';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [
    MatCard,
    MatCardContent,
    CurrencyPipe,
    MatCardActions,
    MatButton,
    MatIcon,
    RouterLink
  ],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
  @Input() product!: Product;

  cartService = inject(CartService);
  router = inject(Router);

  addToCart(product: Product) {
    this.cartService.addItemToCart(product);
    this.router.navigate(['/cart']);   // 👈 navigate after adding
  }
}
