import { Component, OnInit, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-user-layout',
  templateUrl: './user-layout.component.html',
  styleUrls: ['./user-layout.component.css'],
})
export class UserLayoutComponent implements OnInit, AfterViewInit {
  ngOnInit(): void {
    // Initialize default subsection visibility on init
  }

  ngAfterViewInit(): void {
    this.initializeProfileSection();

    // Periodically update badge counts
    setInterval(() => this.updateCartCount(), 10000);
    setInterval(() => this.updateWishlistCount(), 15000);
  }

  showSection(sectionId: string, filter: string | null = null): void {
    const sections = document.querySelectorAll<HTMLElement>('.main-content');
    sections.forEach((section) => section.classList.remove('active'));

    const selectedSection = document.getElementById(sectionId);
    if (selectedSection) {
      selectedSection.classList.add('active');
    }

    const navItems = document.querySelectorAll<HTMLElement>(
      '.bottom-nav .nav-item'
    );
    navItems.forEach((item) => item.classList.remove('active'));

    const activeNavItem = Array.from(navItems).find((item) =>
      item.getAttribute('onclick')?.includes(sectionId)
    );
    if (activeNavItem) {
      activeNavItem.classList.add('active');
    }

    if (sectionId === 'products' && filter) {
      console.log('Filtering products by:', filter);
    }
  }

  showProfileSection(subsectionId: string): void {
    const subsections = document.querySelectorAll<HTMLElement>(
      '.profile-subsection'
    );
    subsections.forEach((subsection) => {
      subsection.classList.remove('active');
      subsection.style.display = 'none';
    });

    const selectedSubsection = document.getElementById(subsectionId);
    if (selectedSubsection) {
      selectedSubsection.classList.add('active');
      selectedSubsection.style.display = 'block';
    }

    const menuItems = document.querySelectorAll<HTMLElement>('.profile-menu a');
    menuItems.forEach((item) => item.classList.remove('active'));

    const activeMenuItem = Array.from(menuItems).find((item) =>
      item.getAttribute('onclick')?.includes(subsectionId)
    );
    if (activeMenuItem) {
      activeMenuItem.classList.add('active');
    }
  }

  private initializeProfileSection(): void {
    const profileSubsections = document.querySelectorAll<HTMLElement>(
      '.profile-subsection'
    );
    profileSubsections.forEach((subsection, index) => {
      subsection.style.display = index === 0 ? 'block' : 'none';
      subsection.classList.toggle('active', index === 0);
    });
  }

  private updateCartCount(): void {
    const cartBadge = document.querySelector<HTMLElement>('.cart-icon .badge');
    if (cartBadge) {
      const current = parseInt(cartBadge.textContent || '0', 10);
      cartBadge.textContent = String(
        Math.max(0, current + Math.floor(Math.random() * 3) - 1)
      );
    }
  }

  private updateWishlistCount(): void {
    const wishlistBadge = document.querySelector<HTMLElement>(
      '.wishlist-icon .badge'
    );
    if (wishlistBadge) {
      const current = parseInt(wishlistBadge.textContent || '0', 10);
      wishlistBadge.textContent = String(
        Math.max(0, current + Math.floor(Math.random() * 2) - 1)
      );
    }
  }
}
