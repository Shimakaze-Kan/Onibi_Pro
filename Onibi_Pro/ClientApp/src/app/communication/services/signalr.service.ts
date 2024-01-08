import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { IMessage, IMessageRecipient } from './message.service';
import { INotification } from './notifications.service';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private _notificationHubConnection!: signalR.HubConnection;
  private _chatHubConnection!: signalR.HubConnection;
  private _notifications = new ReplaySubject<INotification>();
  private _messages = new ReplaySubject<IMessage>();
  private _sentMessages = new ReplaySubject<IMessage>();
  amountOfNotifications = new BehaviorSubject<number>(0);
  amountOfMessages = new BehaviorSubject<number>(0);

  get notifications(): ReplaySubject<INotification> {
    return this._notifications;
  }

  get messages(): ReplaySubject<IMessage> {
    return this._messages;
  }

  get sentMessages(): ReplaySubject<IMessage> {
    return this._sentMessages;
  }

  constructor(private readonly snackBar: MatSnackBar) {
    this.startNotificationHubConnection();
    this.startListeningForNotifications();
    this.startChatHubConnection();
    this.startListeningForMessages();
  }

  clearMessages(): void {
    this._messages.complete();
    this._messages = new ReplaySubject<IMessage>();
    this._sentMessages.complete();
    this._sentMessages = new ReplaySubject<IMessage>();
  }

  sendMessage(recipients: IMessageRecipient[], title: string, text: string) {
    this._chatHubConnection.invoke('SendMessage', recipients, title, text);
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

  private startChatHubConnection(): void {
    this._chatHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/ChatsHub')
      .build();

    this._chatHubConnection.start().catch(() =>
      this.snackBar.open('Failed to setup live chat.', 'close', {
        duration: 5000,
      })
    );
  }

  private startListeningForMessages(): void {
    this._chatHubConnection.on('ReceiveMessage', (data) => {
      this.amountOfMessages.next(this.amountOfMessages.value + 1);
      this._messages.next(data);
    });

    this._chatHubConnection.on('SentMessage', (data) =>
      this._sentMessages.next(data)
    );
  }
}
