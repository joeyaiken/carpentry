export class CardImportDto {
    importType: number; //should really be removed
    importPayload: string;
    //Thoughts: props can just be added on a validated payload
}

export class InventoryCard {
    id: number;
    isFoil: boolean;
    statusId: number; //normal === 1, buylist === 2, sellList === 3

    cardId: number;
    name: string;
    set: string;
    collectorNumber: number;

    deckCardId: number | null;
    deckId: number | null;
    deckName: string | null;
    deckCardCategory: string | null;

    // variantName: string;

    //deckCards: InventoryDeckCardDto[];
}

export class InventoryDetailDto {
    cardId: number;
    name: string;
    cards: MagicCard[];
    inventoryCards: InventoryCard[];
}

export class InventoryFilterProps {
    groupBy: string; // "name" | "print" | "unique"; //InventoryGroupMethod;
    sortBy: string; //"name" | "price" | "cmc" | "count" | "collectorNumber"; //InventorySortMethod;
    set: string;
    text: string;
    type: string;
    colorIdentity: string[];
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];
    minCount: number;
    maxCount: number;
    skip: number;
    take: number;
    sortDescending: boolean;
}

export class InventoryOverviewDto { //maybe rename this to "CardOverviewDto" ?
    id: number;
    //card definition properties
    cardId: number;
    setCode: string;
    name: string;
    type: string;
    text: string;
    manaCost: string;
    cmc: number;
    rarityId: string;
    imageUrl: string;
    collectorNumber: number | null;
    color: string;
    colorIdentity: string;
    //prices
    price: number;
    priceFoil: number | null;
    tixPrice: number | null;
    //counts
    totalCount: number;
    deckCount: number;
    inventoryCount: number;
    sellCount: number;
    //
    isFoil: boolean | null; //only populated for ByUnique, otherwise NULL
}

export class InventoryQueryParameter {
    groupBy: string; //InventoryGroupMethod;
    colors: string[];
    type: string;
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];
    set: string;
    text: string;
    skip: number;
    take: number;
    sort: string;
    sortDescending: boolean;
    minCount: number;
    maxCount: number;
}

export class MagicCard {
    cardId: number;
    cmc: number | null;
    colorIdentity: string[];
    colors: string[];
    manaCost: string;
    multiverseId: number;
    name: string;
    // prices: { [key: string]: number | null }
    // variants: { [key: string]: string | null }
    legalities: string[];
    rarity: string;
    set: string;
    text: string;
    type: string;
    collectionNumber: number;
    price: number | null;
    priceFoil: number | null;
    priceTix: number | null;
    imageUrl: string;
}

export class NewInventoryCard {
    constructor(
        cardId: number,
        isFoil: boolean,
        statusId: number
    ){
        this.cardId = cardId;
        this.isFoil = isFoil;
        this.statusId = statusId;
    }
    cardId: number;
    isFoil: boolean;
    statusId: number;
}

export class PendingCardsDto {
    constructor(cardName: string) {
        this.name = cardName;
        this.cardsById = {};
    }
    name: string;
    cardsById: { [cardId: number]: PendingPrintedCard; }
    //TODO - This should store card definitions (so one could click a pending card and see details)
}

export class PendingPrintedCard {
    constructor(){
        this.newCards = [];
        this.countNormal = 0;
        this.countFoil = 0;
    }
    data: CardSearchResultDto;
    newCards: NewInventoryCard[];
    countNormal: number;
    countFoil: number;
}

export class TrimmedCardDto {
  cardName: string;
  cardId: number;
  isFoil: boolean;
  numberToTrim: number;
}

export class TrimmingToolRequest {
  setCode: string;
  searchGroup: string;
  minCount: number;
  maxPrice: number;
  // minBy: string;
  filterBy: string;
}

export class ValidatedCarpentryImportDto {

}




export class CardSearchResultDto {
  cardId: number;
  cmc: number | null;
  colorIdentity: string[];
  colors: string[];
  manaCost: string;
  name: string;
  type: string;
  details: cardSearchResultDetail[];
}

export class cardSearchResultDetail {
    cardId: number;
    setCode: string;
    name: string;
    collectionNumber: number;
    price: number | null;
    priceFoil: number | null;
    priceTix: number | null;
    imageUrl: string;
}

// export class CardListItem {
//     data: CardSearchResultDto;
//     count?: number;
// }
// export class CardListItem extends CardSearchResultDto {
//     count?: number;
// }

