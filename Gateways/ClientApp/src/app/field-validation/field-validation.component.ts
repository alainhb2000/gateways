import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-field-validation',
  templateUrl: './field-validation.component.html',
  styleUrls: ['./field-validation.component.css']
})

export class FieldValidationComponent {

  @Input('group') group: FormGroup;
  @Input('field') field: string;
  @Input('patternMessage') patternMessage: string = 'Incorrect value';

  fieldErrors: Array<{ key, value }> = [];

  getFieldErrors(): Array<{ key, value }> {
    const f = this.group.get(this.field);
    if (!f.errors) return [];
    return Object.keys(f.errors).map(k => { return { key: k, value: f.errors[k] } });
  }

  isFieldInvalid() {
    const f = this.group.get(this.field);
    return f.errors && (f.touched || f.dirty);
  }

  parseError(e): string {
    switch (e.key) {
      case 'required': return 'Required';
      case 'minlength': return `Minimum length is ${e.value.requiredLength}`;
      case 'pattern': return this.patternMessage;
    }
    return '';
  }

}

