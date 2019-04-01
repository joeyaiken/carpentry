/// <reference types="mtgsdk-ts" />


//////////////////////
//  State interfaces/
////////////////////
declare interface State {
    //actions need to be removed completely
    // actions: AppState;
    //core
    data: IDataStore;
    ui: IUIState;
    //components
    deckEditor: IDeckEditorState;
    cardSearch: ICardSearch;
    
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



//
//  DataStore
//


//should be depricated
declare interface IMagicCard {
    //name
    cardId: string;
    data: Card;
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
