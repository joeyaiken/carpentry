import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { DeckCardOverview } from "../../models";

@Component({
    selector: 'app-deck-card-grid',
    templateUrl: 'deck-card-grid.component.html',
    styleUrls: ['deck-card-grid.component.less'],
})
export class DeckCardGridComponent implements OnInit {
    @Input() cardOverviews: DeckCardOverview[];

    @Output() onCardSelected = new EventEmitter<DeckCardOverview>();

    constructor() {}

    ngOnInit(): void {}

    cardSelected(overview: DeckCardOverview): void {
        this.onCardSelected.emit(overview);
    }
}