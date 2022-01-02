import {Component, EventEmitter, Input, Output, ViewEncapsulation} from "@angular/core";
import {FilterOption} from "../../settings/models";
@Component({
  encapsulation: ViewEncapsulation.None,
  selector: 'app-select-field',
  templateUrl: 'select-field.component.html',
  styleUrls: ['select-field.component.less'],
})
export class SelectFieldComponent {
  @Input() title: string;
  @Input() filterOptions: FilterOption[];
  @Input() value: string;
  @Output() valueChange = new EventEmitter<string>();
  constructor() {}
  onChange(newValue: string){
    this.value = newValue;
    this.valueChange.emit(this.value);
  }
}
