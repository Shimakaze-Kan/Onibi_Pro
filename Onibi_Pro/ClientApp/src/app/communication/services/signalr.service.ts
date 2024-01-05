import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private _notificationHubConnection!: signalR.HubConnection;
  private _notifications = new ReplaySubject<INewNotification>();
  amountOfNotifications = new BehaviorSubject<number>(0);

  get notifications(): ReplaySubject<INewNotification> {
    return this._notifications;
  }

  constructor(private readonly snackBar: MatSnackBar) {
    this.startNotificationHubConnection();
    this.startListeningForNotifications();
  }

  private startNotificationHubConnection(): void {
    this._notificationHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/NotificationsHub')
      .build();

    this._notificationHubConnection.start().catch(() =>
      this.snackBar.open('Failed to setup live notifications.', 'close', {
        duration: 5000,
      })
    );
  }

  private startListeningForNotifications(): void {
    this._notificationHubConnection.on('ReceiveNotification', (data) => {
      this.amountOfNotifications.next(this.amountOfNotifications.value + 1);
      this._notifications.next(data);
    });
  }
}

export interface INewNotification {
  notificationId: string;
  text: string;
  date: Date;
  isViewed: boolean;
}
