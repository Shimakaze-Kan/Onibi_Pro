import { BrowserModule } from '@angular/platform-browser';
import { APP_ID, NgModule } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Route, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import {
  FloatLabelType,
  MatFormFieldModule,
} from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { ScheduleComponent } from './schedule/schedule.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTableModule } from '@angular/material/table';
import { PersonelManagementComponent } from './personel-management/personel-management.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { TakeSpaceDirective } from './directives/take-space.directive';
import { WelcomeComponent } from './welcome/welcome.component';
import { AddEmployeeComponent } from './personel-management/add-employee/add-employee.component';
import {
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { CommunicationModule } from './communication/communication.module';
import { UtilsModule } from './utils/utils.module';

const MATERIAL_MODULES = [
  MatButtonModule,
  MatCheckboxModule,
  MatIconModule,
  MatDividerModule,
  MatRadioModule,
  MatFormFieldModule,
  MatInputModule,
  MatSelectModule,
  MatListModule,
  MatMenuModule,
  MatButtonToggleModule,
  MatTableModule,
  MatPaginatorModule,
  NgxMatSelectSearchModule,
  MatDialogModule,
];

const ROUTES: Array<Route> = [
  {
    path: '',
    component: WelcomeComponent /*HomeComponent*/,
    pathMatch: 'full',
  },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'schedule', component: ScheduleComponent },
  { path: 'personel-management', component: PersonelManagementComponent },
  { path: 'welcome', component: WelcomeComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ScheduleComponent,
    PersonelManagementComponent,
    TakeSpaceDirective,
    WelcomeComponent,
    AddEmployeeComponent,
  ],
  imports: [
    BrowserModule, //.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(ROUTES),
    FormsModule,
    ReactiveFormsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    ...MATERIAL_MODULES,
    CommunicationModule,
    UtilsModule,
  ],
  providers: [{ provide: APP_ID, useValue: 'serverApp' }],
  bootstrap: [AppComponent],
})
export class AppModule {}
