import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { CardSearchService } from "src/app/common/card-search.service";
import { FilterService } from "src/app/common/filter-service";
import { CardSearchQueryParameter } from "src/app/common/models";
import { AppFiltersDto } from "src/app/settings/models";
import { InventoryService } from "../inventory.service";
import {
  CardListItem,
  cardSearchResultDetail,
  CardSearchResultDto,
  InventoryFilterProps,
  InventoryQueryParameter,
  NewInventoryCard,
  PendingCardsDto
} from "../models";


export class SelectedCardDetail {
    imageUrl: string;
    // cardName: string;
    name: string;
    cardId: number;
}

@Component({
    selector: 'app-inventory-add-cards',
    templateUrl: 'inventory-add-cards.component.html',
    styleUrls: ['inventory-add-cards.component.less'],
})
export class InventoryAddCardsComponent implements OnInit {
  // @Input() cardItem: InventoryOverviewDto;

  // @Output() onCardSelected = new EventEmitter<number>(); //: (cardId: number) => void;
  // @Output() onToggleViewClick = new EventEmitter<void>();


  isBusy: boolean = false;
  apiSearchResults: CardSearchResultDto[] = [];

  // selectedCard: CardSearchResultDto | null;
  selectedCard: CardListItem | null = null;
  // selectedCardDetails: SelectedCardDetail[];



  viewMode: string;
  // searchFilter: InventoryFilterProps; //CardSearchQueryParameter is what's expected by the API
  //Should just store something that can be send straight to the api
  // (or at least not use something with tons of unused fields)
  searchFilter: CardSearchQueryParameter; //CardSearchQueryParameter is what's expected by the API

  filterOptions: AppFiltersDto;

  searchResults: CardListItem[];
  // pendingCards: PendingCardsDto[];
  pendingCards: { [name: string]: PendingCardsDto }

  private testSearch: boolean = true;

  constructor(
      private filterService: FilterService,
      // private inventoryService: InventoryService,
      private cardSearchService: CardSearchService,
  ) {

  }

    ngOnInit(): void {
        this.searchFilter = new CardSearchQueryParameter();
        this.searchResults = [];
        this.pendingCards = {};
        this.viewMode = 'list';
        this.loadFilterOptions();

        if(this.testSearch) this.runTestSearch();
        
    }


    // mapCardDetails(searchResult: CardSearchResultDto): SelectedCardDetail[] {
    //     return searchResult.details.map(card => ({
    //         cardId: card.cardId,
    //         imageUrl: card.imageUrl,
    //         name: card.name,
    //     } as SelectedCardDetail))
    // }

    mapCardDetail(card: cardSearchResultDetail): SelectedCardDetail {
        return {
            cardId: card.cardId,
            imageUrl: card.imageUrl,
            name: card.name,
        } as SelectedCardDetail;
    }

    onToggleView(): void {
        this.viewMode = (this.viewMode === "list") ? "grid" : "list";
    }

    onSearchClick(): void {
        this.trySearchCards();
    }

    loadFilterOptions(): void {
        // this.isBusy = true;
        this.filterService.getAppFilterOptions().subscribe(
          (data) => {
                this.filterOptions = data;
                // this.isBusy = false;
            }

        )
    }
    onSaveClick(){
        alert('not implemented yet')
        // this.props.dispatch(requestSavePendingCards());
    }

    test(){ 
        this.getPendingCardCount("test");
        this.getPendingCardCount("test", 1, true);
    }


    getPendingCardCount(cardName: string, cardId: number = null, isFoil: boolean = null): string {
        var pendingCard = this.pendingCards[cardName];
        if(!pendingCard) return "";
        if(cardId && isFoil != null) {
            //not implemented, get this pending card
            //  if doesn't exist, should return 0, not emptystring
            //  If exists, should get count of all pending cards matching isFoil
        }

        //not implemented, should return count of all new cards
        //Note that if there are 0 pending cards, a null should have been returned earlier
        //But, as a failsafe, I'll return an emptystring


        //get all card IDs matching this card name
        //sum count of pending cards for all card IDs



        return "";
    }

    addPendingCard(name: string, cardId: number, isFoil: boolean){
        let cardToAdd: PendingCardsDto = this.pendingCards[name];
        if(!cardToAdd){
            cardToAdd = {
                name: name,
                cards: [],
            };
        }

        //These are the only 3 fields used by the api bulkAdd
        cardToAdd.cards.push({
            cardId: cardId,
            isFoil: isFoil,
            statusId: 1,
        } as NewInventoryCard);

        this.pendingCards[name] = cardToAdd;
        this.updateCounts();
    }

    removePendingCard(name: string, cardId: number, isFoil: boolean){
        let objToRemoveFrom = this.pendingCards[name];
        if(objToRemoveFrom) {
            let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);
            if(thisInvCard >= 0){
                objToRemoveFrom.cards.splice(thisInvCard,1);
            }

            //if this pending cards object has 0 items, it should be deleted from the dictionary
            if(objToRemoveFrom.cards.length === 0){
                delete this.pendingCards[name];
            }

            this.updateCounts();
        }
    }

    onCardSelected(item: CardListItem){
        this.selectedCard = item;
    }

    // handleSearchButtonClick(){
    //     this.props.dispatch(requestSearch())
    // }

    // handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
    //     this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    // }

    // handleBoolFilterChange(filter: string, value: boolean): void {
    //     this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
    // }

    private trySearchCards(): any{
        if(this.isBusy){
            return;
        }
        this.isBusy = true;

        // const currentFilterProps = this.searchFilter;
        // const param: CardSearchQueryParameter = {
        //     text: currentFilterProps.text,
        //     colorIdentity: currentFilterProps.colorIdentity ?? [],
        //     exclusiveColorFilters: currentFilterProps.exclusiveColorFilters,
        //     multiColorOnly: currentFilterProps.multiColorOnly,
        //     rarity: currentFilterProps.rarity,
        //     set: currentFilterProps.set,
        //     type: currentFilterProps.type,
        //     searchGroup: currentFilterProps.searchGroup,
        //     excludeUnowned: false,
        // }

        this.cardSearchService.searchInventory(this.searchFilter).subscribe((results) => {
            this.isBusy = false;
            this.apiSearchResults = results;

            //a 'card list item' represents a search result, & the number of pending cards [with that same name?]
            this.searchResults = results.map(card => ({
                data: card,
                count: this.pendingCards[card.name]?.cards?.length,
            } as CardListItem));

            if(this.testSearch) this.test_selectFirstCard();
        });
    }


    //I may need to rethink this general component state
    //  I need to hold a local collection of pending cards
    //  For each search result, I need to know the total pending cards for that name
    //      I also need to know that # for displaying pending cards
    //  For each unique card, I need to know # of pending normal cards, and # of pending foil cards


    //I could: store things in a list that I search through
    //  Either unique cardsToAdd, or grouped by unique print (card number)

    //  This would make adding cards easy, but UI updates slower

    //Or I could have some nested classes that I can dig into
    //  Search Result would just know names, which is all it needs
    //  Same logic for pending cards
    //  Details would know 
    
    //  This would require aggregating for saving, but could make navigation easier
    //      Details could even just pull data with a function taking name & card number or something
    //      I could use this as a chance to save local data too (to click a pending card & see the details)

    private updateCounts(){
        this.searchResults.forEach(result => {
            if(this.pendingCards[result.data.name]){
                var pendingCards = this.pendingCards[result.data.name].cards;
                



                result.count = this.pendingCards[result.data.name].cards.length;
            } else if(result.count) {
                result.count = null;
            }
        })
    }





    private test_selectFirstCard() {
        if(this.searchResults?.length){
            const firstResult = this.searchResults[0];
            this.onCardSelected(firstResult);
            this.addPendingCard(firstResult.data.name, firstResult.data.cardId, false);
        }
    }

    private runTestSearch() {
      this.searchFilter.set = 'khm';
      this.searchFilter.searchGroup = 'RareMythic';
      this.trySearchCards();
    }
}
