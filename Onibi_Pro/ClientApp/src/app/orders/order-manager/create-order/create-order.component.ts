import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';
import { CreateOrderData } from './CreateOrderData';
import { FormControl } from '@angular/forms';
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
  CreateOrderRequest,
  CreateOrderRequest_OrderItem,
  GetMenusResponse,
  OrdersClient,
} from '../../../api/api';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss'],
})
export class CreateOrderComponent implements OnInit {
  private readonly _onDestroy$ = new Subject<void>();
  loading = false;
  menuFilterCtrl = new FormControl<string>('');
  filteredMenus = new ReplaySubject<GetMenusResponse[]>(1);
  orderItems: Array<CreateOrderRequest_OrderItem> = [];

  get canPlaceOrder(): boolean {
    return (
      this.orderItems.length !== 0 &&
      !this.loading &&
      this.orderItems.every((item) => this.isOrderItemValid(item))
    );
  }

  constructor(
    private readonly dialogRef: MatDialogRef<
      CreateOrderComponent,
      { reload: boolean }
    >,
    private readonly snackBar: MatSnackBar,
    private readonly client: OrdersClient,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private data: CreateOrderData,
    private readonly errorParser: ErrorMessagesParserService
  ) {}

  ngOnInit(): void {
    this.filteredMenus.next(this.data.menus.slice());
    this.menuFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterMenus();
      });
  }

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  placeOrder(): void {
    const restaurantId = this.data.restaurantId;
    const orderItems = this.reduceDuplicates(this.orderItems);
    const request = new CreateOrderRequest({ orderItems: orderItems });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.ordersPost(restaurantId, request)),
        tap(() => {
          this.loading = false;
          this.dialogRef.close({ reload: true });
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

  addProduct(): void {
    this.orderItems.push(
      new CreateOrderRequest_OrderItem({ menuItemId: '', quantity: 1 })
    );
  }

  removeProduct(item: CreateOrderRequest_OrderItem): void {
    this.orderItems = this.orderItems.filter((orderItem) => orderItem !== item);
  }

  private filterMenus() {
    if (!this.data.menus) {
      return;
    }

    let search = this.menuFilterCtrl.value;
    if (!search) {
      this.filteredMenus.next(this.data.menus.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredMenus.next(
      this.data.menus.flatMap((menu) => {
        const menuItems =
          menu.menuItems?.filter((item) =>
            item.name?.toLowerCase().includes(search!)
          ) ?? [];
        const menuCopy = JSON.parse(JSON.stringify(menu)) as GetMenusResponse;
        menuCopy.menuItems = menuItems;

        return menuItems.length === 0 ? [] : [menuCopy];
      })
    );
  }

  private isOrderItemValid(item: CreateOrderRequest_OrderItem): boolean {
    return !!item.menuItemId && item.quantity! > 0;
  }

  private reduceDuplicates(
    orderItems: Array<CreateOrderRequest_OrderItem>
  ): Array<CreateOrderRequest_OrderItem> {
    const map = new Map<string | undefined, CreateOrderRequest_OrderItem>();

    for (let item of orderItems) {
      const itemId = item.menuItemId;

      if (map.has(itemId)) {
        const newItem = map.get(itemId)!;
        newItem.quantity! += item.quantity!;

        map.set(itemId, newItem);
      } else {
        map.set(itemId, item);
      }
    }

    return [...map.values()];
  }
}
