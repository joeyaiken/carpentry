import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import { History } from 'history';

import cardDetailReducer, { State as CardDetailReducerState } from './features/decks/card-detail/cardDetailSlice';
// import { cardSearchDataReducer, State as CardSearchDataReducerState } from './common/card-search/data/CardSearchDataSlice';
// import { cardSearchReducer, State as CardSearchReducersState } from './common/card-search/CardSearchSlice';
import cardTagsReducer, { State as CardTagsReducerState } from './features/decks/card-tags/cardTagsSlice';
import coreDataReducer, { State as CoreDataReducerState } from './common/coreDataSlice';
import deckAddCardsReducer, { State as DeckAddCardsReducerState } from './features/decks/deck-add-cards/deckAddCardsSlice';
import deckEditorReducer, { DeckEditorState as DeckEditorReducerState } from './features/decks/deck-editor/deckEditorSlice';
import deckExportReducer, { State as DeckExportReducerState } from './features/decks/deck-export/deckExportSlice';
import decksDataReducer, { State as DecksDataReducerState } from './features/decks/decksDataSlice';
import homeReducer, { State as HomeReducerState } from './features/home/homeSlice';
import importDeckReducer, { State as ImportDeckReducerState } from './features/decks/import-deck/importDeckSlice';
import inventoryAddCardsReducer, { State as InventoryAddCardsReducerState } from './features/inventory/inventory-add-cards/inventoryAddCardsSlice';
import inventoryDataReducer, { State as InventoryDataReducerState } from './features/inventory/inventoryDataSlice';
import inventoryOverviewAppReducer, { State as InventoryOverviewState } from './features/inventory/inventory-overview/inventoryOverviewSlice';
import newDeckReducer, { State as NewDeckReducerState } from './features/decks/new-deck/newDeckSlice';
// import { settingsDataReducer, State as SettingsDataReducerState } from './settings/SettingsDataSlice';
import trackedSetsReducer, { State as TrackedSetsReducerState } from './features/settings/tracked-sets/trackedSetsSlice';
import trimmingToolReducer, { State as TrimmingToolReducerState } from './features/inventory/trimming-tool/trimmingToolSlice';

const rootReducer = (history: History) => combineReducers({
  home: homeReducer,

  decks: combineReducers({
    newDeck: newDeckReducer,
    importDeck: importDeckReducer,
    deckEditor: deckEditorReducer,
    cardDetail: cardDetailReducer,
    cardTags: cardTagsReducer,
    deckAddCards: deckAddCardsReducer,
    deckExport: deckExportReducer,
    data: decksDataReducer,
  }),

  inventory: combineReducers({
    overviews: inventoryOverviewAppReducer,
    inventoryAddCards: inventoryAddCardsReducer,
    trimmingTool: trimmingToolReducer,
    data: inventoryDataReducer,
  }),

  core: coreDataReducer,

  settings: trackedSetsReducer,

  router: connectRouter(history),
})

export interface AppState {
  home: HomeReducerState,
  decks: {
    newDeck: NewDeckReducerState,
    importDeck: ImportDeckReducerState,
    deckEditor: DeckEditorReducerState,
    cardDetail: CardDetailReducerState,
    cardTags: CardTagsReducerState,
    deckAddCards: DeckAddCardsReducerState,
    deckExport: DeckExportReducerState,
    data: DecksDataReducerState
  },
  inventory:{
    overviews: InventoryOverviewState,
    inventoryAddCards: InventoryAddCardsReducerState,
    trimmingTool: TrimmingToolReducerState,
    data: InventoryDataReducerState
  },
  core: CoreDataReducerState,
  settings: TrackedSetsReducerState,
}

export default function configureStore(history: History, initialState?: AppState) {
  const middleware = [
    thunk,
    routerMiddleware(history)
  ];

  const enhancers: any[] = [];
  const windowIfDefined = typeof window === 'undefined' ? null : window as any;
  if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
    enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__());
  }

  return createStore(
    rootReducer(history),
    compose(applyMiddleware(...middleware), ...enhancers)
  );
}