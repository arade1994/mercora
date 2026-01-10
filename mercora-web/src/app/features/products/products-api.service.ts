import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environment';
import { PagedResultDto, ProductDetailsDto, ProductListItemDto, ProductQueryDto } from './products.models';

@Injectable({
  providedIn: 'root',
})
export class ProductsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  getProducts(query: ProductQueryDto) {
    let params = new HttpParams();

    for (const [key, value] of Object.entries(query)) {
      if (value === undefined || value === null || value === '') continue;
      params = params.set(key, String(value));
    }

    return this.http.get<PagedResultDto<ProductListItemDto>>(
      `${this.baseUrl}/api/products`,
      { params }
    );
  }

  getBySlug(slug: string) {
    return this.http.get<ProductDetailsDto>(`${this.baseUrl}/api/products/${encodeURIComponent(slug)}`);
  }
}
