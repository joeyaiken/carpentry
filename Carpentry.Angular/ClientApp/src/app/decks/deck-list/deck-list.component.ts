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



    getDeckOverviews(): Promise<DeckOverviewDto[]> {
        // return this.decksService.getDeckOverviews()
        //     .then(result => {
        //         return result });
        console.log('component get overviews');

        //TODO - replace promise with observable
        return this.decksService.getDeckOverviews().then(result => {
            console.log('assigning overviews');
            this.deckOverviews = result;
            console.log(result);

            return result;
        });
    }

    //process deck overviews ?? 
}