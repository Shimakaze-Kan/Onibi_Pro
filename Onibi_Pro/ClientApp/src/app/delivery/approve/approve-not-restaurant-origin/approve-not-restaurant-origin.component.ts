import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
  AcceptPackageRequest,
  Address,
  GetCouriersResponse,
  RegionalManagersClient,
  ShipmentsClient,
} from '../../../api/api';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';

@Component({
  selector: 'app-approve-not-restaurant-origin',
  templateUrl: './approve-not-restaurant-origin.component.html',
  styleUrls: ['./approve-not-restaurant-origin.component.scss'],
})
export class ApproveNotRestaurantOriginComponent implements OnInit, OnDestroy {
  @Input({ required: true }) packageId!: string;
  @Output() udpatedStatus = new EventEmitter<void>();
  private _onDestroy$ = new Subject<void>();
  couriers: Array<GetCouriersResponse> = [];
  loading = false;

  restaurantIdFilterCtrl = new FormControl<string>('');
  filteredRestaurantIds = new ReplaySubject<string[]>(1);

  courierFilterCtrl = new FormControl<string>('');
  filteredCouriers = new ReplaySubject<GetCouriersResponse[]>(1);

  approveForm = new FormGroup({
    street: new FormControl<string>('', Validators.required),
    city: new FormControl<string>('', Validators.required),
    postalCode: new FormControl<string>('', Validators.required),
    country: new FormControl<string>('', Validators.required),
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
    private readonly errorParser: ErrorMessagesParserService,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
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
    const rawValues = Object.fromEntries(
      Object.entries(this.approveForm.getRawValue()).map(([k, v], _) => [
        k,
        v || undefined,
      ])
    );
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => {
          const request = new AcceptPackageRequest({
            origin: new Address({
              city: rawValues.city,
              country: rawValues.country,
              postalCode: rawValues.postalCode,
              street: rawValues.street,
            }),
            courierId: rawValues.courierId!,
          });
          return this.shipmentClient.approveShipment(this.packageId, request);
        }),
        tap(() => {
          this.loading = false;
          this.udpatedStatus.next();
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
