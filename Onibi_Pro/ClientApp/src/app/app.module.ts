import { BrowserModule } from '@angular/platform-browser';
import { APP_ID, NgModule, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import {
  ActivatedRouteSnapshot,
  CanActivateChildFn,
  CanActivateFn,
  Route,
  RouterModule,
  RouterStateSnapshot,
} from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
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
import { MatDialogModule } from '@angular/material/dialog';
import { CommunicationModule } from './communication/communication.module';
import { UtilsModule } from './utils/utils.module';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { DeliveryModule } from './delivery/delivery.module';
import { DeliveryComponent } from './delivery/delivery/delivery.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatBadgeModule } from '@angular/material/badge';
import { EditEmployeeComponent } from './personel-management/edit-employee/edit-employee.component';
import { OrdersModule } from './orders/orders.module';
import { PermissionChecker } from './auth/permission-checker.service';
import { AuthInterceptor } from './auth/auth.interceptor';
import { Observable } from 'rxjs';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RequiredStarDirective } from './directives/required-star.directive';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { OrderManagerComponent } from './orders/order-manager/order-manager.component';
import { UserTypes } from './utils/UserTypes';

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
  MatBadgeModule,
  MatProgressBarModule,
  MatSnackBarModule,
  MatProgressSpinnerModule,
  MatDatepickerModule,
  MatNativeDateModule,
];

const canActivateAnything: CanActivateChildFn = (
  _route: ActivatedRouteSnapshot,
  _state: RouterStateSnapshot
): Observable<boolean> => {
  return inject(PermissionChecker).canActivateAnything();
};

const canActivateUserTypes: CanActivateFn = (
  next: ActivatedRouteSnapshot,
  _state: RouterStateSnapshot
): Observable<boolean> => {
  return inject(PermissionChecker).canActivateUserTypes(next);
};

export const ROUTES: Array<Route> = [
  {
    path: '',
    canActivateChild: [canActivateAnything],
    children: [
      { path: '', redirectTo: 'welcome', pathMatch: 'full' },
      { path: 'welcome', component: WelcomeComponent },
      {
        path: 'schedule',
        component: ScheduleComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.manager] },
      },
      {
        path: 'personel-management',
        component: PersonelManagementComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.manager] },
      },
      {
        path: 'delivery',
        component: DeliveryComponent,
        canActivate: [canActivateUserTypes],
        data: {
          userTypes: [
            UserTypes.manager,
            UserTypes.regionalManager,
            UserTypes.courier,
          ],
        },
      },
      {
        path: 'order',
        component: OrderManagerComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.manager] },
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ScheduleComponent,
    PersonelManagementComponent,
    TakeSpaceDirective,
    WelcomeComponent,
    AddEmployeeComponent,
    EditEmployeeComponent,
    RequiredStarDirective,
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
    NgxSkeletonLoaderModule,
    DeliveryModule,
    OverlayModule,
    OrdersModule,
  ],
  providers: [
    { provide: APP_ID, useValue: 'serverApp' },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
