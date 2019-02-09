import DeckList from "../components/DeckList";
import DeckDetail from "../components/DeckDetail";
import { stat } from "fs";
import { Card } from 'mtgsdk-ts'


const CACHED_UI_STATE_IDENTIFIER = 'CARPENTRY_CACHED_UI_STATE_IDENTIFIER';
const CACHED_DECK_DETAIL_IDENTIFIER = 'CARPENTRY_CACHED_DECK_DETAIL_IDENTIFIER';
const CACHED_CARD_LIST_IDENTIFIER = 'CARPENTRY_CACHED_CARD_LIST_IDENTIFIER';

// import React from 'react';


//So I don't really know WHERE I should be loading the default states, but here are the methods that will be called!
export function loadInitialUIState(): IUIState {
    
    //try to cache a loaded UI state?
    //What do we REALLY want to cache besides selected deck ID?
    //Would it hurt to cache everything and only apply certain settings?
    const cachedUIState = loadUIStateCache();

    const initialUIState: IUIState = loadUIStateCache() || {
        isNavOpen: false,
        isSideSheetOpen: false,
        // selectedDeckId: 0,
        selectedDeckId: (cachedUIState && cachedUIState.selectedDeckId) || 0,
        visibleSideSheet: ''
    }

    return initialUIState;
}

export function loadInitialDeckEditorState(): IDeckEditorState {

    const initialDeckEditorState: IDeckEditorState = {
        deckView: 'card',
        deckGroup: 'none',
        deckSort: 'name',
        deckFilter: '',
        activeDeckVisibleCards: [],
        sectionVisibilities: [true,true,true,true,true,true],
        selectedCard: null
    }

    return initialDeckEditorState;

}

export function loadInitialCardSearchState(): ICardSearch {

    let initialCardSearchState: ICardSearch = {
        requestedCards: [],
        searchFilter: {
            name: '',
            results: []
        },
        searchIsFetching: false
    }

    return initialCardSearchState;
}

export function loadInitialDataStore(): IDataStore {

    // const cachedData: string|null = localStorage.getItem('deck-cache');
    // const cachedIndexData: string|null = localStorage.getItem('card-index-cache');
    // const cachedIndexDataByName: string|null = localStorage.getItem('card-index-cache-by-name');

    // const cachedCardLists = loadDeckCardsCache();

    let defaultData: {
        cardLists: ICardList[],
        detailList: IDeckDetail[]
    } = require('./logpile.json');


    let defaultIndex: ICardIndex = require('./cardIndex.json');

    // console.log('did I load default data?');
    // console.log(defaultIndex['Anointed Procession']);
    // console.log(defaultIndex);
    
    //loadDeckDetailCache
    // const cachedDeckDetails = loadDeckDetailCache();

    // var cardLists: ICardList[] = JSON.parse(cardListsById);

    // console.log('CARD LISTS');
    // console.log(cardLists);


    // let cardIndex: ICardIndex = {}
    let cardIndex = defaultIndex;
    
    // if(cachedIndexDataByName){
    //     // var rawCache = JSON.parse(cachedIndexData);
    //     cardIndex = JSON.parse(cachedIndexDataByName);
    // }


    // let cardIndexByName: ICardIndex = {}

    //const cachedIndexData: string|null = localStorage.getItem('card-index-cache');
    

    // Object.keys(cardIndex).forEach((key) => {
    //     let card = cardIndex[key];
    //     cardIndexByName[card.name] = card;
    // });

    // localStorage.setItem('card-index-cache-by-name',JSON.stringify(cardIndexByName))

    //generate a card index cache by ID? 
   
    // cardLists.map((list) => {
    //     var someCardNames = list.cards.map((card) => {
    //         return cardIndex[card].name;
    //     });

    //     console.log('did we get cards by name?');
    //     console.log(someCardNames)
    //     //return list.
    // })
    

    
    // let detailList: IDeckDetail[] = [];
    // let detailList = cachedDeckDetails || [];//defaultData
    let detailList = defaultData.detailList || [];//defaultData

    //defaultDeckDetails
    // if(defaultDeckDetails && defaultDeckDetails.length > 0 && detailList.length == 0){
        // detailList = JSON.parse(defaultDeckDetails);

        //Should only really need to do this once
    detailList.forEach((deckDetail) => {
        if(!deckDetail.basicLands){
            deckDetail.basicLands = generateLandCounterInstance();
        }
    })
    // }

    // let cardLists: ICardList[] = cachedCardLists || []
    let cardLists: ICardList[] = defaultData.cardLists || []
    console.log('default data')
    console.log(defaultData);
    const cachedUIState = loadUIStateCache();

    const initialDataStore: IDataStore = {
        // deckList: deckList,
        cardLists: cardLists,
        detailList: detailList,
        cardIndex: cardIndex,
        selectedCard: null,
        selectedDeckId: (cachedUIState && cachedUIState.selectedDeckId) || 0
    }

    return initialDataStore;
}



////////cache access
//I'd really like to wrap this in a class or some other object




export function cacheUIState(state: IUIState): void {
    const UIStateString: string = JSON.stringify(state);
    localStorage.setItem(CACHED_UI_STATE_IDENTIFIER, UIStateString);
}

function loadUIStateCache(): IUIState | null {
    const cachedUIStateData: string|null = localStorage.getItem(CACHED_UI_STATE_IDENTIFIER)
    if(cachedUIStateData){
        return JSON.parse(cachedUIStateData);  //what if this parse fails?
    } else {
        return null;
    }
}
////
export function cacheDeckDetails(detail: IDeckDetail[]): void {
    const deckDetailString: string = JSON.stringify(detail);
    localStorage.setItem(CACHED_DECK_DETAIL_IDENTIFIER, deckDetailString);
}

function loadDeckDetailCache(): IDeckDetail[] | null {
    const cachedDeckDetail: string|null = localStorage.getItem(CACHED_DECK_DETAIL_IDENTIFIER)
    if(cachedDeckDetail){
        return JSON.parse(cachedDeckDetail);  //what if this parse fails?
    } else {
        return null;
    }
}
////
export function cacheDeckCards(cards: ICardList[] | null): void {
    const cardsString: string = JSON.stringify(cards);
    localStorage.setItem(CACHED_CARD_LIST_IDENTIFIER, cardsString);
}

function loadDeckCardsCache(): ICardList[] | null {
    const cardsString: string|null = localStorage.getItem(CACHED_CARD_LIST_IDENTIFIER)
    if(cardsString){
        return JSON.parse(cardsString);  //what if this parse fails?
    } else {
        return null;
    }
}

// export function cacheDeckEditorState(state: IDeckEditorState): void {



// }

//don't think we want to try caching the deck editor state right?


//
//  Methods for caching states
//


//export function loadInitialActionsState(): 

//More states will need to be loaded ofc

//What abt a load default app state?





///////////////

export default class lumberyard {
    constructor(props: any){

    }

}



// export function tryLoadData(): AppState {
    

    
//     // console.log('trying to load some of the new parsed data');
//     // let storedDeckDetailInstance = loadDeckDetailIndex();
//     // console.log(storedDeckDetailInstance);

//     // console.log(loadLandCountIndex())
//     // console.log(loadCardListIndex())

//     //also need to cache the rare binder
//     // let initialState: AppState = generateAppStateInstance();

    

//     // if(!deckList || !deckList.length){
//     //     deckList = generateDeckData();
//     // }
//     // initialState.deckList = deckList;// || generateDeckData();

//     // if(cachedIndexData){
//     //     // var rawCache = JSON.parse(cachedIndexData);
//     //     let cardIndex: ICardIndex = JSON.parse(cachedIndexData);
//     //     initialState.cardIndex = cardIndex || {};
//     // }
    
//     //Check for any cards that need to be loaded in the initial deck
//     // let activeDeck = initialState.deckList[initialState.selectedDeckId];
//     // activeDeck.cards.forEach((cardId: string) => {
//     //     let cardFromIndex = initialState.cardIndex[cardId];
//     //     if(!cardFromIndex){
//     //         initialState.requestedCards.push(cardId);
//     //     }
//     // });
//     // console.log('audit result');
//     // console.log(initialState.requestedCards)
    

//     // if(deckList && deckList.length){
//     //     initialState.deckList = deckList;
//     // } else {
//     //     initialState.deckList = generateDeckData();
//     // }

//     return initialState;
// }

// function generateAppStateInstance(): AppState {
//     return {

//         deckList: [],
//         selectedDeckId: 0,
//         searchFilter: {
//             name: '',
//             results: []
//         },
//         cardIndex: {},
//         sectionVisibilities: [true,true,true,true,true,true],
//         searchIsFetching: false,
//         requestedCards: [],
//         activeDeckVisibleCards: []
//     }
// }

function generateLandCounterInstance(): ILandCount {
    return {
        R: 0,
        U: 0,
        G: 0,
        W: 0,
        B: 0
    }
}



export function loadDefaultUIState(): IUIState {
    return {
        // isFindModalVisible: true
        isNavOpen: false,
        isSideSheetOpen: false,
        visibleSideSheet: '',
        selectedDeckId: 0

        // deckView: 'card',
        // deckGroup: 'none',
        // deckSort: 'name',
        // deckFilter: ''
    }// a
}
