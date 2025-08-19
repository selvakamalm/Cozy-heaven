// src/app/room-management.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-room-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './room-management.component.html',
  styleUrls: ['./room-management.component.css']
})
export class RoomManagementComponent implements OnInit {
  rooms: any[] = [];
  hotelId: number | null = null;
  token = '';
  errorMessage = '';
  editingRoom: any = null;
  isAddingRoom = false;

  newRoom = {
    roomSize: '',
    bedType: 'Single',
    maxOccupancy: 1,
    baseFare: 1000,
    isAC: false,
    availabilities: []
  };

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
        this.fetchRooms();
      },
      error: (err) => {
        this.errorMessage = 'Failed to fetch hotel info.';
        console.error(err);
      }
    });
  }

  fetchRooms(): void {
    if (!this.hotelId) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${this.token}` });

    this.http.get<any[]>(`https://localhost:7298/api/Room/by-hotel/${this.hotelId}`, { headers }).subscribe({
      next: (data) => this.rooms = data,
      error: (err) => {
        this.errorMessage = 'Could not load rooms.';
        console.error(err);
      }
    });
  }

  submitRoom(): void {
    if (!this.hotelId) return;

    if (!this.newRoom.roomSize || this.newRoom.roomSize.trim().length < 3) {
      alert('Room Size is required.');
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.token}`,
      'Content-Type': 'application/json'
    });

    const roomData = { ...this.newRoom, hotelId: this.hotelId };

    this.http.post(`https://localhost:7298/api/Room`, roomData, { headers }).subscribe({
      next: () => {
        alert('Room added successfully!');
        this.fetchRooms();
        this.cancelAddRoom();
      },
      error: (err) => {
        this.errorMessage = 'Failed to add room.';
        console.error('❌ Add Room Error:', err);
      }
    });
  }

  cancelAddRoom(): void {
    this.isAddingRoom = false;
    this.newRoom = {
      roomSize: '',
      bedType: 'Single',
      maxOccupancy: 1,
      baseFare: 1000,
      isAC: false,
      availabilities: []
    };
  }

  startEditRoom(room: any) {
    this.editingRoom = { ...room };
  }

  cancelEdit() {
    this.editingRoom = null;
  }

  saveRoomChanges() {
    if (!this.editingRoom || !this.editingRoom.id) return;

    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.token}`,
      'Content-Type': 'application/json'
    });

    this.http.put(`https://localhost:7298/api/Room/${this.editingRoom.id}`, this.editingRoom, { headers }).subscribe({
      next: () => {
        alert('Room updated successfully!');
        this.editingRoom = null;
        this.fetchRooms();
      },
      error: (err) => {
        this.errorMessage = 'Failed to update room.';
        console.error(err);
      }
    });
  }

  deleteRoom(roomId: number): void {
    if (!confirm('Are you sure you want to delete this room?')) return;

    const headers = new HttpHeaders({ Authorization: `Bearer ${this.token}` });

    this.http.delete(`https://localhost:7298/api/Room/${roomId}`, { headers }).subscribe({
      next: () => {
        alert('Room deleted successfully!');
        this.fetchRooms();
      },
      error: (err) => {
        this.errorMessage = 'Failed to delete room.';
        console.error(err);
      }
    });
  }
}
