import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ReplaySubject, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss'],
})
export class AddEmployeeComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();

  newEmployeeForm = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    firstName: new FormControl<string>('', Validators.required),
    lastName: new FormControl<string>('', Validators.required),
    city: new FormControl<string>('', Validators.required),
    restaurantId: new FormControl<number | undefined>(
      undefined,
      Validators.required
    ),
    position: new FormControl<string>('', Validators.required),
    supervisor: new FormControl<string>('', Validators.required),
  });

  supervisorFilterCtrl = new FormControl<string>('');
  cityFilterCtrl = new FormControl<string>('');
  positionFilterCtrl = new FormControl<string>('');

  filteredCities = new ReplaySubject<string[]>(1);
  filteredSupervisors = new ReplaySubject<string[]>(1);
  filteredPositions = new ReplaySubject<string[]>(1);

  constructor(private readonly dialogRef: MatDialogRef<AddEmployeeComponent>) {}

  ngOnInit(): void {
    this.filteredSupervisors.next(this.supervisors.slice());

    this.supervisorFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterSupervisors();
      });

    this.filteredCities.next(this.cities.slice());

    this.cityFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterCities();
      });

    this.filteredPositions.next(this.positions.slice());

    this.positionFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterPositions();
      });
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onClose() {
    this.dialogRef.close();
  }

  private filterSupervisors() {
    if (!this.supervisors) {
      return;
    }

    let search = this.supervisorFilterCtrl.value;
    if (!search) {
      this.filteredSupervisors.next(this.supervisors.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredSupervisors.next(
      this.supervisors.filter((supervisor) =>
        supervisor.toLowerCase().includes(search!)
      )
    );
  }

  private filterCities() {
    if (!this.cities) {
      return;
    }

    let search = this.cityFilterCtrl.value;
    if (!search) {
      this.filteredCities.next(this.cities.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredCities.next(
      this.cities.filter((city) => city.toLowerCase().includes(search!))
    );
  }

  private filterPositions() {
    if (!this.positions) {
      return;
    }

    let search = this.positionFilterCtrl.value;
    if (!search) {
      this.filteredPositions.next(this.positions.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredPositions.next(
      this.positions.filter((position) =>
        position.toLowerCase().includes(search!)
      )
    );
  }

  // dummy data
  supervisors = ['Jane Smith', 'Bob Johnson'];
  cities = ['New York', 'Sosnowiec'];
  positions = ['Cashier', 'Restaurant Manager', 'Regional Manager'];
}
