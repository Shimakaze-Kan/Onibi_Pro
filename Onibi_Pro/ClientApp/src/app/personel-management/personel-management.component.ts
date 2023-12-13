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
import { ReplaySubject, Subject, take, takeUntil, tap } from 'rxjs';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { EditEmployeeComponent } from './edit-employee/edit-employee.component';
import { GetEmployeesResponse, RestaurantsClient } from '../api/api';

@Component({
  selector: 'app-personel-management',
  templateUrl: './personel-management.component.html',
  styleUrls: ['./personel-management.component.scss'],
})
export class PersonelManagementComponent
  implements AfterViewInit, OnDestroy, OnInit
{
  private readonly _restaurantId = 'E2E115CF-4A20-40E4-ADD5-67CF34788A0A';
  private _employees: Array<EmployeeRecord> = [];
  private _onDestroy$ = new Subject<void>();
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

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  employeeSearchForm = new FormGroup({
    email: new FormControl<string>(''),
    firstName: new FormControl<string>(''),
    lastName: new FormControl<string>(''),
    restaurantId: new FormControl<string>(''),
    supervisors: new FormControl<string>(''),
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
    private readonly client: RestaurantsClient
  ) {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

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

    this.getEmploees().subscribe();
  }

  reset() {
    this.employeeSearchForm.reset();
  }

  openAddEmployeeDialog() {
    const dialogRef = this.dialog.open(AddEmployeeComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '750px',
    });

    dialogRef.afterClosed().subscribe((result: { reload: boolean }) => {
      if (result.reload) {
        this.getEmploees().subscribe();
      }
    });
  }

  openEditEmployeeDialog(data: EmployeeRecord) {
    const dialogRef = this.dialog.open(EditEmployeeComponent, {
      minHeight: '80%',
      maxHeight: '100%',
      maxWidth: '750px',
      data: data,
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }

  private getEmploees() {
    this.loading = true;

    const formValues = this.employeeSearchForm.getRawValue();
    return this.client
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

  searchEmployees(): void {
    this.getEmploees().subscribe();
  }

  supervisors = ['Jane Smith', 'Bob Johnson'];
  cities = ['New York', 'Sosnowiec'];
  positions = ['Cashier', 'Restaurant Manager', 'Regional Manager'];
}

export class EmployeeRecord {
  constructor(data: GetEmployeesResponse, restaurantId: string) {
    this.email = data.email;
    this.firstName = data.firstName;
    this.lastName = data.lastName;
    this.supervisors = data.supervisors;
    this.city = data.city;
    this.restaurantId = restaurantId;
    this.positions = data.positions?.join(', ');
  }
  email: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  supervisors: string | undefined;
  city: string | undefined;
  restaurantId: string;
  positions: string | undefined;
}
