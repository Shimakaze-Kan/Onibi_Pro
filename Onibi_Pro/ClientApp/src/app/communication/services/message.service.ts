import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private _inboxMessages: Array<IMessage> = undefined!;
  private _outboxMessages: Array<IMessage> = undefined!;

  get inboxMessages(): Observable<Array<IMessage>> {
    if (!this._inboxMessages) {
      return this.getInboxMessage();
    }

    return of(this._inboxMessages);
  }

  get outboxMessages(): Observable<Array<IMessage>> {
    if (!this._outboxMessages) {
      return this.getOutboxMessage();
    }

    return of(this._outboxMessages);
  }

  constructor(private readonly httpClient: HttpClient) {}

  private getInboxMessage() {
    return this.httpClient
      .get<Array<IMessage>>('/messages/inbox')
      .pipe(tap((result) => (this._inboxMessages = result ?? [])));
  }

  private getOutboxMessage() {
    return this.httpClient
      .get<Array<IMessage>>('/messages/outbox')
      .pipe(tap((result) => (this._outboxMessages = result ?? [])));
  }
}

export interface IMessage {
  id: string;
  title: string;
  authorName: string;
  authorId: string;
  reciptients: Array<IMessageRecipient>;
  sentTime: Date;
  text: string;
}

export interface IMessageRecipient {
  userId: string;
  name: string;
}
