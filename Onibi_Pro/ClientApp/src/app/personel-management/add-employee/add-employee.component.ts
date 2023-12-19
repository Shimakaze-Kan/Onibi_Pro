import { Component, OnDestroy, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  ReplaySubject,
  Subject,
  catchError,
  of,
  take,
  takeUntil,
  tap,
} from 'rxjs';
import {
  CreateEmployeeRequest,
  GetManagerDetailsResponse,
  RestaurantsClient,
} from '../../api/api';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Positions } from '../Positions';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss'],
})
export class AddEmployeeComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();
  loading = false;

  newEmployeeForm = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    firstName: new FormControl<string>('', Validators.required),
    lastName: new FormControl<string>('', Validators.required),
    city: new FormControl<string>('', Validators.required),
    restaurantId: new FormControl<string>(
      { value: '', disabled: true },
      Validators.required
    ),
    position: new FormControl<Array<string>>([], Validators.required),
    supervisors: new FormControl<string>({ value: '', disabled: true }),
  });

  supervisorFilterCtrl = new FormControl<string>('');
  positionFilterCtrl = new FormControl<string>('');

  filteredSupervisors = new ReplaySubject<string[]>(1);
  filteredPositions = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialogRef: MatDialogRef<
      AddEmployeeComponent,
      { reload: boolean }
    >,
    private readonly client: RestaurantsClient,
    private readonly snackBar: MatSnackBar,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private data: GetManagerDetailsResponse,
    private readonly errorParser: ErrorMessagesParserService
  ) {
    const supervisors = data.sameRestaurantManagers
      ?.map((supervisor) => `${supervisor.firstName} ${supervisor.lastName}`)
      .join(', ');

    this.newEmployeeForm.controls.supervisors.setValue(supervisors || null);
    this.newEmployeeForm.controls.restaurantId.setValue(
      data.restaurantId || null
    );
  }

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
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  createEmployee(): void {
    this.loading = true;
    const formValues = this.newEmployeeForm.getRawValue();
    const request = new CreateEmployeeRequest({
      firstName: formValues.firstName || undefined,
      city: formValues.city || undefined,
      email: formValues.email || undefined,
      lastName: formValues.lastName || undefined,
      employeePositions: formValues.position || undefined,
    });
    this.client
      .employeePost(this.data.restaurantId || '', request)
      .pipe(
        take(1),
        tap(() => this.dialogRef.close({ reload: true })),
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
  cities = ['New York', 'Sosnowiec'];
  positions = Positions.positions;
}
