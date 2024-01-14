import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  Address,
  CreateRestaurantRequest,
  GetRegionalManagerResponse_RegionalManagerItem,
  RegionalManagersClient,
  RestaurantsClient,
} from '../../../api/api';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';
import {
  ReplaySubject,
  Subject,
  catchError,
  debounceTime,
  distinctUntilChanged,
  of,
  startWith,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';

@Component({
  selector: 'app-add-restaurant',
  templateUrl: './add-restaurant.component.html',
  styleUrls: ['./add-restaurant.component.scss'],
})
export class AddRestaurantComponent implements OnInit, OnDestroy {
  private readonly typingDebounceTimeout = 500;
  private readonly _onDestroy$ = new Subject<void>();
  loading = false;

  newRestaurantForm = new FormGroup({
    street: new FormControl<string>('', Validators.required),
    city: new FormControl<string>('', Validators.required),
    postalCode: new FormControl<string>('', Validators.required),
    country: new FormControl<string>('', Validators.required),
    regionalManager:
      new FormControl<GetRegionalManagerResponse_RegionalManagerItem>(
        undefined!,
        Validators.required
      ),
  });

  regionalManagerFilterCtrl = new FormControl<string>('');
  filteredRegionalManagers = new ReplaySubject<
    GetRegionalManagerResponse_RegionalManagerItem[]
  >(1);

  get selectedRegionalManager(): GetRegionalManagerResponse_RegionalManagerItem | null {
    return this.newRestaurantForm.controls.regionalManager.value;
  }

  constructor(
    private readonly dialogRef: MatDialogRef<
      AddRestaurantComponent,
      { reload: boolean }
    >,
    private readonly restaurantClient: RestaurantsClient,
    private readonly regionalManagerClient: RegionalManagersClient,
    private readonly snackBar: MatSnackBar,
    private readonly errorParser: ErrorMessagesParserService
  ) {}

  ngOnInit(): void {
    this.regionalManagerFilterCtrl.valueChanges
      .pipe(
        startWith(''),
        takeUntil(this._onDestroy$),
        debounceTime(this.typingDebounceTimeout),
        distinctUntilChanged(),
        switchMap((value) =>
          this.regionalManagerClient.regionalManagersGet(
            1,
            10,
            undefined,
            undefined,
            undefined,
            value || '',
            undefined
          )
        ),
        tap((result) => {
          const regionalManagers = result.regionalManagers?.slice() || [];
          const selectedRegionalManager =
            this.newRestaurantForm.controls.regionalManager.value;
          const combinedRegionalManagers = [
            selectedRegionalManager,
            ...regionalManagers,
          ].flatMap((r) => (!r ? [] : [r]));

          const regionalManagerOptions = [
            ...new Map(
              combinedRegionalManagers.map((r) => [r?.regionalManagerId, r])
            ).values(),
          ];

          this.filteredRegionalManagers.next(regionalManagerOptions);
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  createRestaurant(): void {
    const rawValues = this.newRestaurantForm.getRawValue();
    const address = new Address({
      city: rawValues.city || '',
      country: rawValues.city || '',
      postalCode: rawValues.postalCode || '',
      street: rawValues.street || '',
    });
    const request = new CreateRestaurantRequest({
      address: address,
      regionalManagerId: rawValues.regionalManager?.regionalManagerId,
    });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.restaurantClient.restaurantsPost(request)),
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
}
