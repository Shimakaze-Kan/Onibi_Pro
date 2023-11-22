import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class PermissionChecker {
  constructor(private readonly cookieService: CookieService) {}

  canActivateAnything(): boolean {
    const cookieExists = this.cookieService.get('OnibiAuth');

    if (cookieExists) {
      return true;
    } else {
      window.location.href = '/';
      return false;
    }
  }
}
