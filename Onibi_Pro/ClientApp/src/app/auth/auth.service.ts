import { Injectable, Optional, Inject } from '@angular/core';
import { API_BASE_URL, AuthenticationClient } from '../api/api';
import { Observable, map, of, take, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _baseUrl = '';
  private _isAuthenticated = false;
  constructor(
    private readonly client: AuthenticationClient,
    private readonly httpClient: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this._baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : '';
  }

  logoutCurrentUser(): void {
    this.client
      .logout()
      .pipe(
        tap(() => {
          this._isAuthenticated = false;
          window.location.reload();
        })
      )
      .subscribe();
  }

  isAuthenticated(): Observable<boolean> {
    if (!this._isAuthenticated) {
      const salt = new Date().getTime();
      const url = '/api/Authentication/isAuthenticated';

      return this.httpClient
        .get(`${url}?${salt}`, { responseType: 'text' })
        .pipe(
          map((response) => JSON.parse(response) as boolean),
          tap((result) => (this._isAuthenticated = result))
        );
    }

    return of(this._isAuthenticated);
  }
}
