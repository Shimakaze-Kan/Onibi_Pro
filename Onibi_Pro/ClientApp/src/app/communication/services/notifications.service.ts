import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  private _notifications: Array<INotification> = [];

  get notifications(): Observable<Array<INotification>> {
    if (this._notifications.length === 0) {
      return this.getNotifications();
    }

    return of(this._notifications);
  }

  constructor(private readonly httpClient: HttpClient) {}

  private getNotifications() {
    return this.httpClient
      .get<Array<INotification>>('notifications')
      .pipe(tap((result) => (this._notifications = result)));
  }
}

export interface INotification {
  notificationId: string;
  text: string;
  date: Date;
  isViewed: boolean;
}
