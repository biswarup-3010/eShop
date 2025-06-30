import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserLayoutComponent } from './components/Layouts/user-layout/user-layout.component';
import { CommonLayoutComponent } from './components/Layouts/common-layout/common-layout.component';
import { VendorLayoutComponent } from './components/Layouts/vendor-layout/vendor-layout.component';
import { AdminLayoutComponent } from './components/Layouts/admin-layout/admin-layout.component';
import { AuthenticateComponent } from './components/authenticate/authenticate.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ProductComponent } from './components/product/product.component';

@NgModule({
  declarations: [
    AppComponent,
    UserLayoutComponent,
    CommonLayoutComponent,
    VendorLayoutComponent,
    AdminLayoutComponent,
    AuthenticateComponent,
    ProductComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, FormsModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
