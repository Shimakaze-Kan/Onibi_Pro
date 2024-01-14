import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  BehaviorSubject,
  Observable,
  Subject,
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  filter,
  map,
  merge,
  startWith,
  switchMap,
  tap,
} from 'rxjs';
import { GetRestaurantsResponse, RestaurantsClient } from '../../api/api';
import { MatDialog } from '@angular/material/dialog';
import { AddRestaurantComponent } from './add-restaurant/add-restaurant.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-restaurant',
  templateUrl: './restaurant.component.html',
  styleUrls: ['./restaurant.component.scss'],
})
export class RestaurantComponent implements OnInit, OnDestroy {
  private readonly typingDebounceTimeout = 500;
  private _onDestroy$ = new Subject<void>();
  private _refresh$ = new Subject<void>();
  private _querySubject$ = new BehaviorSubject<string>('');
  filteredRestaurants$ = new Observable<Array<GetRestaurantsResponse>>();
  numberOfRestaurants = 0;
  loading = true;

  constructor(
    private readonly client: RestaurantsClient,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.filteredRestaurants$ = combineLatest([
      this._refresh$.pipe(startWith(undefined)),
      this._querySubject$.pipe(
        debounceTime(this.typingDebounceTimeout),
        distinctUntilChanged()
      ),
    ]).pipe(
      map(([_, value]) => value || ''),
      tap(() => (this.loading = true)),
      switchMap((query) => this.client.restaurantsGet(query)),
      tap((result) => {
        this.loading = false;
        this.numberOfRestaurants = result?.length || 0;
      })
    );
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  handleOnChange = (event: Event) => {
    const value = (event.target as HTMLInputElement).value;
    this._querySubject$.next(value);
  };

  openAddRestaurantDialog(): void {
    const dialogRef = this.dialog.open(AddRestaurantComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '750px',
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        tap(() => {
          this._refresh$.next();
          this.snackBar.open('Restaurant created successfully.', 'close', {
            duration: 5000,
          });
        })
      )
      .subscribe();
  }
}
