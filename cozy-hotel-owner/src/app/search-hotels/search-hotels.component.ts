import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';

interface Hotel {
  id: number;
  name: string;
  city: string;
  address: string;
  facilities: string[];
}

@Component({
  selector: 'app-search-hotels',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search-hotels.component.html',
  styleUrls: ['./search-hotels.component.css']
})
export class SearchHotelsComponent {
  location = '';
  fromDate = '';
  toDate = '';
  roomCount = 1;
  filteredHotels: Hotel[] = [];
  selectedHotel: Hotel | null = null;

  bookingDetails = {
    userId: Number(localStorage.getItem('userId')),
    hotelId: 0,
    roomId: 0,
    checkInDate: '',
    checkOutDate: '',
    numberOfGuests: 1,
    totalAmount: 0,
    status: 'Confirmed'
  };

  constructor(private http: HttpClient) {}

  searchHotels(): void {
    if (!this.location || !this.fromDate || !this.toDate) {
      alert('Please fill in all fields');
      return;
    }

    const checkIn = new Date(this.fromDate);
    const checkOut = new Date(this.toDate);
    if (checkOut <= checkIn) {
      alert('Check-out must be after check-in date');
      return;
    }

    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<Hotel[]>(`https://localhost:7298/api/Hotel/location?location=${this.location}`, { headers })
      .subscribe({
        next: (res) => this.filteredHotels = res,
        error: (err) => {
          console.error('Search error:', err);
          alert('Failed to search hotels');
        }
      });
  }

  selectHotel(hotel: Hotel) {
    this.selectedHotel = hotel;
    this.bookingDetails.hotelId = hotel.id;
  }

  bookHotel() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    this.http.post('https://localhost:7298/api/Booking', this.bookingDetails, { headers }).subscribe({
      next: () => {
        alert('Booking successful!');
        this.selectedHotel = null;
        this.bookingDetails = {
          userId: Number(localStorage.getItem('userId')),
          hotelId: 0,
          roomId: 0,
          checkInDate: '',
          checkOutDate: '',
          numberOfGuests: 1,
          totalAmount: 0,
          status: 'Confirmed'
        };
      },
      error: (err) => {
        console.error('Booking error:', err);
        alert('Failed to book hotel');
      }
    });
  }
}
