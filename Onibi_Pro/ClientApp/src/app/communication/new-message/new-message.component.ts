import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { ReplaySubject, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-new-message',
  templateUrl: './new-message.component.html',
  styleUrls: ['./new-message.component.scss'],
})
export class NewMessageComponent implements OnInit, OnDestroy {
  @Output() messageSent: EventEmitter<void> = new EventEmitter<void>();
  @Input() title: string = '';
  @Input() receiverId: number | undefined = undefined;
  @Input() text: string = '';
  private _onDestroy$ = new Subject<void>();
  newMessageForm = new FormGroup({
    title: new FormControl<string>('', Validators.required),
    receivers: new FormControl<Array<number>>([], [this.atLeastOneValidator]),
    text: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(250),
    ]),
  });

  receiversFilterCtrl = new FormControl<string>('');
  filteredReceivers = new ReplaySubject<IReceiver[]>(1);

  ngOnInit(): void {
    this.newMessageForm.setValue({
      title: this.title,
      receivers: !!this.receiverId ? [this.receiverId] : [],
      text: this.text,
    });
    this.filteredReceivers.next(this.receivers.slice());
    this.receiversFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterReceivers();
      });
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onSubmit(): void {
    this.messageSent.emit();
  }

  private filterReceivers() {
    if (!this.receivers) {
      return;
    }

    let search = this.receiversFilterCtrl.value;
    if (!search) {
      this.filteredReceivers.next(this.receivers.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredReceivers.next(
      this.receivers.filter((receivers) =>
        receivers.name.toLowerCase().includes(search!)
      )
    );
  }

  private atLeastOneValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const array = control.value;
    if (array && Array.isArray(array) && array.length > 0) {
      return null;
    }
    return { atLeastOne: true };
  }

  receivers: IReceiver[] = [
    { id: 1, name: 'twoja stara' },
    { id: 2, name: 'zapierdala' },
    { id: 3, name: 'po' },
    { id: 4, name: 'pewexie' },
  ];
}

interface IReceiver {
  id: number;
  name: string;
}
