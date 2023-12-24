import { AfterViewInit, Component, OnInit } from '@angular/core';
import {
  CalendarView,
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
} from 'angular-calendar';
import { privateDecrypt } from 'crypto';
import {
  subDays,
  startOfDay,
  addDays,
  endOfMonth,
  addHours,
  endOfDay,
  isSameDay,
  isSameMonth,
} from 'date-fns';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent {
  loading = false;
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();
  refresh = new Subject<void>();
  activeDayIsOpen: boolean = true;
  workingSchedule: ScheduleItem = null!;
  priorities = Priorities;

  private _actions: CalendarEventAction[] = [
    {
      label: '<img class="action-icon" src="assets/icons/pencil.svg"/>',
      a11yLabel: 'Edit',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
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

  scheduleItems: ScheduleItem[] = [
    new ScheduleItem(
      'Team Meeting',
      new Date('2023-12-23T10:00:00'),
      new Date('2023-12-23T11:30:00'),
      Priorities.Standard
    ),
    new ScheduleItem(
      'Training Session',
      new Date('2023-12-23T14:00:00'),
      new Date('2023-12-23T16:00:00'),
      Priorities.Essential
    ),
    new ScheduleItem(
      'Project Review',
      new Date('2023-12-23T09:30:00'),
      new Date('2023-12-23T12:00:00'),
      Priorities.Critical
    ),
    new ScheduleItem(
      'Networking Event',
      new Date('2023-12-23T18:00:00'),
      new Date('2023-12-23T20:00:00'),
      Priorities.Standard
    ),
    new ScheduleItem(
      'Deadline Extension',
      new Date('2023-12-23T16:00:00'),
      new Date('2023-12-23T18:00:00'),
      Priorities.Essential
    ),
  ];

  events: CalendarEvent[] = ScheduleItem.toCalendarEvents(
    this.scheduleItems,
    this._actions
  );

  get canAddNewEvent(): boolean {
    return !this.workingSchedule;
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
    this.events = this.events.map((iEvent) => {
      if (iEvent === event) {
        return {
          ...event,
          start: newStart,
          end: newEnd,
        };
      }
      return iEvent;
    });
    this.handleEvent('Dropped or resized', event);
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
    this.events = this.events.filter((event) => event.id !== eventToDelete.id);
    this.scheduleItems = this.scheduleItems.filter(
      (event) => event.id !== eventToDelete.id
    );

    this.refresh.next();
  };

  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }

  clearWorkingSchedule = (): void => {
    this.workingSchedule = null!;
  };

  canUpdateOrAddEvent(event: ScheduleItem): boolean {
    return event.isValid();
  }
}

class ScheduleItem {
  private static _internalId: number = 0;
  private _id: number;
  private _title: string;
  private _start: Date;
  private _end: Date;
  private _priority: Priorities;

  constructor(
    title: string,
    start: Date,
    end: Date,
    priority: Priorities,
    id?: number
  ) {
    this._title = title;
    this._start = start;
    this._end = end;
    this._priority = priority;
    if (!id) {
      ScheduleItem._internalId++;
      this._id = ScheduleItem._internalId;
    } else {
      this._id = id;
    }
  }

  get id(): number {
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

  set title(value: string) {
    this._title = value;
  }

  set start(value: Date) {
    this._start = value;
  }

  set end(value: Date) {
    this._end = value;
  }

  set priority(value: Priorities) {
    this._priority = value;
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
      +(event.id || 0)
    );
  }

  static fromCalendarEvents(array: Array<CalendarEvent>): Array<ScheduleItem> {
    return array.map((event) => ScheduleItem.fromCalendarEvent(event));
  }

  isValid(): boolean {
    return [this._title, this.start, this.end].every((x) => !!x);
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
