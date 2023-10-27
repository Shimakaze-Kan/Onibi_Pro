import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ReplaySubject, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-personel-management',
  templateUrl: './personel-management.component.html',
  styleUrls: ['./personel-management.component.scss'],
})
export class PersonelManagementComponent
  implements AfterViewInit, OnDestroy, OnInit
{
  private _onDestroy$ = new Subject<void>();
  displayedColumns = [
    'email',
    'firstName',
    'lastName',
    'supervisor',
    'city',
    'restaurantId',
  ];

  dataSource = new MatTableDataSource<EmployeeRecord>(employees);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  employeeSearchForm = new FormGroup({
    email: new FormControl<string>(''),
    firstName: new FormControl<string>(''),
    lastName: new FormControl<string>(''),
    restaurantId: new FormControl<number | undefined>(undefined),
    supervisor: new FormControl<string>(''),
    city: new FormControl<string>(''),
    position: new FormControl<string>(''),
  });

  supervisorFilterCtrl = new FormControl<string>('');
  cityFilterCtrl = new FormControl<string>('');
  positionFilterCtrl = new FormControl<string>('');

  filteredCities = new ReplaySubject<string[]>(1);
  filteredSupervisors = new ReplaySubject<string[]>(1);
  filteredPositions = new ReplaySubject<string[]>(1);

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
  }

  reset() {
    this.employeeSearchForm.reset();
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

  filterCollection() {
    const props = Object.keys(this.employeeSearchForm.controls).map((key) => ({
      key: key,
      value: this.employeeSearchForm.get(key),
    }));

    this.dataSource.data = employees.filter((x) => {
      let state = true;
      props.forEach((pair) => {
        const key = pair.key as ObjectKey;

        if (
          pair.key !== 'position' &&
          !x[key]
            .toString()
            .toLocaleLowerCase()
            .includes((pair.value?.value || '').toString().toLowerCase())
        ) {
          state = false;
        }
      });

      return state;
    });
  }

  supervisors = ['Jane Smith', 'Bob Johnson'];
  cities = ['New York', 'Sosnowiec'];
  positions = ['Cashier', 'Restaurant Manager', 'Regional Manager'];
}

type ObjectKey = keyof (typeof employees)[0];

interface EmployeeRecord {
  email: string;
  firstName: string;
  lastName: string;
  supervisor: string;
  city: string;
  restaurantId: string;
}

let employees: Array<EmployeeRecord> = [
  {
    email: 'john.doe@example.com',
    firstName: 'John',
    lastName: 'Doe',
    supervisor: 'Jane Smith',
    city: 'New York',
    restaurantId: '12345',
  },
  {
    email: 'jane.smith@example.com',
    firstName: 'Jane',
    lastName: 'Smith',
    supervisor: 'Bob Johnson',
    city: 'Los Angeles',
    restaurantId: '67890',
  },
  {
    email: 'mike.jones@example.com',
    firstName: 'Mike',
    lastName: 'Jones',
    supervisor: 'Sarah Brown',
    city: 'Chicago',
    restaurantId: '45678',
  },
  {
    email: 'lisa.wilson@example.com',
    firstName: 'Lisa',
    lastName: 'Wilson',
    supervisor: 'David Lee',
    city: 'Houston',
    restaurantId: '54321',
  },
  {
    email: 'chris.white@example.com',
    firstName: 'Chris',
    lastName: 'White',
    supervisor: 'Emily Davis',
    city: 'San Francisco',
    restaurantId: '98765',
  },
  {
    email: 'susan.jackson@example.com',
    firstName: 'Susan',
    lastName: 'Jackson',
    supervisor: 'Michael Turner',
    city: 'Boston',
    restaurantId: '11223',
  },
  {
    email: 'robert.anderson@example.com',
    firstName: 'Robert',
    lastName: 'Anderson',
    supervisor: 'Laura Martinez',
    city: 'Miami',
    restaurantId: '55555',
  },
  {
    email: 'karen.harris@example.com',
    firstName: 'Karen',
    lastName: 'Harris',
    supervisor: 'William Clark',
    city: 'Seattle',
    restaurantId: '98712',
  },
  {
    email: 'steven.wilson@example.com',
    firstName: 'Steven',
    lastName: 'Wilson',
    supervisor: 'Melissa Turner',
    city: 'Dallas',
    restaurantId: '66554',
  },
  {
    email: 'linda.martin@example.com',
    firstName: 'Linda',
    lastName: 'Martin',
    supervisor: 'Richard Garcia',
    city: 'Denver',
    restaurantId: '23145',
  },
  {
    email: 'daniel.brown@example.com',
    firstName: 'Daniel',
    lastName: 'Brown',
    supervisor: 'Patricia Smith',
    city: 'Phoenix',
    restaurantId: '78901',
  },
  {
    email: 'pamela.taylor@example.com',
    firstName: 'Pamela',
    lastName: 'Taylor',
    supervisor: 'John Adams',
    city: 'Atlanta',
    restaurantId: '12312',
  },
  {
    email: 'james.green@example.com',
    firstName: 'James',
    lastName: 'Green',
    supervisor: 'Susan Moore',
    city: 'San Diego',
    restaurantId: '45698',
  },
  {
    email: 'natalie.johnson@example.com',
    firstName: 'Natalie',
    lastName: 'Johnson',
    supervisor: 'Robert Wilson',
    city: 'Philadelphia',
    restaurantId: '55566',
  },
  {
    email: 'andrew.wilson@example.com',
    firstName: 'Andrew',
    lastName: 'Wilson',
    supervisor: 'Maria Lopez',
    city: 'Las Vegas',
    restaurantId: '87654',
  },
  {
    email: 'emily.hall@example.com',
    firstName: 'Emily',
    lastName: 'Hall',
    supervisor: 'Chris Evans',
    city: 'Detroit',
    restaurantId: '32100',
  },
  {
    email: 'mark.thompson@example.com',
    firstName: 'Mark',
    lastName: 'Thompson',
    supervisor: 'Lisa Taylor',
    city: 'Portland',
    restaurantId: '11234',
  },
  {
    email: 'sarah.wright@example.com',
    firstName: 'Sarah',
    lastName: 'Wright',
    supervisor: 'James Harris',
    city: 'Minneapolis',
    restaurantId: '87632',
  },
  {
    email: 'matthew.jones@example.com',
    firstName: 'Matthew',
    lastName: 'Jones',
    supervisor: 'Jessica Adams',
    city: 'Charlotte',
    restaurantId: '45321',
  },
  {
    email: 'olivia.miller@example.com',
    firstName: 'Olivia',
    lastName: 'Miller',
    supervisor: 'Daniel White',
    city: 'Raleigh',
    restaurantId: '98765',
  },
  {
    email: 'michael.morris@example.com',
    firstName: 'Michael',
    lastName: 'Morris',
    supervisor: 'Catherine Moore',
    city: 'Tampa',
    restaurantId: '23111',
  },
  {
    email: 'jessica.king@example.com',
    firstName: 'Jessica',
    lastName: 'King',
    supervisor: 'Matthew Davis',
    city: 'Nashville',
    restaurantId: '78987',
  },
  {
    email: 'jason.murphy@example.com',
    firstName: 'Jason',
    lastName: 'Murphy',
    supervisor: 'Jennifer Wilson',
    city: 'San Antonio',
    restaurantId: '11222',
  },
  {
    email: 'amanda.hall@example.com',
    firstName: 'Amanda',
    lastName: 'Hall',
    supervisor: 'Steven Brown',
    city: 'Kansas City',
    restaurantId: '54678',
  },
];
