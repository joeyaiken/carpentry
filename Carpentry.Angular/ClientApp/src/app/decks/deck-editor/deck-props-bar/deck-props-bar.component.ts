import { Component, OnInit } from "@angular/core";
import { DeckPropertiesDto } from "../../models";

@Component({
    selector: 'app-deck-props-bar',
    templateUrl: 'deck-props-bar.component.html',
    styleUrls: ['deck-props-bar.component.less']
})
export class DeckPropsBarComponent implements OnInit {
    deckProperties: DeckPropertiesDto;
    
    constructor() {
        this.deckProperties = {
            id: 0,
            name: "loading...",
            notes: '',
            format: '',
            basicB: 0,
            basicG: 0,
            basicR: 0,
            basicU: 0,
            basicW: 0,
        }
    }

    ngOnInit(): void {
        
    }

    onToggleViewClick(): void {

    }

    onEditClick(): void {

    }

    onExportClick(): void {

    }

    onAddCardsClick(): void {

    }
}