import { Component, OnDestroy, OnInit } from '@angular/core';
import { IReplyMessage } from '../message-view/message-view.component';
import {
  IMessage,
  IMessageRecipient,
  MessageService,
} from '../services/message.service';
import {
  Observable,
  Subject,
  forkJoin,
  merge,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { SignalrService } from '../services/signalr.service';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { subscribe } from 'diagnostics_channel';

@Component({
  selector: 'app-message-manager',
  templateUrl: './message-manager.component.html',
  styleUrls: ['../communication.scss'],
})
export class MessageManagerComponent implements OnInit, OnDestroy {
  private readonly _destroy$ = new Subject<void>();
  messagesToBeDeleted: Array<string> = [];
  isOutboxTabSelected = false;
  viewMessageId: string = '';
  showNewMessage: boolean = false;
  isLoadingList = false;
  replyMessage: IReplyMessage = {
    title: '',
    receiverId: '',
    text: '',
    date: '',
    sender: '',
  };

  inboxMessages: Array<IMessage> = [];
  outboxMessages: Array<IMessage> = [];

  get showMessageView(): boolean {
    return !!this.viewMessageId && !this.showNewMessage;
  }

  get showMessageList(): boolean {
    return !this.viewMessageId && !this.showNewMessage;
  }

  get viewedMessage(): IMessage {
    const inboxMessage = this.inboxMessages.find(
      (message) => message.id === this.viewMessageId
    );

    const outboxMessage = this.outboxMessages.find(
      (message) => message.id === this.viewMessageId
    );

    return inboxMessage || outboxMessage!;
  }

  get disableClearButton(): boolean {
    return this.messagesToBeDeleted.length === 0;
  }

  constructor(
    private readonly messageService: MessageService,
    private readonly signalRService: SignalrService
  ) {}

  ngOnInit(): void {
    this.getInboxMessages().subscribe();

    this.signalRService.amountOfMessages.next(0);
    this.signalRService.messages
      .pipe(
        takeUntil(this._destroy$),
        tap((result) => (this.inboxMessages = [result, ...this.inboxMessages]))
      )
      .subscribe();

    this.signalRService.sentMessages
      .pipe(
        takeUntil(this._destroy$),
        tap(
          (result) => (this.outboxMessages = [result, ...this.outboxMessages])
        )
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  viewMessage(messageId: string): void {
    this.viewMessageId = messageId;

    this.messageService
      .markMessageAsViewed(messageId)
      .pipe(switchMap(() => this.getInboxMessages()))
      .subscribe();
  }

  onMessageSent(message: ISendMessage) {
    this.signalRService.sendMessage(
      message.recipients,
      message.title,
      message.text
    );

    this.backToMessageList();
  }

  backToMessageList(): void {
    this.viewMessageId = '';
    this.showNewMessage = false;
    this.replyMessage = {
      title: '',
      receiverId: '',
      text: '',
      date: '',
      sender: '',
    };
  }

  onTabChange(event: MatTabChangeEvent): void {
    if (event.index === 0) {
      this.isOutboxTabSelected = false;
      this.getInboxMessages().subscribe();
    } else {
      this.isOutboxTabSelected = true;
      this.getOutboxMessages().subscribe();
    }
  }

  onReply(data: IReplyMessage): void {
    this.backToMessageList();
    this.replyMessage = {
      title: `Re: ${data.title}`,
      receiverId: data.receiverId,
      text: `\n[From ${data.sender} on ${data.date}]\n${data.text
        .split('\n')
        .map((line) => `> ${line}`)
        .join('\n')}`,
      date: data.date,
      sender: data.sender,
    };
    this.showNewMessage = true;
  }

  toggleMessageDeleteFlag(messageId: string): void {
    const indexOfMessage = this.messagesToBeDeleted.findIndex(
      (x) => x === messageId
    );

    if (indexOfMessage !== -1) {
      this.messagesToBeDeleted = this.messagesToBeDeleted.filter(
        (x) => x !== messageId
      );
    } else {
      this.messagesToBeDeleted.push(messageId);
    }
  }

  checkMessageDeleteFlag(messageId: string): boolean {
    return !!this.messagesToBeDeleted.find((x) => x === messageId);
  }

  deleteMessages(): void {
    const requests: Array<Observable<Object>> = [];

    for (const messageId of this.messagesToBeDeleted) {
      requests.push(this.messageService.markMessageAsDeleted(messageId));
    }

    of({})
      .pipe(
        tap(() => (this.isLoadingList = true)),
        switchMap(() => forkJoin(requests)),
        tap(() => {
          this.signalRService.clearMessages();
          this.messageService.resetCollections();
          this.inboxMessages = [];
          this.outboxMessages = [];

          this.signalRService.messages
            .pipe(
              takeUntil(this._destroy$),
              tap(
                (result) =>
                  (this.inboxMessages = [result, ...this.inboxMessages])
              )
            )
            .subscribe();
          this.signalRService.sentMessages
            .pipe(
              takeUntil(this._destroy$),
              tap(
                (result) =>
                  (this.outboxMessages = [result, ...this.outboxMessages])
              )
            )
            .subscribe();
        }),
        switchMap(() => this.getInboxMessages()),
        tap(() => (this.isLoadingList = false))
      )
      .subscribe();
  }

  private getInboxMessages() {
    return of({}).pipe(
      tap(() => (this.isLoadingList = true)),
      switchMap(() => this.messageService.inboxMessages),
      tap((result) => {
        this.isLoadingList = false;
        const allMessages = [...this.inboxMessages, ...result];

        this.inboxMessages = [
          ...new Map(
            allMessages.map((message) => [message.id, message])
          ).values(),
        ];
        this.inboxMessages.sort(
          (a, b) =>
            new Date(b.sentTime).getTime() - new Date(a.sentTime).getTime()
        );
      })
    );
  }

  private getOutboxMessages() {
    return of({}).pipe(
      tap(() => (this.isLoadingList = true)),
      switchMap(() => this.messageService.outboxMessages),
      tap((result) => {
        this.isLoadingList = false;

        const allMessages = [...this.outboxMessages, ...result];

        this.outboxMessages = [
          ...new Map(
            allMessages.map((message) => [message.id, message])
          ).values(),
        ];
        this.outboxMessages.sort(
          (a, b) =>
            new Date(b.sentTime).getTime() - new Date(a.sentTime).getTime()
        );
      })
    );
  }
}

export interface ISendMessage {
  recipients: Array<IMessageRecipient>;
  title: string;
  text: string;
}
