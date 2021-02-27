import { Component, OnInit } from "@angular/core";
import { resourceUsage } from "process";
import { DecksService } from "../decks.service";
import { DeckOverviewDto } from "../models";

@Component({
    selector: 'app-deck-list',
    templateUrl: './deck-list.component.html',
    styleUrls: ['./deck-list.component.less']
})
export class DeckListComponent implements OnInit {
    displayedColumns: string[] = ['name','format','color','validationIssues'];

    public deckOverviews: DeckOverviewDto[] = [];
    
    constructor(
        private decksService: DecksService
    ) { }

    ngOnInit(): void { 
        this.getDeckOverviews();
    }

    getDeckOverviews(): void {
        this.decksService.getDeckOverviews().subscribe(result => {
            this,this.deckOverviews = result;
        }, err => console.log(`getDeckOverviews error: ${err}`));
    }

}