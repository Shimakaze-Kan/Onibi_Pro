import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as printJS from 'print-js';

@Component({
  selector: 'app-show-qr-code',
  templateUrl: './show-qr-code.component.html',
  styleUrls: ['./show-qr-code.component.scss'],
})
export class ShowQrCodeComponent {
  @ViewChild('qrCodeElement') qrCodeElement!: ElementRef;
  qrCode: string;

  constructor(
    private readonly dialogRef: MatDialogRef<ShowQrCodeComponent>,
    @Inject(MAT_DIALOG_DATA) private data: { code: string }
  ) {
    this.qrCode = data.code;
  }

  download(): void {
    const dataUrl = this.getCanvasAsDataUrl();

    this.downloadImage(dataUrl, `onibi_code_${+new Date()}.png`);
  }

  print(): void {
    const dataUrl = this.getCanvasAsDataUrl();

    printJS({
      printable: dataUrl,
      type: 'image',
      header: `Code: ${this.qrCode}`,
      showModal: true,
      modalMessage: 'Printing Qr Code...',
      documentTitle: `onibi_code_${+new Date()}`,
    });
  }

  onClose(): void {
    this.dialogRef.close();
  }

  private downloadImage(data: string, filename = 'untitled.jpeg') {
    const a = document.createElement('a');
    a.href = data;
    a.download = filename;
    a.click();
  }

  private getCanvas(): HTMLCanvasElement {
    return this.qrCodeElement.nativeElement.getElementsByTagName('canvas')[0];
  }

  private getCanvasAsDataUrl(): string {
    const canvas = this.getCanvas();

    return canvas.toDataURL('image/png');
  }

  qrCodeReady(_: unknown) {
    const canvas = this.getCanvas();
    const ctx = canvas.getContext('2d')!;

    ctx.font = '14px serif';
    ctx.fillText(`Code: ${this.qrCode}`, 0, 290);
  }
}
