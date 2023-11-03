import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MessageManagerComponent } from './message-manager/message-manager.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MessageViewComponent } from './message-view/message-view.component';
import { UtilsModule } from '../utils/utils.module';
import { NewMessageComponent } from './new-message/new-message.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatSelectModule } from '@angular/material/select';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { MatTabsModule } from '@angular/material/tabs';

@NgModule({
  declarations: [
    MessageManagerComponent,
    MessageViewComponent,
    NewMessageComponent,
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
  exports: [MessageManagerComponent],
  providers: [DatePipe],
})
export class CommunicationModule {}
