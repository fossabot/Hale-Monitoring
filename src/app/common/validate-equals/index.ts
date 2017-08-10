import { Directive, Attribute, Input } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, Validator } from '@angular/forms';

@Directive({
  selector: '[appValidateEquals]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: ValidateEqualsDirective,
    multi: true
  }]
})
export class ValidateEqualsDirective implements Validator {
  @Input() appValidateEquals: any;
  constructor(@Attribute('reverse') public reverse: string) {}
  validate(control: AbstractControl): {[key: string]: any} {
    const val = control.value;
    const cmp = this.appValidateEquals.control;

    const isReverse = this.reverse === 'true';

    if (isReverse) {
      const areEqual = cmp.value === val;
      if (areEqual && cmp.errors) {
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
