import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { APP_ID, NgModule, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import {
  ActivatedRouteSnapshot,
  CanActivateChildFn,
  CanActivateFn,
  Route,
  RouterModule,
  RouterStateSnapshot,
} from '@angular/router';

import { OverlayModule } from '@angular/cdk/overlay';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { Observable } from 'rxjs';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { PermissionChecker } from './auth/permission-checker.service';
import { CommunicationModule } from './communication/communication.module';
import { DeliveryModule } from './delivery/delivery.module';
import { DeliveryComponent } from './delivery/delivery/delivery.component';
import { RequiredStarDirective } from './directives/required-star.directive';
import { TakeSpaceDirective } from './directives/take-space.directive';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { OrderManagerComponent } from './orders/order-manager/order-manager.component';
import { OrdersModule } from './orders/orders.module';
import { ManagersManagementComponent } from './personel-management/managers-management/managers-management.component';
import { PersonelManagementModule } from './personel-management/personel-management.module';
import { RestaurantPersonelManagementComponent } from './personel-management/restaurant-personel-management/restaurant-personel-management.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { UserTypes } from './utils/UserTypes';
import { UtilsModule } from './utils/utils.module';
import { WelcomeComponent } from './welcome/welcome.component';
import { RegionalmanagersManagementComponent } from './personel-management/regionalmanagers-management/regionalmanagers-management.component';
import { RestaurantComponent } from './restaurant/restaurant/restaurant.component';
import { RestaurantModule } from './restaurant/restaurant.module';

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
        path: 'restaurant-personel-management',
        component: RestaurantPersonelManagementComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.manager] },
      },
      {
        path: 'manager-management',
        component: ManagersManagementComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.regionalManager] },
      },
      {
        path: 'regionalmanager-management',
        component: RegionalmanagersManagementComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.globalManager] },
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
      {
        path: 'restaurant',
        component: RestaurantComponent,
        canActivate: [canActivateUserTypes],
        data: { userTypes: [UserTypes.globalManager] },
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
    TakeSpaceDirective,
    WelcomeComponent,
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
    PersonelManagementModule,
    RestaurantModule,
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
