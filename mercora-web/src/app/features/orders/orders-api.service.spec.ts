import { TestBed } from '@angular/core/testing';

import { OrdersApi } from './orders-api.service';

describe('OrdersApi', () => {
  let service: OrdersApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrdersApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
