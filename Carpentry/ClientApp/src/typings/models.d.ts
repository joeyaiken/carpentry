/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

///////
//Models from the data layer

// interface DeckCardDto

declare interface DeckCardDto {
    id: number;
    deckId: number;
    categoryId: string | null;
    inventoryCard: InventoryCard;
}

declare interface DeckCard {
    // id: number;
    // deckId: number;
    // card: CardDto;SS
}

interface FilterOptionDto {
        sets: FilterOption[];
        types : FilterOption[];
        formats : FilterOption[];
        colors : FilterOption[];
        rarities : FilterOption[];
        statuses : FilterOption[];
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

interface InventoryDetailDto {
    cards: MagicCard[];
    inventoryCards: InventoryCard[];
    // deckCards: any[];
}



enum DeckFormats {
    None, Standard, Legacy, Modern, Commander, Oathbreaker
}

interface CardFilterVisibilities {
    set: boolean;
    type: boolean;
    color: boolean;
    rarity: boolean;
    name: boolean;
    count: boolean;
    format: boolean;
    text: boolean;
}

interface CardFilterProps{
    set: string;
    
    type: string;

    text: string;

    colorIdentity: string[];
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    
    rarity: string[];

    cardName: string;
    exclusiveName: boolean;

    format: string;

    minCount: number | null;
    maxCount: number | null;
}

interface CardSearchFilter {
    
    searchMethod: "set" | "web" | "inventory";
    // props: CardFilterProps;

    // set: string;
    
    // type: string;

    // colorIdentity: string[];
    // exclusiveColorFilters: boolean;
    // multiColorOnly: boolean;
    
    // rarity: string[];

    // cardName: string;
    // exclusiveName: boolean;

    //eventually an option for that text filter will go here, but I really just need to get things added
}
////////////



///////////////////
interface InventorySearchFilter {
    searchMethod: "name" | "quantity" | "price"; // | "sellList";
    // props: CardFilterProps;
    viewMode: "list" | "grid";
    // sort: string;
    // text: string;
}

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

declare interface InventoryDeckCardDto {
    id: number;
    deckId: number;
    deckName: string;
    inventoryCardId: number;
    category: string;
}

declare interface Card {
    id: number;
    multiverseId: number;
    isFoil: boolean;
    deckId: number | null;
}

declare interface CardCollectionDto {
    cards: Card[];
    data: { [key: number]: MagicCardDto };
}

// declare interface CardDto {
//     card: Card;
//     data: MagicCard;
// }

declare interface DeckDto
{
    props: DeckProperties;
    // cards: Card[];
    // data: { [key: number]: MagicCard };
    cardOverviews: InventoryOverviewDto[];
    cardDetails: InventoryCard[];
    stats: DeckStats;
}


declare interface DeckStats {
    totalCount: number;
    typeCounts: {[type: string]: number};
    costCounts: {[type: string]: number};
    totalCost: number;
}


declare interface DeckProperties {
    id: number;
    name: string;
    //format: DeckFormats;
    format: null | 'Standard' | 'Legacy' | 'Modern' | 'Commander' | 'Oathbreaker';
    notes: string;

    basicW: number;
    basicU: number;
    basicB: number;
    basicR: number;
    basicG: number;
}



declare interface FilterDescriptor {
    name: string;
    value: any;
}

declare interface MagicCardDto {
    cmc: number | null;
    colorIdentity: string[];
    colors: string[];
    flavor: string;
    id: string;
    imageUrl: string;
    layout: string;
    manaCost: string;
    multiverseId: number;
    name: string;
    number: string;
    price: number | null;
    priceFoil: number | null;
    printings: string[];
    rarity: string;
    set: string;
    setName: string;
    text: string;
    type: string;
    types: string[];
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

declare interface InventoryQueryResult {
    name: string;
    cards: MagicCard[];
    items: Card[];
}

declare interface InventoryQueryParameter {
//

    groupBy: string;
    colors: string[];
    types: string[];
    type: string;
    exclusiveColorFilters: boolean;
    multiColorOnly: boolean;
    rarity: string[];

    //sets: string[];
    set: string;
    text: string ;
    skip: number;
    take: number;
    format: string | null;
    sort: string
    //other things to add?
    //Format / Legality
    minCount: number;
    maxCount: number;

}
