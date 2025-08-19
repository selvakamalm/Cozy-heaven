// src/app/manage-bookings.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-manage-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './manage-bookings.component.html',
  styleUrls: ['./manage-bookings.component.css']
})
export class ManageBookingsComponent implements OnInit {
  bookings: any[] = [];
  token = '';
  hotelId: number | null = null;
  errorMessage = '';
  selectedBooking: any = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.token = localStorage.getItem('token') || '';
    const ownerId = localStorage.getItem('ownerId');

    if (!this.token || !ownerId) {
      this.errorMessage = 'Missing token or owner ID';
      return;
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${this.token}` });

    this.http.get(`https://localhost:7298/api/Hotel/owner/${ownerId}`, { headers }).subscribe({
      next: (hotelData: any) => {
        const hotel = Array.isArray(hotelData) ? hotelData[0] : hotelData;
        this.hotelId = hotel.id;
        this.fetchBookings();
      },
      error: () => this.errorMessage = 'Failed to fetch hotel data.'
    });
  }

  fetchBookings(): void {
    if (!this.hotelId) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${this.token}` });

    this.http.get<any[]>(`https://localhost:7298/api/Booking`)
      .subscribe({
        next: bookings => {
          // Filter by hotelId
          this.bookings = bookings.filter(b => b.hotelId === this.hotelId);
        },
        error: (err) => {
          this.errorMessage = 'Failed to fetch bookings';
          console.error(err);
        }
      });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed': return 'badge bg-success';
      case 'cancelled': return 'badge bg-danger';
      case 'pending': return 'badge bg-warning text-dark';
      default: return 'badge bg-secondary';
    }
  }

  viewDetails(booking: any): void {
    const headers = new HttpHeaders({ Authorization: `Bearer ${this.token}` });

    this.http.get(`https://localhost:7298/api/User/${booking.userId}`, { headers })
      .subscribe({
        next: user => {
          this.selectedBooking = { ...booking, user };
        },
        error: (err) => {
          this.errorMessage = 'Failed to fetch user details';
          console.error(err);
        }
      });
  }

cancelBooking(bookingId: number): void {
  if (!confirm('Are you sure you want to cancel this booking?')) return;

  const headers = new HttpHeaders({ Authorization: `Bearer ${this.token}` });

  const update = {
    status: 'Cancelled'
  };

  this.http.put(`https://localhost:7298/api/Booking/${bookingId}`, update, { headers }).subscribe({
    next: () => {
      alert('Booking cancelled successfully.');
      this.fetchBookings();
      this.selectedBooking = null;
    },
    error: (err) => {
      this.errorMessage = 'Failed to cancel booking.';
      console.error(err);
    }
  });
}

}
