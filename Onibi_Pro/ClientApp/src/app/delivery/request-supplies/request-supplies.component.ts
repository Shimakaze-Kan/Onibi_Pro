import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  NgForm,
  Validators,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ReplaySubject, Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-request-supplies',
  templateUrl: './request-supplies.component.html',
  styleUrls: ['./request-supplies.component.scss'],
})
export class RequestSuppliesComponent implements OnInit, OnDestroy {
  private readonly _onDestroy$ = new Subject<void>();
  private _editingIndexes: Array<number> = [];
  readonly currentDate = new Date();
  supplyOperation = SupplyOperation;

  productFilterCtrl = new FormControl<string>('');
  filteredProducts = new ReplaySubject<string[]>(1);

  newSupplyForm = this.fb.group({
    until: new FormControl<Date | undefined>(undefined, Validators.required),
    isUrgent: new FormControl<boolean>(false),
    products: this.fb.array<ISupply>([]),
  });

  tmpSupplyForm = this.fb.group({
    name: new FormControl<string>('', Validators.required),
    amount: new FormControl<string>('', Validators.required),
  });

  get supplies() {
    return this.newSupplyForm.controls.products as FormArray;
  }

  constructor(
    private readonly dialogRef: MatDialogRef<RequestSuppliesComponent>,
    private readonly fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.filteredProducts.next(this.products.slice());
    this.productFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy$))
      .subscribe(() => {
        this.filterProducts();
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
  }

  ngOnDestroy(): void {
    this._onDestroy$.next();
    this._onDestroy$.complete();
  }

  onClose(): void {
    this.dialogRef.close();
  }

  addSupplyForm(): void {
    const productForm = this.fb.group({
      name: new FormControl<string>('', Validators.required),
      amount: new FormControl<string>('', Validators.required),
    });

    productForm.patchValue(this.tmpSupplyForm.getRawValue());

    if (productForm.invalid) {
      return;
    }

    this.tmpSupplyForm.reset();

    this.supplies.push(productForm);
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

  private filterProducts() {
    if (!this.products) {
      return;
    }

    let search = this.productFilterCtrl.value;
    if (!search) {
      this.filteredProducts.next(this.products.slice());
      return;
    } else {
      search = search.toLowerCase();
    }

    this.filteredProducts.next(
      this.products.filter((product) => product.toLowerCase().includes(search!))
    );
  }

  products = [
    'Burger buns',
    'Ground beef',
    'Chicken fillets',
    'Lettuce',
    'Tomatoes',
    'Cheese slices',
    'Pickles',
    'Ketchup',
    'Mustard',
    'Mayonnaise',
    'French fries',
    'Soft drink cups',
    'Straws',
    'Napkins',
    'Disposable plates',
    'Disposable utensils',
    'Cooking oil',
    'Salt',
    'Pepper',
    'Onions',
    'Ice cream',
    'Cups for ice cream',
    'Milkshakes',
    'Salad dressing',
    'Grill cleaning supplies',
    'Cash register rolls',
    'To-go containers',
    'Cleaning wipes',
    'Trash bags',
    'Hand soap',
    'Disposable gloves',
  ];
}

interface ISupply {
  name: string;
  code: string;
  amount: string;
}

enum SupplyOperation {
  Add,
  Update,
}
