import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  ReplaySubject,
  Subject,
  catchError,
  of,
  take,
  takeUntil,
  tap,
} from 'rxjs';
import { ErrorMessagesParserService } from '../../../utils/services/error-messages-parser.service';
import { IAddEmployeeData } from './IAddEmployeeData';
import { RestaurantsClient, CreateEmployeeRequest } from '../../../api/api';

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

  positionFilterCtrl = new FormControl<string>('');

  filteredPositions = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialogRef: MatDialogRef<
      AddEmployeeComponent,
      { reload: boolean }
    >,
    private readonly client: RestaurantsClient,
    private readonly snackBar: MatSnackBar,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private data: IAddEmployeeData,
    private readonly errorParser: ErrorMessagesParserService
  ) {
    const supervisors = data.managerDetails.sameRestaurantManagers
      ?.map((supervisor) => `${supervisor.firstName} ${supervisor.lastName}`)
      .join(', ');

    this.newEmployeeForm.controls.supervisors.setValue(supervisors || null);
    this.newEmployeeForm.controls.restaurantId.setValue(
      data.managerDetails.restaurantId || null
    );
  }

  ngOnInit(): void {
    this.filteredPositions.next(this.data.positions.slice());

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
      .employeePost(this.data.managerDetails.restaurantId || '', request)
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

  private filterPositions() {
    if (!this.data.positions) {
      return;
    }

    let search = this.positionFilterCtrl.value;
    if (!search) {
      this.filteredPositions.next(this.data.positions.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredPositions.next(
      this.data.positions.filter((position) =>
        position.toLowerCase().includes(search!)
      )
    );
  }
}
