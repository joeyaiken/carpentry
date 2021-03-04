

import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { CardOverviewGroup, DeckCardDetail, DeckCardOverview } from "../../models";

@Component({
    selector: 'app-grouped-deck-card-list',
    templateUrl: 'grouped-deck-card-list.component.html',
    styleUrls: ['grouped-deck-card-list.component.less'],
})
export class GroupedDeckCardListComponent implements OnInit {
    @Input() groupedCardOverviews: CardOverviewGroup[];
    @Input() cardDetailsById: { [deckCardId: number]: DeckCardDetail };
    
    @Output() onCardSelected = new EventEmitter<DeckCardOverview>()    
    @Output() onCardDetailClick = new EventEmitter<number>();
    @Output() onCardTagsClick = new EventEmitter<number>();

    constructor() {}
    ngOnInit(): void {}

    cardSelected(overview: DeckCardOverview): void {
        this.onCardSelected.emit(overview);
    }

    cardDetailClick(id: number): void {
        this.onCardDetailClick.emit(id);
    }

    cardTagsClick(id: number): void {
        this.onCardTagsClick.emit(id);
    }
}