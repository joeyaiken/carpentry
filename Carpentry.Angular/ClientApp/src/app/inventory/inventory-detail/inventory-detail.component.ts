import { Component, OnInit } from "@angular/core";
import { CardOverviewGroup, DeckCardOverview, DeckDetailDto, DeckPropertiesDto, GroupedCardOverview } from "src/app/decks/models";
import { InventoryCard, MagicCard } from "../models";

@Component({
  selector: 'app-inventory-detail',
  templateUrl: 'inventory-detail.component.html',
  styleUrls: ['inventory-detail.component.less'],
})
export class InventoryDetailComponent implements OnInit {
  
    // @Input() deckId: number;

    // deckDetail: DeckDetailDto;

    // deckId: number;
    // viewMode: 'grid' | 'list' | 'grouped'; // DeckEditorViewMode;
    
    // deckProperties: DeckPropertiesDto | null;

    // // formatFilterOptions: FilterOption[];

    // // deckStats: DeckStats | null;

    // // isPropsDialogOpen: boolean;

    // // deckDialogProperties: DeckPropertiesDto | null;

    // // groupedCardOverviews: CardOverviewGroup[];
    // // cardDetailsById: { [deckCardId: number]: DeckCardDetail };

    // // cardMenuAnchor: HTMLButtonElement | null;
    // // cardMenuAnchorId: number;

    // // selectedCard: DeckCardOverview | null;
    // // selectedInventoryCards: DeckCardDetail[];

    // // selectedCardId: number;
    // // isCardDetailDialogOpen: boolean;
    // // isCardTagsDialogOpen: boolean;

    // // isExportDialogOpen: boolean;
    // // selectedExportType: DeckExportType;

    // // cardOverviews: {
    // //     byId: { [id: number]: DeckCardOverview }
    // //     allIds: number[];
    // // };

    // // cardDetails: {
    // //     byId: { [deckCardId: number]: DeckCardDetail };
    // //     allIds: number[];
    // // };


    // // cardGroups: NamedCardGroup[];
    
    // groupedCardOverviews: CardOverviewGroup[];

    // newGroupingMethod: GroupedCardOverview[];

    
    // // selectedOverviewCardId: number | null;
    // selectedOverview: DeckCardOverview | null;


    // selectedDetailItem: InventoryDetailDto;
    modalIsOpen: boolean;
    selectedCardId: number;

    // selectedCardId: number;
    cardsById: { [cardId: number]: MagicCard }
    allCardIds: number[];
    inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
    cardGroups: { [cardId: number]: number[] }

    cardName: string;

    
    constructor() { }
    ngOnInit(): void {
      // this.dialogTitle = `Inventory Detail - ${cardName}`;
      this.loadInventoryDetail(); //TODO - subscribe and whatnot


      this.setCardName();



    }
    
    private loadInventoryDetail(): void {
      //get card id from dialog props

      //do whatever react does...


      //--------------------------


      //this.props.dispatch(ensureInventoryDetailLoaded(this.props.selectedCardId))
      

      // //Has a load been requested?
      // //  if yes, return
      // //So this could be two variables: queryIsLoaded, loadedId
      // //  OR...wait still 2 variables: queryIsLoading, loadedId
      // //  Okay, so I guess I won't change that approach

      // // if(queryInProgress){
      // //     return;
      // // }
      // const selectedCardId = state.inventory.data.detail.selectedCardId;
      // const queryIsLoading = state.inventory.data.detail.isLoading;

      // if(queryIsLoading || cardId === selectedCardId){
      //     return;
      // }

      // //So, should I dispatch this data action, or add a local action somewhere?
      // dispatch(inventoryDetailRequested(cardId));

      // inventoryApi.getInventoryDetail(cardId).then((result) => {
      //     dispatch(inventoryDetailReceived(result));
      // }).catch((error) => {
      //     dispatch(inventoryDetailReceived(null));
      // });


    }

    //   const inventoryDetailRequested = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    //     const selectedCardId = action.payload;
    //     const newState: InventoryDataReducerState = {
    //         ...state,
    //         detail: {
    //             ...initialState.detail,
    //             isLoading: true,
    //             selectedCardId: selectedCardId, //will be number
    //         }
    //     };
    //     return newState;
    // }


    //   const inventoryDetailReceived = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    //     const detailResult: InventoryDetailDto | null = action.payload;
    
    //     if(detailResult === null){
    //         return {
    //             ...initialState,
    //         }
    //     }
    
    //     let inventoryCardsById = {}
    //     detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);
    //     const allInventoryCardIds = detailResult.inventoryCards.map(card => card.id);
    
    //     let cardsById = {}
    //     detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);
    //     const allCardIds = detailResult.cards.map(card => card.multiverseId);
    
    //     //fill card groups
        
    //     let cardGroups: { [cardId: number]: number[] } = {};
    //     // let cardGroupIds: number[];
    //     allCardIds.forEach(cardId => {
    //         const thisCard = cardsById[cardId];
    //         // cardGroupIds.push()
    //         const inventoryCardIds = allInventoryCardIds
    //             .filter(inventoryCardId => inventoryCardsById[inventoryCardId].set === thisCard.set 
    //                 && inventoryCardsById[inventoryCardId].collectorNumber === thisCard.collectionNumber);
    //         cardGroups[cardId] = inventoryCardIds;
    //     });
        
    //     //
    
    
    //     const newState: InventoryDataReducerState = {
    //         ...state,
    //         detail: {
    //             selectedCardId: state.detail.selectedCardId,
    //             selectedCardName: detailResult.name,
    //             isLoading: false,
    
    //             inventoryCardsById: inventoryCardsById,
    //             inventoryCardAllIds: allInventoryCardIds,
                
    //             cardsById: cardsById,
    //             allCardIds: allCardIds,
    
    //             cardGroups: cardGroups,
    //         },
    //     };
    //     return newState;
    // }


  //   function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
  //     // const containterState = state.
  //     const detailData = state.inventory.data.detail;
  //     const result: PropsFromState = {
  //         // selectedDetailItem: selectInventoryDetail(state),
  //         // modalIsOpen: state.ui.isInventoryDetailModalOpen, //TODO - get this mapped from router state
  //         selectedCardId: ownProps.match.params.cardId,
  //         allCardIds: detailData.allCardIds,
  //         cardsById: detailData.cardsById,
  //         cardGroups: detailData.cardGroups,
  //         inventoryCardsById: detailData.inventoryCardsById,
  //         modalIsOpen: Boolean(ownProps.match.params.cardId != null),
  //     }
  //     return result;
  // }

    private setCardName(){
      this.cardName = "Unknown";

      if(this.allCardIds.length > 0){
        this.cardName = this.cardsById[this.allCardIds[0]].name;
      }
    }
}