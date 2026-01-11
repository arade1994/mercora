import { Routes } from '@angular/router';
import { ProductsListPage } from './features/products/products-list-page/products-list-page';
import { ProductDetailsPage } from './features/products/product-details-page/product-details-page';
import { CartPage } from './features/cart/cart-page/cart-page';
import { CheckoutPage } from './features/checkout/checkout-page/checkout-page';

export const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'products' },
    { path: 'products', component: ProductsListPage },
    { path: 'products/:slug', component: ProductDetailsPage },
    { path: 'cart', component: CartPage },
    { path: 'checkout', component: CheckoutPage }
];
