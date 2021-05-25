import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ApiDeckCardOverview, DeckCardDetail, DeckCardOverview } from "../../models";

@Component({
    selector: 'app-deck-card-detail',
    templateUrl: 'deck-card-detail.component.html',
    styleUrls: ['deck-card-detail.component.less']
    
})
export class DeckCardDetailComponent implements OnInit {
    // @Input() selectedCard: DeckCardOverview | null;
    @Input() selectedCard: ApiDeckCardOverview | null;
    // @Input() inventoryCards: DeckCardDetail[];

    @Output() onMenuClick = new EventEmitter<void>();
    // (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;

    @Output() onMenuClose = new EventEmitter<void>();
    @Output() onCardDetailClick = new EventEmitter<number>();
    @Output() onCardTagsClick = new EventEmitter<number>();

    
    constructor() {}

    ngOnInit(): void {}

    menuClick(): void {
        this.onMenuClick.emit();
    }

    menuClose(): void {
        this.onMenuClose.emit();
    }

    cardDetailClick(): void {
        this.onCardDetailClick.emit();
    }

    cardTagsClick(): void {
        this.onCardTagsClick.emit();
    }
}