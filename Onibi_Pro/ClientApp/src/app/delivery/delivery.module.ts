import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
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
import { ApproveNotRestaurantOriginComponent } from './approve/approve-not-restaurant-origin/approve-not-restaurant-origin.component';
import { ApproveRestaurantOriginComponent } from './approve/approve-restaurant-origin/approve-restaurant-origin.component';
import { ConfirmDeliveryComponent } from './confirm-delivery/confirm-delivery.component';
import { DeliveryComponent } from './delivery/delivery.component';
import { RequestSuppliesComponent } from './request-supplies/request-supplies.component';
import { ShowQrCodeComponent } from './show-qr-code/show-qr-code.component';

@NgModule({
  declarations: [
    DeliveryComponent,
    ConfirmDeliveryComponent,
    RequestSuppliesComponent,
    ShowQrCodeComponent,
    ApproveRestaurantOriginComponent,
    ApproveNotRestaurantOriginComponent,
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
  exports: [DeliveryComponent],
})
export class DeliveryModule {}
