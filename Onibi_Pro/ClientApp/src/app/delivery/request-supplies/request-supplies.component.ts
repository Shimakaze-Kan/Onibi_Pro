import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import {
  ReplaySubject,
  Subject,
  catchError,
  of,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { ShowQrCodeComponent } from '../show-qr-code/show-qr-code.component';
import { GetIngredientResponse, MenusClient } from '../../api/api';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-request-supplies',
  templateUrl: './request-supplies.component.html',
  styleUrls: ['./request-supplies.component.scss'],
})
export class RequestSuppliesComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();
  private _editingIndexes: Array<number> = [];
  readonly currentDate = new Date();
  ingredients: Array<GetIngredientResponse> = [];
  loading = false;
  supplyOperation = SupplyOperation;

  ingredientFilterCtrl = new FormControl<string>('');
  filteredIngredients = new ReplaySubject<GetIngredientResponse[]>(1);

  newSupplyForm = this.fb.group({
    until: new FormControl<Date | undefined>(undefined, Validators.required),
    isUrgent: new FormControl<boolean>(false),
    ingredients: this.fb.array<ISupply>([]),
  });

  tmpSupplyForm = this.fb.group({
    name: new FormControl<string>('', Validators.required),
    unit: new FormControl<string>('', Validators.required),
    amount: new FormControl<string>('', Validators.required),
  });

  get supplies() {
    return this.newSupplyForm.controls.ingredients as FormArray;
  }

  constructor(
    private readonly dialogRef: MatDialogRef<RequestSuppliesComponent>,
    private readonly fb: FormBuilder,
    private readonly dialog: MatDialog,
    private readonly client: MenusClient,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.ingredientFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterIngredients();
      });

    this.newSupplyForm.controls.isUrgent.valueChanges
      .pipe(
        takeUntil(this._onDestroy$),
        tap((state: boolean | null) => {
          const untilControl = this.newSupplyForm.controls.until;
          if (!!state) {
            untilControl.disable();
            untilControl.setValue(new Date());
          } else {
            untilControl.enable();
          }
        })
      )
      .subscribe();

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.ingredients()),
        tap((result) => {
          this.ingredients = result;
          this.filteredIngredients.next(this.ingredients.slice());
          this.loading = false;
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

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onClose(): void {
    this.dialogRef.close();
  }

  addSupplyForm(): void {
    const ingredientForm = this.fb.group({
      name: new FormControl<string>('', Validators.required),
      unit: new FormControl<string>('', Validators.required),
      amount: new FormControl<string>('', Validators.required),
    });

    ingredientForm.patchValue(this.tmpSupplyForm.getRawValue());

    if (ingredientForm.invalid) {
      return;
    }

    this.tmpSupplyForm.reset();

    this.supplies.push(ingredientForm);
  }

  deleteSupply(index: number): void {
    this.supplies.removeAt(index);
  }

  editSupply(index: number): void {
    this._editingIndexes.push(index);
  }

  updateSupply(index: number): void {
    if (this.supplies.at(index).invalid) {
      return;
    }
    this._editingIndexes = this._editingIndexes.filter((x) => x !== index);
  }

  isIndexEdited(index: number): boolean {
    return this._editingIndexes.includes(index);
  }

  addOrUpdate(supplyOperation: SupplyOperation, index: number | undefined) {
    if (supplyOperation === SupplyOperation.Update) {
      this.updateSupply(index!);
    } else {
      this.addSupplyForm();
    }
  }

  onAccept(): void {
    this.onClose();

    this.dialog.open(ShowQrCodeComponent, {
      data: { code: '0a2b90e2-4f29-4a7d-ac71-a64f8e292b56' },
    });
  }

  populateIngredientToForm(event: MatSelectChange, form: FormGroup): void {
    const ingredient = event.value;
    form.get('name')?.setValue(ingredient?.name);
    form.get('unit')?.setValue(ingredient?.unit);
  }

  getIngredientFromForm(form: FormGroup): GetIngredientResponse | undefined {
    const name = form.get('name')?.value;
    const unit = form.get('unit')?.value;

    return this.ingredients.find((x) => x.name === name && x.unit === unit);
  }

  private filterIngredients() {
    if (!this.ingredients) {
      return;
    }

    let search = this.ingredientFilterCtrl.value;
    if (!search) {
      this.filteredIngredients.next(this.ingredients.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredIngredients.next(
      this.ingredients.filter((ingredient) =>
        ingredient.name?.toLowerCase().includes(search!)
      )
    );
  }
}

interface ISupply {
  name: string;
  unit: string;
  amount: string;
}

enum SupplyOperation {
  Add,
  Update,
}
