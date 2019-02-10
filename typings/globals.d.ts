/// <reference types="mtgsdk-ts" />

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

    selectedDeckId: number;
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
    selectedCard: string | null;

    selectedDeckId: number; // | null ?
    //  active deck visible cards

    //  maybe even section visibilities?
    
    //  probably at least sections
    
    //dictionary of cards "card index"
    cardIndex: ICardIndex;

    //dictionary of decks
    // deckList: CardDeck[]
    //instead of a list of decks, we have a list of details and a list of cards

    //These are the only records that need to be stored under source controll
    detailList: IDeckDetail[]; //should this be a dictionary instead?
    cardLists: ICardList[]; //should this be a dictionary instead?
    // rareStore: ICardList;
}

declare interface ICardSearch {
    searchFilter: SearchFilterProps;
    searchIsFetching: boolean;

    requestedCards: string[];
}

//API state?

declare interface SearchFilterProps {
    name: string;
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

declare interface CardDeck {
    id: number;
    name: string;
    description: string;
    //cards: MagicCard[];
    cards: string[]; //Cards are just a collection of string IDs represengint a Magic.Card.id

    basicLands: ILandCount;

    type: string; // edh / standard / legacy
    colors: string;
    //colors (calculate this?)

    //"gimick" ?
}

declare interface ICardDeck {
    id: number;
    detail: IDeckDetail;
    lands: IDeckLandCount;
    // cards: string[]; //list of card names
    //cards are left in a separate object for convenience
}


declare interface IDeckDetailIndex {
    [id: number]: IDeckDetail;
}
declare interface IDeckDetail {
    id: number;
    //
    name: string;
    description: string;
    type: string;
    colors: string;

    basicLands: ILandCount;
}

// declare interface IDeckLandCountIndex {
//     [id: number]: IDeckLandCount;
// }
// declare interface IDeckLandCount {
//     id: number;
//     basicLands: ILandCount;
// }

declare interface ICardListIndex {
    [id: number]: ICardList;
}
declare interface ICardList {
    id: number;
    cards: string[]; //Cards are just a collection of string IDs represengint a Magic.Card.Name
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

declare interface IDeckIndex {
    [id: string]: ICardDeck;
}

declare interface ICardIndex {
    [id: string]: ICard;
}

declare interface IMagicCard {
    //name
    cardId: string;
    data: Card;
    // updateRequested: boolean;
    //ID ?

    //color(s)

    //set

    //???
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





