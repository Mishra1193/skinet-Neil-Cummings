import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';
@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'http://localhost:5000/api/';
  private http = inject(HttpClient);

  brands: string[] = [];
  types: string[] = [];

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams()
      .set('pageSize', shopParams.pageSize)
      .set('pageIndex', shopParams.pageNumber); // must match API param

    if (shopParams.brands.length) {
      params = params.set('brand', shopParams.brands.join(','));
    }
    if (shopParams.types.length) {
      params = params.set('type', shopParams.types.join(','));
    }
    if (shopParams.sort) {
      params = params.set('sort', shopParams.sort);
    }
    if (shopParams.search) {
      params = params.set('search', shopParams.search);
    }

    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {
      params,
    });
  }

  getProduct(id: number){
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) return;
    return this.http
      .get<string[]>(this.baseUrl + 'products/brands')
      .subscribe({ next: (r) => (this.brands = r) });
  }

  getTypes() {
    if (this.types.length > 0) return;
    return this.http
      .get<string[]>(this.baseUrl + 'products/types')
      .subscribe({ next: (r) => (this.types = r) });
  }
}

// import { HttpClient, HttpParams } from '@angular/common/http';
// import { inject, Injectable } from '@angular/core';
// import { Pagination } from '../../shared/models/pagination';
// import { Product } from '../../shared/models/product';

// @Injectable({
//   providedIn: 'root',
// })
// export class ShopService {
//   baseUrl = 'http://localhost:5000/api/';
//   private http = inject(HttpClient);
//   brands: string[] = [];
//   types: string[] = [];

//   getProducts(brands?: string[], types?: string[], sort?: string) {
//   let params = new HttpParams().set('pageSize', 20);

//   if (brands?.length) {
//     // API expects "brand"
//     params = params.set('brand', brands.join(','));
//   }

//   if (types?.length) {
//     // API expects "type"
//     params = params.set('type', types.join(','));
//   }

//   if (sort) {
//     // API expects "sort"
//     params = params.set('sort', sort);
//   }

//   return this.http.get<Pagination<Product>>(this.baseUrl + 'products', { params });
// }

//   // getProducts(brands?: string[], types?: string[]) {
//   //   let params = new HttpParams();

//   //   if (brands?.length) {
//   //     params = params.set('brands', brands.join(',')); // use set to avoid duplicates
//   //   }

//   //   if (types?.length) {
//   //     params = params.set('types', types.join(','));
//   //   }

//   //   params = params.set('pageSize', 20);

//   //   return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {params});
//   // }

//   // getBrands()
//   getBrands() {
//     if (this.brands.length > 0) return;
//     return this.http
//       .get<string[]>(this.baseUrl + 'products/brands')
//       .subscribe({ next: (r) => (this.brands = r) });
//   }

//   // getTypes()
//   getTypes() {
//     if (this.types.length > 0) return;
//     return this.http
//       .get<string[]>(this.baseUrl + 'products/types')
//       .subscribe({ next: (r) => (this.types = r) });
//   }
// }
