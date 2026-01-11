export interface CartItem {
    variantId: number;
    sku: string;
    productName: string;
    variantName?: string | null;
    unitPrice: number;
    currencyCode: string;
    quantity: number;
    maxQuantity?: number;
}