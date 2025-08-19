// src/app/login/login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage = '';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

 login() {
  if (this.loginForm.invalid) return;

  const loginData = this.loginForm.value;
  console.log('Sending login:', loginData);

  this.http.post('https://localhost:7298/api/Auth/login', loginData, {
    headers: { 'Content-Type': 'application/json' }
  }).subscribe({
    next: (res: any) => {
      console.log('Login success:', res);

      // Store token and role
      localStorage.setItem('token', res.token);
      localStorage.setItem('role', res.role);

      // Navigate based on role

      if (res.role === 'Hotel Owner') {
        localStorage.setItem('ownerId', res.ownerId);
        this.router.navigate(['/dashboard']);
      } else if (res.role === 'User') {
        localStorage.setItem('userId', res.userId);
        this.router.navigate(['/user/dashboard']);
      } else {
        window.location.href = 'http://localhost:25822/Admin/Dashboard';
      }
    },
    error: (err) => {
      console.error('Login error:', err);
      if (err.status === 0) {
        this.errorMessage = 'Cannot connect to the server. Is the API running?';
      } else if (err.error && typeof err.error === 'string') {
        this.errorMessage = err.error;
      } else {
        this.errorMessage = 'Login failed. Please check credentials.';
      }
    }
  });
}
}
