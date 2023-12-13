import { Component, Input, OnDestroy, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  Subject,
  ReplaySubject,
  takeUntil,
  take,
  tap,
  catchError,
  of,
} from 'rxjs';
import { EmployeeRecord } from '../personel-management.component';
import { Positions } from '../Positions';
import { EditEmployeeRequest, RestaurantsClient } from '../../api/api';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-edit-employee',
  templateUrl: './edit-employee.component.html',
  styleUrls: ['./edit-employee.component.scss'],
})
export class EditEmployeeComponent implements OnInit, OnDestroy {
  private readonly _restaurantId = 'E2E115CF-4A20-40E4-ADD5-67CF34788A0A';
  private readonly _onDestroy$ = new Subject<void>();

  editEmployeeForm = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    firstName: new FormControl<string>('', Validators.required),
    lastName: new FormControl<string>('', Validators.required),
    city: new FormControl<string>('', Validators.required),
    restaurantId: new FormControl<string>(
      { value: this._restaurantId, disabled: true },
      Validators.required
    ),
    positions: new FormControl<Array<string>>([], Validators.required),
    supervisors: new FormControl<string>({ value: '', disabled: true }),
  });

  loading = false;
  supervisorFilterCtrl = new FormControl<string>('');
  positionFilterCtrl = new FormControl<string>('');

  filteredSupervisors = new ReplaySubject<string[]>(1);
  filteredPositions = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialogRef: MatDialogRef<
      EditEmployeeComponent,
      { reload: boolean }
    >,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private employeeData: EmployeeRecord,
    private readonly client: RestaurantsClient,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.filteredSupervisors.next(this.supervisors.slice());

    this.supervisorFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterSupervisors();
      });

    this.filteredPositions.next(this.positions.slice());

    this.positionFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterPositions();
      });

    this.editEmployeeForm.setValue({
      city: this.employeeData.city || null,
      email: this.employeeData.email || null,
      firstName: this.employeeData.firstName || null,
      lastName: this.employeeData.lastName || null,
      positions:
        this.employeeData.positions?.split(',').map((x) => x.trim()) || [],
      restaurantId: this._restaurantId,
      supervisors: this.employeeData.supervisors || null,
    });
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onClose() {
    this.dialogRef.close({ reload: false });
  }

  editUser(): void {
    this.loading = true;
    const formValues = this.editEmployeeForm.getRawValue();
    const request = new EditEmployeeRequest({
      city: formValues.city || undefined,
      email: formValues.email || undefined,
      employeeId: this.employeeData.id,
      employeePositions: formValues.positions || [],
      firstName: formValues.firstName || undefined,
      lastName: formValues.lastName || undefined,
    });
    this.client
      .employeePut(this._restaurantId, request)
      .pipe(
        take(1),
        tap(() => {
          this.loading = false;
          this.dialogRef.close({ reload: true });
        }),
        catchError((error) => {
          const description = JSON.parse(error.response).title;
          this.snackBar.open(description, 'close', { duration: 5000 });
          this.loading = false;

          return of(error);
        })
      )
      .subscribe();
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
  cities = ['New York', 'Sosnowiec', 'Chicago'];
  positions = Positions.positions;
}
