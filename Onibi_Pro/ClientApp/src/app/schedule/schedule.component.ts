import { Component, OnInit } from '@angular/core';
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
  CalendarView,
} from 'angular-calendar';
import { endOfDay, isSameDay, isSameMonth, startOfDay } from 'date-fns';
import {
  ReplaySubject,
  Subject,
  catchError,
  forkJoin,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import {
  CreateScheduleRequest,
  DeleteScheduleRequest,
  EditScheduleRequest,
  GetEmployeesResponse,
  GetScheduleResponse,
  IdentityClient,
  RestaurantsClient,
} from '../api/api';
import { IdentityService } from '../utils/services/identity.service';
import { FormControl } from '@angular/forms';
import { ErrorMessagesParserService } from '../utils/services/error-messages-parser.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent implements OnInit {
  private _restaurantId: string = undefined!;
  private _onDestroy$ = new Subject<void>();
  editingRowId = '';
  employees: Array<GetEmployeesResponse> = [];
  loading = false;
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();
  refresh = new Subject<void>();
  activeDayIsOpen: boolean = true;
  workingSchedule: ScheduleItem = null!;
  priorities = Priorities;
  employeeFilterCtrl = new FormControl<string>('');
  filteredEmployees = new ReplaySubject<GetEmployeesResponse[]>(1);

  private _actions: CalendarEventAction[] = [
    {
      label: '<img class="action-icon" src="assets/icons/pencil.svg"/>',
      a11yLabel: 'Edit',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        const scheduleItem = ScheduleItem.fromCalendarEvent(event);
        this.editingRowId = scheduleItem.id;
        document.getElementById(scheduleItem.id)?.scrollIntoView({
          behavior: 'smooth',
          block: 'center',
          inline: 'nearest',
        });
      },
    },
    {
      label: '<img class="action-icon" src="assets/icons/trash-1.svg"/>',
      a11yLabel: 'Delete',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.deleteEvent(ScheduleItem.fromCalendarEvent(event));
      },
    },
  ];

  scheduleItems: ScheduleItem[] = [];
  events: CalendarEvent[] = [];

  get canAddNewEvent(): boolean {
    return !this.workingSchedule;
  }

  constructor(
    private readonly identityClient: IdentityClient,
    private readonly identityService: IdentityService,
    private readonly restaurantClient: RestaurantsClient,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.getManagerDetails()),
        switchMap((result) =>
          forkJoin({
            employees: this.getEmployees(result.restaurantId || ''),
            schedule: this.restaurantClient.scheduleGet(
              result.restaurantId || ''
            ),
          })
        ),
        tap(({ employees, schedule }) => {
          this.employees = employees;
          this.filteredEmployees.next(employees.slice());
          this.scheduleItems = ScheduleItem.fromGetScheduleResponse(schedule);
          this.updateCalendarEvents();
          this.loading = false;
        })
      )
      .subscribe();

    this.employeeFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterEmployees();
      });
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
      this.viewDate = date;
    }
  }

  eventTimesChanged({
    event,
    newStart,
    newEnd,
  }: CalendarEventTimesChangedEvent): void {
    const scheduleItem = this.scheduleItems.find((x) => x.id === event.id)!;
    scheduleItem.start = newStart;
    scheduleItem.end = newEnd!;

    this.updateEvent(scheduleItem);
  }

  handleEvent(action: string, event: CalendarEvent): void {
    //this.modalData = { event, action };
    //this.modal.open(this.modalContent, { size: 'lg' });
  }

  addEvent(): void {
    this.workingSchedule = new ScheduleItem(
      '',
      new Date(),
      new Date(),
      Priorities.Standard
    );
  }

  deleteEvent = (eventToDelete: ScheduleItem): void => {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.restaurantClient.scheduleDelete(
            this._restaurantId,
            new DeleteScheduleRequest({ scheduleId: eventToDelete.id })
          )
        ),
        switchMap(() => this.restaurantClient.scheduleGet(this._restaurantId)),
        tap((schedule) => {
          this.scheduleItems = ScheduleItem.fromGetScheduleResponse(schedule);
          this.clearWorkingSchedule();
          this.updateCalendarEvents();
          this.loading = false;
        }),
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
  };

  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }

  clearWorkingSchedule = (): void => {
    this.workingSchedule = null!;
    this.editingRowId = '';
  };

  canUpdateOrAddEvent(event: ScheduleItem): boolean {
    return event.isValid();
  }

  createEvent = (event: ScheduleItem): void => {
    const request = event.toCreateScheduleRequest();
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.restaurantClient.schedulePost(this._restaurantId, request)
        ),
        switchMap(() => this.restaurantClient.scheduleGet(this._restaurantId)),
        tap((schedule) => {
          this.scheduleItems = ScheduleItem.fromGetScheduleResponse(schedule);
          this.clearWorkingSchedule();
          this.updateCalendarEvents();
          this.loading = false;
        }),
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
  };

  updateEvent = (event: ScheduleItem): void => {
    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.restaurantClient.schedulePut(
            this._restaurantId,
            event.toEditScheduleRequest()
          )
        ),
        switchMap(() => this.restaurantClient.scheduleGet(this._restaurantId)),
        tap((schedule) => {
          this.scheduleItems = ScheduleItem.fromGetScheduleResponse(schedule);
          this.clearWorkingSchedule();
          this.updateCalendarEvents();
          this.loading = false;
        }),
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
  };

  getDisplayEmployee(ids: Array<string>): string {
    return this.employees
      .flatMap((employee) =>
        ids.includes(employee.id || '')
          ? `${employee.firstName} ${employee.lastName}`
          : []
      )
      .join(', ');
  }

  private updateCalendarEvents(): void {
    this.events = ScheduleItem.toCalendarEvents(
      this.scheduleItems,
      this._actions
    );
    this.refresh.next();
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
          this._restaurantId = result.restaurantId || '';
        })
      );
  }

  private getEmployees(restaurantId: string) {
    return this.restaurantClient.employees(
      restaurantId,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined
    );
  }

  private filterEmployees() {
    if (!this.employees) {
      return;
    }

    let search = this.employeeFilterCtrl.value;
    if (!search) {
      this.filteredEmployees.next(this.employees.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredEmployees.next(
      this.employees.filter((employee) => {
        const name = `${employee.firstName} ${employee.lastName}`;
        return name.toLowerCase().includes(search!);
      })
    );
  }
}

class ScheduleItem {
  private static _internalId: number = 0;
  private _id: string;
  private _title: string;
  private _start: Date;
  private _end: Date;
  private _priority: Priorities;
  private _employeeIds: Array<string>;
  private _isEdited = false;

  constructor(
    title: string,
    start: Date,
    end: Date,
    priority: Priorities,
    employeesIds: Array<string> = [],
    id?: string
  ) {
    this._title = title;
    this._start = start;
    this._end = end;
    this._priority = priority;
    this._employeeIds = employeesIds;
    if (!id) {
      ScheduleItem._internalId++;
      this._id = ScheduleItem._internalId.toString();
    } else {
      this._id = id;
    }
  }

  get id(): string {
    return this._id;
  }

  get title(): string {
    return this._title;
  }

  get start(): Date {
    return this._start;
  }

  get end(): Date {
    return this._end;
  }

  get priority(): Priorities {
    return this._priority;
  }

  get employeeIds(): Array<string> {
    return this._employeeIds;
  }

  get isEdited(): boolean {
    return this._isEdited;
  }

  set title(value: string) {
    this._title = value;
    this._isEdited = true;
  }

  set start(value: Date) {
    this._start = value;
    this._isEdited = true;
  }

  set end(value: Date) {
    this._end = value;
    this._isEdited = true;
  }

  set priority(value: Priorities) {
    this._priority = value;
    this._isEdited = true;
  }

  set employeeIds(value: Array<string>) {
    this._employeeIds = value;
    this._isEdited = true;
  }

  toCalendarEvent(actions?: Array<CalendarEventAction>): CalendarEvent {
    const color = priorityColors[this._priority];

    return {
      title: this._title,
      start: startOfDay(this._start),
      end: endOfDay(this._end),
      color: { primary: color, secondary: color },
      allDay: true,
      draggable: true,
      actions: actions,
      id: this._id,
    };
  }

  toCreateScheduleRequest(): CreateScheduleRequest {
    return new CreateScheduleRequest({
      title: this.title,
      employeeIds: this.employeeIds,
      endDate: this.end,
      startDate: this.start,
      priority: Priorities[this.priority],
    });
  }

  toEditScheduleRequest(): EditScheduleRequest {
    return new EditScheduleRequest({
      scheduleId: this.id,
      title: this.title,
      employeeIds: this.employeeIds,
      endDate: this.end,
      startDate: this.start,
      priority: Priorities[this.priority],
    });
  }

  static toCalendarEvents(
    array: Array<ScheduleItem>,
    actions?: Array<CalendarEventAction>
  ): Array<CalendarEvent> {
    return array.map((item) => item.toCalendarEvent(actions));
  }

  static fromCalendarEvent(event: CalendarEvent): ScheduleItem {
    return new ScheduleItem(
      event.title,
      event.start,
      event.end!,
      getPriorityByColor(event.color?.primary!),
      [],
      event.id?.toString()
    );
  }

  static fromCalendarEvents(array: Array<CalendarEvent>): Array<ScheduleItem> {
    return array.map((event) => ScheduleItem.fromCalendarEvent(event));
  }

  static fromGetScheduleResponse(
    array: Array<GetScheduleResponse>
  ): Array<ScheduleItem> {
    return array.map(
      (schedule) =>
        new ScheduleItem(
          schedule.title!,
          schedule.startDate!,
          schedule.endDate!,
          toPriority(schedule.priority!)!,
          schedule.employeeIds,
          schedule.scheduleId!
        )
    );
  }

  isValid(): boolean {
    return [this._title, this.start, this.end, this._employeeIds.length].every(
      (x) => !!x
    );
  }
}

enum Priorities {
  Critical,
  Essential,
  Standard,
}

const priorityColors: Record<Priorities, string> = {
  [Priorities.Critical]: '#d20a0a',
  [Priorities.Essential]: '#ffc933',
  [Priorities.Standard]: '#0070f2',
};

function getPriorityByColor(color: string): Priorities {
  return Object.keys(priorityColors).find(
    (key) => priorityColors[key as unknown as Priorities] === color
  ) as unknown as Priorities;
}

function toPriority(value: string): Priorities | undefined {
  const keys = Object.keys(Priorities);
  const foundKey = keys.find(
    (key) => key.toLowerCase() === value.toLowerCase()
  );
  return foundKey !== undefined
    ? Priorities[foundKey as keyof typeof Priorities]
    : undefined;
}
