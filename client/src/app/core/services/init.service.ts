// src/app/core/services/init.service.ts
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { CartService } from './cart.service';

@Injectable({ providedIn: 'root' })
export class InitService {
  constructor(private cartService: CartService) {}

  async init(): Promise<void> {
    const cartId = this.cartService.cartIdFromStorage;
    if (!cartId) return;
    await firstValueFrom(this.cartService.getCart(cartId));
  }
}
