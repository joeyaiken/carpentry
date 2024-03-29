import { Component, OnInit } from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import { DecksService } from "../decks.service";
import { CardOverviewGroup, DeckCardDetail, DeckCardOverview, DeckDetailDto, DeckPropertiesDto, GroupedCardOverview, NamedCardGroup } from "../models";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog"
import { DeckPropertiesComponentResult, DeckPropsDialogComponent } from "./deck-props-dialog/deck-props-dialog.component";
//TODO - Consider finding a way to show basic lands in the traditional card list, instead of in an AppBar

@Component({
    selector: 'app-deck-editor',
    templateUrl: 'deck-editor.component.html',
    styleUrls: ['deck-editor.component.less']
})
export class DeckEditorComponent implements OnInit {

    // @Input() deckId: number;

    deckDetail: DeckDetailDto;

    deckId: number;
    viewMode: 'grid' | 'list' | 'grouped'; // DeckEditorViewMode;

    deckProperties: DeckPropertiesDto | null;

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

    // cardOverviews: {
    //     byId: { [id: number]: DeckCardOverview }
    //     allIds: number[];
    // };

    // cardDetails: {
    //     byId: { [deckCardId: number]: DeckCardDetail };
    //     allIds: number[];
    // };


    // cardGroups: NamedCardGroup[];

    groupedCardOverviews: CardOverviewGroup[];

    newGroupingMethod: GroupedCardOverview[];


    // selectedOverviewCardId: number | null;
    selectedOverview: DeckCardOverview | null;




    constructor(
        private decksService: DecksService,
        private route: ActivatedRoute,
        private router: Router,
        public dialog: MatDialog,
        ) {

    }
    ngOnInit(): void {
        //get deck props
        // this.decksService

        this.viewMode = "grouped";

        this.deckProperties = new DeckPropertiesDto();
        // this.viewMode = "list";
        this.deckId = +this.route.snapshot.paramMap.get('deckId');

        this.loadDeckDetail();

        // this.cardOverviews = {
        //     byId: {}, allIds: []
        // }
        // this.cardDetails = {
        //     byId: {}, allIds: []
        // }

    }

    loadDeckDetail(): void {


        this.decksService.getDeckDetail(this.deckId).subscribe(result => {
            this.applyDeckDetail(result);

            // this.openPropsDialog();
        }, error => console.log(error));
    }

    //Just going to snag redux logic instead of bothering to refactor out the dictionary for now
    applyDeckDetail(detail: DeckDetailDto){
        this.deckDetail = detail;

        this.deckProperties = detail.props;

        // this.cardOverviews.byId = {};
        // this.cardOverviews.allIds = [];

        // this.cardDetails.byId = {};
        // this.cardDetails.allIds = [];

        this.groupedCardOverviews = [];

        this.newGroupingMethod = [];

        // detail.cards.forEach(cardOverview => {
        //     const overviewId = cardOverview.id;
        //     this.cardOverviews.allIds.push(overviewId);
        //     this.cardOverviews.byId[overviewId] = {
        //         category: cardOverview.category,
        //         cmc: cardOverview.cmc,
        //         cost: cardOverview.cost,
        //         count: cardOverview.count,
        //         detailIds: cardOverview.details.map(detail => detail.id),
        //         id: cardOverview.id,
        //         img: cardOverview.img,
        //         name: cardOverview.name,
        //         type: cardOverview.type,
        //         cardId: cardOverview.cardId,
        //         tags: cardOverview.tags,
        //     };
        //     cardOverview.details.forEach(detail => {
        //         const detailId = detail.id;
        //         this.cardDetails.allIds.push(detailId);
        //         this.cardDetails.byId[detailId] = {
        //             category: detail.category,
        //             id: detail.id,
        //             isFoil: detail.isFoil,
        //             name: detail.name,
        //             overviewId: cardOverview.id,
        //             set: detail.set,
        //             collectorNumber: detail.collectorNumber,
        //             inventoryCardId: detail.inventoryCardId,

        //             inventoryCardStatusId: 0,
        //             cardId: detail.cardId,
        //             deckId: detail.deckId,
        //             availabilityId: detail.availabilityId,
        //         };
        //     });
        // });

        // this.cardGroups = this.selectGroupedDeckCards(this.cardOverviews.byId, this.cardOverviews.allIds);


        const cardGroups = ["Commander", "Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands", "Sideboard"]; //todo should be component constant

        cardGroups.forEach(groupName => {



            const cardsInGroup = detail.cards.filter(card => card.category === groupName);

            if(cardsInGroup.length){
                this.newGroupingMethod.push({
                    isGroup: true,
                    name: groupName,
                } as GroupedCardOverview);

                let mappedCards: GroupedCardOverview[] = cardsInGroup.map(card => ({
                    isGroup: false,
                    ...card,
                }));

                this.newGroupingMethod.push(...mappedCards);
            }



//////////////////////

            // this.newGroupingMethod.push(cardsInGroup.map(card => ({

            // })))

            // this.newGroupingMethod.push(
            //     cardsInGroup.map(card => return new GroupedCardOverview(){
            //         isGroup: false,
            //         ...card,
            //     })

            // )

            // console.log('cards in group')
            // console.log(groupName)
            // console.log(cardsInGroup);
            if(cardsInGroup.length > 0){
                this.groupedCardOverviews.push({
                    name: groupName,
                    // cardOverviewIds: cardsInGroup
                    cardOverviews: cardsInGroup,
                });
            }
        });

    }


    selectGroupedDeckCards(overviewsById: { [id: number]: DeckCardOverview }, allOverviewIds: number[]): NamedCardGroup[] {
        var result: NamedCardGroup[] = [];

        const cardGroups = ["Commander", "Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands", "Sideboard"]; //todo should be component constant

        //Am I worried about the fact that cards might get excluded if I mess up the groups?....

        // console.log('figuring out mappings')
        // console.log(overviewsById);

        cardGroups.forEach(groupName => {

            const cardsInGroup = allOverviewIds.filter(id => overviewsById[id].category === groupName);

            // console.log('cards in group')
            // console.log(groupName)
            // console.log(cardsInGroup);
            if(cardsInGroup.length > 0){
                result.push({
                    name: groupName,
                    cardOverviewIds: cardsInGroup
                });
            }
        });

        return result;
    }

    onToggleViewClick(): void {
        switch(this.viewMode){
            case "list": this.viewMode = "grid"; break;
            case "grid": this.viewMode = "grouped"; break;
            case "grouped": this.viewMode = "list"; break;
            default: this.viewMode = "grouped"; break;
        }
    }

    onEditClick(): void {
        this.openPropsDialog();
    }

    private openPropsDialog(): void {
        this.dialog.open(DeckPropsDialogComponent, {
            width: '500px',
            data: {
                deck: this.deckProperties
            },
            disableClose: true,
        })
        .afterClosed().subscribe((result: DeckPropertiesComponentResult) => {
            // alert(result.action)

            switch(result.action){
                case "save":
                    this.saveDeckProps(result.props)
                    break;
                case "cancel":
                    break;
                case "disassemble":
                    // alert('disassemble not implemented')
                    this.disassembleDeck();
                    break;
                case "delete":
                    alert('delete not implemented')
                    break;
                default:
                    console.log(`PropsDialog cannot recognize action ${result.action}`);
                    alert(`PropsDialog cannot recognize action ${result.action}`)
            }
        });
    }

    private saveDeckProps(props: DeckPropertiesDto) {
        this.decksService.updateDeck(props).subscribe(
            () => this.loadDeckDetail(),
            (error) => alert(`save error ${error}`),
        )
    }

    private disassembleDeck() {
        this.decksService.dissassembleDeck(this.deckProperties.id).subscribe(
            () => this.loadDeckDetail()
            ,(error) => console.log(`disassembleDeck error ${error}`)
        );
    }

    onExportClick(): void {

    }

    onAddCardsClick(): void {
      this.router.navigate(['decks',this.deckId,'add-cards'])
        .then(result => {}, error => console.log(error));
    }

    onCardSelected(card: DeckCardOverview): void {
        // alert('ping')
        //TODO - implement from react app
        // this.selectedOverviewCardId = card.cardId;
        this.selectedOverview = card;

        // const overview: DeckCardOverview = this.deckDetail.cards

        // const selectedOverviewCardId = state.decks.deckEditor.selectedOverviewCardId;
        // if(selectedOverviewCardId){
        //     return state.decks.data.detail.cardOverviews.byId[selectedOverviewCardId];
        // }
        // return null;

    }

}
