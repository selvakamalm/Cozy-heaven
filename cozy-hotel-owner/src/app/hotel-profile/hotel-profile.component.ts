import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-hotel-profile',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './hotel-profile.component.html',
  styleUrls: ['./hotel-profile.component.css']
})
export class HotelProfileComponent implements OnInit {
  hotel: any = null;
  errorMessage = '';
  isEditing = false;


  constructor(private http: HttpClient) {}

  ngOnInit(): void {
  const token = localStorage.getItem('token');
  const ownerId = localStorage.getItem('ownerId');

  console.log('Token:', token);
  console.log('OwnerId:', ownerId);

  if (!token || !ownerId) {
    this.errorMessage = 'Missing  Failed    login credentials';
    return;
  }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    const apiUrl = `https://localhost:7298/api/Hotel/owner/${ownerId}`;

    this.http.get(apiUrl, { headers }).subscribe({
      next: (data: any) => {
  console.log('✅ Hotel data:', data); // 👈 Add this to inspect
  this.hotel = Array.isArray(data) ? data[0] : data; // handles both array or object
  localStorage.setItem('hotelId', this.hotel.id);
},
      error: (err) => {
        console.error('Error loading hotel:', err);
        this.errorMessage = 'Failed to load hotel details.';
      }
    });
  }

 editHotel() {
  this.isEditing = true;
}

cancelEdit() {
  this.isEditing = false;
}

saveHotel() {
  if (!this.hotel || !this.hotel.id) return;

  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json'
  });

  this.http.put(`https://localhost:7298/api/Hotel/${this.hotel.id}`, this.hotel, { headers })
    .subscribe({
      next: () => {
        alert('Hotel updated successfully!');
        this.isEditing = false;
      },
      error: (err) => {
        console.error('Update failed:', err);
        this.errorMessage = 'Failed to update hotel.';
      }
    });
}


deleteHotel() {
  if (!this.hotel || !this.hotel.id) return;

  const confirmed = confirm(`Are you sure you want to delete hotel: ${this.hotel.name}?`);
  if (!confirmed) return;

  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`
  });

  this.http.delete(`https://localhost:7298/api/Hotel/${this.hotel.id}`, { headers })
    .subscribe({
      next: () => {
        alert('Hotel deleted successfully!');
        this.hotel = null;
      },
      error: (err) => {
        console.error('❌ Delete failed:', err);
        this.errorMessage = 'Failed to delete hotel.';
      }
    });
}


}
