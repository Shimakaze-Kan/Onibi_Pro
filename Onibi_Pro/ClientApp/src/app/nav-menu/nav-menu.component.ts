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
import { ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import {
  BehaviorSubject,
  Observable,
  Subject,
  filter,
  forkJoin,
  fromEvent,
  map,
  of,
  takeUntil,
  tap,
} from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { ROUTES } from '../app.module';
import { PermissionChecker } from '../auth/permission-checker.service';
import { IdentityService } from '../utils/services/identity.service';
import { TextHelperService } from '../utils/services/text-helper.service';
import { SignalrService } from '../communication/services/signalr.service';

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
  private _numberOfNotifications = 0;
  private _numberOfMessages = 0;
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
  currentUserData$: Observable<ICurrentUser>;
  showLeftScrollButton$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  showRightScrollButton$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  historyRoutes: IHistoryRouteRecord[] = [];

  private _pages: Array<IPage> = [
    { name: 'Main Page', id: 1, url: '/welcome', canActivate: false },
    { name: 'Delivery', id: 2, url: '/delivery', canActivate: false },
    { name: 'Schedule', id: 3, url: '/schedule', canActivate: false },
    {
      name: 'Personel Management',
      id: 4,
      url: '/restaurant-personel-management',
      canActivate: false,
    },
    { name: 'Order', id: 5, url: '/order', canActivate: false },
    {
      name: 'Personel Management',
      id: 6,
      url: '/manager-management',
      canActivate: false,
    },
  ];

  get pages(): Array<IPage> {
    return this._pages.filter((page) => !!page.canActivate);
  }

  get currentPageName(): string {
    return this._pages.find((x) => x?.url === this.currentPageUrl)?.name || '';
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
    return this._numberOfNotifications;
  }

  set numberOfNotifications(value: number) {
    this._numberOfNotifications = value;
  }

  get numberOfMessages(): number {
    return this._numberOfMessages;
  }

  set numberOfMessages(value: number) {
    this._numberOfMessages = value;
  }

  constructor(
    private readonly router: Router,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly titleService: Title,
    private readonly scrollStrategyOptions: ScrollStrategyOptions,
    private readonly authService: AuthService,
    private readonly permissionChecker: PermissionChecker,
    private readonly identityService: IdentityService,
    private readonly textHelper: TextHelperService,
    private readonly signalRService: SignalrService
  ) {
    this.scrollStrategy = this.scrollStrategyOptions.block();
    this.currentUserData$ = this.identityService.getUserData().pipe(
      map((result) => ({
        email: result.email || '',
        userType: textHelper.splitCamelCaseToString(result.userType),
      }))
    );
  }

  ngAfterViewInit(): void {
    const navContainerHeight =
      this.navContainer.nativeElement.getBoundingClientRect().height;
    this.placeholder.nativeElement.style.height = `${navContainerHeight}px`;

    this.updatePagesAccess()
      .pipe(
        tap(() => {
          setTimeout(() => {
            this.observeScrollContainer();
          }, 0);
        })
      )
      .subscribe();
  }

  ngOnInit(): void {
    this.signalRService.amountOfNotifications
      .pipe(
        takeUntil(this._destroy$),
        tap((num) => (this._numberOfNotifications = num))
      )
      .subscribe();

    this.signalRService.amountOfMessages
      .pipe(
        takeUntil(this._destroy$),
        tap((num) => (this._numberOfMessages = num))
      )
      .subscribe();

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

  logout(): void {
    this.authService.logoutCurrentUser();
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

  updatePagesAccess() {
    const accessChecks$ = this._pages.map((page) => {
      const routeConfig = ROUTES[0].children?.find(
        (r) => r.path === page.url.replace('/', '')
      );
      if (routeConfig) {
        if (routeConfig.data) {
          const routeSnapshot = new ActivatedRouteSnapshot();
          routeSnapshot.data = routeConfig.data;

          return this.permissionChecker
            .canActivateUserTypes(routeSnapshot)
            .pipe(map((canActivate) => ({ page, canActivate })));
        } else {
          return of({ page, canActivate: true });
        }
      } else {
        return of({ page, canActivate: false });
      }
    });

    return forkJoin(accessChecks$).pipe(
      tap((results) => {
        results.forEach((result) => {
          result.page.canActivate = result.canActivate;
        });
      })
    );
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

interface IPage {
  name: string;
  id: number;
  url: string;
  canActivate: boolean;
}

interface ICurrentUser {
  email: string;
  userType: string;
}
