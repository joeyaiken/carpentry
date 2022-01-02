import {Component, Inject, OnInit} from "@angular/core";
import {InventoryCard, MagicCard} from "../models";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {InventoryService} from "../inventory.service";

export interface InventoryDetailComponentProps {
  //action: string;
  cardId: number,
}

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

  // selectedDetailResult: InventoryDetailDto;

  modalIsOpen: boolean;
  selectedCardId: number;

  // selectedCardId: number;
  cardsById: { [cardId: number]: MagicCard } = {};
  allCardIds: number[] = [];

  inventoryCardsById: { [inventoryCardId: number]: InventoryCard } = {};
  allInventoryCardIds: number[] = [];

  cardGroups: { [cardId: number]: number[] } = {};

  cardName: string;

  displayedColumns: string[] = ['style','status'];

  constructor(
    public dialogRef: MatDialogRef<InventoryDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: InventoryDetailComponentProps,
    public inventoryService: InventoryService,
  ) {
    this.selectedCardId = data.cardId;
  }
  ngOnInit(): void {

    // this.dialogTitle = `Inventory Detail - ${cardName}`;

    this.loadInventoryDetail();


    // console.log('setting card name');

  }

  private loadInventoryDetail(): void {

    this.inventoryService.getInventoryDetail(this.selectedCardId).subscribe(detailResult => {

      //TODO - Should dictionary creation & card grouping be done on the server?
      //  This logic is duplicated between both app versions, moving to the controller could simply things

      // const selectedDetailResult = detailResult;

      this.inventoryCardsById = {}
      detailResult.inventoryCards.forEach(invCard => this.inventoryCardsById[invCard.id] = invCard);
      this.allInventoryCardIds = detailResult.inventoryCards.map(invCard => invCard.id);

      this.cardsById = {}
      detailResult.cards.forEach(card => this.cardsById[card.cardId] = card);
      this.allCardIds = detailResult.cards.map(card => card.cardId);


      this.cardGroups = {};

      //for each card
      this.allCardIds.forEach(cardId => {

        //get the card
        const thisCard = this.cardsById[cardId];

        this.cardGroups[cardId] = this.allInventoryCardIds
          .filter(inventoryCardId =>
            this.inventoryCardsById[inventoryCardId].set === thisCard.set
            &&
            this.inventoryCardsById[inventoryCardId].collectorNumber === thisCard.collectionNumber
          );

      });

      this.setCardName();

    });

    //get card id from dialog props

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

  public cardDefinitions(): MagicCard[] {
    return this.allCardIds.map(cardId => this.cardsById[cardId]);
  }

  public getGroupCards(cardId: number): InventoryCard[] {
    // return [];


    const thisGroup = this.cardGroups[cardId];
    // console.log('cardId', cardId);
    // console.log('cardGroups', this.cardGroups)
    // console.log('thisGroup',thisGroup);
    return thisGroup.map(inventoryCardId => this.inventoryCardsById[inventoryCardId]);
  }

  public isFoilString(card: InventoryCard): string {
    return card.isFoil ? "foil" : "normal";
  }

  public cardStatusString(card: InventoryCard): string {
    if(card.deckId) return card.deckName;
    switch (card.statusId){
      case 1: return "Inventory";
      case 2: return "Wish List";
      case 3: return "Sell List";
      default: return "";
    }
  }

  public cardGroupTitleString(card: MagicCard): string {
    return `${card.set} (${card.collectionNumber}) - $${card.price} | $${card.priceFoil}`;
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
    // if(this.selectedDetailResult && this.selectedDetailResult.cards){
    //   this.cardName = this.selectedDetailResult.cards[0].name;
    // }
    if(this.allCardIds.length > 0){
      this.cardName = this.cardsById[this.allCardIds[0]].name;
    }
  }

  public closeClick() {
    alert('close!')
    // this.dialogRef.close();
  }
}
