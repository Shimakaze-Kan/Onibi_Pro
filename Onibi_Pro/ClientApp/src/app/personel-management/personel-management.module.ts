import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { QRCodeModule } from 'angularx-qrcode';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { UtilsModule } from '../utils/utils.module';
import { RestaurantPersonelManagementComponent } from './restaurant-personel-management/restaurant-personel-management.component';
import { ManagersManagementComponent } from './managers-management/managers-management.component';
import { AddManagerComponent } from './managers-management/add-manager/add-manager.component';
import { EditManagerComponent } from './managers-management/edit-manager/edit-manager.component';
import { AddEmployeeComponent } from './restaurant-personel-management/add-employee/add-employee.component';
import { EditEmployeeComponent } from './restaurant-personel-management/edit-employee/edit-employee.component';

@NgModule({
  declarations: [
    RestaurantPersonelManagementComponent,
    ManagersManagementComponent,
    AddEmployeeComponent,
    EditEmployeeComponent,
    AddManagerComponent,
    EditManagerComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatDividerModule,
    MatListModule,
    MatIconModule,
    UtilsModule,
    MatSelectModule,
    NgxMatSelectSearchModule,
    NgxSkeletonLoaderModule,
    MatTabsModule,
    ZXingScannerModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    QRCodeModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
  ],
  exports: [RestaurantPersonelManagementComponent, ManagersManagementComponent],
})
export class PersonelManagementModule {}
