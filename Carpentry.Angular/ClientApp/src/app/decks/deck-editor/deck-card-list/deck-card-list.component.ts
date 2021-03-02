import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { DeckCardOverview } from "../../models";

@Component({
    selector: 'app-deck-card-list',
    templateUrl: 'deck-card-list.component.html',
    styleUrls: ['deck-card-list.component.less']
})
export class DeckCardListComponent implements OnInit {
    @Input() cardOverviews: DeckCardOverview[];
    @Output() onCardSelected = new EventEmitter<DeckCardOverview>();

    constructor() { }
    ngOnInit(): void { }    

    cardSelected(card: DeckCardOverview): void {
        this.onCardSelected.emit(card);
    }
}