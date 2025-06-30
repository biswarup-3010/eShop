import { Component, OnInit } from '@angular/core';
import { ProductDto } from 'src/app/Interfaces/poductdto';

@Component({
  selector: 'app-common-layout',
  templateUrl: './common-layout.component.html',
  styleUrls: ['./common-layout.component.css'],
})
export class CommonLayoutComponent implements OnInit {
  activeSection = 'home';
  isScrolled = false;
  isMobileMenuOpen = false;

  products: ProductDto[] = [
    {
      productId: 1,
      productName: 'Premium Collection',
      description: 'Exquisite handcrafted pieces for the discerning individual',
      price: 999,
      stock: 10,
      categoryId: 1,
      showWishlistMenu: false,
    },
    {
      productId: 2,
      productName: 'Luxury Series',
      description: 'Sophisticated designs that embody elegance and refinement',
      price: 1499,
      stock: 8,
      categoryId: 1,
      showWishlistMenu: false,
    },
    {
      productId: 3,
      productName: 'Elite Edition',
      description: 'Exclusive limited pieces for the ultimate connoisseur',
      price: 2999,
      stock: 5,
      categoryId: 2,
      showWishlistMenu: false,
    },
  ];

  carouselImages = [
    {
      url: 'https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=800',
      title: 'Exquisite Craftsmanship',
      subtitle: 'Handcrafted with precision and passion',
    },
    {
      url: 'https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=800',
      title: 'Timeless Elegance',
      subtitle: 'Designs that transcend generations',
    },
    {
      url: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800',
      title: 'Modern Luxury',
      subtitle: 'A blend of tradition and innovation',
    },
  ];

  currentSlide = 0;
  carouselInterval: any;

  testimonials = [
    {
      text: 'Absolutely extraordinary quality and service. Every piece tells a story of masterful craftsmanship.',
      author: 'Victoria Sterling',
      role: 'Art Collector',
    },
    {
      text: 'The attention to detail is unparalleled. These are not just products, they are works of art.',
      author: 'Edmund Blackwood',
      role: 'Design Connoisseur',
    },
    {
      text: 'A truly aristocratic experience from browsing to purchase. Exceptional in every way.',
      author: 'Isabella Worthington',
      role: 'Luxury Enthusiast',
    },
  ];

  currentTestimonial = 0;

  ngOnInit() {
    window.addEventListener('scroll', () => {
      this.isScrolled = window.scrollY > 50;
    });

    window.addEventListener('scroll', () => {
      const sections = ['home', 'products', 'contact'];
      const scrollPos = window.scrollY + 100;

      sections.forEach((section) => {
        const element = document.getElementById(section);
        if (element) {
          const offsetTop = element.offsetTop;
          const offsetHeight = element.offsetHeight;

          if (scrollPos >= offsetTop && scrollPos < offsetTop + offsetHeight) {
            this.activeSection = section;
          }
        }
      });
    });

    this.startCarousel();
    this.startTestimonialCarousel();
    this.setupScrollAnimations();
  }

  startCarousel() {
    this.carouselInterval = setInterval(() => {
      this.nextSlide();
    }, 5000);
  }

  startTestimonialCarousel() {
    setInterval(() => {
      this.currentTestimonial =
        (this.currentTestimonial + 1) % this.testimonials.length;
    }, 4000);
  }

  nextSlide() {
    this.currentSlide = (this.currentSlide + 1) % this.carouselImages.length;
  }

  prevSlide() {
    this.currentSlide =
      this.currentSlide === 0
        ? this.carouselImages.length - 1
        : this.currentSlide - 1;
  }

  goToSlide(index: number) {
    this.currentSlide = index;
    clearInterval(this.carouselInterval);
    this.startCarousel();
  }

  goToTestimonial(index: number) {
    this.currentTestimonial = index;
  }

  scrollToProducts() {
    document.getElementById('products')?.scrollIntoView({ behavior: 'smooth' });
  }

  setupScrollAnimations() {
    const observerOptions = {
      threshold: 0.1,
      rootMargin: '0px 0px -50px 0px',
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          entry.target.classList.add('animate-in');
        }
      });
    }, observerOptions);

    setTimeout(() => {
      document.querySelectorAll('.animate-on-scroll').forEach((el) => {
        observer.observe(el);
      });
    }, 100);
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
  }

  toggleWishlistMenu(product: ProductDto) {
    product.showWishlistMenu = !product.showWishlistMenu;
  }

  addToWishlist(product: ProductDto) {
    console.log('Adding to wishlist:', product);
    product.showWishlistMenu = false;
    // Hook your wishlist service call here
  }

  removeFromWishlist(product: ProductDto) {
    console.log('Removing from wishlist:', product);
    product.showWishlistMenu = false;
    // Hook your wishlist service call here
  }

  buyNow(product: ProductDto) {
    console.log('Buying product:', product);
    // Hook your buy-now logic here
  }

  addToCart(product: ProductDto) {
    console.log('Adding to cart:', product);
    // Hook your add-to-cart logic here
  }
}
