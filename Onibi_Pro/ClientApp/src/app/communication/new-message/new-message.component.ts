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
import { ISendMessage } from '../message-manager/message-manager.component';
import { IMessageRecipient } from '../services/message.service';

@Component({
  selector: 'app-new-message',
  templateUrl: './new-message.component.html',
  styleUrls: ['./new-message.component.scss'],
})
export class NewMessageComponent implements OnInit, OnDestroy {
  @Output() messageSent = new EventEmitter<ISendMessage>();
  @Input() title: string = '';
  @Input() receiverId: string = '';
  @Input() text: string = '';
  private _onDestroy$ = new Subject<void>();
  newMessageForm = new FormGroup({
    title: new FormControl<string>('', Validators.required),
    receivers: new FormControl<Array<IMessageRecipient>>(
      [],
      [this.atLeastOneValidator]
    ),
    text: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(250),
    ]),
  });

  receiversFilterCtrl = new FormControl<string>('');
  filteredReceivers = new ReplaySubject<IMessageRecipient[]>(1);

  ngOnInit(): void {
    this.newMessageForm.setValue({
      title: this.title,
      receivers: [this.receivers.find((x) => x.userId === this.receiverId)!],
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
    const rawFormValues = this.newMessageForm.getRawValue();
    const message: ISendMessage = {
      text: rawFormValues.text || '',
      title: rawFormValues.title || '',
      recipients: rawFormValues.receivers || [],
    };
    this.messageSent.emit(message);
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

  receivers: IMessageRecipient[] = [
    { userId: 'f59cf698-6f65-4902-8593-87e790931cbf', name: 'John Johnson' },
    { userId: 'E7A4344B-6141-422D-9F61-5421958ED8B4', name: 'Donal Trump' },
  ];
}
