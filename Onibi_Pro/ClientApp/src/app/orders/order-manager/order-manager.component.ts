import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { filter, forkJoin, map, mergeMap, of, switchMap, tap } from 'rxjs';
import {
  GetOrdersResponse_Order,
  IdentityClient,
  MenusClient,
  OrdersClient,
} from '../../api/api';
import { MatDialog } from '@angular/material/dialog';
import { CreateOrderComponent } from './create-order/create-order.component';
import { IdentityService } from '../../utils/services/identity.service';
import { CreateOrderData } from './create-order/CreateOrderData';

@Component({
  selector: 'app-order-manager',
  templateUrl: './order-manager.component.html',
  styleUrls: ['./order-manager.component.scss'],
})
export class OrderManagerComponent implements OnInit {
  private _createOrderData: CreateOrderData = null!;
  private _expandedItemIds: Array<string> = [];
  totalRecords = 0;
  pageSize = 10;
  pageIndex = 0;
  orders: Array<GetOrdersResponse_Order> = [];
  loading = false;
  displayedColumns = ['orderId', 'total', 'orderTime', 'isCancelled', 'expand'];
  state = false;
  dataSource = new MatTableDataSource<GetOrdersResponse_Order>();

  // @ViewChild(MatPaginator, { static: false })
  // set paginator(value: MatPaginator) {
  //   if (this.state === false) {
  //     this.dataSource.paginator = value;
  //     this.state = true;
  //   }
  //   // if (this.dataSource && !this.dataSource.paginator) {
  //   //   this.dataSource.paginator = value;
  //   // }
  // }

  constructor(
    private readonly ordersClient: OrdersClient,
    private readonly menuClient: MenusClient,
    private readonly identity: IdentityService,
    private readonly identityClient: IdentityClient,
    private readonly dialog: MatDialog
  ) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        mergeMap(() =>
          forkJoin({
            orders: this.getOrders(undefined, undefined),
            menus: this.menuClient.menusGet(),
            restaurantId: this.identity
              .getUserData()
              .pipe(
                switchMap((result) =>
                  this.identityClient
                    .managerDetails(result.userId || '')
                    .pipe(
                      map((managerDetails) => managerDetails.restaurantId || '')
                    )
                )
              ),
          })
        ),
        tap(({ menus, restaurantId }) => {
          this._createOrderData = new CreateOrderData(restaurantId, menus);
          this.loading = false;
        })
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

  openCreateOrderDialog(): void {
    const dialogRef = this.dialog.open(CreateOrderComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      minWidth: '600px',
      maxWidth: '750px',
      data: this._createOrderData,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(
          (result): result is { reload: boolean } => !!result && result.reload
        ),
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.getOrders(this.pageIndex * this.pageSize, this.pageSize)
        ),
        tap(() => (this.loading = false))
      )
      .subscribe();
  }

  pageChangeEvent(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.getOrders(1 + this.pageIndex * this.pageSize, this.pageSize)
        ),
        tap(() => (this.loading = false))
      )
      .subscribe();
  }

  private getOrders(startRow: number | undefined, amount: number | undefined) {
    return this.ordersClient.ordersGet(startRow, amount).pipe(
      tap((result) => {
        this.orders = result.orders!;
        this.dataSource.data = result.orders!;
        this.totalRecords = result.totalCount!;
      })
    );
  }
}
