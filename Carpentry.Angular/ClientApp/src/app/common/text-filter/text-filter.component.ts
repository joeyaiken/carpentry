import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
//TODO - consider removing, I kinda think this component just overly complicates things
//  Followup - it does NOT overly complicate things, I don't want to check how a material input is structured 100 times
@Component({
    selector: 'app-text-filter',
    templateUrl: 'text-filter.component.html',
    styleUrls:['text-filter.component.less'],
})
export class TextFilterComponent implements OnInit {
    @Input() name: string;
    @Input() value: string;
    @Output() valueChange = new EventEmitter<string>();

    // @Output() onFilterChange = new EventEmitter<void>();//: (event: React.ChangeEvent<HTMLInputElement>) => void;

    constructor(){}
    ngOnInit(): void {}

    // filterChange(): void {
    //     this.onFilterChange.emit();
    // }
    onChange(newValue: string) {
        this.value = newValue;
        this.valueChange.emit(this.value);
    }
}
