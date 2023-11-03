import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';

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

  confirmDeliveryForm = new FormGroup({
    code: new FormControl<string>('', Validators.required),
  });

  constructor(
    private readonly dialogRef: MatDialogRef<ConfirmDeliveryComponent>
  ) {}

  onClose() {
    this.dialogRef.close();
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
}
