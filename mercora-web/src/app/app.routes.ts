import { Routes } from '@angular/router';
import { ProductsListPage } from './features/products/products-list-page/products-list-page';
import { ProductDetailsPage } from './features/products/product-details-page/product-details-page';

export const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'products' },
    { path: 'products', component: ProductsListPage },
    { path: 'products/:slug', component: ProductDetailsPage },
];
