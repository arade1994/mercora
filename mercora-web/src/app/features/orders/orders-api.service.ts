import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environment';
import { PlaceOrderRequestDto, PlaceOrderResponseDto } from './orders.models';

@Injectable({
  providedIn: 'root',
})
export class OrdersApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  placeOrder(request: PlaceOrderRequestDto) {
    return this.http.post<PlaceOrderResponseDto>(`${this.baseUrl}/api/orders`, request);
  }
}
