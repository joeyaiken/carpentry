import { Component, OnInit } from "@angular/core";
// import { resourceUsage } from "process";
// import { CoreService } from "src/app/settings/core.service";
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

    formatFilter: string;
    sortBy: string;
    includeDissasembled: boolean = false;

    private isLoading: boolean = false;

    constructor(
        private decksService: DecksService,
        // private coreService: CoreService,
    ) { }

    ngOnInit(): void {
      this.formatFilter = ""; //"";
      this.sortBy = "color"; //"name";
      this.includeDissasembled = true;
      this.getDeckOverviews();
    }

    private getDeckOverviews(): void {
      if(this.isLoading) return;
      this.isLoading = true;
      this.deckOverviews = [];
      this.decksService.getDeckOverviews(this.formatFilter, this.sortBy, this.includeDissasembled).subscribe(result => {
        this.isLoading = false;
        this.deckOverviews = result;
      }, err => {console.log(`getDeckOverviews error: ${err}`); this.isLoading = false;});
    }


    onFormChange(): void {
        this.getDeckOverviews();
    }

    // getFormatOptions(): void {
    //     // this.coreServce.
    // }

}
