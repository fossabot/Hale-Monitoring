import { Directive } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, Validator } from '@angular/forms';

@Directive({
  selector: '[validatePassword]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: ValidatePasswordDirective,
    multi: true
  }]
})
export class ValidatePasswordDirective implements Validator {
  validate(contol: AbstractControl): {[key: string]: any} {
    const isValid = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/.test(contol.value);
    console.log(isValid);
    return !isValid
      ? { invalidPassword: true }
      : null;
  }
}
