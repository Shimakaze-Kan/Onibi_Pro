import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { UtilsModule } from '../utils/utils.module';
import { MessageManagerComponent } from './message-manager/message-manager.component';
import { MessageViewComponent } from './message-view/message-view.component';
import { NewMessageComponent } from './new-message/new-message.component';
import { NotificationManagerComponent } from './notification-manager/notification-manager.component';

@NgModule({
  declarations: [
    MessageManagerComponent,
    MessageViewComponent,
    NewMessageComponent,
    NotificationManagerComponent,
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
  ],
  exports: [MessageManagerComponent, NotificationManagerComponent],
  providers: [DatePipe],
})
export class CommunicationModule {}
