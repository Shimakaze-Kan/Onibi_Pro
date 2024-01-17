import { Component, Inject } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, of, switchMap, tap } from 'rxjs';
import {
  AddMenuItemRequest,
  AddMenuItemRequest_Ingredient,
  MenusClient,
} from '../../api/api';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';
import { AddMenuComponent } from '../add-menu/add-menu.component';

@Component({
  selector: 'app-add-menuitem',
  templateUrl: './add-menuitem.component.html',
  styleUrls: ['./add-menuitem.component.scss'],
})
export class AddMenuitemComponent {
  loading = false;
  menuItemForm: FormGroup = this.fb.group(
    {
      name: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0.01)]],
      ingredients: this.fb.array([]),
    },
    { validator: AtLeasOneIngredientValidator }
  );

  getIngredientControls(): FormGroup[] {
    return (this.menuItemForm.get('ingredients') as FormArray)
      .controls as FormGroup[];
  }

  constructor(
    private readonly client: MenusClient,
    private readonly snackBar: MatSnackBar,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly dialogRef: MatDialogRef<
      AddMenuComponent,
      { reload: boolean }
    >,
    private readonly fb: FormBuilder,
    // @ts-ignore
    @Inject(MAT_DIALOG_DATA) private menu: { menuId: string }
  ) {}

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  createIngredient(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      unit: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(0.01)]],
    });
  }

  addIngredient() {
    const ingredients = this.menuItemForm.get('ingredients') as FormArray;
    ingredients.push(this.createIngredient());
  }

  createMenu(): void {
    const menuFormValue = this.menuItemForm.value;

    const request = {
      menuId: this.menu.menuId,
      name: menuFormValue.name,
      price: menuFormValue.price,
      ingredients: menuFormValue.ingredients.map((ingredient: any) => {
        const createIngredient = {
          name: ingredient.name,
          unit: ingredient.unit,
          quantity: ingredient.quantity,
        } as AddMenuItemRequest_Ingredient;
        return createIngredient;
      }),
    } as AddMenuItemRequest;

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.menusPut(request)),
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

const AtLeasOneIngredientValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const ingredients = (control as FormGroup).get('ingredients')?.value;

  return ingredients !== null && ingredients.length > 0
    ? null
    : { ingredients: true };
};
