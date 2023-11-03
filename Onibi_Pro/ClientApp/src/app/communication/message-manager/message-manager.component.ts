import { Component } from '@angular/core';
import { IReplyMessage } from '../message-view/message-view.component';

@Component({
  selector: 'app-message-manager',
  templateUrl: './message-manager.component.html',
  styleUrls: ['./message-manager.component.scss'],
})
export class MessageManagerComponent {
  viewMessageId: number | undefined;
  showNewMessage: boolean = false;
  replyMessage: IReplyMessage = {
    title: '',
    receiverId: undefined,
    text: '',
    date: '',
    sender: '',
  };

  messages: Array<IMessage> = [
    {
      title: 'Hello',
      sender: 'John',
      isViewed: true,
      date: new Date(),
      messageId: 1,
    },
    {
      title: 'Meeting Reminder',
      sender: 'Alice',
      isViewed: false,
      date: new Date(),
      messageId: 2,
    },
    {
      title: 'Important Update',
      sender: 'Bob',
      isViewed: true,
      date: new Date(),
      messageId: 3,
    },
    {
      title: 'Invitation',
      sender: 'Sarah',
      isViewed: false,
      date: new Date(),
      messageId: 4,
    },
    {
      title: 'New Project Details',
      sender: 'David',
      isViewed: true,
      date: new Date(),
      messageId: 5,
    },
    {
      title: 'Weekly Report',
      sender: 'Eva',
      isViewed: false,
      date: new Date(),
      messageId: 6,
    },
    {
      title: 'Vacation Plans',
      sender: 'Michael',
      isViewed: true,
      date: new Date(),
      messageId: 7,
    },
    {
      title: 'Feedback Request',
      sender: 'Linda',
      isViewed: false,
      date: new Date(),
      messageId: 8,
    },
    {
      title: 'Announcement',
      sender: 'Mark',
      isViewed: true,
      date: new Date(),
      messageId: 9,
    },
    {
      title: 'New Feature Demo',
      sender: 'Grace',
      isViewed: false,
      date: new Date(),
      messageId: 10,
    },
  ];

  get showMessageView(): boolean {
    return !!this.viewMessageId && !this.showNewMessage;
  }

  get showMessageList(): boolean {
    return !this.viewMessageId && !this.showNewMessage;
  }

  viewMessage(messageId: number): void {
    this.viewMessageId = messageId;
  }

  backToMessageList(): void {
    this.viewMessageId = undefined;
    this.showNewMessage = false;
    this.replyMessage = {
      title: '',
      receiverId: undefined,
      text: '',
      date: '',
      sender: '',
    };
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
}

interface IMessage {
  title: string;
  sender: string;
  isViewed: boolean;
  date: Date;
  messageId: number;
}
