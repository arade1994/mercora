export interface PlaceOrderLineDto {
    variantId: number;
    quantity: number;
}

export interface PlaceOrderRequestDto {
    userId: number;
    lines: PlaceOrderLineDto[];
}

export interface PlaceOrderResponseDto {
    orderId: number;
    orderNumber: string;
    totalAmount: number;
    currencyCode: string;
}