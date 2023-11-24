import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly _baseUrl: string;

  constructor(
    private readonly http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this._baseUrl = baseUrl;
  }

  logoutCurrentUser(): void {
    this.http
      .post(this._baseUrl + 'api/authentication/logout', null, {
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
      })
      .subscribe(
        (result) => {
          window.location.href = '/';
        },
        (error) => console.error(error)
      );
  }
}
