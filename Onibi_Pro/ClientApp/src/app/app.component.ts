import { ViewportScroller } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subject, filter, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnDestroy {
  private readonly _destroy$ = new Subject<void>();
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
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
