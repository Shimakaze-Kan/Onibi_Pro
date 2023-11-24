import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PermissionChecker {
  constructor() {}

  canActivateAnything(): boolean {
    // const cookieExists = this.cookieService.get('OnibiAuth');

    // if (cookieExists) {
    //   return true;
    // } else {
    //   window.location.href = '/';
    //   return false;
    // }

    return true;
  }
}
