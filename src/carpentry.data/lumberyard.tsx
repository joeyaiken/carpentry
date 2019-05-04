  ////////////////
 // lumberyard //
////////////////

//A lumberyard is a shitty data solution for the carpentry app
//Eventually, it should be replaced with something actually competent
//It's..a set of functions that manages a few data objects that exist either in .json objects or in browser cache
//It should keep track of which version (cache vs .json) is more up to date, and be able to notify the user about changes

//The onlything that should interact with the lumberyard is a lumberjack




//consider calling non-state-related objects 'stores' ?

import { Card } from 'mtgsdk-ts'

//CCID == Carpentry Cache Identifier

//const CCID_Timestamps
//const CCID_Decks ?

const CACHED_UI_STATE_IDENTIFIER = 'CARPENTRY_CACHED_UI_STATE_IDENTIFIER';
const CACHED_DECK_DATA_IDENTIFIER = 'CACHED_DECK_DATA_IDENTIFIER';
const CACHED_CARD_LIST_IDENTIFIER = 'CARPENTRY_CACHED_CARD_LIST_IDENTIFIER';
const CACHED_CARD_INDEX_IDENTIFIER = 'CACHED_CARD_INDEX_IDENTIFIER';










//I need a name for the class that handles the caching / loading / default initialization of a specific record type

//create a "state manager" ?
//  state manager properties are ones that we would never want to store in a JSON file, only cache

//create a "data manager" ?
//  a data manager class worries about data we would want to store in JSON files
//  additionally, it will/should store differences between the data store and live data










//  A lumberyard is a data store for managing MTG card decks
//  It should be the "source of truth" of deck & library data
// (should it actually be a class, or a series of functions?
//  Maybe it's a class w/o much of a constructor)
export class Lumberyard{
    //private vars

    // constructor(){
    //     //When the lumberyard initializes, it should load initial data from files / cache
    //     console.log('Lumberyard instance initialized');
    // }

    //Gotta have some static classes & shit




    //static defaultStateInstance_cardSearch = (): ICardSearch => {
    
    static Collections_All_BySet(): ICardIndex {
        return {
            "AKH": this.CollectionIndex_AKH(),
            "HOU": this.CollectionIndex_HOU(),
            "DOM": this.CollectionIndex_DOM(),
            "WAR": this.CollectionIndex_WAR()
            // ...this.CollectionIndex_Misc()
        }
    }

    static getAllCardsForSet = (setCode: string): ICardDictionary | null => {
        switch(setCode){
            case '10E':
                return Lumberyard.CollectionIndex_10E();
            case 'AER':
                return Lumberyard.CollectionIndex_AER();
            case 'AKH':
                return Lumberyard.CollectionIndex_AKH();
            case 'DGM':
                return Lumberyard.CollectionIndex_DGM();
            case 'DOM':
                return Lumberyard.CollectionIndex_DOM();
            case 'EMN':
                return Lumberyard.CollectionIndex_EMN();
            case 'FRF':
                return Lumberyard.CollectionIndex_FRF();
            case 'GRN':
                return Lumberyard.CollectionIndex_GRN();
            case 'HOU':
                return Lumberyard.CollectionIndex_HOU();
            case 'M15':
                return Lumberyard.CollectionIndex_M15();
            case 'MM3':
                return Lumberyard.CollectionIndex_MM3();
            case 'RIX':
                return Lumberyard.CollectionIndex_RIX();
            case 'RNA':
                return Lumberyard.CollectionIndex_RNA();
            case 'RTR':
                return Lumberyard.CollectionIndex_RTR();
            case 'SOI':
                return Lumberyard.CollectionIndex_SOI();
            case 'THS':
                return Lumberyard.CollectionIndex_THS();
            case 'XLN':
                return Lumberyard.CollectionIndex_XLN();
            case 'WAR':
                return Lumberyard.CollectionIndex_WAR();
        }
        //Lumberjack.getSet
        return null;
    }


    static CollectionIndex_10E(): ICardDictionary {
        let Misc_CardIndex: ICardDictionary = require('./index/10E.index.json');
        return Misc_CardIndex;
    }

    static CollectionIndex_AER(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/AER.index.json');
        return cardIndex;
    }

    static CollectionIndex_AKH(): ICardDictionary {
        let Misc_CardIndex: ICardDictionary = require('./index/AKH.index.json');
        return Misc_CardIndex;
    }

    static CollectionIndex_DGM(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/DGM.index.json');
        return cardIndex;
    }

    static CollectionIndex_DOM(): ICardDictionary {
        let DOM_Index: ICardDictionary = require('./index/DOM.index.json');
        return DOM_Index;
    }

    static CollectionIndex_EMN(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/EMN.index.json');
        return cardIndex;
    }

    static CollectionIndex_FRF(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/FRF.index.json');
        return cardIndex;
    }

    static CollectionIndex_GRN(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/GRN.index.json');
        return cardIndex;
    }

    static CollectionIndex_HOU(): ICardDictionary {
        let HOU_CardIndex: ICardDictionary = require('./index/HOU.index.json');
        return HOU_CardIndex;
    }

    static CollectionIndex_M15(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/M15.index.json');
        return cardIndex;
    }

    static CollectionIndex_MM3(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/MM3.index.json');
        return cardIndex;
    }

    static CollectionIndex_RIX(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/RIX.index.json');
        return cardIndex;
    }

    static CollectionIndex_RNA(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/RNA.index.json');
        return cardIndex;
    }
  
    static CollectionIndex_RTR(): ICardDictionary {
        let HOU_CardIndex: ICardDictionary = require('./index/RTR.index.json');
        return HOU_CardIndex;
    }

    static CollectionIndex_SOI(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/SOI.index.json');
        return cardIndex;
    }

    static CollectionIndex_THS(): ICardDictionary {
        let HOU_CardIndex: ICardDictionary = require('./index/THS.index.json');
        return HOU_CardIndex;
    }

    static CollectionIndex_XLN(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/XLN.index.json');
        return cardIndex;
    }

    static CollectionIndex_WAR(): ICardDictionary {
        let cardIndex: ICardDictionary = require('./index/WAR.index.json');
        return cardIndex;
    }

    static cache_save_cardInventory(cards: DataInventoryCard[]): void {
        const stringToSave: string = JSON.stringify(cards);
        localStorage.setItem("LUMBERYARD_CARD_INVENTORY_CACHE", stringToSave);
    }

    static cache_load_cardInventory(): DataInventoryCard[] {
        const cachedData: string|null = localStorage.getItem("LUMBERYARD_CARD_INVENTORY_CACHE");
        if(cachedData){
            console.log('found cache')
            console.log(cachedData)
            const cardIndex: DataInventoryCard[] = JSON.parse(cachedData);
            return cardIndex;
        } else {
            console.log('no cache')
            return [];
        }
    }

    //this stuff should be refactord out probs
    static legacy_loadInitialUIState(): IUIState{
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

    static legacy_cacheUIState(state: IUIState): void {
        const UIStateString: string = JSON.stringify(state);
        localStorage.setItem(CACHED_UI_STATE_IDENTIFIER, UIStateString);
    }

    static legacy_loadInitialDataStore(): IDataStore {

        // const cachedData: string|null = localStorage.getItem('deck-cache');
        // const cachedIndexData: string|null = localStorage.getItem('card-index-cache');
        // const cachedIndexDataByName: string|null = localStorage.getItem('card-index-cache-by-name');
    
        // const cachedCardLists = loadDeckCardsCache();
    
        let defaultData: ICardDeck[] = require('./legacy2/deckData_legacy.json');
    
        // console.log('default deck data');
        // console.log(defaultData);
        const cachedDeckData = loadDeckDataCache();
    
        //mapCardToICard
        let defaultIndex: ICardIndex = loadCardIndexCache() || require('./legacy2/cardIndexData_legacy.json');
    
        const cachedUIState = loadUIStateCache();
    
        const initialDataStore: IDataStore = {
            selectedCard: null,
            selectedDeckId: (cachedUIState && cachedUIState.selectedDeckId) || 0,
            deckList: defaultData,
            cardIndex: defaultIndex
        }
    
        return initialDataStore;
    }

    static legacy_cacheDeckData(data: ICardDeck[]): void {
        const deckDataString: string = JSON.stringify(data);
        localStorage.setItem(CACHED_DECK_DATA_IDENTIFIER, deckDataString);
    }

    static legacy_saveCardIndexCache(index: ICardIndex): void {
        localStorage.setItem(CACHED_CARD_INDEX_IDENTIFIER, JSON.stringify(index))
    }

}




// export class LumberyardDataStore {
// }

 


function loadCardIndexCache(): ICardIndex | null {
    const cachedData: string|null = localStorage.getItem(CACHED_CARD_INDEX_IDENTIFIER);
    if(cachedData){
        const cardIndex: ICardIndex = JSON.parse(cachedData);
        return cardIndex;
    } else {
        return null;
    }
}


        
        
        
        
////



function loadDeckDataCache(): ICardDeck[] | null {
    const cachedDeckData: string|null = localStorage.getItem(CACHED_DECK_DATA_IDENTIFIER)
    if(cachedDeckData){
        return JSON.parse(cachedDeckData);  //what if this parse fails?
    } else {
        return null;
    }
}
////

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