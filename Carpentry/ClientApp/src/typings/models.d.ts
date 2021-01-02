/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

declare interface CardImportDto {

}

declare interface ValidatedDeckImportDto {

}

declare interface ValidatedCarpentryImportDto {
    
}

declare interface CardSearchQueryParameter {
    text: string;
    set: string;
    type: string;
    colorIdentity: string[];
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];
    excludeUnowned: boolean;
    searchGroup: string | null;
}

declare interface CardSearchResultDto {
    cardId: number;
    cmc: number | null;
    colorIdentity: string[];
    colors: string[];
    manaCost: string;
    name: string;
    type: string;
    details: cardSearchResultDetail[];
}

declare interface cardSearchResultDetail {
    cardId: number;
    setCode: string;
    name: string;
    collectionNumber: number;
    price: number | null;
    priceFoil: number | null;
    priceTix: number | null;
    imageUrl: string;
}

// declare interface DeckCard {
//     id: number;
//     multiverseId: number;
//     name: string;
//     set: string;
//     isFoil: boolean;
//     variantName: string;
//     category: string;
// }

// declare interface DeckCardOverview {
//     id: number;
//     name: string;
//     type: string;
//     cost: string;
//     cmc: number;
//     category: string;
//     img: string;
//     count: number;
// }

declare interface DeckDetailDto {
    props: DeckPropertiesDto;
    // cardOverviews: DeckCardOverview[];
    // cards: DeckCard[];
    cards: ApiDeckCardOverview[];
    stats: DeckStats;
}

declare interface ApiDeckCardOverview {
    id: number;
    name: string;
    type: string;
    cost: string;
    cmc: number;
    img: string;
    count: number;
    category: string;
    cardId: number;
    details: ApiDeckCardDetail[];
    tags: string[];
}

declare interface ApiDeckCardDetail {
    id: number;
    deckId: number;
    overviewId: number;
    //multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    //variantName: string;
    collectorNumber: number | null;
    category: string;
    inventoryCardId: number | null;
    cardId: number | null;
    availabilityId: number;
}

declare interface DeckOverviewDto {
    id: number;
    name: string;
    format: string;
    colors: string[];
    isValid: boolean;
    validationIssues: string;
}

declare interface DeckPropertiesDto {
    id: number;
    name: string;
    //format: DeckFormats;
    format: null | DeckFormatOption;
    notes: string;

    basicW: number;
    basicU: number;
    basicB: number;
    basicR: number;
    basicG: number;
}

declare interface DeckStats {
    totalCount: number;
    typeCounts: {[type: string]: number};
    costCounts: {[cost: string]: number};
    tagCounts: {[tag: string]: number}
    totalCost: number;
    colorIdentity: string[];
}


declare interface AppFiltersDto
{
    sets: FilterOption[];
    types: FilterOption[];
    formats: FilterOption[];
    colors: FilterOption[];
    rarities: FilterOption[];
    statuses: FilterOption[];

    groupBy: FilterOption[];
    sortBy: FilterOption[];

    searchGroups: FilterOption[];
}

declare interface FilterOption
{
    name: string;
    value: string;
}

declare interface DeckCardDto {
    id: number;
    deckId: number;
    cardName: string;
    categoryId: string | null;

    inventoryCardId: number | null;
    cardId: number;
    isFoil: boolean;
    inventoryCardStatusId: number;
    // inventoryCard: InventoryCard;
}

// declare interface DeckDto {
//     props: DeckProperties;
//     // cards: Card[];
//     // data: { [key: number]: MagicCard };
//     cardOverviews: InventoryOverviewDto[];
//     cardDetails: InventoryCard[];
//     stats: DeckStats;
// }


// declare interface DeckStats {
//     totalCount: number;
//     typeCounts: { [type: string]: number };
//     costCounts: { [type: string]: number };
//     totalCost: number;
// }

// declare interface FilterDescriptor {
//     name: string;
//     value: any;
// }


// declare interface FilterOption {
//     name: string;
//     value: string;
// }



// interface FilterOptionDto {
//     sets: FilterOption[];
//     types: FilterOption[];
//     formats: FilterOption[];
//     colors: FilterOption[];
//     rarities: FilterOption[];
//     statuses: FilterOption[];
// }

declare interface InventoryCard {
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

// declare interface InventoryDeckCardDto {
//     id: number;
//     deckId: number;
//     deckName: string;
//     inventoryCardId: number;
//     category: string;
// }

interface InventoryDetailDto {
    cardId: number;
    name: string;
    cards: MagicCard[];
    inventoryCards: InventoryCard[];
}


interface InventoryOverviewDto { //maybe rename this to "CardOverviewDto" ?
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
    ownedCount: number;
    deckCount: number;
    //
    isFoil: boolean | null; //only populated for ByUnique, otherwise NULL
}

declare interface InventoryQueryParameter {
    groupBy: InventoryGroupMethod;
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

declare interface MagicCard {
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

declare interface SetDetailDto {
    setId: number;
    code: string;
    name: string;
    dataLastUpdated: Date | null;
    inventoryCount: number;
    collectedCount: number;
    totalCount: number;
    isTracked: boolean;
}



declare interface CardTagDto {
    deckId: number;
    cardName: string;
    tag: string;
}
declare interface CardTagDetailDto {
    cardId: number;
    cardName: string;
    existingTags: CardTagDetailTag[];
    tagSuggestions: string[];
}
declare interface CardTagDetailTag {
    cardTagId: number;
    tag: string;
}