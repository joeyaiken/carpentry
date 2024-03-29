﻿import { configureStore } from '@reduxjs/toolkit'
import {combineReducers} from "redux";
// import cardTagsReducer from '../features/decks/card-tags/cardTagsSlice'
// import decksDataReducer from '../features/decks/decksDataSlice'
import deckDetailReducer from '../features/decks/deckDetailSlice'
import deckOverviewsReducer from '../features/decks/deckOverviewsSlice'
import deckEditorReducer from '../features/decks/deck-editor/deckEditorSlice'
import deckAddCardsReducer from '../features/decks/deck-add-cards/deckAddCardsSlice'
import settingsReducer from '../features/settings/settingsSlice'
import inventoryOverviewReducer from '../features/inventory/inventory-overview/inventoryOverviewSlice'
import inventoryAddCardsReducer from "../features/inventory/inventory-add-cards/inventoryAddCardsSlice";
import coreDataReducer from '../common/coreDataSlice';
// import {connectRouter} from "connected-react-router";
// import {History} from 'history';

const rootReducer = combineReducers({
  
  decks: combineReducers({
    // newDeck: newDeckReducer,
    // importDeck: importDeckReducer,
    deckEditor: deckEditorReducer,
    // cardDetail: cardDetailReducer,
    // cardTags: cardTagsReducer,
    // //cardSearch | addDeckCards | deckAddCards | ??
    deckAddCards: deckAddCardsReducer,
    // deckExport: deckExportReducer,
    // data: decksDataReducer,
    
    overviews: deckOverviewsReducer,
    detail: deckDetailReducer,
  }),

  inventory: combineReducers({
    overviews: inventoryOverviewReducer,
    inventoryAddCards: inventoryAddCardsReducer,
  //   trimmingTool: trimmingToolReducer,
  //   data: inventoryDataReducer,
  }),
  //
  
  core: coreDataReducer,
  // core: combineReducers({
  //   data: coreDataReducer,
  // }),
  
  
  //
  // settings: combineReducers({
  //   trackedSets: trackedSetsReducer,
  // }),
  
  settings: settingsReducer,

  // router: connectRouter(history)
})

export const store = configureStore({
  reducer: rootReducer,
})

export type RootState = ReturnType<typeof rootReducer>;

export type AppDispatch = typeof store.dispatch;