import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProductsApiService } from '../products-api.service';
import { CommonModule } from '@angular/common';
import { ProductDetailsDto } from '../products.models';
import { CartStore } from '../../cart/cart-store.service';

type LoadState = 'idle' | 'loading' | 'loaded' | 'error';

@Component({
  selector: 'app-product-details-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './product-details-page.html',
  styleUrl: './product-details-page.scss',
})
export class ProductDetailsPage {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(ProductsApiService);
  private readonly cart = inject(CartStore);

  readonly state = signal<LoadState>('idle');
  readonly error = signal<string | null>(null);
  readonly product = signal<ProductDetailsDto | null>(null);

  readonly slug = computed(() => this.route.snapshot.paramMap.get('slug') ?? '');

  constructor() {
    this.load();
  }

  load() {
    const slug = this.slug().trim();
    if (!slug) return;

    this.state.set('loading');
    this.error.set(null);

    this.api.getBySlug(slug).subscribe({
      next: (p) => {
        this.product.set(p);
        this.state.set('loaded');
      },
      error: (err) => {
        this.state.set('error');
        this.error.set(err?.message ?? 'Failed to load product');
      },
    });
  }

  addToCart(variantId: number) {
    const p = this.product();
    if (!p) return;

    const v = p.variants.find(v => v.variantId === variantId);
    if (!v) return;

    this.cart.add({
      variantId: v.variantId,
      sku: v.sku,
      productName: p.name,
      variantName: v.variantName,
      unitPrice: v.price,
      currencyCode: p.currencyCode,
      quantity: 1,
      maxQuantity: v.quantityOnHand,
    })
  }
}
