import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatMail',
})
export class FormatMailPipe implements PipeTransform {
  transform(text: string): string {
    return text.replace(/(^\s*>+\s*)/gm, (match) => '⠀'.repeat(match.length));
  }
}
