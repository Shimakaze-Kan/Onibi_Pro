import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PermissionChecker {
  constructor(private readonly auth: AuthService) {}

  canActivateAnything(): Observable<boolean> {
    return this.auth.isAuthenticated().pipe(
      tap((isAuthenticated) => {
        if (!isAuthenticated) {
          window.location.href = '/';
        }
      })
    );
  }
}
