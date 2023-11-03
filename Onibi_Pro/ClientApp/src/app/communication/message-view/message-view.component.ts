import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-message-view',
  templateUrl: './message-view.component.html',
  styleUrls: ['./message-view.component.scss'],
})
export class MessageViewComponent implements OnInit {
  @Input({ required: true }) messageId!: number;
  @Output() replyMessage = new EventEmitter<IReplyMessage>();
  message!: IMessage;

  constructor(private readonly datePipe: DatePipe) {}

  ngOnInit(): void {
    //get messages
    const dummyMessage = {
      title: 'Message with Replies',
      sender: 'Bob',
      date: new Date(),
      text: `This is the main message text.
  
  > This is a reply to the main message.
  >> This is a reply to the first reply.
  >>> This is a reply to the second reply.
  
  > Another reply to the main message.`,
    };

    this.message = dummyMessage;
  }

  reply(): void {
    this.replyMessage.emit({
      title: this.message.title,
      text: this.message.text,
      receiverId: 2,
      date: this.datePipe.transform(this.message.date, 'short') || '',
      sender: this.message.sender,
    });
  }
}

interface IMessage {
  title: string;
  sender: string;
  date: Date;
  text: string;
}

export interface IReplyMessage {
  title: string;
  receiverId: number | undefined;
  text: string;
  date: string;
  sender: string;
}
