import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DecksService } from "../decks.service";
import { DeckCardOverview, DeckDetailDto } from "../models";

@Component({
    selector: 'app-deck-editor',
    templateUrl: 'deck-editor.component.html',
    styleUrls: ['deck-editor.component.less']
})
export class DeckEditorComponent implements OnInit {

    // @Input() deckId: number;

    deckDetail: DeckDetailDto;

    deckId: number;
    viewMode: "list" | "grid"; // DeckEditorViewMode;
    
    // deckProperties: DeckPropertiesDto | null;

    // formatFilterOptions: FilterOption[];

    // deckStats: DeckStats | null;


    // isPropsDialogOpen: boolean;
    // deckDialogProperties: DeckPropertiesDto | null;

    // groupedCardOverviews: CardOverviewGroup[];
    // cardDetailsById: { [deckCardId: number]: DeckCardDetail };

    // cardMenuAnchor: HTMLButtonElement | null;
    // cardMenuAnchorId: number;

    // selectedCard: DeckCardOverview | null;
    // selectedInventoryCards: DeckCardDetail[];

    // selectedCardId: number;
    // isCardDetailDialogOpen: boolean;
    // isCardTagsDialogOpen: boolean;

    // isExportDialogOpen: boolean;
    // selectedExportType: DeckExportType;

    

    constructor(
        private decksService: DecksService,
        private route: ActivatedRoute,
        ) {
        
    }
    ngOnInit(): void {
        //get deck props
        // this.decksService

        this.viewMode = "list";

        this.loadDeckDetail();
    }

    loadDeckDetail(): void {
        const deckId = +this.route.snapshot.paramMap.get('deckId');

        this.decksService.getDeckDetail(deckId).subscribe(result => {
            this.deckDetail = result;
        }, error => console.log(error));
    }


    onToggleViewClick(): void {

    }
    
    onEditClick(): void {
        
    }

    onExportClick(): void {
        
    }

    onAddCardsClick(): void {
        
    }

    onCardSelected(card: DeckCardOverview): void {
        //TODO - implement from react app
    }
    
}