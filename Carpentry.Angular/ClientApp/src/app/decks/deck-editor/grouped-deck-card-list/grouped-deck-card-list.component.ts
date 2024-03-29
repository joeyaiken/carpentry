

import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { CardOverviewGroup, DeckCardDetail, DeckCardOverview, GroupedCardOverview } from "../../models";

@Component({
    selector: 'app-grouped-deck-card-list',
    templateUrl: 'grouped-deck-card-list.component.html',
    styleUrls: ['grouped-deck-card-list.component.less'],
})
export class GroupedDeckCardListComponent implements OnInit {
    //This needs to take a new class that can function as either a row or group (screw that any[] bullshit)
        //That new class is/was the GroupedCardOverviews[]



    @Input() groupedCardOverviews: CardOverviewGroup[]; //Should be replaced
    @Input() cardOverviews: GroupedCardOverview[];
    // @Input() cardDetailsById: { [deckCardId: number]: DeckCardDetail };
    
    @Output() onCardSelected = new EventEmitter<DeckCardOverview>()    
    @Output() onCardDetailClick = new EventEmitter<number>();
    @Output() onCardTagsClick = new EventEmitter<number>();
    

    displayedColumns: string[] = [
        'count','name','tags','cost','status'
    ];

    constructor() {}
    ngOnInit(): void {}

    cardSelected(overview: DeckCardOverview): void {
        // alert('ping1')
        this.onCardSelected.emit(overview);
    }

    cardDetailClick(id: number): void {
        this.onCardDetailClick.emit(id);
    }

    cardTagsClick(id: number): void {
        this.onCardTagsClick.emit(id);
    }


    isGroup(index: number, item: GroupedCardOverview): boolean{
        return item.isGroup;
    }

    public getAvailabilityColor(availabilityId: number): string {
        console.log(`get availability ${availabilityId}`);
        switch(availabilityId){
            case 1: return "green";
            case 2: return "yellow";
            case 3: return "orange";
            case 4: return "red";
            default: return "blue";
        }
    }
}