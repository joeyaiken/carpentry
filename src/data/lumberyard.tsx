/////////////////////////
//CURRENT GOALS
//  I don't like the way the app state is initializing
//  Initial states should be empty objects, should not load cached data
//  After the app actually initializes, we can load cached & stored data
/////////////////////////

//consider calling non-state-related objects 'stores' ?

import { Card } from 'mtgsdk-ts'

const CACHED_UI_STATE_IDENTIFIER = 'CARPENTRY_CACHED_UI_STATE_IDENTIFIER';
const CACHED_DECK_DETAIL_IDENTIFIER = 'CARPENTRY_CACHED_DECK_DETAIL_IDENTIFIER';
const CACHED_CARD_LIST_IDENTIFIER = 'CARPENTRY_CACHED_CARD_LIST_IDENTIFIER';
const CACHED_CARD_INDEX_IDENTIFIER = 'CACHED_CARD_INDEX_IDENTIFIER';

//lumberyard.UI.loadInitialState();
//lumberyard.UI.save(currentState)

//I need a name for the class that handles the caching / loading / default initialization of a specific record type

//create a "state manager" ?
//  state manager properties are ones that we would never want to store in a JSON file, only cache

//create a "data manager" ?
//  a data manager class worries about data we would want to store in JSON files
//  additionally, it will/should store differences between the data store and live data


export class UIStateManager {

    static loadInitialUIState(){
        //try to cache a loaded UI state?
        //What do we REALLY want to cache besides selected deck ID?
        //Would it hurt to cache everything and only apply certain settings?
        const cachedUIState = loadUIStateCache();

        const initialUIState: IUIState = loadUIStateCache() || {
            isNavOpen: (cachedUIState && cachedUIState.isNavOpen) || false,
            isSideSheetOpen: false,
            // selectedDeckId: 0,
            // selectedDeckId: null,
            selectedDeckId: (cachedUIState && cachedUIState.selectedDeckId) || null,
            visibleSideSheet: ''
        }
        return initialUIState;
    }

    static cacheUIState(state: IUIState): void {
        const UIStateString: string = JSON.stringify(state);
        localStorage.setItem(CACHED_UI_STATE_IDENTIFIER, UIStateString);
    }

}

export class LumberyardDataStore {

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
    
export function saveCardIndexCache(index: ICardIndex): void {
    localStorage.setItem(CACHED_CARD_INDEX_IDENTIFIER, JSON.stringify(index))
}

function loadCardIndexCache(): ICardIndex | null {
    const cachedData: string|null = localStorage.getItem(CACHED_CARD_INDEX_IDENTIFIER);
    if(cachedData){
        const cardIndex: ICardIndex = JSON.parse(cachedData);
        return cardIndex;
    } else {
        return null;
    }
}

export function loadInitialDataStore(): IDataStore {

    // const cachedData: string|null = localStorage.getItem('deck-cache');
    // const cachedIndexData: string|null = localStorage.getItem('card-index-cache');
    // const cachedIndexDataByName: string|null = localStorage.getItem('card-index-cache-by-name');

    // const cachedCardLists = loadDeckCardsCache();

    let defaultData: {
        cardLists: ICardList_Legacy[],
        detailList: IDeckDetail[]
    } = require('./logpile_legacy.json');

    const cachedDeckDetails = loadDeckDetailCache();
    const cachedDeckCards = loadDeckCardsCache();

    if(cachedDeckDetails){
        defaultData.detailList = cachedDeckDetails;
    }
    if(cachedDeckCards){
        defaultData.cardLists = cachedDeckCards;
    }

    //mapCardToICard
    let defaultIndex: ICardIndex = loadCardIndexCache() || require('./cardIndex_legacy.json');
    // let defaultIndex: ICardIndex = {};
    
    // Object.keys(fullIndex).forEach((key) => {
    //     defaultIndex[key] = mapCardToICard(fullIndex[key])
    // })

    //loadDeckDetailCache
    // const cachedDeckDetails = loadDeckDetailCache();

    // var cardLists: ICardList[] = JSON.parse(cardListsById);


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
    let cardLists: ICardList_Legacy[] = defaultData.cardLists || []
    // console.log('default data')
    // console.log(defaultData);
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
export function cacheDeckCards(cards: ICardList_Legacy[] | null): void {
    const cardsString: string = JSON.stringify(cards);
    localStorage.setItem(CACHED_CARD_LIST_IDENTIFIER, cardsString);
}
        
function loadDeckCardsCache(): ICardList_Legacy[] | null {
    const cardsString: string|null = localStorage.getItem(CACHED_CARD_LIST_IDENTIFIER)
    if(cardsString){
        return JSON.parse(cardsString);  //what if this parse fails?
    } else {
        return null;
    }
}
        
function generateLandCounterInstance(): ILandCount {
return {
    R: 0,
    U: 0,
    G: 0,
    W: 0,
    B: 0
}
}

export function mapCardToICard(card: any): ICard {
return {
    cmc: card.cmc,
    colorIdentity: card.colorIdentity,
    colors: card.colors,
    flavor: card.flavor,
    id: card.id,
    imageUrl: card.imageUrl,
    layout: card.layout,
    manaCost: card.manaCost,
    multiverseid: card.multiverseid,
    name: card.name,
    number: card.number,
    printings: card.printings,
    rarity: card.rarity,
    set: card.set,
    setName: card.setName,
    text: card.text,
    type: card.type,
    types: card.types
}
}

export function loadDefaultUIState(): IUIState {
return {
    // isFindModalVisible: true
    isNavOpen: false,
    isSideSheetOpen: false,
    visibleSideSheet: '',
    selectedDeckId: 0
}// a
}

function loadUIStateCache(): IUIState | null {
    const cachedUIStateData: string|null = localStorage.getItem(CACHED_UI_STATE_IDENTIFIER)
    if(cachedUIStateData){
        return JSON.parse(cachedUIStateData);  //what if this parse fails?
    } else {
        return null;
    }
}

/*
What types of objects am I currently caching/loading?
    various app states (possibly cached)
        Note: some app states are never cached
    stored data

    UI state
    deck editor state
    data store
*/




