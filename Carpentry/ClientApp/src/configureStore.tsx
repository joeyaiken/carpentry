import {combineReducers} from 'redux';

import deckListReducer from './features/decks/deck-list/DeckListSlice'
import { newDeckReducer, State as NewDeckReducerState } from './decks/new-deck/state/NewDeckReducer';
import { importDeckReducer, State as ImportDeckReducerState } from './decks/import-deck/state/ImportDeckReducer';
import { deckEditorReducer, State as DeckEditorReducerState } from './decks/deck-editor/state/DeckEditorReducer';
// import deckEditorReducer from './decks/deck-editor/deckEditorSlice';
import deckDetailReducer from './decks/deck-editor/deckDetailSlice';

import { cardDetailReducer, State as CardDetailReducerState } from './decks/card-detail/state/CardDetailReducer';
// import cardDetailReducer from "./decks/card-detail/cardDetailSlice"; 

import deckExportReducer from './features/decks/deck-export/deckExportSlice';

// import { cardTagsReducer, State as CardTagsReducerState } from './decks/card-tags/state/CardTagsReducer';
import cardTagsReducer from './features/decks/card-tags/cardTagsSlice';

import { deckAddCardsReducer, State as DeckAddCardsReducerState } from './decks/deck-add-cards/state/DeckAddCardsReducer';
import { decksDataReducer, State as DecksDataReducerState } from './decks/state/decksDataReducer';

import inventoryOverviewReducer from './features/inventory/inventory-overview/inventoryOverviewSlice';
import inventoryDetailReducer from './features/inventory/inventory-detail/InventoryDetailSlice';
import inventoryAddCardsReducer from './features/inventory/inventory-add-cards/inventoryAddCardsSlice';
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
    // home: homeReducer,

    decks: combineReducers({
      deckList: deckListReducer,
      
      newDeck: newDeckReducer,
      importDeck: importDeckReducer,
      deckEditor: deckEditorReducer,
      deckDetailData: deckDetailReducer,
      
      cardDetail: cardDetailReducer,
      cardTags: cardTagsReducer,
      deckAddCards: deckAddCardsReducer,
      deckExport: deckExportReducer,
      
      // data: decksDataReducer,
    }),

    inventory: combineReducers({
      overviews: inventoryOverviewReducer,
      detail: inventoryDetailReducer,
      inventoryAddCards: inventoryAddCardsReducer,
      trimmingTool: trimmingToolReducer,
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
