import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDeliveryComponent } from '../confirm-delivery/confirm-delivery.component';
import { RequestSuppliesComponent } from '../request-supplies/request-supplies.component';
import { ShowQrCodeComponent } from '../show-qr-code/show-qr-code.component';
import {
  GetManagerDetailsResponse,
  GetRegionalManagerDetailsResponse,
  IdentityClient,
  PackageItem,
  ShipmentsClient,
} from '../../api/api';
import {
  Subject,
  catchError,
  filter,
  forkJoin,
  map,
  mergeMap,
  of,
  switchMap,
  tap,
} from 'rxjs';
import { TextHelperService } from '../../utils/services/text-helper.service';
import { IdentityService } from '../../utils/services/identity.service';
import { UserTypes } from '../../utils/UserTypes';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.scss'],
})
export class DeliveryComponent implements OnInit {
  private _packageItems: Array<PackageItem> = [];
  private _expandedItemIds: Array<string> = [];
  regionalManagerDetails$ = new Subject<GetRegionalManagerDetailsResponse>();
  managerDetails$ = new Subject<GetManagerDetailsResponse>();
  shipmentStatus = ShipmentStatus;
  isRegionalManager = false;
  loadingUserType = false;
  loading = false;
  totalRecords = 0;
  pageSize = 10;
  pageIndex = 0;

  dataSource = new MatTableDataSource<PackageItem>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  displayedColumns = [
    'packageId',
    'originAddress',
    'destinationAddress',
    'status',
    'until',
    'isUrgent',
    'action',
    'expand',
  ];

  constructor(
    private readonly dialog: MatDialog,
    private readonly shipmentClient: ShipmentsClient,
    private readonly identityService: IdentityService,
    private readonly textHelper: TextHelperService,
    private readonly identityClient: IdentityClient,
    private readonly snackBar: MatSnackBar,
    private readonly errorParser: ErrorMessagesParserService
  ) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => {
          this.loading = true;
          this.loadingUserType = true;
        }),
        mergeMap(() =>
          forkJoin({
            packages: this.getPackages(1, this.pageSize),
            identity: this.identityService.getUserData(),
          })
        ),
        map(({ identity }) => {
          this.isRegionalManager =
            identity.userType === UserTypes.regionalManager;
          return {
            isRegionalManager: this.isRegionalManager,
            userId: identity.userId,
          };
        }),
        switchMap(({ isRegionalManager, userId }) => {
          if (isRegionalManager) {
            return this.identityClient
              .regionalManagerDetails(userId!)
              .pipe(tap((result) => this.regionalManagerDetails$.next(result)));
          } else {
            return this.identityClient
              .managerDetails(userId!)
              .pipe(tap((result) => this.managerDetails$.next(result)));
          }
        }),
        tap(() => {
          this.loading = false;
          this.loadingUserType = false;
        })
      )
      .subscribe();
  }

  openConfirmDeliveryDialog() {
    this.dialog.open(ConfirmDeliveryComponent, {
      maxWidth: '750px',
      width: '600px',
    });
  }

  openRequestSuppliesDialog() {
    const dialogRef = this.dialog.open(RequestSuppliesComponent, {
      maxWidth: '750px',
      width: '750px',
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(
          (result): result is { reload: boolean } => !!result && result.reload
        ),
        tap(() => (this.loading = true)),
        switchMap(() => this.getPackages(this.getStartRow(), this.pageSize)),
        tap(() => (this.loading = false))
      )
      .subscribe();
  }

  openShowQrCodeDialog(code: string) {
    this.dialog.open(ShowQrCodeComponent, {
      data: { code: code },
    });
  }

  pageChangeEvent(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.getPackages(this.getStartRow(), this.pageSize)),
        tap(() => (this.loading = false))
      )
      .subscribe();
  }

  expandOrHideItemDetails(id: string): void {
    if (this._expandedItemIds.includes(id)) {
      this._expandedItemIds = this._expandedItemIds.filter(
        (item) => item !== id
      );
    } else {
      this._expandedItemIds.push(id);
    }
  }

  isExpandedItem(id: string): boolean {
    return this._expandedItemIds.includes(id);
  }

  containsNextStatus(
    availableTransitions: Array<string>,
    nextStatus: ShipmentStatus
  ): boolean {
    return availableTransitions.includes(nextStatus);
  }

  updateRow(packageId: string) {
    this.replaceRow(packageId).subscribe();
  }

  rejectPackage(packageId: string): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.shipmentClient.rejectShipment(packageId)),
        switchMap(() => this.replaceRow(packageId)),
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

  private replaceRow(packageId: string) {
    return of({}).pipe(
      tap(() => (this.loading = true)),
      switchMap(() => this.shipmentClient.id(packageId)),
      tap((result) => {
        const indexOfPackage = this._packageItems.findIndex(
          (x) => x.packageId === result.packageId
        );
        this._packageItems[indexOfPackage] = result;
        this.dataSource.data = this._packageItems;
        this.snackBar.open(
          'Updated the status of the package successfully.',
          'close',
          { duration: 5000 }
        );
        this.loading = false;
      })
    );
  }

  private getPackages(
    startRow: number | undefined,
    amount: number | undefined
  ) {
    return this.shipmentClient.shipmentsGet(startRow, amount).pipe(
      tap((result) => {
        this._packageItems = result.packages ?? [];

        for (const item of this._packageItems) {
          item.status = this.textHelper.splitCamelCaseToString(item.status);
        }

        this.totalRecords = result.total ?? 0;
        this.dataSource.data = this._packageItems;
      })
    );
  }

  private getStartRow(): number {
    return 1 + this.pageIndex * this.pageSize;
  }
}

enum ShipmentStatus {
  PendingRegionalManagerApproval = 'PendingRegionalManagerApproval',
  PendingRestaurantManagerApproval = 'PendingRestaurantManagerApproval',
  ApprovedToPickupWarehouse = 'ApprovedToPickupWarehouse',
  ApprovedToPickupFromRestaurant = 'ApprovedToPickupFromRestaurant',
  CourierPickedUp = 'CourierPickedUp',
  Delivered = 'Delivered',
  Rejected = 'Rejected',
}
