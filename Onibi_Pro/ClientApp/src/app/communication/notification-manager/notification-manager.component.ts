import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, of, switchMap, takeUntil, tap } from 'rxjs';
import { SignalrService } from '../services/signalr.service';
import {
  INotification,
  NotificationsService,
} from '../services/notifications.service';

@Component({
  selector: 'app-notification-manager',
  templateUrl: './notification-manager.component.html',
  styleUrls: ['../communication.scss'],
})
export class NotificationManagerComponent implements OnInit, OnDestroy {
  private readonly _destroy$ = new Subject<void>();
  notifications: Array<INotification> = [];

  isLoadingList = false;

  constructor(
    private readonly signalRService: SignalrService,
    private readonly notificationsService: NotificationsService
  ) {}

  deleteNotifications(): void {
    of({})
      .pipe(
        tap(() => (this.isLoadingList = true)),
        switchMap(() => this.notificationsService.deleteNotifications()),
        switchMap(() => this.notificationsService.notifications),
        tap((result) => {
          this.isLoadingList = false;
          this.notifications = result;
        })
      )
      .subscribe();
  }

  ngOnInit(): void {
    of({})
      .pipe(
        tap(() => (this.isLoadingList = true)),
        switchMap(() => this.notificationsService.notifications),
        tap((result) => {
          this.isLoadingList = false;
          this.notifications = result;
        })
      )
      .subscribe();

    this.signalRService.amountOfNotifications.next(0);
    this.signalRService.notifications
      .pipe(
        takeUntil(this._destroy$),
        tap((data) => {
          this.notifications = [data, ...this.notifications];
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
