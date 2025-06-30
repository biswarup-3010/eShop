import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticateComponent } from './components/authenticate/authenticate.component';
import { CommonLayoutComponent } from './components/Layouts/common-layout/common-layout.component';
import { UserLayoutComponent } from './components/Layouts/user-layout/user-layout.component';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  { path: 'auth', component: AuthenticateComponent },
  { path: 'home', component: CommonLayoutComponent },
  { path: 'user', component: UserLayoutComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
