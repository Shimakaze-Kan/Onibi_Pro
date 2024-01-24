import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  AuthenticationClient,
  CreateCourierRequest,
  RegionalManagersClient,
  RegisterRequest,
} from '../../api/api';
import { catchError, of, switchMap, tap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-courier',
  templateUrl: './add-courier.component.html',
  styleUrls: ['./add-courier.component.scss'],
})
export class AddCourierComponent {
  loading = false;

  newCourierForm = new FormGroup({
    email: new FormControl<string>('', Validators.required),
    phone: new FormControl<string>('', Validators.required),
    firstName: new FormControl<string>('', Validators.required),
    lastName: new FormControl<string>('', Validators.required),
  });

  constructor(
    private readonly dialogRef: MatDialogRef<
      AddCourierComponent,
      { reload: boolean }
    >,
    private readonly authenticationClient: AuthenticationClient,
    private readonly regionalManager: RegionalManagersClient,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly snackBar: MatSnackBar
  ) {}

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  createCourier(): void {
    const rawValues = this.newCourierForm.getRawValue();

    const request = new RegisterRequest({
      email: rawValues.email || '',
      firstName: rawValues.firstName || '',
      lastName: rawValues.lastName || '',
      userType: 'Courier',
    });

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.authenticationClient.register(request)),
        switchMap((userId) =>
          this.regionalManager.courierPost(
            new CreateCourierRequest({
              phone: rawValues.phone || '',
              userId: userId,
            })
          )
        ),
        tap(() => {
          this.loading = false;
          this.dialogRef.close({ reload: true });
        }),
        catchError((error) => {
          const description = this.errorParser.extractErrorMessage(
            JSON.parse(error.response)
          );
          this.snackBar.open(description, 'close', { duration: 5000 });
          this.loading = false;

          return of(error);
        })
      )
      .subscribe();
  }
}
