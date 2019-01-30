/// <reference types="mtgsdk-ts" />

declare interface State {
    actions: AppState
    ui: UIState;
}

declare interface AppState {
    //nav properties
    // isNavOpen: boolean;

    //editor UI props
    isSearchOpen: boolean;
    isRareBinderOpen: boolean;
    isDetailOpen: boolean;

    // cardBinderView: string;
    // cardBinderSort: string;
    // cardBinderFiter: string;
    // cardBinderGroup: string;
    //also card binder section visibilities

    // activeDeck?: CardDeck;
    activeDeckVisibleCards: IMagicCard[];
    //need sections, not visible cards

    deckList: CardDeck[]
    selectedDeckId: number;

    //ui == unused
    ui: UIProps;
    searchFilter: SearchFilterProps;
    
    cardIndex: ICardIndex;

    sectionVisibilities: boolean[];
    


    searchIsFetching: boolean;
    requestedCards: string[];
    //Things needed by our web api
    //isFetching
    //requestedCards6
}

declare interface UIState {
    isNavOpen: boolean;
    isSideSheetOpen: boolean;
    visibleSideSheet: string;

    //make interface "deck"?
    deckView: string;
    deckGroup: string;
    deckSort: string;
    deckFilter: string;

    //deck section visibilities?
    // sectionVisibilities: boolean[];

    //interface "search"?

    //interface "detail"?

    //interface "rare binder"?

}

declare interface ICardIndex {
    [id: string]: IMagicCard;
}


declare interface SearchFilterProps {
    name: string;
    // isFetching: boolean;
    results: any;
    selectedCardId?: string;
}

declare interface UIProps {
    isFindModalVisible: boolean;
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

declare interface ILandCount {
    R: number,
    U: number,
    G: number,
    W: number,
    B: number,
}


//other typings
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
