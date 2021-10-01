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

        // this.runTestSearch();
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

    // handleSearchMethodTabClick(name: string): void {
    //     this.props.dispatch(cardSearchSearchMethodChanged(name));
    // }

    // handleToggleViewClick(): void {
    //     this.props.dispatch(toggleCardSearchViewMode());
    // }

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

            // this.test_selectFirstCard();
        });
    }

    private updateCounts(){
        this.searchResults.forEach(result => {
            if(this.pendingCards[result.data.name]){
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
      this.searchFilter.searchGroup = 'Blue';
      this.trySearchCards();
    }
}
