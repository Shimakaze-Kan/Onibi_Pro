import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import {
  ReplaySubject,
  Subject,
  filter,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import {
  GetManagersResponse,
  IdentityClient,
  RegionalManagersClient,
} from '../../api/api';
import { IdentityService } from '../../utils/services/identity.service';
import { IAddManagerData } from './add-manager/IAddManagerData';
import { AddManagerComponent } from './add-manager/add-manager.component';
import { IEditManagerData } from './edit-manager/IEditManagerData';
import { EditManagerComponent } from './edit-manager/edit-manager.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-managers-management',
  templateUrl: './managers-management.component.html',
  styleUrls: ['./managers-management.component.scss'],
})
export class ManagersManagementComponent implements OnInit, OnDestroy {
  private _onDestroy$ = new Subject<void>();

  restaurants: Array<string> = [];
  positions = ['Manager', 'Courier'];
  loading = false;
  displayedColumns = [
    'managerId',
    'restaurantId',
    'firstName',
    'lastName',
    'email',
    'action',
  ];

  dataSource = new MatTableDataSource<GetManagersResponse>();

  @ViewChild(MatPaginator, { static: false })
  set paginator(value: MatPaginator) {
    if (this.dataSource) {
      this.dataSource.paginator = value;
    }
  }

  managerSearchForm = new FormGroup({
    email: new FormControl<string>(''),
    firstName: new FormControl<string>(''),
    lastName: new FormControl<string>(''),
    restaurant: new FormControl<string>(''),
    managerId: new FormControl<string>(''),
    position: new FormControl<string>(''),
  });

  restaurantFilterCtrl = new FormControl<string>('');
  filteredRestaurants = new ReplaySubject<string[]>(1);

  constructor(
    private readonly regionalManagerClient: RegionalManagersClient,
    private readonly identityClient: IdentityClient,
    private readonly identityService: IdentityService,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.identityService.getUserData()),
        switchMap((userData) =>
          this.identityClient.regionalManagerDetails(userData.userId!)
        ),
        tap((result) => {
          this.restaurants = result.restaurantIds || [];
          this.filteredRestaurants.next(this.restaurants.slice());
          this.loading = false;
          this.onSubmit();
        })
      )
      .subscribe();

    this.restaurantFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterRestaurants();
      });
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  reset(): void {
    this.managerSearchForm.reset();
  }

  onSubmit(): void {
    const rawValues = Object.fromEntries(
      Object.entries(this.managerSearchForm.getRawValue()).map(
        ([key, value]) => [key, value === null ? undefined : value]
      )
    );

    of({})
      .pipe(
        tap(() => (this.loading = false)),
        switchMap(() =>
          this.regionalManagerClient.managerGet(
            rawValues.restaurantId,
            rawValues.managerId,
            rawValues.firstName,
            rawValues.lastName,
            rawValues.email
          )
        ),
        tap((result) => {
          this.dataSource.data = result;
          this.loading = false;
        })
      )
      .subscribe();
  }

  openAddManagerDialog() {
    const dialogRef = this.dialog.open(AddManagerComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '770px',
      data: { restaurants: this.restaurants } as IAddManagerData,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        tap((_) => this.onSubmit()),
        tap(() =>
          this.snackBar.open('Manager created successfully.', 'close', {
            duration: 5000,
          })
        )
      )
      .subscribe();
  }

  openEditManagerDialog(manager: GetManagersResponse) {
    const dialogRef = this.dialog.open(EditManagerComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '770px',
      data: {
        restaurants: this.restaurants,
        manager: manager,
      } as IEditManagerData,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        tap((_) => this.onSubmit()),
        tap(() =>
          this.snackBar.open('Manager edited successfully.', 'close', {
            duration: 5000,
          })
        )
      )
      .subscribe();
  }

  private filterRestaurants() {
    if (!this.restaurants) {
      return;
    }

    let search = this.restaurantFilterCtrl.value;
    if (!search) {
      this.filteredRestaurants.next(this.restaurants.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredRestaurants.next(
      this.restaurants.filter((restaurant) =>
        restaurant.toLowerCase().includes(search!)
      )
    );
  }
}
