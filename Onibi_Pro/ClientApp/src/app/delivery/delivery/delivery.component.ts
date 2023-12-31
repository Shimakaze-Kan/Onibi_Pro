import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDeliveryComponent } from '../confirm-delivery/confirm-delivery.component';
import { RequestSuppliesComponent } from '../request-supplies/request-supplies.component';
import { ShowQrCodeComponent } from '../show-qr-code/show-qr-code.component';
import { PackageItem, ShipmentsClient } from '../../api/api';
import { filter, of, switchMap, tap } from 'rxjs';
import { TextHelperService } from '../../utils/services/text-helper.service';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.scss'],
})
export class DeliveryComponent implements OnInit {
  private _packageItems: Array<PackageItem> = [];
  private _expandedItemIds: Array<string> = [];
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
    private readonly textHelper: TextHelperService
  ) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.getPackages(1, this.pageSize)),
        tap(() => (this.loading = false))
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
