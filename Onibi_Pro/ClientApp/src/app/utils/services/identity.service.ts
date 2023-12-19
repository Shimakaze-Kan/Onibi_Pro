import { Injectable } from '@angular/core';
import { GetWhoamiResponse, IdentityClient } from '../../api/api';
import { Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private _userData: GetWhoamiResponse = undefined!;

  constructor(private readonly client: IdentityClient) {}

  getUserData(): Observable<GetWhoamiResponse> {
    if (!!this._userData) {
      return of(this._userData);
    }

    return this.client
      .whoami()
      .pipe(tap((result) => (this._userData = result)));
  }
}
