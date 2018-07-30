import { Directive, Attribute, Input, forwardRef } from '@angular/core';
import { AbstractControl, Validator, NG_ASYNC_VALIDATORS } from '@angular/forms';
import { Users } from 'app/api/users';
import 'rxjs/add/operator/debounceTime.js';
import 'rxjs/add/operator/first.js';

@Directive({
  selector: '[appValidateUsername]',
  providers: [{
    provide: NG_ASYNC_VALIDATORS,
    useExisting: forwardRef(() => ValidateUsernameDirective),
    multi: true
  }]
})
export class ValidateUsernameDirective implements Validator {
  constructor(private Users: Users) {}

  validate(control: AbstractControl): {[key: string]: any} {

    return this.Users
      .checkIfAvailable(control.value)
      .map((response: any) => {
        return response !== true
          ? { usernameTaken: true}
          : null;
      });
  }
}
