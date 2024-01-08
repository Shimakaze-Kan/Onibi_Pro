import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  private _notifications: Array<INotification> = undefined!;

  get notifications(): Observable<Array<INotification>> {
    if (!this._notifications) {
      return this.getNotifications();
    }

    return of(this._notifications);
  }

  constructor(private readonly httpClient: HttpClient) {}

  deleteNotifications(): Observable<Object> {
    if (!this._notifications) {
      return of();
    }

    const ids = this._notifications.map((id) => id.notificationId).join(',');

    return this.httpClient
      .delete(`notifications/${ids}`)
      .pipe(tap(() => (this._notifications = undefined!)));
  }

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
}
