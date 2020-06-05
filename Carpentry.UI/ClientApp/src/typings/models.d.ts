﻿/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

declare interface CardSearchQueryParameter {
    set: string;
    type: string;
    colorIdentity: string[];
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];
}

declare interface DeckCard {
    id: number;
    multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    variantName: string;
    category: string;
}

declare interface DeckCardOverview {
    id: number;
    name: string;
    type: string;
    cost: string;
    cmc: number;
    category: string;
    img: string;
    count: number;
}

declare interface DeckDetailDto {
    props: DeckProperties;
    cardOverviews: DeckCardOverview[];
    cards: DeckCard[];
    stats: DeckStats;
}

declare interface DeckOverviewDto {
    id: number;
    name: string;
    format: string;
    colors: string[];
    isValid: boolean;
    validationIssues: string;
}

declare interface DeckProperties {
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

    // [JsonProperty("sets")]
    // public List<FilterOption> Sets { get; set; }

    // [JsonProperty("types")]
    // public List<FilterOption> Types { get; set; }

    // [JsonProperty("formats")]
    // public List<FilterOption> Formats { get; set; }

    // [JsonProperty("colors")]
    // public List<FilterOption> ManaColors { get; set; }

    // [JsonProperty("rarities")]
    // public List<FilterOption> Rarities { get; set; }

    // [JsonProperty("statuses")]
    // public List<FilterOption> Statuses { get; set; }
}

declare interface FilterOption
{
    name: string;
    value: string;
}

declare interface DeckCardDto {
    id: number;
    deckId: number;
    categoryId: string | null;
    inventoryCard: InventoryCard;
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
    multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    variantName: string;
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
    cards: MagicCard[];
    inventoryCards: InventoryCard[];
}


interface InventoryOverviewDto { //maybe rename this to "CardOverviewDto" ?
    id: number;
    multiverseId: number;
    name: string;
    type: string;
    cost: string;
    img: string;
    count: number;
    description: string;
}

declare interface InventoryQueryParameter {
    //

    //groupBy: "name" | "mid" | "unique";
    groupBy: InventorySearchMethod;
    colors: string[];
    types: string[];
    type: string;
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];

    // //sets: string[];
    set: string;
    // setId: number | null;
    text: string;
    skip: number;
    take: number;
    format: string | null;
    sort: string;
    sortDescending: boolean;
    // //other things to add?
    // //Format / Legality
    minCount: number;
    maxCount: number;

    // [JsonProperty("statusId")]
    // public int StatusId { get; set; }
}

declare interface MagicCard {
    cmc: number | null;
    colorIdentity: string[];
    colors: string[];
    manaCost: string;
    multiverseId: number;
    name: string;
    prices: { [key: string]: number | null }
    variants: { [key: string]: string | null }
    legalities: string[];
    rarity: string;
    set: string;
    text: string;
    type: string;
}