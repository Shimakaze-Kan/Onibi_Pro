import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ErrorMessagesParserService {
  extractErrorMessage(response: ErrorResponse): string {
    if (response.errors && Object.keys(response.errors).length > 0) {
      const errorMessages: string[] = [];

      for (const property in response.errors) {
        if (response.errors.hasOwnProperty(property)) {
          const propertyErrors = response.errors[property];
          const propertyErrorMessage = propertyErrors.join(' ');
          errorMessages.push(propertyErrorMessage);
        }
      }

      return errorMessages.join('\n');
    } else if (response.title) {
      return response.title;
    } else {
      return 'An error occurred.';
    }
  }
}

interface ErrorResponse {
  type: string;
  title: string;
  status: number;
  errors?: { [key: string]: string[] };
  traceId: string;
}
