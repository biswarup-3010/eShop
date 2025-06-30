import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { CategoryDto } from 'src/app/Interfaces/categoryDto';
import { ProductDto } from 'src/app/Interfaces/poductdto';
import { ProductImageDto } from 'src/app/Interfaces/productimagedto';
import { ReviewDto } from 'src/app/Interfaces/reviewDto';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  GetProducts(): Observable<ProductDto[]> {
    return this.http
      .get<ProductDto[]>('https://localhost:7121/api/Product/GetAllProducts')
      .pipe(catchError(this.errorHandler));
  }

  GetCategories(): Observable<CategoryDto[]> {
    return this.http
      .get<CategoryDto[]>(
        'https://localhost:7121/api/Category/GetAllCategories'
      )
      .pipe(catchError(this.errorHandler));
  }

  GetReviews(productId: number): Observable<ReviewDto[]> {
    return this.http
      .get<ReviewDto[]>(
        'https://localhost:7121/api/Review/GetByProduct/' + productId
      )
      .pipe(catchError(this.errorHandler));
  }

  GetProductImages(productId: number): Observable<ProductImageDto[]> {
    return this.http
      .get<ProductImageDto[]>(
        'https://localhost:7121/api/ProductImage/GetImagesByProductId/' +
          productId
      )
      .pipe(catchError(this.errorHandler));
  }
  errorHandler(err: HttpErrorResponse) {
    return throwError(err.message || 'server error');
  }
}
