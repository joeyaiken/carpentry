/// <reference types="mtgsdk-ts" />






//interfaces for AddPack containter component
interface IAddPackSection {
    //cards: IAddPackCard[];
    cards: ICard[];
    name: string;
    totalCount?: number;
    color?: string;//styling info
    isVisible?: boolean;
    isOpen?: boolean;
    //sort priority?
}

interface IAddPackCard {
    data: ICard;
    normalCount: number;
    foilCount: number;
    color?: string;//styling info
}




interface thisNewAddPackState {

    //groups: all potential groups of cards


    //cards: dictionary of cards?

}







declare interface IUIState {
    isNavOpen: boolean;
    isSideSheetOpen: boolean;
    visibleSideSheet: string;
    // selectedDeckId: string;

    selectedDeckId: number | null;
}

declare interface IDeckEditorState {
    deckView: string;
    deckGroup: string;
    deckSort: string;
    deckFilter: string;

    
    selectedCard: string | null;

    //selected deck ID ? Or does that belong in UI ?
    //IDK how one will reach the other so maybe both for now? Ugh


    //active deck visible cards?

    //active deck cards?

    //section visibilities

    //DUMP
    activeDeckVisibleCards: IMagicCard[];
    sectionVisibilities: boolean[];


}

//Need a data object
//possible other thigns to include
//  selected deck id
//  active deck visible cards
//  maybe even section visibilities?
//  probably at least sections
declare interface IDataStore {
    //Is this actually the most recent data store?

    //Architect - needs the list of decks
    deckList: ICardDeck[];
    //this may eventually be too much data


    //selectedCard: string | null;
    selectedCard: IDeckCard | null;

    selectedDeckId: number | null; // | null ?
    //  active deck visible cards

    //  maybe even section visibilities?
    
    //  probably at least sections

    deckList: ICardDeck[];
    cardIndex: ICardIndex;

}

declare interface ICardSearch {
    searchFilter: SearchFilterProps;
    searchIsFetching: boolean;

    requestedCards: string[];
}

declare interface ICardScoutCardSearch {
    filter: ICardScoutSearchFilter;
    
    searchIsInProgress: boolean;

    searchResults?: ICard[];
}

declare interface ICardScoutSearchFilter {
    set: string;
    name: string;
    type: string;
    colorIdentity: string;
}

declare interface IArchitectCardSearchFilter {
    //what do I even want for filters?
}

declare interface IArchitectCardSearchState {
    filter: IArchitectCardSearchFilter;
    //more search related vars



}
//API state?

declare interface SearchFilterProps {
    name: string;

    //mana types
    includeRed: boolean;
    includeBlue: boolean;
    includeGreen: boolean;
    includeWhite: boolean;
    includeBlack: boolean;
    colorIdentity: string;
    //sets
    setFilterString: string;

    // isFetching: boolean;
    results: any; //results is an array of cards, right?
    selectedCardId?: string;
    selectedCardName?: string;
}


// declare 

declare interface ReduxAction extends AnyAction {
    type: any,
    error?: any,
    payload?: any,
    meta?: any
}



declare interface INamedCardArray {
    name: string;
    count?: number;
    cards: ICard[];
}



declare interface NamedInventoryCardArray {
    name: string;
    cards: InventoryCard[];
}


declare interface InventoryCard {
    //Does this need to store the name?
    data: ICard;
    inDecks: number;
    inInventory: number;
    inInventoryFoil: number;
}

declare interface ITempCard{
    name: string;
}

///////////////////////
//  Object interfaces/
/////////////////////

declare interface ICardList {
    id: number;
    cards: IDeckCard[]; //Cards are just a collection of string IDs represengint a Magic.Card.Name
    lands: ILandCount;
}

declare interface ILandCount {
    R: number,
    U: number,
    G: number,
    W: number,
    B: number,
}


declare interface DataInventoryStore {
    updated: Date;
    data: DataInventoryCard[];
}

declare interface DataInventoryCard {
    set: string,
    name: string,
    norm: number,
    foil: number
}


//
//  DataStore
//


//should be depricated
declare interface IMagicCard {
    //name
    cardId: string;
    data: Card;
}

declare interface IntDictionary {
    [key: string]: number[]
}

declare interface ICard {
    //Guess I'll just put relevant info by ID
    cmc: number;
    colorIdentity: string[];
    colors: string[];
    flavor: string | undefined;
    id: string;
    imageUrl: string;
    layout: string;
    manaCost: string;
    multiverseid: number;
    name: string;
    number: string;
    printings: string[];
    rarity: string;
    set: string;
    setName: string;
    text: string;
    type: string;
    types: string[];
}

