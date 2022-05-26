import {combineReducers} from 'redux';
import { decksDataReducer, State as DecksDataReducerState } from './decks/state/decksDataReducer';
import { inventoryDataReducer, InventoryDataReducerState } from './inventory/state/inventoryDataReducer';
import { inventoryOverviewAppReducer, State as InventoryOverviewState } from './inventory/inventory-overview/state/InventoryOverviewAppReducer';
// import { cardSearchDataReducer, State as CardSearchDataReducerState } from './common/card-search/data/CardSearchDataReducer';
import { deckEditorReducer, State as DeckEditorReducerState } from './decks/deck-editor/state/DeckEditorReducer';
// import { cardSearchReducer, State as CardSearchReducersState } from './common/card-search/state/CardSearchReducer';
import { newDeckReducer, State as NewDeckReducerState } from './decks/new-deck/state/NewDeckReducer';
import { deckAddCardsReducer, State as DeckAddCardsReducerState } from './decks/deck-add-cards/state/DeckAddCardsReducer';
import { inventoryAddCardsReducer, State as InventoryAddCardsReducerState } from './inventory/inventory-add-cards/state/InventoryAddCardsReducer';
import { cardDetailReducer, State as CardDetailReducerState } from './decks/card-detail/state/CardDetailReducer';
import { homeReducer, State as HomeReducerState } from './home/state/HomeReducer';
import { deckExportReducer, State as DeckExportReducerState } from './decks/deck-export/state/DeckExportReducer';
import { cardTagsReducer, State as CardTagsReducerState } from './decks/card-tags/state/CardTagsReducer';
import { importDeckReducer, State as ImportDeckReducerState } from './decks/import-deck/state/ImportDeckReducer';
//import { trimmingToolReducer, State as TrimmingToolReducerState } from './inventory/trimming-tool/state/TrimmingToolReducer';
import trimmingToolReducer from './features/inventory/trimming-tool/trimmingToolSlice';
import coreDataReducer from './features/common/coreDataSlice';
import settingsReducer from './features/settings/settingsSlice';
import {configureStore} from "@reduxjs/toolkit";

//TODO - consider renaming this file to just "store.tsx"

const rootReducer = 
  //(history: History) => 
    combineReducers({
    //// reducers,
    // data: reducers.data,
    // app: reducers.app,
    // ui: reducers.ui,

    // cardSearch: combineReducers({  // cardSearch | search | core | common
    //     //state | app | cardSearch | search
    //     state: cardSearchReducer,
    //     data: cardSearchDataReducer,
    // }),
    home: homeReducer,

    decks: combineReducers({
        newDeck: newDeckReducer,
        importDeck: importDeckReducer,
        deckEditor: deckEditorReducer,
        cardDetail: cardDetailReducer,
        cardTags: cardTagsReducer,
        //cardSearch | addDeckCards | deckAddCards | ??
        deckAddCards: deckAddCardsReducer,
        deckExport: deckExportReducer,
        data: decksDataReducer,
    }),

    inventory: combineReducers({
        overviews: inventoryOverviewAppReducer,
        inventoryAddCards: inventoryAddCardsReducer,
        // trimmingToolLegacy: trimmingToolReducer,
        trimmingTool: trimmingToolReducer,
        data: inventoryDataReducer,
    }),

    core: coreDataReducer,
    settings: settingsReducer,
        
    //router: connectRouter(history)
});

export const store = configureStore({
    reducer: rootReducer,
})

export type RootState = ReturnType<typeof rootReducer>;

export type AppDispatch = typeof store.dispatch;

// export default function configureStore(history: History, initialState?: AppState) {
//     const middleware = [
//         thunk,
//         routerMiddleware(history)
//     ];
//
//     // const rootReducer = combineReducers({
//     //     ...reducers,
//     //     router: connectRouter(history)
//     // });
//
//     const enhancers: any[] = [];
//     const windowIfDefined = typeof window === 'undefined' ? null : window as any;
//     if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
//         enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__());
//     }
//
//     return createStore(
//         rootReducer(history),
//         // initialState,
//         compose(applyMiddleware(...middleware), ...enhancers)
//     );
// }
