import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetRegionalManagerDetailsResponse } from '../../../api/api';
import { ReplaySubject, Subject, takeUntil } from 'rxjs';

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
  restaurantIds: Array<string> = [];
  loading = false;

  restaurantIdFilterCtrl = new FormControl<string>('');
  filteredRestaurantIds = new ReplaySubject<string[]>(1);

  approveForm = new FormGroup({
    restaurantId: new FormControl<string>('', Validators.required),
  });

  constructor() {}

  ngOnInit(): void {
    this.restaurantIds = this.regionalManager.restaurantIds ?? [];
    this.filteredRestaurantIds.next(this.restaurantIds.slice());

    this.restaurantIdFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterRestaurantIds();
      });
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
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
}
