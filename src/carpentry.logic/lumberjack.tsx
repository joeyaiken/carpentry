//A Lumberjack works in the Lumberyard, building decks

//Until this gets replaced with something more fancy, it'll be a collection of functions

//Each function name will be prefixed with "Lumberjack_"

//A Lumberjack should have EXCLUSIVE access to the Lumberyard





///////////////////////////////
//default instance generators//
///////////////////////////////

import { Lumberyard } from '../carpentry.data/lumberyard'


export class Lumberjack {
    
    // constructor(){
    //     // let defaultStates = new DefaultInstanceStateGenerator();
    //     //this.handleSectionToggle = this.handleSectionToggle.bind(this);
    //     this.defaultSateInstance_dataStore = this.defaultSateInstance_dataStore.bind(this);
    // }
    //defaultStates
    //defaultStates: DefaultInstanceStateGeneratorl

    //what have I been breaking?

    static defaultStateInstance_cardSearch = (): ICardSearch => {
        const initialCardSearchState: ICardSearch = {
            requestedCards: [],
            searchFilter: {
                name: '',
                setFilterString: '',
                includeRed: false,
                includeBlue: false,
                includeGreen: false,
                includeWhite: false,
                includeBlack: false,
                colorIdentity: '',
                results: []
            },
            searchIsFetching: false
        }
        return initialCardSearchState;
    }

    static defaultStateInstance_dataStore = (): IDataStore => {
        return {
            deckList: [],
            cardIndex: {},
            selectedCard: null,
            selectedDeckId: null
        }
    }

    static defaultStateInstance_deckEditor = (): IDeckEditorState => {
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

    //misc - need to be refactored probs
    static legacy_loadInitialUIState = (): IUIState => {
        return Lumberyard.legacy_loadInitialUIState();
    }

    static legacy_cacheUIState = (state: IUIState): void => {
        Lumberyard.legacy_cacheUIState(state);
    }

    static legacy_cacheDeckData = (data: ICardDeck[]): void => {
        Lumberyard.legacy_cacheDeckData(data);
    }

    static legacy_saveCardIndexCache = (index: ICardIndex): void => {
        Lumberyard.legacy_saveCardIndexCache(index);
    }

    static legacy_loadInitialDataStore = (): IDataStore => {
        return Lumberyard.legacy_loadInitialDataStore();
    }
    
}

