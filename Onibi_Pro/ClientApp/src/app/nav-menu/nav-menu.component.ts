import { animate, style, transition, trigger } from '@angular/animations';
import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NavigationEnd, Router } from '@angular/router';
import {
  BehaviorSubject,
  Subject,
  filter,
  fromEvent,
  takeUntil,
  tap,
} from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateY(-100%)', zIndex: '-1' }),
        animate('150ms ease-in', style({ transform: 'translateY(0%)' })),
      ]),
      transition(':leave', [
        style({ zIndex: '-1' }),
        animate('150ms ease-in', style({ transform: 'translateY(-100%)' })),
      ]),
    ]),
  ],
})
export class NavMenuComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('placeholder') private placeholder!: ElementRef;
  @ViewChild('navContainer') private navContainer!: ElementRef;
  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;
  @ViewChild('messagesPanel', { static: false }) messagesPanel!: ElementRef;
  private readonly _destroy$ = new Subject<void>();
  private _observer!: IntersectionObserver;
  private _title = (name: string) => `${name} :: Onibi Pro`;
  showMessages = false;
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
    { name: 'Welcome', id: 4, url: '/welcome' },
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

  constructor(
    private readonly router: Router,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly titleService: Title
  ) {}

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

            this.titleService.setTitle(this._title(this.currentPageName));
          }
        })
      )
      .subscribe();

    fromEvent(window, 'resize')
      .pipe(
        takeUntil(this._destroy$),
        filter(() => !!this.messagesPanel),
        tap(() => this.resizeMessagePanel())
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

  toggleMessages() {
    this.showMessages = !this.showMessages;

    if (this.showMessages) {
      this.changeDetectorRef.detectChanges();
      this.resizeMessagePanel();
    }
  }

  private resizeMessagePanel(): void {
    const top = this.messagesPanel.nativeElement.offsetTop;
    this.messagesPanel.nativeElement.style.height = `${
      window.innerHeight - top
    }px`;
  }

  private observeScrollContainer(): void {
    const firstDivClass = 'first-button-container';
    const lastDivClass = 'last-button-container';

    const options = {
      root: null,
      rootMargin: '0px',
      threshold: 0.99,
    };

    const lastButton = document.getElementsByClassName(firstDivClass)[0];
    const firstButton = document.getElementsByClassName(lastDivClass)[0];

    this._observer = new IntersectionObserver((entries, _) => {
      entries.forEach((entry: IntersectionObserverEntry) => {
        if (entry.isIntersecting) {
          if (entry.target.classList.contains(firstDivClass)) {
            this.showLeftScrollButton$.next(false);
          }
          if (entry.target.classList.contains(lastDivClass)) {
            this.showRightScrollButton$.next(false);
          }
        } else {
          if (entry.target.classList.contains(firstDivClass)) {
            this.showLeftScrollButton$.next(true);
          }
          if (entry.target.classList.contains(lastDivClass)) {
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
