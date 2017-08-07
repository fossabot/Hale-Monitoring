import { Directive, Attribute } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, Validator } from '@angular/forms';

@Directive({
  selector: '[validateEquals]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: ValidateEqualsDirective,
    multi: true
  }]
})
export class ValidateEqualsDirective implements Validator {
  constructor(
    @Attribute('validateEquals') public validateEquals: string,
    @Attribute('reverse') public reverse: string) {}
  validate(control: AbstractControl): {[key: string]: any} {
    const val = control.value;
    const cmp = control.root.get(this.validateEquals);

    const isReverse = this.reverse === 'true';

    if (isReverse) {
      const areEqual = cmp.value === val;
      if (areEqual) {
        delete cmp.errors['notEqual'];
        if (!Object.keys(cmp.errors).length) {
          cmp.setErrors(null);
        }
      } else {
        cmp.setErrors({ notEqual: true});
      }

      return null;
    }

    return cmp.value !== val
      ? { notEqual: true }
      : null;
  }
}
