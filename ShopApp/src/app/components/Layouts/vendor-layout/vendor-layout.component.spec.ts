import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorLayoutComponent } from './vendor-layout.component';

describe('VendorLayoutComponent', () => {
  let component: VendorLayoutComponent;
  let fixture: ComponentFixture<VendorLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VendorLayoutComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VendorLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
