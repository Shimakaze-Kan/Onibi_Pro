import { Component, Inject, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { catchError, of, switchMap, tap } from 'rxjs';
import { ShipmentsClient } from '../../api/api';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';
import { IUserType } from '../delivery/delivery.component';

@Component({
  selector: 'app-confirm-delivery',
  templateUrl: './confirm-delivery.component.html',
  styleUrls: ['./confirm-delivery.component.scss'],
})
export class ConfirmDeliveryComponent {
  @ViewChild('scanner', { static: false }) scanner!: ZXingScannerComponent;
  showCamera = false;
  cameraNumber = -1;
  firstRun = true;
  cameras: Array<MediaDeviceInfo> = [];
  showNoCameraException = false;
  currentCamera: MediaDeviceInfo | undefined = undefined;
  loading = false;

  get title(): string {
    if (!this.userType) {
      return '';
    }

    if (this.userType.isCourier) {
      return 'Pick Up Package';
    } else if (this.userType.isManager) {
      return 'Confirm Delivery';
    }

    return '';
  }

  confirmDeliveryForm = new FormGroup({
    code: new FormControl<string>('', Validators.required),
  });

  constructor(
    private readonly dialogRef: MatDialogRef<
      ConfirmDeliveryComponent,
      { reload: boolean }
    >,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private userType: IUserType,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly shipmentClient: ShipmentsClient,
    private readonly snackBar: MatSnackBar
  ) {}

  onClose() {
    this.dialogRef.close({ reload: false });
  }

  camerasFoundHandler(event: Array<MediaDeviceInfo>): void {
    this.cameras = event;
    this.currentCamera = undefined;
  }

  camerasNotFoundHandler(_: unknown): void {
    this.showCamera = false;
    this.showNoCameraException = true;
  }

  nextCamera(): void {
    this.firstRun = false;
    this.cameraNumber++;
    if (this.cameraNumber > this.cameras.length - 1) {
      this.cameraNumber = 0;
    }
    this.scanner.restart();
    this.currentCamera = this.cameras[this.cameraNumber];
  }

  onCodeResult(event: string): void {
    this.confirmDeliveryForm.controls.code.setValue(event);
    this.showCamera = false;
  }

  openScanner(): void {
    this.firstRun = true;
    this.showCamera = true;
    this.cameraNumber = -1;
    this.confirmDeliveryForm.controls.code.setValue('');
  }

  accept(): void {
    const packageId = this.confirmDeliveryForm.controls.code.value!;

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => {
          if (this.userType.isCourier) {
            return this.shipmentClient.pickupShipment(packageId);
          } else if (this.userType.isManager) {
            return this.shipmentClient.confirmDelivery(packageId);
          }

          throw new Error('Unsupported user type.');
        }),
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
