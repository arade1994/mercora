import { computed, inject, Injectable, signal } from '@angular/core';
import { ProductsApiService } from './products-api.service';
import { PagedResultDto, ProductListItemDto, ProductQueryDto, ProductSort } from './products.models';

type LoadState = 'idle' | 'loading' | 'loaded' | 'error';

@Injectable({
  providedIn: 'root',
})
export class ProductsStoreService {
  private readonly api = inject(ProductsApiService);

  private readonly _state = signal<LoadState>('idle');
  private readonly _error = signal<string | null>(null);
  private readonly _query = signal<ProductQueryDto>({
    page: 1,
    pageSize: 12,
    sort: 'newest',
    search: '',
  });
  private readonly _result = signal<PagedResultDto<ProductListItemDto> | null>(null);

  readonly state = computed(() => this._state());
  readonly error = computed(() => this._error());
  readonly query = computed(() => this._query());
  readonly result = computed(() => this._result());

  readonly items = computed(() => this._result()?.items ?? []);
  readonly totalCount = computed(() => this._result()?.totalCount ?? 0);
  readonly page = computed(() => this._result()?.page ?? this._query().page ?? 1);
  readonly pageSize = computed(() => this._result()?.pageSize ?? this._query().pageSize ?? 12);
  readonly totalPages = computed(() => {
    const total = this.totalCount();
    const size = this.pageSize();
    return size > 0 ? Math.max(1, Math.ceil(total / size)) : 1;
  });

  load() {
    this._state.set('loading');
    this._error.set(null);

    const q = this._query();

    this.api.getProducts(q).subscribe({
      next: (res) => {
        this._result.set(res);
        this._state.set('loaded');
      },
      error: (err) => {
        this._state.set("error");
        this._error.set(err?.message ?? "Failed to load products");
      },
    });
  }

  setSearch(search: string) {
    this._query.update((q) => ({...q, search, page: 1}));
  }

  setSort(sort: ProductSort) {
    this._query.update((q) => ({...q, sort, page: 1}));
  }

  setPage(page: number) {
    this._query.update((q) => ({...q, page}));
  }

  setPageSize(pageSize: number) {
    this._query.update((q) => ({...q, pageSize, page: 1}));
  }
}
