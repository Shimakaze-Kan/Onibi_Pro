import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  AcceptPackageRequest,
  GetCouriersResponse,
  GetRegionalManagerDetailsResponse,
  RegionalManagersClient,
  RestaurantsClient,
  ShipmentsClient,
} from '../../../api/api';
import {
  ReplaySubject,
  Subject,
  catchError,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-approve-restaurant-origin',
  templateUrl: './approve-restaurant-origin.component.html',
  styleUrls: ['./approve-restaurant-origin.component.scss'],
})
export class ApproveRestaurantOriginComponent implements OnInit, OnDestroy {
  @Input({ required: true }) packageId!: string;
  @Input({ required: true })
  regionalManager!: GetRegionalManagerDetailsResponse;
  private _onDestroy$ = new Subject<void>();
  couriers: Array<GetCouriersResponse> = [];
  restaurantIds: Array<string> = [];
  loading = false;

  restaurantIdFilterCtrl = new FormControl<string>('');
  filteredRestaurantIds = new ReplaySubject<string[]>(1);

  courierFilterCtrl = new FormControl<string>('');
  filteredCouriers = new ReplaySubject<GetCouriersResponse[]>(1);

  approveForm = new FormGroup({
    restaurantId: new FormControl<string>('', Validators.required),
    courierId: new FormControl<string>('', Validators.required),
  });

  get currentlySelectedCourier(): GetCouriersResponse | undefined {
    return this.couriers.find(
      (x) => x.courierId === this.approveForm.controls.courierId.value
    );
  }

  constructor(
    private readonly regionalManagersClient: RegionalManagersClient,
    private readonly shipmentClient: ShipmentsClient,
    private readonly restaurantClient: RestaurantsClient,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.restaurantIds = this.regionalManager.restaurantIds ?? [];
    this.filteredRestaurantIds.next(this.restaurantIds.slice());

    this.restaurantIdFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterRestaurantIds();
      });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.regionalManagersClient.courierGet()),
        tap((result) => {
          this.couriers = result;
          this.filteredCouriers.next(this.couriers.slice());
          this.loading = false;
        })
      )
      .subscribe();

    this.courierFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterCouriers();
      });
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  approve(): void {
    const rawValues = this.approveForm.getRawValue();
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.restaurantClient.address(rawValues.restaurantId!)),
        switchMap((address) => {
          const request = new AcceptPackageRequest({
            sourceRestaurantId: rawValues.restaurantId!,
            origin: address,
            courierId: rawValues.courierId!,
          });
          return this.shipmentClient.approveShipment(this.packageId, request);
        }),
        tap(() => (this.loading = false)),
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

  private filterRestaurantIds() {
    if (!this.restaurantIds) {
      return;
    }

    let search = this.restaurantIdFilterCtrl.value;
    if (!search) {
      this.filteredRestaurantIds.next(this.restaurantIds.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredRestaurantIds.next(
      this.restaurantIds.filter((restaurantId) =>
        restaurantId.toLowerCase().includes(search!)
      )
    );
  }

  private filterCouriers() {
    if (!this.couriers) {
      return;
    }

    let search = this.courierFilterCtrl.value;
    if (!search) {
      this.filteredCouriers.next(this.couriers.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredCouriers.next(
      this.couriers.filter((courier) =>
        courier.email?.toLowerCase().includes(search!)
      )
    );
  }
}
