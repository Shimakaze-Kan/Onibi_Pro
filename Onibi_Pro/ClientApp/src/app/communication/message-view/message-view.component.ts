import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IMessage } from '../services/message.service';

@Component({
  selector: 'app-message-view',
  templateUrl: './message-view.component.html',
  styleUrls: ['./message-view.component.scss'],
})
export class MessageViewComponent {
  @Input({ required: true }) message!: IMessage;
  @Output() replyMessage = new EventEmitter<IReplyMessage>();

  constructor(private readonly datePipe: DatePipe) {}

  reply(): void {
    this.replyMessage.emit({
      title: this.message.title,
      text: this.message.text,
      receiverId: this.message.authorId,
      date: this.datePipe.transform(this.message.sentTime, 'short') || '',
      sender: this.message.authorName,
    });
  }
}

export interface IReplyMessage {
  title: string;
  receiverId: string;
  text: string;
  date: string;
  sender: string;
}
