import { Component, OnInit, ViewChild } from '@angular/core';
import {
  GetOrdersResponse,
  OrderItemDtoResponse,
  OrdersClient,
} from '../../api/api';
import { of, switchMap, tap } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-order-manager',
  templateUrl: './order-manager.component.html',
  styleUrls: ['./order-manager.component.scss'],
})
export class OrderManagerComponent implements OnInit {
  private _expandedItemIds: Array<string> = [];
  orders: Array<GetOrdersResponse> = [];
  loading = false;
  displayedColumns = ['orderId', 'total', 'orderTime', 'isCancelled', 'expand'];

  dataSource = new MatTableDataSource<GetOrdersResponse>();

  @ViewChild(MatPaginator, { static: false })
  set paginator(value: MatPaginator) {
    if (this.dataSource) {
      this.dataSource.paginator = value;
    }
  }

  constructor(private readonly client: OrdersClient) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.ordersGet(undefined, undefined)),
        tap((result) => {
          this.orders = result;
          this.dataSource.data = result;
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
}
