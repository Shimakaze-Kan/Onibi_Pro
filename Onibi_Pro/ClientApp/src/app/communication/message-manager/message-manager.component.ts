import { Component, OnDestroy, OnInit } from '@angular/core';
import { IReplyMessage } from '../message-view/message-view.component';
import {
  IMessage,
  IMessageRecipient,
  MessageService,
} from '../services/message.service';
import { Subject, of, switchMap, takeUntil, tap } from 'rxjs';
import { SignalrService } from '../services/signalr.service';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-message-manager',
  templateUrl: './message-manager.component.html',
  styleUrls: ['../communication.scss'],
})
export class MessageManagerComponent implements OnInit, OnDestroy {
  private readonly _destroy$ = new Subject<void>();
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
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  viewMessage(messageId: string): void {
    this.viewMessageId = messageId;
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
      this.getInboxMessages().subscribe();
    } else {
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

  private getInboxMessages() {
    return of({}).pipe(
      tap(() => (this.isLoadingList = true)),
      switchMap(() => this.messageService.inboxMessages),
      tap((result) => {
        this.isLoadingList = false;
        this.inboxMessages = result;
      })
    );
  }

  private getOutboxMessages() {
    return of({}).pipe(
      tap(() => (this.isLoadingList = true)),
      switchMap(() => this.messageService.outboxMessages),
      tap((result) => {
        this.isLoadingList = false;
        this.outboxMessages = result;
      })
    );
  }
}

export interface ISendMessage {
  recipients: Array<IMessageRecipient>;
  title: string;
  text: string;
}
