import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-layout',
  imports:[CommonModule,RouterModule],
  templateUrl: './user-layout.html',
  styleUrls: ['./user-layout.css']
})
export class UserLayoutComponent {
  constructor(private router: Router) {}
  logout() {
    this.router.navigate(['/login']);
  }
}
