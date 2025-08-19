import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.css']
})
export class MyBookingsComponent implements OnInit {
  bookings: any[] = [];
  token = localStorage.getItem('token');
  userId = localStorage.getItem('userId');

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings() {
  if (!this.token || !this.userId) return;

  const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);

  this.http.get<any[]>(`https://localhost:7298/api/Booking/user/${this.userId}`, { headers }).subscribe({
    next: (res) => {
      this.bookings = res;

      // Optional: fetch hotel names if only hotelId is present
      this.bookings.forEach((booking, i) => {
        if (!booking.hotel && booking.hotelId) {
          this.http.get<any>(`https://localhost:7298/api/Hotel/${booking.hotelId}`, { headers }).subscribe({
            next: (hotel) => this.bookings[i].hotel = hotel,
            error: (err) => console.error('Failed to load hotel:', err)
          });
        }
      });
    },
    error: (err) => {
      console.error('Failed to fetch user bookings', err);
    }
  });
}

  cancelBooking(id: number) {
    if (!confirm('Are you sure you want to cancel this booking?')) return;

    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
    this.http.delete(`https://localhost:7298/api/Booking/${id}`, { headers }).subscribe({
      next: () => {
        alert('Booking cancelled.');
        this.loadBookings();
      },
      error: (err) => {
        console.error('Cancel failed', err);
      }
    });
  }

  viewBooking(booking: any) {
    alert(`Booking details:\nHotel: ${booking.hotel?.name}\nFare: ₹${booking.totalAmount}`);
  }

  reviewBooking(booking: any) {
    const comment = prompt('Leave a review for ' + booking.hotel?.name);
    if (!comment) return;

    const review = {
      hotelId: booking.hotelId,
      userId: this.userId,
      comment: comment,
      rating: 5
    };

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.token}`,
      'Content-Type': 'application/json'
    });

    this.http.post('https://localhost:7298/api/Review', review, { headers }).subscribe({
      next: () => alert('Review submitted!'),
      error: (err) => {
        console.error('Review failed', err);
        alert('Review could not be submitted');
      }
    });
  }
}
