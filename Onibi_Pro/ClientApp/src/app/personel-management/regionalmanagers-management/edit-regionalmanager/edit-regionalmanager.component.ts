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
import {
  RegionalManagersClient,
  UpdateRegionalManagerRequest,
} from '../../../api/api';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';
import { AddRegionalmanagerComponent } from '../add-regionalmanager/add-regionalmanager.component';
import { IEditRegionalManagerData } from './IEditRegionalManagerData';

@Component({
  selector: 'app-edit-regionalmanager',
  templateUrl: './edit-regionalmanager.component.html',
  styleUrls: ['./edit-regionalmanager.component.scss'],
})
export class EditRegionalmanagerComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();
  loading = false;
  restaurants: Array<string> = [];

  editRegionalManagerForm = new FormGroup({
    restaurants: new FormControl<Array<string>>([], Validators.required),
    email: new FormControl<string>('', Validators.required),
    firstName: new FormControl<string>('', Validators.required),
    lastName: new FormControl<string>('', Validators.required),
  });

  restaurantFilterCtrl = new FormControl<string>('');
  filteredRestaurants = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialogRef: MatDialogRef<
      AddRegionalmanagerComponent,
      { reload: boolean }
    >,
    private readonly client: RegionalManagersClient,
    private readonly snackBar: MatSnackBar,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private data: IEditRegionalManagerData,
    private readonly errorParser: ErrorMessagesParserService
  ) {
    this.restaurants = data.restaurants;
  }

  ngOnInit(): void {
    this.editRegionalManagerForm.setValue({
      restaurants: this.data.regionalManager.restaurantIds || [],
      email: this.data.regionalManager.email || null,
      firstName: this.data.regionalManager.firstName || null,
      lastName: this.data.regionalManager.lastName || null,
    });

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

  editRegionalManager(): void {
    const rawValues = this.editRegionalManagerForm.getRawValue();

    const request = new UpdateRegionalManagerRequest({
      regionalManagerId: this.data.regionalManager.regionalManagerId,
      email: rawValues.email || '',
      firstName: rawValues.firstName || '',
      lastName: rawValues.lastName || '',
      restaurantIds: rawValues.restaurants || [],
    });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.regionalManagersPut(request)),
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
      this.data.restaurants.filter((restaurant: string) =>
        restaurant.toLowerCase().includes(search!)
      )
    );
  }
}
