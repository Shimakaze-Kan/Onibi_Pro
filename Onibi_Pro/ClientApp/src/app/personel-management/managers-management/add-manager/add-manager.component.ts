import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  ReplaySubject,
  Subject,
  catchError,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { CreateManagerRequest, RegionalManagersClient } from '../../../api/api';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';
import { IAddManagerData } from './IAddManagerData';

@Component({
  selector: 'app-add-manager',
  templateUrl: './add-manager.component.html',
  styleUrls: ['./add-manager.component.scss'],
})
export class AddManagerComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();
  loading = false;

  newManagerForm = new FormGroup({
    restaurant: new FormControl<string>('', Validators.required),
    email: new FormControl<string>('', Validators.required),
    firstName: new FormControl<string>('', Validators.required),
    lastName: new FormControl<string>('', Validators.required),
  });

  restaurantFilterCtrl = new FormControl<string>('');
  filteredRestaurants = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialogRef: MatDialogRef<
      AddManagerComponent,
      { reload: boolean }
    >,
    private readonly client: RegionalManagersClient,
    private readonly snackBar: MatSnackBar,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private data: IAddManagerData,
    private readonly errorParser: ErrorMessagesParserService
  ) {}

  ngOnInit(): void {
    this.filteredRestaurants.next(this.data.restaurants.slice());

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

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  createManager(): void {
    const rawValues = Object.fromEntries(
      Object.entries(this.newManagerForm.getRawValue()).map(([key, value]) => [
        key,
        value === null ? undefined : value,
      ])
    );

    const command = new CreateManagerRequest({
      email: rawValues.email,
      firstName: rawValues.firstName,
      lastName: rawValues.lastName,
      restaurantId: rawValues.restaurant,
    });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.managerPost(command)),
        tap(() => {
          this.dialogRef.close({ reload: true });
          this.loading = false;
        }),
        catchError((error) => {
          const description = this.errorParser.extractErrorMessage(
            JSON.parse(error.response)
          );
          this.snackBar.open(description, 'close', { duration: 5000 });
          this.loading = false;

          return of(error);
        })
      )
      .subscribe();
  }

  private filterRestaurants() {
    if (!this.data.restaurants) {
      return;
    }

    let search = this.restaurantFilterCtrl.value;
    if (!search) {
      this.filteredRestaurants.next(this.data.restaurants.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredRestaurants.next(
      this.data.restaurants.filter((restaurant) =>
        restaurant.toLowerCase().includes(search!)
      )
    );
  }
}
