import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import {
  ReplaySubject,
  Subject,
  filter,
  switchMap,
  take,
  takeUntil,
  tap,
} from 'rxjs';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { EditEmployeeComponent } from './edit-employee/edit-employee.component';
import {
  GetEmployeesResponse,
  GetManagerDetailsResponse,
  IdentityClient,
  RestaurantsClient,
} from '../api/api';
import { IEditEmployeeData } from './edit-employee/IEditEmployeeData';
import { IdentityService } from '../utils/services/identity.service';

@Component({
  selector: 'app-personel-management',
  templateUrl: './personel-management.component.html',
  styleUrls: ['./personel-management.component.scss'],
})
export class PersonelManagementComponent implements OnDestroy, OnInit {
  private _restaurantId = '';
  private _employees: Array<EmployeeRecord> = [];
  private _onDestroy$ = new Subject<void>();
  private _managerDetails: GetManagerDetailsResponse = undefined!;
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
    restaurantId: new FormControl<string>(''),
    supervisors: new FormControl<string>({ value: '', disabled: true }),
    city: new FormControl<string>(''),
    positions: new FormControl<string>(''),
  });

  loading = false;
  supervisorFilterCtrl = new FormControl<string>('');
  cityFilterCtrl = new FormControl<string>('');
  positionFilterCtrl = new FormControl<string>('');

  filteredCities = new ReplaySubject<string[]>(1);
  filteredSupervisors = new ReplaySubject<string[]>(1);
  filteredPositions = new ReplaySubject<string[]>(1);

  constructor(
    private readonly dialog: MatDialog,
    private readonly restaurantClient: RestaurantsClient,
    private readonly identityClient: IdentityClient,
    private readonly identityService: IdentityService
  ) {}

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

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

    this.loading = true;
    this.getManagerDetails()
      .pipe(
        switchMap(() => this.getEmployees()),
        tap(() => (this.loading = false))
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
      data: this._managerDetails,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        switchMap((_) => this.getEmployees())
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
      } as IEditEmployeeData,
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        filter((result: { reload: boolean }) => result.reload),
        switchMap((_) => this.getEmployees())
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
        })
      );
  }

  searchEmployees(): void {
    this.getEmployees().subscribe();
  }

  supervisors = ['Jane Smith', 'Bob Johnson'];
  cities = ['New York', 'Sosnowiec'];
  positions = ['Cashier', 'Restaurant Manager', 'Regional Manager'];
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
