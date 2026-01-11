import { Component, inject } from '@angular/core';
import { CartStore } from '../cart-store.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart-page',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cart-page.html',
  styleUrl: './cart-page.scss',
})
export class CartPage {
  readonly cart = inject(CartStore);

  remove(variantId: number) {
    this.cart.remove(variantId);
  }

  setQuantity(variantId: number, quantity: number) {
    this.cart.setQuantity(variantId, quantity);
  }

  clear() {
    this.cart.clear();
  }
}
