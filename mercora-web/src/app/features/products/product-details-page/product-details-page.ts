import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProductsApiService } from '../products-api.service';
import { CommonModule } from '@angular/common';
import { ProductDetailsDto } from '../products.models';

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
}
