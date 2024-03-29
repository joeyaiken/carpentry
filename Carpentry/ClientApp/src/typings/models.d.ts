﻿/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

declare interface CardImportDto {
    importType: number; //should really be removed
    importPayload: string;
    //Thoughts: props can just be added on a validated payload
}

declare interface ValidatedDeckImportDto {
    isValid: boolean;
    deckProps: DeckPropertiesDto;
    validatedCards: ValidatedCardDto[];
    untrackedSets: ValidatedDtoUntrackedSet[];
}

declare interface ValidatedCardDto
{
    sourceString: string;
    isValid: boolean;
    isBasicLand: boolean;
    isEmpty: boolean;

    count: number;
    name: string;
    category: string | null;

    cardId: number;
    setCode: string;
    collectorNumber: number;
    isFoil: boolean;
}

declare interface ValidatedDtoUntrackedSet {
    setId: number;
    setCode: string;
}

declare interface ImportListRecord {
    count: number;
    name: string;
    code: string;
    number: number;
    isFoil: boolean;
    category: string | null;
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
    //format: null | DeckFormatOption;
    format: string;
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
    totalCount: number;
    deckCount: number;
    inventoryCount: number;
    sellCount: number;
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

// Used in SETTINGS area

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

declare interface InventoryTotalsByStatusResult {
    statusId: number;
    statusName: string;
    totalPrice: number;
    totalCount: number;
}

// End used in SETTINGS area

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

declare interface TrimmingToolRequest {
    setCode: string;
    searchGroup: string;
    minCount: number;
    maxPrice: number;
    filterBy: string;
}

declare interface TrimmingToolResult {
    id: number;
    cardId: number;
    name: string;
    isFoil: boolean | null;
    printDisplay: string;
    price: number;
    unusedCount: number;
    totalCount: number;
    allPrintsCount: number;
    recommendedTrimCount: number;
    imageUrl: string;
}

declare interface TrimmedCardDto {
    cardName: string; //TODO - Can this be removed?
    cardId: number;
    isFoil: boolean;
    numberToTrim: number;
}

declare interface NewInventoryCard {
    cardId: number;
    isFoil: boolean;
    statusId: number;
}

declare interface NormalizedList<T> {
    byId: { [key:number]: T };
    allIds: number[];
}