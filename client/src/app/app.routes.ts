import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ProductDetailsComponent } from './features/shop/product-details/product-details.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { CartComponent } from './features/cart/cart.component';
import { ShopComponent } from './app/features/shop/shop.component';
import { CheckoutComponent } from './features/checkout/checkout.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'shop', component: ShopComponent },
  { path: 'shop/:id', component: ProductDetailsComponent },
  { path: 'cart', component: CartComponent },
  {path:'checkout', component: CheckoutComponent}, // Assuming checkout uses the same component for simplicity
  { path: 'test-error', component: TestErrorComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' } // Wildcard route for a 404 page
];
