import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-notification-manager',
  templateUrl: './notification-manager.component.html',
  styleUrls: ['../communication.scss'],
})
export class NotificationManagerComponent implements OnInit {
  notifications: Array<INotification> = [
    {
      title: 'New message from User 1',
      isViewed: false,
      date: new Date(Date.now() - 60000), // One minute ago
      notificationId: 1,
    },
    {
      title: 'Meeting reminder',
      isViewed: true,
      date: new Date(Date.now() - 120000), // Two minutes ago
      notificationId: 2,
    },
    {
      title: 'You have a new follower',
      isViewed: false,
      date: new Date(Date.now() - 180000), // Three minutes ago
      notificationId: 3,
    },
    {
      title: 'Important update',
      isViewed: true,
      date: new Date(Date.now() - 240000), // Four minutes ago
      notificationId: 4,
    },
    {
      title: 'Discount offer',
      isViewed: false,
      date: new Date(Date.now() - 300000), // Five minutes ago
      notificationId: 5,
    },
    {
      title: 'Reminder: Complete survey',
      isViewed: true,
      date: new Date(Date.now() - 360000), // Six minutes ago
      notificationId: 6,
    },
    {
      title: 'New product announcement',
      isViewed: false,
      date: new Date(Date.now() - 420000), // Seven minutes ago
      notificationId: 7,
    },
    {
      title: 'Upcoming event',
      isViewed: true,
      date: new Date(Date.now() - 480000), // Eight minutes ago
      notificationId: 8,
    },
    {
      title: "Congratulations! You've earned a badge",
      isViewed: false,
      date: new Date(Date.now() - 540000), // Nine minutes ago
      notificationId: 9,
    },
    {
      title: 'System maintenance scheduled',
      isViewed: true,
      date: new Date(Date.now() - 600000), // Ten minutes ago
      notificationId: 10,
    },
  ];

  isLoadingList = true;

  ngOnInit(): void {
    setTimeout(() => (this.isLoadingList = false), 1000);
  }
}

interface INotification {
  title: string;
  isViewed: boolean;
  date: Date;
  notificationId: number;
}
