import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ReplaySubject, Subject, of, switchMap, takeUntil, tap } from 'rxjs';
import {
  GetRegionalManagerResponse,
  RegionalManagersClient,
  RestaurantsClient,
} from '../../api/api';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-regionalmanagers-management',
  templateUrl: './regionalmanagers-management.component.html',
  styleUrls: ['./regionalmanagers-management.component.scss'],
})
export class RegionalmanagersManagementComponent implements OnInit, OnDestroy {
  private _onDestroy$ = new Subject<void>();

  loading = false;
  restaurants: Array<string> = [];
  totalRecords = 0;
  pageSize = 10;
  pageIndex = 0;

  displayedColumns = [
    'regionalManagerId',
    'restaurantId',
    'firstName',
    'lastName',
    'email',
    'numberOfManagers',
    'action',
  ];

  regionalManagerSearchForm = new FormGroup({
    email: new FormControl<string>(''),
    firstName: new FormControl<string>(''),
    lastName: new FormControl<string>(''),
    restaurant: new FormControl<string>(''),
    regionalManagerId: new FormControl<string>(''),
  });

  restaurantFilterCtrl = new FormControl<string>('');
  filteredRestaurants = new ReplaySubject<string[]>(1);
  dataSource = new MatTableDataSource<GetRegionalManagerResponse>();

  constructor(
    private readonly restaurantClient: RestaurantsClient,
    private readonly regionalManagersClient: RegionalManagersClient
  ) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.getRegionalManagers()),
        switchMap(() => this.restaurantClient.restaurantsGet()),
        tap((result) => {
          this.restaurants = result;
          this.filteredRestaurants.next(this.restaurants.slice());
          this.loading = false;
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
    this.regionalManagerSearchForm.reset();
  }

  onSubmit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.getRegionalManagers()),
        tap(() => (this.loading = false))
      )
      .subscribe();
  }

  pageChangeEvent(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;

    this.onSubmit();
  }

  private getRegionalManagers() {
    const rawValues = Object.fromEntries(
      Object.entries(this.regionalManagerSearchForm.getRawValue()).map(
        ([key, value]) => [key, value === null ? undefined : value]
      )
    );

    return of({}).pipe(
      switchMap(() =>
        this.regionalManagersClient.regionalManagers(
          this.pageIndex + 1,
          this.pageSize,
          rawValues.regionalManagerId,
          rawValues.firstName,
          rawValues.lastName,
          rawValues.email,

          rawValues.restaurant
        )
      ),
      tap((result) => {
        this.dataSource.data = result.regionalManagers || [];
        this.totalRecords = result.totalRecords || 0;
      })
    );
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
