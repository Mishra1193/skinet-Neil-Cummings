import { Component, signal } from '@angular/core';
import { HeaderComponent } from './layout/header/header.component';
import { ShopComponent } from "./app/features/shop/shop.component";

@Component({
  selector: 'app-root',
  imports: [HeaderComponent, ShopComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent{
  title = signal('Welcome to Skinet');
}
