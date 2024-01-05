import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil, tap } from 'rxjs';
import { INewNotification, SignalrService } from '../signalr/signalr.service';

@Component({
  selector: 'app-notification-manager',
  templateUrl: './notification-manager.component.html',
  styleUrls: ['../communication.scss'],
})
export class NotificationManagerComponent implements OnInit, OnDestroy {
  private readonly _destroy$ = new Subject<void>();
  notifications: Array<INewNotification> = [];

  isLoadingList = false;

  constructor(private readonly signalRService: SignalrService) {}

  ngOnInit(): void {
    this.signalRService.amountOfNotifications.next(0);
    this.signalRService.notifications
      .pipe(
        takeUntil(this._destroy$),
        tap((data) => {
          this.notifications.push(data);
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
