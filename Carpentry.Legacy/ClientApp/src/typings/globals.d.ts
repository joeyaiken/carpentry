/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

//interfaces here are only used by the client app, not consuming services
//Other interfaces are stored in models.d.ts

// declare enum AppContainerEnum {
//     None,
//     //primary containers
//     DeckEditor,
//     Inventory,


//     //secondary containers
//     CardSearch,
    
// }

//declare type DataStateOption =  'coreFilterOptions' | 'deckList' | 'deckDetail' | 'inventoryOverview' | 'inventoryDetail' | 'cardSearchResults' | 'cardSearchInventoryDetail'

declare type AppContainerEnum = 'deckEditor' | 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null;

// declare type AppPrimaryContainerType = null | 'Inventory' | 'DeckEditor' | 'BuyList';
// declare type AppSecondaryContainerType = null | 'Inventory' | 'CardSearch';
// declare type AppModalContainerType = null | 'DeckQuickAdd';


declare type AppBarButtonType = 'add' | 'filter' | 'menu';

declare type CardSearchViewMode = 'grid' | 'list';

declare type DeckEditorViewMode = 'grid' | 'list' | 'grouped';
declare type DeckEditorCardMenuOption = "commander" | "sideboard" | "mainboard" | "inventory" | "delete" | "search";

declare type FilterPropOptions = 'inventoryFilterProps' | 'cardSearchFilterProps';

declare type MenuAnchorOptions = 'deckListMenuAnchor' | 'deckEditorMenuAnchor';

declare type InventorySearchMethod =  "name" | "quantity" | "price"; // | "sellList";
declare type InventoryViewMode = "list" | "grid";


declare type ApiScopeOption = 'coreFilterOptions' | "deckList" | "deckDetail" | "inventoryOverview" | "inventoryDetail" | "cardSearchResults" | "cardSearchInventoryDetail" ;


declare interface CardListItem {
    data: MagicCard;
    count?: number;
}

declare interface DeckViewOptions {
    view: "img" | "list";
    group: "none" | "type" | "rarity" | "set";
    sort: "name" | "manaCost"; //| "rarity";
}


//need an object that represents a group of cards in the DE
//This represents grouped card names
declare interface NamedCardGroup {
    name: string;
    cardOverviewIds: number[];
}
//This represents actual grouped card overviews
declare interface CardOverviewGroup {
    name: string;
    cardOverviews: DeckCardOverview[];
}


declare interface CardGroup {
   id: number;
   name: string;
}

declare interface PendingCardsDto {
    // data: MagicCard;
    multiverseId: number;
    name: string;
    cards: InventoryCard[];
}

declare interface CoreFilterOptions {
    sets: FilterOption[];
    types: FilterOption[];
    colors: FilterOption[];
    rarities: FilterOption[];
}

///




declare interface FilterOption {
    name: string;
    value: string;
}

declare interface ReduxAction extends AnyAction {
    type: any, //Should be combined type of all fuckin action types?
    error?: any,
    payload?: any,
    meta?: any
}

declare interface DisplayCard {
    data: MagicCardDto;
    id: string;
    countNormal: number;
    countFoil: number;
}

declare interface InventoryItem {
    id: string;
    data: MagicCardDto;
    countNormal: number;
    countFoil: number;
}

//represents a MTG card
// declare interface MTGCard {
//     //Guess I'll just put relevant info by ID
//     cmc: number;
//     colorIdentity: string[];
//     colors: string[];
//     flavor: string | undefined;
//     id: string;
//     imageUrl: string;
//     layout: string;
//     manaCost: string;
//     multiverseid: number;
//     name: string;
//     number: string;
//     printings: string[];
//     rarity: string;
//     set: string;
//     setName: string;
//     text: string;
//     type: string;
//     types: string[];
// }

declare interface InventoryDisplayItem {
    name: string;
    data: MagicCardDto;
    // instances: InventoryItemSetData[];
    deckCount: number;
    inventoryCount: number;
    // cards: Card[];
}

declare interface InventoryItemSetData {
    set: string;
    count: number;
    price: number;
    countFoil: number;
    priceFoil: number;
}

declare interface IMagicCard { //don't like this name, it holds the data needed by a visual deck card
    //name
    // cardId: string;
    card: Card;
    data: MagicCardDto;
}

declare interface IDeckCard {
    name: string;
    set: string;
}

declare interface ILandCount {
    R: number,
    U: number,
    G: number,
    W: number,
    B: number,
}

declare interface ICardDeck {
    //an index
    id: number;

    //some settings / properties
    details: IDeckDetail;

    //some statistics
    stats?: IDeckStats | null;

    //a collection of cards
    cards: IDeckCard[];
    //basic lands (for now)
    basicLands: ILandCount;
}

declare interface ICardIndex {
    [set: string]: ICardDictionary;
}
