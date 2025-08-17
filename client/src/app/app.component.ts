import { Component, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  ngOnInit(): void {
    this.http.get<Pagination<Product>>(this.baseUrl + 'products')
    .subscribe({
      next:response => this.products = response.data,
      error: error => console.log(error),
      complete : () => console.log('complete')
    })
  }
  products : Product[] = []
  baseUrl = 'http://localhost:5000/api/'
  private http = inject(HttpClient);
  title = signal('Welcome to Skinet');
}
