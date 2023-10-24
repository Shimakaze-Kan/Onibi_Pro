import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Renderer2,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent {
  showBack = true;
  isExpanded = false;
  currentPageId = 1;

  pages = [
    { name: 'Main Page', id: 1, url: '/' },
    { name: 'Delivery', id: 2, url: '/counter' },
    { name: 'Profile', id: 3 },
    { name: 'History', id: 4 },
    { name: 'Calendar', id: 5 },
  ];

  get currentPageName(): string {
    return this.pages.find((x) => x.id === this.currentPageId)!.name;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
