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
import {
  ReplaySubject,
  Subject,
  debounceTime,
  distinctUntilChanged,
  map,
  merge,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { GetUsersResponse, IdentityClient } from '../../api/api';
import { ISendMessage } from '../message-manager/message-manager.component';
import { IMessageRecipient } from '../services/message.service';
import { TextHelperService } from '../../utils/services/text-helper.service';

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
  isLoadingFirstChunkOfUsers = false;
  newMessageForm = new FormGroup({
    title: new FormControl<string>('', Validators.required),
    receivers: new FormControl<Array<GetUsersResponse>>(
      [],
      [this.atLeastOneValidator]
    ),
    text: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(250),
    ]),
  });

  receiversFilterCtrl = new FormControl<string>('');
  filteredReceivers = new ReplaySubject<GetUsersResponse[]>(1);

  get receiverNames(): string {
    return (
      this.newMessageForm.controls.receivers.value?.map(
        (x) => `${x.firstName} ${x.lastName}`
      ) || []
    ).join(', ');
  }

  get receivers(): Array<GetUsersResponse> {
    return this.newMessageForm.controls.receivers.value || [];
  }

  constructor(
    private readonly identityClient: IdentityClient,
    private readonly textHelper: TextHelperService
  ) {}

  ngOnInit(): void {
    this.isLoadingFirstChunkOfUsers = true;
    of({})
      .pipe(
        switchMap(() =>
          this.receiverId
            ? this.identityClient.users(this.receiverId, undefined)
            : []
        ),
        tap((receiverResult) => {
          const receivers = receiverResult || [];
          this.newMessageForm.setValue({
            title: this.title,
            receivers: receivers,
            text: this.text,
          });
          this.filteredReceivers.next(receivers);

          this.isLoadingFirstChunkOfUsers = false;
        })
      )
      .subscribe();

    this.receiversFilterCtrl.valueChanges
      .pipe(
        takeUntil(this._onDestroy$),
        debounceTime(200),
        distinctUntilChanged(),
        switchMap((value) => {
          if (!value || value.trim() === '') {
            return of([]);
          } else {
            return this.identityClient.users(undefined, value.toLowerCase());
          }
        }),
        map((users) => {
          const keyValueReceivers = this.receivers.map((receiver) => [
            receiver.id || '',
            receiver,
          ]);
          const keyValueUsers = users.map((receiver) => [
            receiver.id || '',
            receiver,
          ]);

          const combinedUsers =
            // @ts-ignore:next-line ts is just stupid here
            [...new Map([...keyValueReceivers, ...keyValueUsers]).values()];

          return combinedUsers as GetUsersResponse[];
        }),
        tap((users) => this.filteredReceivers.next(users))
      )
      .subscribe();
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
      recipients:
        rawFormValues.receivers?.map(
          (x) =>
            ({
              name: `${x.firstName} ${x.lastName}`,
              userId: x.id,
            } as IMessageRecipient)
        ) || [],
    };
    this.messageSent.emit(message);
  }

  splitCamelCase(value: string | undefined): string {
    return this.textHelper.splitCamelCaseToString(value);
  }

  compareReceivers = (r1: GetUsersResponse, r2: GetUsersResponse): boolean => {
    return r1.id === r2.id;
  };

  private atLeastOneValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const array = control.value;
    if (array && Array.isArray(array) && array.length > 0) {
      return null;
    }
    return { atLeastOne: true };
  }
}
