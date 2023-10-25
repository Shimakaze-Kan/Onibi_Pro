import { ViewportScroller } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import {
  NavigationEnd,
  NavigationStart,
  Router,
  RoutesRecognized,
} from '@angular/router';
import { Subject, filter, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('placeholder') private placeholder!: ElementRef;
  @ViewChild('navContainer') private navContainer!: ElementRef;
  private readonly _destroy$ = new Subject<void>();
  isExpanded = false;
  currentPageId = 1;
  currentPageUrl = '/';

  historyRoutes: IHistoryRouteRecord[] = [];

  pages = [
    { name: 'Main Page', id: 1, url: '/' },
    { name: 'Delivery', id: 2, url: '/counter' },
    { name: 'Schedule', id: 3, url: '/schedule' },
    { name: 'History', id: 4 },
    { name: 'Calendar', id: 5 },
  ];

  get currentPageName(): string {
    return this.pages.find((x) => x.url === this.currentPageUrl)!.name;
  }

  get showBack(): boolean {
    return this.historyRoutes.length > 1;
  }

  get lastRoute(): IHistoryRouteRecord | undefined {
    if (this.historyRoutes.length > 1) {
      return this.historyRoutes[this.historyRoutes.length - 2];
    }

    return;
  }

  constructor(private readonly router: Router) {}

  ngAfterViewInit(): void {
    const navContainerHeight =
      this.navContainer.nativeElement.getBoundingClientRect().height;
    this.placeholder.nativeElement.style.height = `${navContainerHeight}px`;
  }

  ngOnInit(): void {
    this.router.events
      .pipe(
        takeUntil(this._destroy$),
        tap((event) => {
          const currentUrl = this.router.url;
          const urlWithoutQuery = currentUrl.split('?')[0];
          this.currentPageUrl = `/${urlWithoutQuery.split('/').slice(-1)}`;

          if (event instanceof NavigationEnd) {
            this.historyRoutes.push({
              url: event.urlAfterRedirects,
              name: this.currentPageName,
            });
          }
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

interface IHistoryRouteRecord {
  url: string;
  name: string;
}