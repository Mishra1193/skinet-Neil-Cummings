import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { Product } from '../../../shared/models/product';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from '../../../features/shop/filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from '@angular/material/list';
import { ShopParams } from '../../../shared/models/shopParams';
import { CommonModule } from '@angular/common';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    CommonModule,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenuTrigger,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatPaginator,
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  pagination?: Pagination<Product>;   // holds full response
  products: Product[] = [];

  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);

  shopParams: ShopParams = new ShopParams();
  pageSizeOptions: number[] = [5, 10, 15, 20];   // transcript: 5 per row

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price Low–High', value: 'priceAsc' },
    { name: 'Price High–Low', value: 'priceDesc' },
  ];

  ngOnInit(): void {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (res) => {
        this.pagination = res;
        this.products = res.data;
      },
      error: (error) => console.log(error),
    });
  }

  onSearchChange(){
    this.shopParams.pageNumber = 1; // reset to page 1 on search change
    this.getProducts();
  }

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1; // MatPaginator 0-based
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  onSortChange(event: MatSelectionListChange) {
    const option = event.options[0];
    if (!option) return;
    this.shopParams.sort = option.value as string;
    this.shopParams.pageNumber = 1; // reset to page 1
    this.getProducts();
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types,
      },
    });

    dialogRef.afterClosed().subscribe({
      next: (result?: { selectedBrands: string[]; selectedTypes: string[] }) => {
        if (!result) return;
        this.shopParams.brands = result.selectedBrands ?? [];
        this.shopParams.types = result.selectedTypes ?? [];
        this.shopParams.pageNumber = 1; // reset to page 1
        this.getProducts();
      },
    });
  }
}


// import { Component, inject, OnInit } from '@angular/core';
// import { ShopService } from '../../../core/services/shop.service';
// import { Product } from '../../../shared/models/product';
// import { ProductItemComponent } from './product-item/product-item.component';
// import { MatDialog } from '@angular/material/dialog';
// import { FiltersDialogComponent } from '../../../features/shop/filters-dialog/filters-dialog.component';
// import { MatButton } from '@angular/material/button';
// import { MatIcon } from '@angular/material/icon';
// import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
// import {
//   MatListOption,
//   MatSelectionList,
//   MatSelectionListChange,
// } from '@angular/material/list';
// import { ShopParams } from '../../../shared/models/shopParams';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-shop',
//   standalone: true,
//   imports: [
//     CommonModule,
//     ProductItemComponent,
//     MatButton,
//     MatIcon,
//     MatMenuTrigger,
//     MatMenu,
//     MatSelectionList,
//     MatListOption,
//   ],
//   templateUrl: './shop.component.html',
//   styleUrl: './shop.component.scss',
// })
// export class ShopComponent implements OnInit {
//   products: Product[] = [];
//   private shopService = inject(ShopService);
//   private dialogService = inject(MatDialog);

//   shopParams: ShopParams = new ShopParams();

//   sortOptions = [
//     { name: 'Alphabetical', value: 'name' },
//     { name: 'Price Low–High', value: 'priceAsc' },
//     { name: 'Price High–Low', value: 'priceDesc' },
//   ];

//   ngOnInit(): void {
//     this.shopService.getBrands();
//     this.shopService.getTypes();
//     this.getProducts();
//   }

//   getProducts() {
//     this.shopService.getProducts(this.shopParams).subscribe({
//       next: (response) => (this.products = response.data),
//       error: (error) => console.log(error),
//     });
//   }

//   onSortChange(event: MatSelectionListChange) {
//     const option = event.options[0];
//     if (!option) return;
//     this.shopParams.sort = option.value as string;
//     console.log('Sort selected:', this.shopParams.sort);
//     this.getProducts();
//   }

//   openFiltersDialog() {
//     const dialogRef = this.dialogService.open(FiltersDialogComponent, {
//       minWidth: '500px',
//       data: {
//         selectedBrands: this.shopParams.brands,
//         selectedTypes: this.shopParams.types,
//       },
//     });

//     dialogRef.afterClosed().subscribe({
//       next: (result?: { selectedBrands: string[]; selectedTypes: string[] }) => {
//         if (!result) return;
//         this.shopParams.brands = result.selectedBrands ?? [];
//         this.shopParams.types = result.selectedTypes ?? [];
//         this.getProducts();
//         console.log('Filters applied:', this.shopParams.brands, this.shopParams.types);
//       },
//     });
//   }
// }



// import { Component, inject, OnInit } from '@angular/core';
// import { ShopService } from '../../../core/services/shop.service';
// import { Product } from '../../../shared/models/product';
// import { ProductItemComponent } from './product-item/product-item.component';
// import { MatDialog } from '@angular/material/dialog';
// import { FiltersDialogComponent } from '../../../features/shop/filters-dialog/filters-dialog.component';
// import { MatButton } from '@angular/material/button';
// import { MatIcon } from '@angular/material/icon';
// import { ShopParams } from '../../../shared/shopParams';
// import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
// import {
//   MatListOption,
//   MatSelectionList,
//   MatSelectionListChange,
// } from '@angular/material/list';

// @Component({
//   selector: 'app-shop',
//   standalone: true,
//   imports: [
//     ProductItemComponent,
//     MatButton,
//     MatIcon,
//     MatMenuTrigger,
//     MatMenu,
//     MatSelectionList,
//     MatListOption,
//   ],
//   templateUrl: './shop.component.html',
//   styleUrl: './shop.component.scss',
// })
// export class ShopComponent implements OnInit {
//   products: Product[] = [];
//   private shopService = inject(ShopService);
//   private dialogService = inject(MatDialog);
//   selectedBrands: string[] = [];
//   selectedTypes: string[] = [];
//   selectedSort: string = 'name';
//   sortOptions = [
//     { name: 'Alphabetical', value: 'name' },
//     { name: 'Price Low–High', value: 'priceAsc' },
//     { name: 'Price High–Low', value: 'priceDesc' },
//   ];
//   shopParams: ShopParams = new ShopParams();

//   sortOption = [
//     { name: 'Alphabetical', value: 'name' },
//     { name: 'Price Low–High', value: 'priceAsc' },
//     { name: 'Price High–Low', value: 'priceDesc' },
//   ];

//   ngOnInit(): void {
//     this.shopService.getBrands();
//     this.shopService.getTypes();
//     this.getProducts(); // ← load once on page load
//   }

//   getProducts() {
//     this.shopService
//       .getProducts(this.selectedBrands, this.selectedTypes)
//       .subscribe({
//         next: (response) => (this.products = response.data),
//         error: (error) => console.log(error),
//       });
//   }

//   loadProducts() {
//     this.shopService
//       .getProducts(this.selectedBrands, this.selectedTypes, this.selectedSort)
//       .subscribe({
//         next: (res) => (this.products = res.data),
//         error: (err) => console.log(err),
//       });
//   }
//   // onSortChanged(value: string) {
//   //   this.selectedSort = value;
//   //   this.getProducts();
//   // }

//   onSortChange(event: MatSelectionListChange) {
//     const option = event.options[0];
//     if (!option) return;
//     this.selectedSort = option.value as string;
//     // Optional: console log to mirror Sir's check
//     console.log(this.selectedSort);
//     this.loadProducts();
//   }

//   openFiltersDialog() {
//     const dialogRef = this.dialogService.open(FiltersDialogComponent, {
//       minWidth: '500px',
//       data: {
//         selectedBrands: this.selectedBrands,
//         selectedTypes: this.selectedTypes,
//       },
//     });

//     dialogRef.afterClosed().subscribe({
//       next: (result?: {
//         selectedBrands: string[];
//         selectedTypes: string[];
//       }) => {
//         if (!result) return;
//         this.selectedBrands = result.selectedBrands ?? [];
//         this.selectedTypes = result.selectedTypes ?? [];
//         this.loadProducts(); // Reload products after filters are applied
//         console.log(
//           'Filters applied:',
//           this.selectedBrands,
//           this.selectedTypes
//         );
//       },
//     });
//   }
// }
