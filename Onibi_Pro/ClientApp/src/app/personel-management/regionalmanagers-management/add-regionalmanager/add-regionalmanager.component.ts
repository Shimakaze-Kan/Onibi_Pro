import { Component, OnDestroy, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
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
  CreateRegionalManagerRequest,
  RegionalManagersClient,
} from '../../../api/api';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';

@Component({
  selector: 'app-add-regionalmanager',
  templateUrl: './add-regionalmanager.component.html',
  styleUrls: ['./add-regionalmanager.component.scss'],
})
export class AddRegionalmanagerComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();
  loading = false;
  restaurants: Array<string> = [];

  newRegionalManagerForm = new FormGroup({
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
    @Inject(MAT_DIALOG_DATA) private data: IAddRegionalManagerData,
    private readonly errorParser: ErrorMessagesParserService
  ) {
    this.restaurants = data.restaurants;
  }

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

  createRegionalManager(): void {
    const rawValues = this.newRegionalManagerForm.getRawValue();

    const request = new CreateRegionalManagerRequest({
      email: rawValues.email || '',
      firstName: rawValues.firstName || '',
      lastName: rawValues.lastName || '',
      restaurantIds: rawValues.restaurants || [],
    });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.regionalManagersPost(request)),
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
