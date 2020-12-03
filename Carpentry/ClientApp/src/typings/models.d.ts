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
    details: ApiDeckCardDetail[];
}

declare interface ApiDeckCardDetail {
    id: number;
    overviewId: number;
    //multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    //variantName: string;
    collectorNumber: number | null;
    category: string;
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
    costCounts: {[type: string]: number};
    totalCost: number;
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
    cardId: number;
    // multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    collectorNumber: number;
    // variantName: string;
    statusId: number; //normal === 1, buylist === 2, sellList === 3
    deckCards: InventoryDeckCardDto[];
}

// declare interface InventoryDeckCardDto {
//     id: number;
//     deckId: number;
//     deckName: string;
//     inventoryCardId: number;
//     category: string;
// }

interface InventoryDetailDto {
    name: string;
    cards: MagicCard[];
    inventoryCards: InventoryCard[];
}


interface InventoryOverviewDto { //maybe rename this to "CardOverviewDto" ?
    cardId: number;
    category: string;
    cmc: number;
    cost: string;
    count: number;
    description: string;
    id: number;
    img: string;
    isFoil: boolean;
    name: string;
    price: number;
    type: string;
    variant: string;
    // multiverseId: number;
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
