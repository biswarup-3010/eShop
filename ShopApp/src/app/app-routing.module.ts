import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticateComponent } from './components/authenticate/authenticate.component';
import { CommonLayoutComponent } from './components/Layouts/common-layout/common-layout.component';
import { UserLayoutComponent } from './components/Layouts/user-layout/user-layout.component';
import { AuthGuard } from './services/auth.guard';
import { ProductComponent } from './components/product/product.component';

const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  { path: 'auth', component: AuthenticateComponent },
  { path: 'home', component: CommonLayoutComponent },
  {
    path: 'user',
    component: UserLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {
        path: 'home',
        redirectTo: '',
      },
      {
        path: 'products',
        component: ProductComponent,
      },
      {
        path: 'contact',
        component: ProductComponent,
      },
      {
        path: 'wishlist',
        component: ProductComponent,
      },
      {
        path: 'cart',
        component: ProductComponent,
      },
      {
        path: 'profile',
        component: ProductComponent,
      },
    ],
  },
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
