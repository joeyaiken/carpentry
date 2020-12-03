/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

declare type ApiScopeOption = 
    'coreFilterOptions' | 
    "deckOverviews" | 
    "deckDetail" | 
    "inventoryOverview" | 
    "inventoryDetail" | 
    "cardSearchResults" | 
    "cardSearchInventoryDetail" |
    "trackedSets";

declare type DeckFormatOption = 'Standard' | 'Legacy' | 'Modern' | 'Commander' | 'Oathbreaker';

declare type DeckEditorCardMenuOption = "commander" | "sideboard" | "mainboard" | "inventory" | "delete" | "search";
declare type DeckEditorViewMode = 'grid' | 'list' | 'grouped';


declare interface ReduxAction extends AnyAction {
    type: any, //Should be combined type of all fuckin action types?
    error?: any,
    payload?: any,
    meta?: any
}

declare interface NamedCardGroup {
    name: string;
    cardOverviewIds: number[];
}


interface CardFilterProps {
    set: string;
    // setId: number | null;

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

    group: string | null;
}

interface InventoryFilterProps {
    groupBy: InventoryGroupMethod;
    sortBy: InventorySortMethod;
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

declare interface DeckCardDetail {
    id: number;
    overviewId: number;
    // multiverseId: number;
    name: string;
    set: string;
    isFoil: boolean;
    // variantName: string;
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
    detailIds: number[];
}

///////
// /// <reference types="react-scripts" />
// /// <reference types="redux" />
// /// <reference types="react-redux" />
// /// <reference types="redux-thunk" />

// //interfaces here are only used by the client app, not consuming services
// //Other interfaces are stored in models.d.ts

// // declare enum AppContainerEnum {
// //     None,
// //     //primary containers
// //     DeckEditor,
// //     Inventory,


// //     //secondary containers
// //     CardSearch,
    
// // }

// //declare type DataStateOption =  'coreFilterOptions' | 'deckList' | 'deckDetail' | 'inventoryOverview' | 'inventoryDetail' | 'cardSearchResults' | 'cardSearchInventoryDetail'

// declare type AppContainerEnum = 'deckEditor' | 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null;

// // declare type AppPrimaryContainerType = null | 'Inventory' | 'DeckEditor' | 'BuyList';
// // declare type AppSecondaryContainerType = null | 'Inventory' | 'CardSearch';
// // declare type AppModalContainerType = null | 'DeckQuickAdd';


// declare type AppBarButtonType = 'add' | 'filter' | 'menu';

declare type CardSearchViewMode = 'grid' | 'list';




// declare type FilterPropOptions = 'inventoryFilterProps' | 'cardSearchFilterProps';

// declare type MenuAnchorOptions = 'deckListMenuAnchor' | 'deckEditorMenuAnchor';

declare type InventorySearchMethod =  "name" | "mid" | "unique"; //"name" | "quantity" | "price"; // | "sellList";

declare type InventoryGroupMethod = "name" | "print" | "unique";
declare type InventorySortMethod = "name" | "price" | "cmc" | "count" | "collectorNumber";
// declare type InventoryViewMode = "list" | "grid";


// declare type ApiScopeOption = 'coreFilterOptions' | "deckList" | "deckDetail" | "inventoryOverview" | "inventoryDetail" | "cardSearchResults" | "cardSearchInventoryDetail" ;


declare interface CardListItem {
    data: CardSearchResultDto;
    count?: number;
}

// declare interface DeckViewOptions {
//     view: "img" | "list";
//     group: "none" | "type" | "rarity" | "set";
//     sort: "name" | "manaCost"; //| "rarity";
// }


// //need an object that represents a group of cards in the DE
// //This represents grouped card names
// declare interface NamedCardGroup {
//     name: string;
//     cardOverviewIds: number[];
// }

//This represents actual grouped card overviews (used in Deck Editor at leasat)
declare interface CardOverviewGroup {
    name: string;
    cardOverviews: DeckCardOverview[];
}


// declare interface CardGroup {
//    id: number;
//    name: string;
// }

//Represents a payload of cards to be added
//1 model = multiple inventory cards for a unique CardId
//Group by Name or CardId ??
//Fuck it, grouping by name for now
declare interface PendingCardsDto {
    // data: MagicCard;
    // multiverseId: number;
    // cardId: number;
    name: string;
    cards: InventoryCard[]; //this might need to be something else
}
//pendingCards: { [name: string]: PendingCardsDto } //key === name, should this also have a list to track all keys?

// declare interface PendingCardsCollection {

// }

// declare interface CoreFilterOptions {
//     sets: FilterOption[];
//     types: FilterOption[];
//     colors: FilterOption[];
//     rarities: FilterOption[];
// }

// ///




// declare interface FilterOption {
//     name: string;
//     value: string;
// }

// declare interface ReduxAction extends AnyAction {
//     type: any, //Should be combined type of all fuckin action types?
//     error?: any,
//     payload?: any,
//     meta?: any
// }

// declare interface DisplayCard {
//     data: MagicCardDto;
//     id: string;
//     countNormal: number;
//     countFoil: number;
// }

// declare interface InventoryItem {
//     id: string;
//     data: MagicCardDto;
//     countNormal: number;
//     countFoil: number;
// }

// //represents a MTG card
// // declare interface MTGCard {
// //     //Guess I'll just put relevant info by ID
// //     cmc: number;
// //     colorIdentity: string[];
// //     colors: string[];
// //     flavor: string | undefined;
// //     id: string;
// //     imageUrl: string;
// //     layout: string;
// //     manaCost: string;
// //     multiverseid: number;
// //     name: string;
// //     number: string;
// //     printings: string[];
// //     rarity: string;
// //     set: string;
// //     setName: string;
// //     text: string;
// //     type: string;
// //     types: string[];
// // }

// declare interface InventoryDisplayItem {
//     name: string;
//     data: MagicCardDto;
//     // instances: InventoryItemSetData[];
//     deckCount: number;
//     inventoryCount: number;
//     // cards: Card[];
// }

// declare interface InventoryItemSetData {
//     set: string;
//     count: number;
//     price: number;
//     countFoil: number;
//     priceFoil: number;
// }

// declare interface IMagicCard { //don't like this name, it holds the data needed by a visual deck card
//     //name
//     // cardId: string;
//     card: Card;
//     data: MagicCardDto;
// }

// declare interface IDeckCard {
//     name: string;
//     set: string;
// }

// declare interface ILandCount {
//     R: number,
//     U: number,
//     G: number,
//     W: number,
//     B: number,
// }

// declare interface ICardDeck {
//     //an index
//     id: number;

//     //some settings / properties
//     details: IDeckDetail;

//     //some statistics
//     stats?: IDeckStats | null;

//     //a collection of cards
//     cards: IDeckCard[];
//     //basic lands (for now)
//     basicLands: ILandCount;
// }

// declare interface ICardIndex {
//     [set: string]: ICardDictionary;
// }
