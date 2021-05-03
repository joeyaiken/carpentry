import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { InventoryOverviewDto } from "../../models";



@Component({
    selector: 'app-inventory-card',
    templateUrl: 'inventory-card.component.html',
    styleUrls: ['inventory-card.component.less'],
})
export class InventoryCardComponent implements OnInit {
    @Input() cardItem: InventoryOverviewDto;

    @Output() onCardSelected = new EventEmitter<number>(); //: (cardId: number) => void;

    constructor() {}
    ngOnInit(): void {}
    
    cardSelected(cardId: number): void {
        this.onCardSelected.emit(cardId);
    }
}