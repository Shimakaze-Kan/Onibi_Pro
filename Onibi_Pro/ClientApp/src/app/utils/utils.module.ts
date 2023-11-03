import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormatMailPipe } from './pipes/format-mail.pipe';

@NgModule({
  declarations: [FormatMailPipe],
  imports: [CommonModule],
  exports: [FormatMailPipe],
})
export class UtilsModule {}
