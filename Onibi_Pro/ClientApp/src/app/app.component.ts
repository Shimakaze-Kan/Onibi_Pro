import { Component, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subject, filter, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnDestroy {
  private readonly _destroy$ = new Subject<void>();
  private readonly _assetImages = [
    'assets/imgs/blueprint.jpg',
    'assets/imgs/delivery.jpg',
    'assets/imgs/manager.jpg',
    'assets/logos/onibi.svg',
    'assets/logos/onibi_ico.svg',
    'assets/logos/onibi_plain_s.svg',
  ];
  title = 'app';

  constructor(private readonly router: Router) {
    router.events
      .pipe(
        takeUntil(this._destroy$),
        filter((event) => event instanceof NavigationEnd),
        tap(() => {
          setTimeout(() => window.scrollTo(0, 0), 0);
        })
      )
      .subscribe();

    // all assets images are needed immediately
    this._assetImages.forEach((x) => (new Image().src = x));
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
