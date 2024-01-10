import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import {
  ReplaySubject,
  Subject,
  filter,
  forkJoin,
  mergeMap,
  switchMap,
  take,
  takeUntil,
  tap,
} from 'rxjs';
import {
  GetEmployeesResponse,
  GetManagerDetailsResponse,
  IdentityClient,
  RestaurantsClient,
} from '../../api/api';
import { IdentityService } from '../../utils/services/identity.service';
import { EditEmployeeComponent } from './edit-employee/edit-employee.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { IEditEmployeeData } from './edit-employee/IEditEmployeeData';
import { IAddEmployeeData } from './add-employee/IAddEmployeeData';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-personel-management',
  templateUrl: './restaurant-personel-management.component.html',
  styleUrls: ['./restaurant-personel-management.component.scss'],
})
export class RestaurantPersonelManagementComponent
  implements OnDestroy, OnInit
{
  private _restaurantId = '';
  private _employees: Array<EmployeeRecord> = [];
  private _onDestroy$ = new Subject<void>();
  private _managerDetails: GetManagerDetailsResponse = undefined!;
  positions: Array<string> = [];
  cities: Array<string> = [];
  displayedColumns = [
    'email',
    'firstName',
    'lastName',
    'supervisors',
    'city',
    'restaurantId',
    'positions',
    'action',
  ];

  dataSource = new MatTableDataSource<EmployeeRecord>();

  @ViewChild(MatPaginator, { static: false })
  set paginator(value: MatPaginator) {
    if (this.dataSource) {
      this.dataSource.paginator = value;
    }
  }

  employeeSearchForm = new FormGroup({
    email: new FormControl<string>(''),
    firstName: new FormControl<string>(''),
    lastName: new FormControl<string>(''),
    restaurantId: new FormControl<string>({ value: '', disabled: true }),
    supervisors: new FormControl<string>({ value: '', disabled: true }),
    city: new FormControl<string>(''),
    positions: new FormControl<string>(''),
  });

  loading = false;
  cityFilterCtrl = new FormControl<string>('');
  positionFilterCtrl = new FormControl<string>('');

  filteredCities = new ReplaySubject<string[]>(1);
  filteredPositions = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialog: MatDialog,
    private readonly restaurantClient: RestaurantsClient,
    private readonly identityClient: IdentityClient,
    private readonly identityService: IdentityService,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  ngOnInit(): void {
    this.cityFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterCities();
      });

    this.positionFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterPositions();
      });

    this.loading = true;
    this.getManagerDetails()
      .pipe(
        switchMap(() => this.getEmployees()),
        mergeMap(() =>
          forkJoin({
            positions: this.restaurantClient.employeePositions(),
            cities: this.restaurantClient.employeeCities(),
          })
        ),
        tap(({ positions, cities }) => {
          this.positions = positions;
          this.cities = cities;
          this.filteredCities.next(this.cities.slice());
          this.filteredPositions.next(this.positions.slice());
          this.loading = false;
        })
      )
      .subscribe();
  }

  reset() {
    this.employeeSearchForm.reset();
  }

  openAddEmployeeDialog() {
    const dialogRef = this.dialog.open(AddEmployeeComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '750px',
      data: {
        managerDetails: this._managerDetails,
        positions: this.positions,
      } as IAddEmployeeData,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        switchMap((_) => this.getEmployees()),
        tap(() =>
          this.snackBar.open('Employee created successfully.', 'close', {
            duration: 5000,
          })
        )
      )
      .subscribe();
  }

  openEditEmployeeDialog(employeeData: EmployeeRecord) {
    const dialogRef = this.dialog.open(EditEmployeeComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '750px',
      data: {
        employeeData: employeeData,
        managerDetails: this._managerDetails,
        positions: this.positions,
      } as IEditEmployeeData,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        switchMap((_) => this.getEmployees()),
        tap(() =>
          this.snackBar.open('Employee edited successfully.', 'close', {
            duration: 5000,
          })
        )
      )
      .subscribe();
  }

  private getEmployees() {
    this.loading = true;

    const formValues = this.employeeSearchForm.getRawValue();
    return this.restaurantClient
      .employees(
        this._restaurantId,
        formValues.firstName || undefined,
        formValues.lastName || undefined,
        formValues.email || undefined,
        formValues.city || undefined,
        formValues.positions || undefined
      )
      .pipe(
        take(1),
        tap((result) => {
          this._employees = result.map(
            (x) => new EmployeeRecord(x, this._restaurantId)
          );
          this.dataSource.data = this._employees;
          this.loading = false;
        })
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

  private getManagerDetails() {
    return this.identityService
      .getUserData()
      .pipe(
        switchMap((userData) =>
          this.identityClient.managerDetails(userData.userId || '')
        )
      )
      .pipe(
        tap((result) => {
          this._managerDetails = result;
          this._restaurantId = result.restaurantId || '';
          this.employeeSearchForm.controls.restaurantId.setValue(
            result.restaurantId || ''
          );
        })
      );
  }

  searchEmployees(): void {
    this.getEmployees().subscribe();
  }
}

export class EmployeeRecord {
  constructor(data: GetEmployeesResponse, restaurantId: string) {
    this.id = data.id;
    this.email = data.email;
    this.firstName = data.firstName;
    this.lastName = data.lastName;
    this.supervisors = data.supervisors;
    this.city = data.city;
    this.restaurantId = restaurantId;
    this.positions = data.positions?.join(', ');
  }
  id: string | undefined;
  email: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  supervisors: string | undefined;
  city: string | undefined;
  restaurantId: string;
  positions: string | undefined;
}
