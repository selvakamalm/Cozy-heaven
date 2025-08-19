import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: any = {
    id: 0,
    fullName: '',
    email: '',
    contactNumber: '',
    address: '',
    gender: ''
  };

  isEditMode = false;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    const userId = localStorage.getItem('userId');
    const token = localStorage.getItem('token');
    if (!userId || !token) return;

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    this.http.get<any>(`https://localhost:7298/api/User/${userId}`, { headers })
      .subscribe({
        next: (res) => this.user = res,
        error: (err) => console.error('Failed to load user:', err)
      });
  }

  save() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    this.http.put(`https://localhost:7298/api/User/${this.user.id}`, this.user, { headers })
      .subscribe({
        next: () => {
          alert('Profile updated successfully!');
          this.isEditMode = false;
        },
        error: () => {
          alert('Update failed');
        }
      });
  }
}
