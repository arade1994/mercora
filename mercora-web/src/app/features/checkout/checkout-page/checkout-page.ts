import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CartStore } from '../../cart/cart-store.service';
import { OrdersApi } from '../../orders/orders-api.service';
import { PlaceOrderResponseDto } from '../../orders/orders.models';

type CheckoutState = 'idle' | 'submitting' | 'success' | 'error';

@Component({
  selector: 'app-checkout-page',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './checkout-page.html',
  styleUrl: './checkout-page.scss',
})
export class CheckoutPage {
  readonly cart = inject(CartStore);
  private readonly ordersApi = inject(OrdersApi);

  readonly userId = signal<number>(1);
  readonly state = signal<CheckoutState>('idle');
  readonly errorMessage = signal<string | null>(null);
  readonly success = signal<PlaceOrderResponseDto | null>(null);

  readonly canSubmit = computed(() => this.cart.items().length > 0 && this.userId() > 0 && this.state() !== 'submitting');

  submit() {
    if (!this.canSubmit()) return;

    this.state.set('submitting');
    this.errorMessage.set(null);
    this.success.set(null);

    const request = {
      userId: this.userId(),
      lines: this.cart.items().map(i => ({
        variantId: i.variantId,
        quantity: i.quantity
      })),
    }

    this.ordersApi.placeOrder(request).subscribe({
      next: (res) => {
        this.success.set(res);
        this.state.set('success');
        this.cart.clear();
      },
      error: (err) => {
        this.state.set('error');
        this.errorMessage.set(extractProblemDetailsMessage(err) ?? 'Checkout failed');
      },
      complete: () => {
        this.state.set('idle');
      }
    })
  }
}

/**
 * Tries to extract a useful message from ASP.NET ProblemDetails responses.
 */
function extractProblemDetailsMessage(err: any): string | null {
  const body = err?.error;

  // Our custom middleware shape or standard ValidationProblemDetails
  if (typeof body?.detail === 'string') return body.detail;
  if (typeof body?.title === 'string' && typeof body?.status === 'number') return body.title;

  // ValidationProblemDetails: { errors: { field: [msg] } }
  const errors = body?.errors;
  if (errors && typeof errors === 'object') {
    const firstKey = Object.keys(errors)[0];
    const first = errors[firstKey]?.[0];
    if (typeof first === 'string') return first;
  }

  // Fallback
  return err?.message ?? null;
}