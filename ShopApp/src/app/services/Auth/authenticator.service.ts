import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginData } from 'src/app/Interfaces/logindata';
import { RegisterData } from 'src/app/Interfaces/registerdata';
import { NewUser } from 'src/app/Interfaces/newUser';

@Injectable({
  providedIn: 'root',
})
export class AuthenticatorService {
  private baseUrl = 'https://localhost:7121/api/Auth';
  constructor(private http: HttpClient) {}
  loginUser(credentials: LoginData): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/LoginUser`, credentials);
  }
  registerUser(registerData: NewUser): Observable<any> {
    const url = 'https://localhost:7121/api/User/RegisterUser';
    return this.http.post(url, registerData);
  }
}
