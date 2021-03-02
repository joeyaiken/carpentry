import { Component, Input, OnInit } from "@angular/core";
import { DeckStats } from "../../models";

@Component({
    selector: 'app-deck-stats-bar',
    templateUrl: 'deck-stats-bar.component.html',
    styleUrls: ['deck-stats-bar.component.less']
})
export class DeckStatsBarComponent implements OnInit {
    @Input() deckStats: DeckStats;
    constructor() { }
    ngOnInit(): void { }
}