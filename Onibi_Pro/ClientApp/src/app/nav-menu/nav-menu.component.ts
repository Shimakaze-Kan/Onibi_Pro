import { animate, style, transition, trigger } from '@angular/animations';
import { ScrollStrategy, ScrollStrategyOptions } from '@angular/cdk/overlay';
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
    trigger('slideInOutFromBottom', [
      transition(':enter', [
        style({ transform: 'translateY(100%)' }),
        animate('150ms ease-in', style({ transform: 'translateY(0%)' })),
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ transform: 'translateY(100%)' })),
      ]),
    ]),
  ],
})
export class NavMenuComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('placeholder') private placeholder!: ElementRef;
  @ViewChild('navContainer') private navContainer!: ElementRef;
  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;
  @ViewChild('communicationPanel', { static: false })
  communicationPanel!: ElementRef;
  private readonly _destroy$ = new Subject<void>();
  private _observer!: IntersectionObserver;
  private _title = (name: string) => `${name} :: Onibi Pro`;
  private _communicationPanelApperance: {
    [key in CommunicationPanelContentType]: { show: boolean; elements: number };
  } = {
    [CommunicationPanelContentType.Messages]: { show: false, elements: 0 },
    [CommunicationPanelContentType.Notifications]: {
      show: false,
      elements: 15,
    },
  };
  communicationPanelContentType = CommunicationPanelContentType;
  currentPageId = 1;
  currentPageUrl = '/';
  showFullScreenMenu = false;
  scrollStrategy: ScrollStrategy;
  showLeftScrollButton$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  showRightScrollButton$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  historyRoutes: IHistoryRouteRecord[] = [];

  pages = [
    { name: 'Main Page', id: 1, url: '/' },
    { name: 'Delivery', id: 2, url: '/delivery' },
    { name: 'Schedule', id: 3, url: '/schedule' },
    { name: 'Welcome', id: 4, url: '/welcome' },
    { name: 'Home', id: 5, url: '/home' },
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

  get showCommunicationPanel(): boolean {
    for (const key in this._communicationPanelApperance) {
      if (this._communicationPanelApperance.hasOwnProperty(key)) {
        if (
          this._communicationPanelApperance[
            key as CommunicationPanelContentType
          ].show
        ) {
          return true;
        }
      }
    }
    return false;
  }

  get communicationPanelType(): CommunicationPanelContentType {
    for (const key in this._communicationPanelApperance) {
      if (this._communicationPanelApperance.hasOwnProperty(key)) {
        if (
          this._communicationPanelApperance[
            key as CommunicationPanelContentType
          ].show
        ) {
          return key as CommunicationPanelContentType;
        }
      }
    }

    throw new Error('Unsupported communication panel type.');
  }

  get numberOfNotifications(): number {
    return this.numberOfPanelElements(
      CommunicationPanelContentType.Notifications
    );
  }

  get numberOfMessages(): number {
    return this.numberOfPanelElements(CommunicationPanelContentType.Messages);
  }

  constructor(
    private readonly router: Router,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly titleService: Title,
    private readonly scrollStrategyOptions: ScrollStrategyOptions
  ) {
    this.scrollStrategy = this.scrollStrategyOptions.block();
  }

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
        filter(() => !!this.communicationPanel),
        tap(() => this.resizeMessagePanel())
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this._observer.disconnect();
  }

  toggle() {
    this.showFullScreenMenu = !this.showFullScreenMenu;
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
    this.togglePanel(CommunicationPanelContentType.Messages);
  }

  toggleNotifications() {
    this.togglePanel(CommunicationPanelContentType.Notifications);
  }

  private togglePanel(type: CommunicationPanelContentType) {
    const panelApperance = this._communicationPanelApperance[type];
    const currentState = panelApperance.show;
    this.resetCommunicationPanelShow();
    panelApperance.show = !currentState;

    if (panelApperance.show) {
      this.changeDetectorRef.detectChanges();
      this.resizeMessagePanel();
    }
  }

  private numberOfPanelElements(type: CommunicationPanelContentType): number {
    return this._communicationPanelApperance[type].elements;
  }

  private resetCommunicationPanelShow(): void {
    for (const key in this._communicationPanelApperance) {
      if (this._communicationPanelApperance.hasOwnProperty(key)) {
        this._communicationPanelApperance[
          key as CommunicationPanelContentType
        ].show = false;
      }
    }
  }

  private resizeMessagePanel(): void {
    const top = this.communicationPanel.nativeElement.offsetTop;
    this.communicationPanel.nativeElement.style.height = `${
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

enum CommunicationPanelContentType {
  Messages = 'messages',
  Notifications = 'notifications',
}
