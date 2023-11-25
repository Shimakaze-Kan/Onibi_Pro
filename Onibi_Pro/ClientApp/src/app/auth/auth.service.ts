import { Injectable } from '@angular/core';
import { AuthenticationClient } from '../api/api';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private readonly client: AuthenticationClient) {}

  logoutCurrentUser(): void {
    this.client
      .logout()
      .pipe(tap(() => (window.location.href = '/')))
      .subscribe();
  }
}
