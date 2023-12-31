import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable, map, tap } from 'rxjs';
import { IdentityService } from '../utils/services/identity.service';
import { ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class PermissionChecker {
  constructor(
    private readonly auth: AuthService,
    private readonly identityService: IdentityService
  ) {}

  canActivateAnything(): Observable<boolean> {
    return this.auth.isAuthenticated().pipe(
      tap((isAuthenticated) => {
        if (!isAuthenticated) {
          window.location.href = '/';
        }
      })
    );
  }

  canActivateUserTypes(next: ActivatedRouteSnapshot): Observable<boolean> {
    const userTypesToCheck = next.data.userTypes as string[];

    return this.identityService
      .getUserData()
      .pipe(map((data) => userTypesToCheck.includes(data.userType || '')));
  }
}
