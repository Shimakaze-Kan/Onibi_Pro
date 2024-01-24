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
  of,
  startWith,
  switchMap,
  tap,
} from 'rxjs';
import { GetCouriersResponse, RegionalManagersClient } from '../api/api';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AddCourierComponent } from './add-courier/add-courier.component';

@Component({
  selector: 'app-courier',
  templateUrl: './courier.component.html',
  styleUrls: ['./courier.component.scss'],
})
export class CourierComponent implements OnInit, OnDestroy {
  private _couriers: Array<GetCouriersResponse> = [];
  private readonly typingDebounceTimeout = 500;
  private _onDestroy$ = new Subject<void>();
  private _refresh$ = new Subject<void>();
  private _querySubject$ = new BehaviorSubject<string>('');
  filteredCouriers$ = new Observable<Array<GetCouriersResponse>>();
  numberOfCouriers = 0;
  loading = true;

  constructor(
    private readonly client: RegionalManagersClient,
    private readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.filteredCouriers$ = combineLatest([
      this._refresh$,
      this._querySubject$.pipe(
        debounceTime(this.typingDebounceTimeout),
        distinctUntilChanged()
      ),
    ]).pipe(
      map(([_, value]) => value || ''),
      map((query) => {
        const searchQuery = query.toLowerCase();
        return this._couriers.filter(
          (x) =>
            x.phone?.includes(searchQuery) || x.email?.includes(searchQuery)
        );
      }),
      tap((result) => {
        this.numberOfCouriers = result?.length || 0;
      })
    );

    this.getCouriers().subscribe();
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  handleOnChange = (event: Event) => {
    const value = (event.target as HTMLInputElement).value;
    this._querySubject$.next(value);
  };

  openAddCourierDialog(): void {
    const dialogRef = this.dialog.open(AddCourierComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '750px',
    });
    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        switchMap(() => this.getCouriers()),
        tap(() => {
          this.snackBar.open('Courier created successfully.', 'close', {
            duration: 5000,
          });
        })
      )
      .subscribe();
  }

  private getCouriers() {
    return of({}).pipe(
      tap(() => (this.loading = true)),
      switchMap(() => this.client.courierGet()),
      tap((result) => {
        this._couriers = result;
        this._refresh$.next();
        this.loading = false;
      })
    );
  }
}
