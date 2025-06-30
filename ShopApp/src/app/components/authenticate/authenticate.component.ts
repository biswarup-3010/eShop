import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { NewUser } from 'src/app/Interfaces/newUser';
import { AuthenticatorService } from 'src/app/services/Auth/authenticator.service';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.component.html',
  styleUrls: ['./authenticate.component.css'],
})
export class AuthenticateComponent {
  isLoginMode: boolean = true;
  showPassword: boolean = false;
  showConfirmPassword: boolean = false;

  loginData = {
    email: '',
    password: '',
  };

  registerData = {
    fullName: '',
    email: '',
    phone: '',
    address: '',
    password: '',
    confirmPassword: '',
  };

  termsAccepted: boolean = false;

  constructor(
    private router: Router,
    private loginService: AuthenticatorService
  ) {}

  togglePasswordVisibility(field: string) {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else if (field === 'confirm') {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  toggleMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  onLoginSubmit(form: NgForm) {
    if (form.invalid) return;

    this.loginService.loginUser(this.loginData).subscribe(
      (response) => {
        // Example: store login status/token
        sessionStorage.setItem('isLogin', 'true');
        sessionStorage.setItem('roleid', String(response.user.roleId));
        sessionStorage.setItem('userid', String(response.user.userId));
        this.router.navigate(['/user']);
      },
      (error) => {
        console.error('Login failed:', error);
        alert('Login failed. Please check your credentials.');
      }
    );
  }

  onRegisterSubmit(form: NgForm) {
    if (form.invalid || this.isPasswordMismatch(form)) {
      alert('Please fix form errors before submitting.');
      return;
    }

    const newUser: NewUser = {
      fullName: this.registerData.fullName,
      email: this.registerData.email,
      password: this.registerData.password,
      phone: this.registerData.phone,
      address: this.registerData.address,
      roleid: '2', // You can set it dynamically or based on user selection; "2" is example
    };

    this.loginService.registerUser(newUser).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        alert('Registration successful! You can now log in.');
        this.toggleMode(); // Switch to login mode after successful registration
      },
      error: (error) => {
        console.error('Registration failed:', error);
        alert('Registration failed. Please try again later.');
      },
    });
  }

  isFieldInvalid(form: NgForm, fieldName: string): boolean {
    const field = form.controls[fieldName];
    return !!field && field.invalid && (field.dirty || field.touched);
  }

  getFieldError(form: NgForm, fieldName: string): string {
    const field = form.controls[fieldName];
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'This field is required.';
    if (field.errors['email']) return 'Invalid email format.';
    if (field.errors['minlength'])
      return `Minimum length is ${field.errors['minlength'].requiredLength} characters.`;
    if (field.errors['maxlength'])
      return `Maximum length is ${field.errors['maxlength'].requiredLength} characters.`;
    if (field.errors['pattern']) return 'Invalid format.';

    return 'Invalid field.';
  }

  isPasswordMismatch(form: NgForm): boolean {
    return (
      form.controls['password']?.value !==
      form.controls['confirmPassword']?.value
    );
  }

  onSocialLogin(provider: string) {
    console.log(`Social login with: ${provider}`);
    alert(`Social login with ${provider} is not implemented yet.`);
  }
}
