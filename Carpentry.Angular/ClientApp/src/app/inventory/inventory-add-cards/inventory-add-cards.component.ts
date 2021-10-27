import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CardSearchService } from "src/app/common/card-search.service";
import { FilterService } from "src/app/common/filter-service";
import { CardSearchQueryParameter } from "src/app/common/models";
import { AppFiltersDto } from "src/app/settings/models";
import { InventoryService } from "../inventory.service";
import {
  CardSearchResultDto,
  NewInventoryCard,
  PendingCardsDto,
  PendingPrintedCard
} from "../models";

@Component({
    selector: 'app-inventory-add-cards',
    templateUrl: 'inventory-add-cards.component.html',
    styleUrls: ['inventory-add-cards.component.less'],
})
export class InventoryAddCardsComponent implements OnInit {
  
  isBusy: boolean = false;
  viewMode: string;
  searchFilter: CardSearchQueryParameter;
  filterOptions: AppFiltersDto;

  searchResults: CardSearchResultDto[] = [];
  selectedCard: CardSearchResultDto | null = null;
  pendingCards: { [name: string]: PendingCardsDto }

  constructor(
      private filterService: FilterService,
      private inventoryService: InventoryService,
      private cardSearchService: CardSearchService,
      private router: Router,
  ) { }

    ngOnInit(): void {
        this.searchFilter = new CardSearchQueryParameter();
        this.searchResults = [];
        this.pendingCards = {};
        this.viewMode = 'list';
        this.loadFilterOptions();
    }

    onToggleView(): void {
        this.viewMode = (this.viewMode === "list") ? "grid" : "list";
    }

    onSearchClick(): void {
        this.trySearchCards();
    }

    onSaveClick(){
        if(this.isBusy) return;

        let newCards: NewInventoryCard[] = [];
        Object.keys(this.pendingCards).forEach(cardName => {
            const pendingCard: PendingCardsDto = this.pendingCards[cardName];
            Object.keys(pendingCard.cardsById).forEach(cardId => {
                const cardPrint: PendingPrintedCard = pendingCard.cardsById[cardId];
                newCards = newCards.concat(cardPrint.newCards);
            });
        });

        this.isBusy = true;
        this.inventoryService.addInventoryCardBatch(newCards).subscribe(result => {
            this.pendingCards = {};
            this.isBusy = false;
            this.router.navigate(['/inventory/']);
        })
    }

    getPendingCardCount(cardName: string, cardId: number = null, isFoil: boolean = null): string {
        var pendingCard = this.pendingCards[cardName];
        if(!pendingCard) return "";

        if(cardId && isFoil != null) {
            let cardPrint = pendingCard.cardsById[cardId];
            if(cardPrint) return (isFoil ? cardPrint.countFoil : cardPrint.countNormal).toString();
            return "0";
        }

        let cardCount = 0;
        Object.keys(pendingCard.cardsById).forEach(key => {
            cardCount += pendingCard.cardsById[key].newCards.length;
        });

        if(cardCount > 0) return cardCount.toString();
        return "";
    }

    addPendingCard(name: string, cardId: number, isFoil: boolean){
        let pendingCard: PendingCardsDto = this.pendingCards[name];
        if(!pendingCard) {
            pendingCard = new PendingCardsDto(name); //TODO - Get relevant card datas & add to this pending card, maybe through the constructor
            console.log("pending card created", pendingCard);
            this.pendingCards[name] = pendingCard;
        }

        var cardPrint = pendingCard.cardsById[cardId];
        if(!cardPrint) {
            cardPrint = new PendingPrintedCard();
            pendingCard.cardsById[cardId] = cardPrint;
        }
        
        cardPrint.newCards.push(new NewInventoryCard(cardId, isFoil, 1));

        isFoil ? cardPrint.countFoil++ : cardPrint.countNormal++;
    }

    removePendingCard(name: string, cardId: number, isFoil: boolean) {
        let pendingCard = this.pendingCards[name];
        if(pendingCard) {
            var cardPrint = pendingCard.cardsById[cardId];

            var invCardIndex = cardPrint.newCards.findIndex(c => c.isFoil === isFoil);

            if(invCardIndex >= 0) {
                cardPrint.newCards.splice(invCardIndex, 1);
                isFoil ? cardPrint.countFoil-- : cardPrint.countNormal--;
            }

            let cardCount = 0;
            Object.keys(pendingCard.cardsById).forEach(key => {
                cardCount += pendingCard.cardsById[key].newCards.length;
            });

            if(cardCount == 0) delete this.pendingCards[name];
        }
    }

    onCardSelected(item: CardSearchResultDto){
        this.selectedCard = item;
    }

    private loadFilterOptions(): void {
        this.filterService.getAppFilterOptions().subscribe((data) => { this.filterOptions = data; });
    }

    private trySearchCards(): any{
        if(this.isBusy){
            return;
        }
        this.isBusy = true;
        this.cardSearchService.searchInventory(this.searchFilter).subscribe((results) => {
            this.searchResults = results;
            this.isBusy = false;
        });
    }
}
