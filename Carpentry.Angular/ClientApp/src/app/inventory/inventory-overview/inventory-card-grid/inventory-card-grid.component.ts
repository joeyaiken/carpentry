import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { InventoryOverviewDto } from "../../models";

@Component({
    selector: 'app-inventory-card-grid',
    templateUrl: 'inventory-card-grid.component.html',
    styleUrls: ['inventory-card-grid.component.less']
})
export class InventoryCardGridComponent implements OnInit{
    // @Input() cardOverviewsById: { [key: number]: InventoryOverviewDto }
    // @Input() cardOverviewIds: number[];
    @Input() cardOverviews: InventoryOverviewDto[];
    
    @Output() onCardSelected = new EventEmitter<number>(); //: (cardId: number) => void;
    @Output() onInfoButtonEnter = new EventEmitter<void>(); //: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    @Output() onInfoButtonLeave = new EventEmitter<void>(); //: () => void;

    constructor() {}

    ngOnInit(): void {}

    cardSelected(cardId: number): void {
        this.onCardSelected.emit(cardId);
    }
}