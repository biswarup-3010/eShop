import {
  Component,
  EventEmitter,
  HostListener,
  Input,
  Output,
  OnInit,
} from '@angular/core';
import { ProductService } from 'src/app/services/product/product.service';
import { ProductDto } from 'src/app/Interfaces/poductdto';
import { CategoryDto } from 'src/app/Interfaces/categoryDto';
import { ReviewDto } from 'src/app/Interfaces/reviewDto';
import { ProductImageDto } from 'src/app/Interfaces/productimagedto';
import { forkJoin } from 'rxjs';

// Extended interface for UI functionality (keeping your original extended features)
interface ProductWithImages extends ProductDto {
  images: ProductImageDto[];
  averageRating: number;
  reviewCount: number;
  primaryImage?: string;
}
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  products: ProductWithImages[] = [];
  filteredProducts: ProductWithImages[] = [];
  categories: CategoryDto[] = [];
  loading = true;

  // Search and Filter states
  searchQuery = '';
  selectedCategory = '';
  priceRange = '';
  sortBy = 'relevance';
  viewMode: 'grid' | 'list' = 'grid';

  // Product detail modal states
  selectedProductDetail: ProductWithImages | null = null;
  selectedProductReviews: ReviewDto[] = [];
  selectedQuantity = 1;
  currentImage = '';

  // Wishlist (stored in component state - you might want to use a service)
  wishlist: Set<number> = new Set();

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.loadInitialData();
  }

  async loadInitialData() {
    try {
      this.loading = true;

      // Load products and categories in parallel
      const [products, categories] = await Promise.all([
        this.productService.GetProducts().toPromise(),
        this.productService.GetCategories().toPromise(),
      ]);

      this.categories = categories || [];

      // Load images and reviews for each product
      if (products) {
        this.products = await Promise.all(
          products.map(async (product) => {
            const [images, reviews] = await Promise.all([
              this.productService
                .GetProductImages(product.productId)
                .toPromise(),
              this.productService.GetReviews(product.productId).toPromise(),
            ]);

            const productWithImages: ProductWithImages = {
              ...product,
              images: images || [],
              averageRating: this.calculateAverageRating(reviews || []),
              reviewCount: reviews?.length || 0,
              primaryImage:
                images?.find((img) => img.isPrimary)?.imageUrl ||
                images?.[0]?.imageUrl,
            };

            return productWithImages;
          })
        );
      }

      this.filteredProducts = this.products;
      this.loading = false;
    } catch (error) {
      console.error('Error loading data:', error);
      this.loading = false;
    }
  }

  calculateAverageRating(reviews: ReviewDto[]): number {
    if (!reviews.length) return 0;
    const sum = reviews.reduce((acc, review) => acc + review.rating, 0);
    return Math.round((sum / reviews.length) * 10) / 10;
  }

  onSearch() {
    this.applyFilters();
  }

  applyFilters() {
    let filtered = [...this.products];

    // Search filter
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase();
      filtered = filtered.filter(
        (product) =>
          product.productName.toLowerCase().includes(query) ||
          product.description?.toLowerCase().includes(query)
      );
    }

    // Category filter
    if (this.selectedCategory) {
      filtered = filtered.filter(
        (product) => product.categoryId === Number(this.selectedCategory)
      );
    }

    // Price range filter
    if (this.priceRange) {
      filtered = filtered.filter((product) => {
        const price = product.price;
        switch (this.priceRange) {
          case '0-25':
            return price <= 25;
          case '25-50':
            return price > 25 && price <= 50;
          case '50-100':
            return price > 50 && price <= 100;
          case '100+':
            return price > 100;
          default:
            return true;
        }
      });
    }

    this.filteredProducts = filtered;
    this.applySorting();
  }

  applySorting() {
    switch (this.sortBy) {
      case 'price-low':
        this.filteredProducts.sort((a, b) => a.price - b.price);
        break;
      case 'price-high':
        this.filteredProducts.sort((a, b) => b.price - a.price);
        break;
      case 'rating':
        this.filteredProducts.sort((a, b) => b.averageRating - a.averageRating);
        break;
      case 'newest':
        this.filteredProducts.sort((a, b) => b.productId - a.productId);
        break;
      default:
        // Keep current order for relevance
        break;
    }
  }

  selectProduct(product: ProductWithImages) {
    this.selectedProductDetail = product;
    this.currentImage = product.primaryImage || '';
    this.selectedQuantity = 1;
    this.loadProductReviews(product.productId);
  }

  async loadProductReviews(productId: number) {
    try {
      this.selectedProductReviews =
        (await this.productService.GetReviews(productId).toPromise()) || [];
    } catch (error) {
      console.error('Error loading reviews:', error);
      this.selectedProductReviews = [];
    }
  }

  closeModal() {
    this.selectedProductDetail = null;
    this.selectedProductReviews = [];
  }

  quickView(product: ProductWithImages, event: Event) {
    event.stopPropagation();
    this.selectProduct(product);
  }

  addToCart(product: ProductWithImages, event?: Event) {
    if (event) event.stopPropagation();

    // Implement your add to cart logic here
    console.log(
      'Adding to cart:',
      product.productName,
      'Quantity:',
      this.selectedQuantity
    );

    // You might want to show a success message or update cart state
    alert(`Added ${product.productName} to cart!`);
  }

  toggleWishlist(product: ProductWithImages, event?: Event) {
    if (event) event.stopPropagation();

    if (this.wishlist.has(product.productId)) {
      this.wishlist.delete(product.productId);
    } else {
      this.wishlist.add(product.productId);
    }
  }

  isInWishlist(productId: number): boolean {
    return this.wishlist.has(productId);
  }

  increaseQuantity() {
    if (
      this.selectedProductDetail &&
      this.selectedQuantity < (this.selectedProductDetail.stock || 0)
    ) {
      this.selectedQuantity++;
    }
  }

  decreaseQuantity() {
    if (this.selectedQuantity > 1) {
      this.selectedQuantity--;
    }
  }

  getStarArray(rating: number): boolean[] {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    const stars: boolean[] = [];

    for (let i = 0; i < 5; i++) {
      if (i < fullStars) {
        stars.push(true);
      } else if (i === fullStars && hasHalfStar) {
        stars.push(true); // You might want to handle half stars differently
      } else {
        stars.push(false);
      }
    }

    return stars;
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString();
  }
}
