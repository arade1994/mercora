export interface PagedResultDto<T> {
    page: number;
    pageSize: number;
    totalCount: number;
    items: T[];
}

export interface ProductListItemDto {
    productId: number;
    name: string;
    slug: string;
    basePrice: number;
    priceFrom: number;
    currencyCode: string;
    primaryImageUrl?: string | null;
}

export interface ProductImageDto {
  imageId: number;
  url: string;
  isPrimary: boolean;
  sortOrder: number;
}

export interface ProductVariantDto {
  variantId: number;
  sku: string;
  variantName?: string | null;
  price: number;
  quantityOnHand: number;
  reorderPoint: number;
  isActive: boolean;
}

export interface ProductDetailsDto {
  productId: number;
  name: string;
  slug: string;
  description?: string | null;
  basePrice: number;
  currencyCode: string;
  primaryImageUrl?: string | null;
  images: ProductImageDto[];
  variants: ProductVariantDto[];
}

export type ProductSort = 'newest' | 'priceAsc' | 'priceDesc' | 'nameAsc' | 'nameDesc';

export interface ProductQueryDto {
  search?: string;
  categorySlug?: string;
  minPrice?: number;
  maxPrice?: number;
  sort?: ProductSort;
  page?: number;
  pageSize?: number;
}