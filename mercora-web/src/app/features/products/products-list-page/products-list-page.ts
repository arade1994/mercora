import { CommonModule } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ProductsStoreService } from '../products-store.service';
import { ProductSort } from '../products.models';

@Component({
  selector: 'app-products-list-page',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './products-list-page.html',
  styleUrl: './products-list-page.scss',
  standalone: true
})
export class ProductsListPage {
  readonly store = inject(ProductsStoreService);

  readonly searchText = signal('');
  readonly sort = signal<ProductSort>('newest');

  constructor() {
    this.store.load();

    effect(() => {
      const q = this.store.query();
      this.searchText.set(q.search ?? '');
      this.sort.set((q.sort ?? 'newest') as ProductSort);
    });
  }

  onSearch() {
    this.store.setSearch(this.searchText().trim());
    this.store.load();
  }

  onSortChange(sort: ProductSort) {
    this.store.setSort(sort);
    this.store.load();
  }

  prevPage() {
    const page = this.store.page();
    if (page > 1) {
      this.store.setPage(page - 1);
      this.store.load();
    }
  }

  nextPage() {
    const page = this.store.page();
    const total = this.store.totalPages();
    if (page < total) {
      this.store.setPage(page + 1);
      this.store.load();
    }
  }
}
