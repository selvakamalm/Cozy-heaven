import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HotelProfileComponent } from './hotel-profile/hotel-profile.component';
import { RoomManagementComponent } from './room-management/room-management.component';
import { ManageBookingsComponent } from './manage-bookings/manage-bookings.component';
import { UserDashboardComponent } from './user-dashboard/user-dashboard.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { SearchHotelsComponent } from './search-hotels/search-hotels.component';
import { MyBookingsComponent } from './my-bookings/my-bookings.component';

import { LayoutComponent } from './layout/layout.component';
import { UserLayoutComponent } from './user-layout/user-layout'; //  fix file name
import { HomeComponent } from './home/home'; // ✅ Add this
import { RegisterComponent } from './register/register';

export const routes: Routes = [
  // ✅ Home Page Route
  { path: '', component: HomeComponent },  // <-- Home is now root
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // ✅ Hotel Owner Layout
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'profile', component: HotelProfileComponent },
      { path: 'rooms', component: RoomManagementComponent },
      { path: 'bookings', component: ManageBookingsComponent }
    ]
  },

  // ✅ User Layout
  {
    path: 'user',
    component: UserLayoutComponent,
    children: [
      { path: 'dashboard', component: UserDashboardComponent },
      { path: 'profile', component: UserProfileComponent },
      { path: 'search', component: SearchHotelsComponent },
      { path: 'bookings', component: MyBookingsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];
