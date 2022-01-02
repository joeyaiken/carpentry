import {Component, EventEmitter, Input, Output, ViewEncapsulation} from "@angular/core";

@Component({
  encapsulation: ViewEncapsulation.None,
  selector: 'app-numeric-field',
  templateUrl: 'numeric-field.component.html',
  styleUrls: ['numeric-field.component.less'],
})
export class NumericFieldComponent {
  @Input() title: string;
  @Input() value: number;
  @Output() valueChange = new EventEmitter<number>();
  constructor() {}
  onChange(newValue: string){
    this.value = +newValue;
    this.valueChange.emit(this.value);
  }
}
