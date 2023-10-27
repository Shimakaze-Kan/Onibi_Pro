import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject, Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('placeholder') private placeholder!: ElementRef;
  @ViewChild('navContainer') private navContainer!: ElementRef;
  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;
  private readonly _destroy$ = new Subject<void>();
  private _observer!: IntersectionObserver;
  isExpanded = false;
  currentPageId = 1;
  currentPageUrl = '/';
  showLeftScrollButton$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  showRightScrollButton$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  historyRoutes: IHistoryRouteRecord[] = [];

  pages = [
    { name: 'Main Page', id: 1, url: '/' },
    { name: 'Delivery', id: 2, url: '/counter' },
    { name: 'Schedule', id: 3, url: '/schedule' },
    { name: 'History', id: 4 },
    { name: 'Calendar', id: 5 },
    { name: 'Personel Management', id: 5, url: '/personel-management' },
  ];

  get currentPageName(): string {
    return this.pages.find((x) => x?.url === this.currentPageUrl)?.name || '';
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

    this.observeScrollContainer();
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

            this.historyRoutes = this.historyRoutes.filter(
              (record, index, history) => record.url !== history[index + 1]?.url
            );
          }
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this._observer.disconnect();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  goBack() {
    this.router.navigate([this.lastRoute?.url]);

    if (this.historyRoutes.length > 0) {
      this.historyRoutes.pop();
    }
  }

  scrollLeft() {
    this.scrollContainer.nativeElement.scrollLeft -= 100;
  }

  scrollRight() {
    this.scrollContainer.nativeElement.scrollLeft += 100;
  }

  private observeScrollContainer(): void {
    const firstDivId = 'first-button-container';
    const lastDivId = 'last-button-container';

    const options = {
      root: null,
      rootMargin: '0px',
      threshold: 0.99,
    };

    const lastButton = document.getElementById(firstDivId);
    const firstButton = document.getElementById(lastDivId);

    this._observer = new IntersectionObserver((entries, _) => {
      entries.forEach((entry: any) => {
        if (entry.isIntersecting) {
          if (entry.target.id === firstDivId) {
            this.showLeftScrollButton$.next(false);
          }
          if (entry.target.id === lastDivId) {
            this.showRightScrollButton$.next(false);
          }
        } else {
          if (entry.target.id === firstDivId) {
            this.showLeftScrollButton$.next(true);
          }
          if (entry.target.id === lastDivId) {
            this.showRightScrollButton$.next(true);
          }
        }
      });
    }, options);

    this._observer.observe(lastButton!);
    this._observer.observe(firstButton!);
  }
}

interface IHistoryRouteRecord {
  url: string;
  name: string;
}
