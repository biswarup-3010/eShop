export interface ProductDto {
  productId: number;
  productName: string;
  description?: string;
  price: number;
  stock?: number;
  categoryId?: number;

  // Optional field for UI (not from API, used for wishlist menu toggle)
  showWishlistMenu?: boolean;
}
