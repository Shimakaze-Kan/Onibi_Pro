import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TextHelperService {
  splitCamelCaseToString(input: string | undefined): string {
    if (!input) {
      return '';
    }

    return input
      .replace(/([A-Z])/g, ' $1')
      .trim()
      .replace(/^./, (str) => str.toUpperCase());
  }
}
