import { combineReducers } from 'redux';

//domain data
import { deckOverviews, DeckOverviewsState } from './deckOverviews.reducer';
import { deckDetail, DeckDetailState } from './deckDetail.reducer';

import { inventoryDataReducer, InventoryDataReducerState } from './inventoryDataReducer';
import { deckEditorReducer, DeckEditorReducerState } from './deckEditorReducer';

////Domain data
//import { dataLoadingState } from './dataLoadingState.reducer'
//import { appFilterOptions } from './appFilterOptions.reducer';
//import { inventoryOverviews } from './inventoryOverviews.reducer';
//import { inventoryDetail } from './inventoryDetail.reducer';
//import { cardSearchResults } from './cardSearchResults.reducer';
//import { cardSearchPendingCards } from './cardSearchPendingCards.reducer';
//import { cardSearchInventoryDetail } from './cardSearchInventoryDetail.reducer';
//import { deckList } from './deckList.reducer';
//import { deckDetail } from './deckDetail.reducer';
////App state
//import { appState  } from './appState.reducer';
//import { deckEditor } from './deckEditor.reducer';
//import { inventory } from './inventory.reducer';
//import { cardSearch } from './cardSearch.reducer';
////ui
// import { ui } from './ui.reducer';

//export const reducers = combineReducers({

// export type { AppState } from './configureStore';

export interface AppState {
    data: {
        deckOverviews: DeckOverviewsState,
        deckDetail: DeckDetailState,
        
        inventory: InventoryDataReducerState,
    },
    app: {
        deckEditor: DeckEditorReducerState,
    }
}

export const reducers = {
    data: combineReducers({
        //isLoading: null,
        deckOverviews,
        deckDetail,

        inventory: inventoryDataReducer,
    }),
    app: combineReducers({
            //    core: appState,
        //    deckEditor,
        deckEditor: deckEditorReducer,
        //    inventory,
        //    cardSearch,

    }),



    //data: combineReducers({
    //    isLoading: dataLoadingState,
    //    appFilterOptions,
    //    inventoryOverviews,
    //    inventoryDetail,
    //    cardSearchResults,
    //    cardSearchPendingCards,
    //    cardSearchInventoryDetail,
    //    deckList,
    //    deckDetail,
    //}),
    
    //app: combineReducers({
    //    core: appState,
    //    deckEditor,
    //    inventory,
    //    cardSearch,
    //}),

    //ui,

};



// import { combineReducers } from 'redux';
// //Domain data
// import { dataLoadingState } from './dataLoadingState.reducer'
// import { appFilterOptions } from './appFilterOptions.reducer';
// import { inventoryOverviews } from './inventoryOverviews.reducer';
// import { inventoryDetail } from './inventoryDetail.reducer';
// import { cardSearchResults } from './cardSearchResults.reducer';
// import { cardSearchPendingCards } from './cardSearchPendingCards.reducer';
// import { cardSearchInventoryDetail } from './cardSearchInventoryDetail.reducer';
// import { deckList } from './deckList.reducer';
// import { deckDetail } from './deckDetail.reducer';
// //App state
// import { appState  } from './appState.reducer';
// import { deckEditor } from './deckEditor.reducer';
// import { inventory } from './inventory.reducer';
// import { cardSearch } from './cardSearch.reducer';
// //ui
// import { ui } from './ui.reducer';

//})

//type AppContainerEnum = 'deckEditor' | 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null;

//declare interface ExampleState {
//    data: {
        
//        isLoading: {
//            deckList: boolean;
//            deckDetail: boolean;
    
//            inventoryOverview: boolean;
//            inventoryDetail: boolean;
    
//            cardSearchResults: boolean;
//            cardSearchInventoryDetail: boolean;
//        }

//        core: {
//            //domain data - Data the app needs to show, use, or modify
//            filterOptions: {
//                sets: FilterOption[];
//                types: FilterOption[];
//                colors: FilterOption[];
//                rarities: FilterOption[];
//            }
//        }

//        inventory: {
//            //domain data - Data the app needs to show, use, or modify
//            inventoryItems: InventoryOverviewDto[];
//            selectedDetailItem: InventoryDetailDto | null;
//        }
//        cardSearch: {
//            //domain data - Data the app needs to show, use, or modify
//            searchResults: MagicCard[];
//            pendingCards: { [key:number]: PendingCardsDto }
//            selectedCard: MagicCard | null;
//            inventoryDetail: InventoryDetailDto | null;
//        }
//        deckEditor: {
//            //domain data - Data the app needs to show, use, or modify    
//            selectedDeckDto: DeckDto | null;
//            selectedCard: InventoryOverviewDto | null;
//            selectedInventoryCards: InventoryCard[];
//        }
//        deckList: {
//            //domain data - Data the app needs to show, use, or modify
//            decks: DeckProperties[];
//        }
//    }

//    core: {
//        //app state - Data specific to the app's behavior
//        newDeckIsSaving: boolean; //Should be removed
//        newDeckDto: DeckProperties; //Should be removed

//        activePrimaryContainer: null | 'Inventory' | 'DeckEditor' | 'BuyList';
//        activeSecondaryContainer: null | 'Inventory' | 'CardSearch';
//        activeModalContainer: null | 'DeckQuickAdd';

//        visibleContainer: AppContainerEnum; //Should be removed
//        selectedDeckId: number | null; //Can this be removed?
//        isCardSearchShowing: boolean; //Should be removed
//    }
//    inventory: {
//        //app state - Data specific to the app's behavior
//        searchMethod: "name" | "quantity" | "price"; // | "sellList";
//        viewMode: "list" | "grid";
//    }
//    cardSearch: {
//        //app state - Data specific to the app's behavior
//        pendingCardsSaving: boolean;
//        cardSearchMethod: "set" | "web" | "inventory";
//    }
//    deckEditor: {
//        //app state - Data specific to the app's behavior
//        viewMode: "list" | "grid";
//    }

//    ui: {
//        //ui - Data that represents how the UI is currently displayed
        
//        //filters
//        inventoryFilterProps: CardFilterProps;
//        cardSearchFilterProps: CardFilterProps;

//        //menu anchors
//        deckListMenuAnchor: HTMLButtonElement | null; //in Deck List 
//        cardMenuAnchor: HTMLButtonElement | null; //in Deck Editor (rename?)

//        //misc
//        deckPropsModalOpen: boolean;
//        isNewDeckModalOpen: boolean; //Should be removed
//    }

//}

//interface AppTsx_PropsFromState {
//    isCardSearchShowing: boolean;
//    // visibleContainer: string;
//    visibleContainerEnum: AppContainerEnum;
//    // availableDecks: DeckProperties[];

//    newDeckDto: DeckProperties;
//    isNewDeckModalOpen: boolean;
//}

//declare interface PotentialRefactoredState {
//    //Goals:
//    //Remove all arrays, should use dicts
//    //See if any data could/should be normalized
//    //consider a single/combinded reducer for all of app state
//    //

//    data: {
//        //data state
//        isLoading: {
//            deckList: boolean;
//            deckDetail: boolean;
    
//            inventoryOverview: boolean;
//            inventoryDetail: boolean;
    
//            cardSearchResults: boolean;
//            cardSearchInventoryDetail: boolean;
//        }
//        //domain data - Data the app needs to show, use, or modify
//        core: {
//            filterOptions: {
//                sets: FilterOption[]; //key = name
//                types: FilterOption[]; //key = name
//                colors: FilterOption[]; //key = name
//                rarities: FilterOption[]; //key = name
//            }
//        }
//        inventory: {
//            inventoryItems: InventoryOverviewDto[]; //key = multiverseId
//            selectedDetailItem: InventoryDetailDto | null;
//        }
//        cardSearch: {
//            searchResults: MagicCard[]; //key = multiverseId
//            pendingCards: { [key:number]: PendingCardsDto }
//            // selectedCard: MagicCard | null; //should probably be an AppState ID
//            inventoryDetail: InventoryDetailDto | null;
//        }
//        deckEditor: {
//            //selectedDeckDto: DeckDto | null;

//            selectedDeckDto: { // could probably be renamed to something like "deckDetailDto"

//                // cards: Card[];
//                // data: { [key: number]: MagicCard };

//                props: DeckProperties;
                
//                cardOverviews: InventoryOverviewDto[]; //key = multiverseId
//                cardDetails: InventoryCard[]; // key = id

//                stats: DeckStats;
//            } | null;

//            //selectedCard: InventoryOverviewDto | null; // should this be an ID in AppState?

//            //This one could / should just be a list of IDs for selected cards, I think.  Or belong in AppState?
//            // selectedInventoryCards: InventoryCard[]; //key = id
//        }
//        deckList: {
//            decks: DeckProperties[]; //key = id
//        }
//    }

//    //app: { (contents of the next few objs until ui) }

//    //app state - Data specific to the app's behavior
//    core: {
//        newDeckIsSaving: boolean; //Should be removed
//        newDeckDto: DeckProperties; //Should be removed

//        //pretty sure I never actually switched to this
//        // activePrimaryContainer: null | 'Inventory' | 'DeckEditor' | 'BuyList';
//        // activeSecondaryContainer: null | 'Inventory' | 'CardSearch';
//        // activeModalContainer: null | 'DeckQuickAdd';

//        //Pretty sure this is actually used, one of the 3 is
//        visibleContainer: AppContainerEnum; //Should be removed
//        // selectedDeckId: number | null; //Can this be removed?
//        isCardSearchShowing: boolean; //Should be removed
//    }
//    inventory: { //Could probably be merged into core, right?
//        searchMethod: "name" | "quantity" | "price"; // | "sellList";
//        viewMode: "list" | "grid";
//    }
//    cardSearch: { //Could probably be merged into core, right?
//        pendingCardsSaving: boolean;
//        cardSearchMethod: "set" | "web" | "inventory";
//        selectedCard: MagicCard | null; //should probably be an AppState ID
//    }
//    deckEditor: { //Could probably be merged into core, right?
//        viewMode: "list" | "grid";
//        selectedCard: InventoryOverviewDto | null; // should this be an ID in AppState?
//        selectedInventoryCards: InventoryCard[]; //key = id, but maybe should be a list of IDs, or removed completely
//    }

//    //ui - Data that represents how the UI is currently displayed
//    ui: {
//        //filters
//        inventoryFilterProps: CardFilterProps;
//        cardSearchFilterProps: CardFilterProps;

//        //menu anchors
//        deckListMenuAnchor: HTMLButtonElement | null; //in Deck List 
//        cardMenuAnchor: HTMLButtonElement | null; //in Deck Editor (rename?)

//        //misc
//        deckPropsModalOpen: boolean;
//        isNewDeckModalOpen: boolean; //Should be removed
//    }

//}


//declare interface YetAnotherPotentialRefactoredState {
//    //
//    //deck editor
//    //should addapt proper state with grouping when implementing...grouping


//    // When having a data-related dictionary, I think I should definitely incorporate an AllIds option
//    //  This would assist in display data in a specific sort order

//    //Goals:
//    //Remove all arrays, should use dicts
//    //See if any data could/should be normalized
//    //consider a single/combinded reducer for all of app state
//    //
//    //Some future goals
//    //  Be able to view a deck contents grouped by type
//    //      and options for sorting
//    //
//    //  Updates to inventory to save/define queries
//    //      This will probably be an easier way of doing things like
//    //          Seeing what cards I have the most of (so I can trim my collection)
//    //              Include ability to only show cards NOT in a deck, or NOT a special variant
//    //          Seeing what cards I'm missing for a set (so I can build my collection)
//    //          Looking at buylist cards
//    //          Looking at sellList cards (aka filtering by status)
//    //
//    //  Consider making Card Search a modal
//    //
//    //  Figure out how to actually accomplish grouping cards, from a state perspective

//    data: {
//        //data state 
//        // & domain data - Data the app needs to show, use, or modify
//        isLoading: {
//            coreFilterOptions: boolean;

//            deckList: boolean;
//            deckDetail: boolean;
    
//            inventoryOverview: boolean;
//            inventoryDetail: boolean;
    
//            cardSearchResults: boolean;
//            cardSearchInventoryDetail: boolean;
//        }

//        //Is this overkill?
//        coreFilterOptions: {
//            sets: {
//                byName: {[name:string]: FilterOption}
//                allKeys: string[]
//            }
//            types: {
//                byName: {[name:string]: FilterOption}
//                allKeys: string[]
//            }
//            colors: {
//                byName: {[name:string]: FilterOption}
//                allKeys: string[]
//            }
//            rarities: {
//                byName: {[name:string]: FilterOption}
//                allKeys: string[]
//            }
//        }

//        inventoryOverviews: { // inventorySearchResults
//            byId: { [multiverseId: number]: InventoryOverviewDto };
//            allIds: number[]
//        }

//        inventoryDetail: {
//            //inventory cards
//            inventoryCardsById: { [id: number]: InventoryCard };
//            inventoryCardAllIds: number[];


//            //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
//            //--Then would need to show each inventory card for a given magic card
//            //      (all inventory cards where I.multiverseId === MC.multiverseId)

//            //magic cards belonging to inventory cards
//            cardsById: { [multiverseId: number]: MagicCard };
//            allCardIds: [];



//        }

//        cardSearchResults: {
//            searchResultsById: {[multiverseId: number]: MagicCard};
//            allSearchResultIds: number[];

//            // selectedCard: MagicCard | null; //should probably be an AppState ID
//            //what if this just used the other reducer?...
//            // inventoryDetail: InventoryDetailDto | null;
//        }

//        cardSearchPendingCards: {
//            pendingCards: { [key:number]: PendingCardsDto } //key === id, should this also have a list to track all keys?
//        }







//        //Should deckEditor be renamed to deckDetail instead of deckEditor
//        deckDetail: { //This should be grouped together because it all comes from a single API call
            
//            deckProps: DeckProperties;
            
//            //This (InventoryOverviewDto) might need a GropId or something
//            cardOverviewsById: { [multiverseId: number]: InventoryOverviewDto }

//            allCardOverviewIds: number[];

//            cardDetailsById: { [id: number]: InventoryCard }; 
//            allCardDetailIds: number[];

//            deckStats: DeckStats;


//            cardGroups: {
//                id: number;
//                name: string;
//            }[];
//        }

//        deckList: {
//            //decks: DeckProperties[]; //key = id
//            decksById: { [id: number]: DeckProperties }
//            deckIds: []
//        }
//    }

//    //app state - Data specific to the app's behavior
//    app: {
//        core: {
//            newDeckIsSaving: boolean;
//            // newDeckDto: DeckProperties;
//            visibleContainer: AppContainerEnum;
//            isCardSearchShowing: boolean;
//        }
//        inventory: { //Could probably be merged into core, right?
//            searchMethod: "name" | "quantity" | "price"; // | "sellList";
//            viewMode: "list" | "grid";
//        }
//        cardSearch: { //Could probably be merged into core, right?
//            pendingCardsSaving: boolean;
//            cardSearchMethod: "set" | "web" | "inventory";
//            selectedCard: MagicCard | null; //should probably be an AppState ID
//        }
//        deckEditor: { //Could probably be merged into core, right?
//            viewMode: "list" | "grid";
//            //
//            //selectedCard: InventoryOverviewDto | null; // should this be an ID in AppState?
            
//            //Do I want to be able to distinguish between a selected card (highlighted) and hover card (mouse-over)
//            //when hover -> show on right
//            //When select but no hover -> show select
//            //else show stats

//            //selected
//            //active
//            //targeted
//            //highlighted
//            //hovered

//            //primary/secondary ??
//            //selected/active ??
//            //(either way it could share an action)

//            selectedOverviewCardId: number; // | null;
//            secondarySelectedCardId: number; // | null;

//            //this is the filtered inventory cards that should show for a deck
//            selectedInventoryCardIds: number[];
//            //selectedInventoryCards: InventoryCard[]; //key = id, but maybe should be a list of IDs, or removed completely

//            //Next question, how exactly do I handle grouped cards?!?
            
//            //
//            groupBy: "type" | null;
//            sortBy: "cost" | null; //| "name", 
            
            








//        }
//    }

//    //ui - Data that represents how the UI is currently displayed
//    ui: {
//        //filters
//        inventoryFilterProps: CardFilterProps;
//        cardSearchFilterProps: CardFilterProps;

//        //menu anchors
//        deckListMenuAnchor: HTMLButtonElement | null; //in Deck List 
//        cardMenuAnchor: HTMLButtonElement | null; //in Deck Editor (rename?)

//        //modal for showing inventory detail?

//        //misc
//        //Should controll this with a single bool
//        deckPropsModalOpen: boolean;
//        isNewDeckModalOpen: boolean; //Should be removed

//        //newDeckDto: DeckProperties;
//        deckModalProps: DeckProperties; //shown in the deckPropsModal
//    }
//}

// export default reducers;

// export type AppState = ReturnType<typeof reducers>

