import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  imports : [CommonModule,ReactiveFormsModule],
  selector: 'app-register',
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage = '';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      passwordHash: ['', Validators.required],
      contactNumber: ['', Validators.required],
      address: ['', Validators.required],
      gender: ['', Validators.required],
      role: ['User']  // default hidden field
    });
  }

  register() {
    if (this.registerForm.invalid) return;

    const formData = this.registerForm.value;

    this.http.post('http://localhost:7298/api/User', formData).subscribe({
      next: () => {
        alert('✅ Registered successfully!');
        this.router.navigate(['/login']);
      },
      error: () => {
        this.errorMessage = '❌ Registration failed. Try a different email.';
      }
    });
  }
}
