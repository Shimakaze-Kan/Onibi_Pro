import { Component, OnInit } from '@angular/core';
import {
  GetMenusResponse,
  MenusClient,
  RemoveMenuItemRequest,
} from '../api/api';
import { filter, of, switchMap, tap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { AddMenuComponent } from './add-menu/add-menu.component';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  openedMenuId = '';
  loading = false;
  isRemoving = false;
  menus: Array<GetMenusResponse> = [];

  constructor(
    private readonly client: MenusClient,
    private readonly snackBar: MatSnackBar,
    private readonly dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.getMenus().subscribe();
  }

  getMenus() {
    return of({}).pipe(
      tap(() => (this.loading = true)),
      switchMap(() => this.client.menusGet()),
      tap((result) => {
        this.menus = result;
        this.loading = false;
      })
    );
  }

  removeItem(menuId: string, menuItemId: string): void {
    of({})
      .pipe(
        tap(() => (this.isRemoving = true)),
        switchMap(() =>
          this.client.menusDelete(
            new RemoveMenuItemRequest({
              menuId: menuId,
              menuItemId: menuItemId,
            })
          )
        ),
        tap(() => {
          this.snackBar.open('Menu item removed successfully.', 'close', {
            duration: 5000,
          });
        }),
        switchMap(() => this.getMenus()),
        tap((result) => {
          this.isRemoving = false;
          this.menus = result;
        })
      )
      .subscribe();
  }

  openAddMenuDialog() {
    const dialogRef = this.dialog.open(AddMenuComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      minWidth: '750px',
      maxWidth: '750px',
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        switchMap((_) => this.getMenus()),
        tap(() =>
          this.snackBar.open('Menu created successfully.', 'close', {
            duration: 5000,
          })
        )
      )
      .subscribe();
  }
}
