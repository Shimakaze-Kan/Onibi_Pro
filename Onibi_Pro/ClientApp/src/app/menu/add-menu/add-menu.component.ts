import { Component } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, of, switchMap, tap } from 'rxjs';
import {
  CreateMenuRequest,
  CreateMenuRequest_Ingredient,
  CreateMenuRequest_MenuItem,
  MenusClient,
} from '../../api/api';
import { ErrorMessagesParserService } from '../../utils/services/error-messages-parser.service';

@Component({
  selector: 'app-add-menu',
  templateUrl: './add-menu.component.html',
  styleUrls: ['./add-menu.component.scss'],
})
export class AddMenuComponent {
  loading = false;
  menuForm: FormGroup = this.fb.group(
    {
      menuName: ['', [Validators.required]],
      menuItems: this.fb.array([]),
    },
    { validator: AtLeasOneMenuItemValidator }
  );

  get menuItems() {
    return this.menuForm.get('menuItems') as FormArray;
  }

  get menuItemControls(): FormGroup[] {
    return this.menuItems.controls as FormGroup[];
  }

  getIngredientControls(menuItem: AbstractControl): FormGroup[] {
    return (menuItem.get('ingredients') as FormArray).controls as FormGroup[];
  }

  constructor(
    private readonly client: MenusClient,
    private readonly snackBar: MatSnackBar,
    private readonly errorParser: ErrorMessagesParserService,
    private readonly dialogRef: MatDialogRef<
      AddMenuComponent,
      { reload: boolean }
    >,
    private readonly fb: FormBuilder
  ) {}

  onClose(): void {
    this.dialogRef.close({ reload: false });
  }

  createMenuItem(): FormGroup {
    return this.fb.group(
      {
        name: ['', Validators.required],
        price: [0, [Validators.required, Validators.min(0.01)]],
        ingredients: this.fb.array([]),
      },
      { validator: AtLeasOneIngredientValidator }
    );
  }

  createIngredient(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      unit: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(0.01)]],
    });
  }

  addMenuItem() {
    this.menuItems.push(this.createMenuItem());
  }

  addIngredient(menuItemIndex: number) {
    const ingredients = this.menuItems
      .at(menuItemIndex)
      .get('ingredients') as FormArray;
    ingredients.push(this.createIngredient());
  }

  createMenu(): void {
    const menuFormValue = this.menuForm.value;

    const createMenuRequest: CreateMenuRequest = {
      name: menuFormValue.menuName,
      menuItems: menuFormValue.menuItems.map((menuItem: any) => {
        const createMenuItem: CreateMenuRequest_MenuItem = {
          name: menuItem.name,
          price: menuItem.price,
          ingredients: menuItem.ingredients.map((ingredient: any) => {
            const createIngredient: CreateMenuRequest_Ingredient = {
              name: ingredient.name,
              unit: ingredient.unit,
              quantity: ingredient.quantity,
            } as CreateMenuRequest_Ingredient;
            return createIngredient;
          }),
        } as CreateMenuRequest_MenuItem;
        return createMenuItem;
      }),
    } as CreateMenuRequest;

    of({})
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(() => this.client.menusPost(createMenuRequest)),
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

const AtLeasOneMenuItemValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const menuItems = (control as FormGroup).get('menuItems')?.value;

  return menuItems !== null && menuItems.length > 0
    ? null
    : { menuItems: true };
};

const AtLeasOneIngredientValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const ingredients = (control as FormGroup).get('ingredients')?.value;

  return ingredients !== null && ingredients.length > 0
    ? null
    : { ingredients: true };
};
