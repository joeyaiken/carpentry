import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import { History } from 'history';
import { 
    ////AppState, 
    // reducers 
} from './';
import { decksDataReducer, State as DecksDataReducerState } from './decks/state/decksDataReducer';
import { inventoryDataReducer, InventoryDataReducerState } from './inventory/state/inventoryDataReducer';
import { inventoryOverviewAppReducer, State as InventoryOverviewState } from './inventory/inventory-overview/state/InventoryOverviewAppReducer';
import { coreDataReducer, State as CoreDataReducerState } from './common/state/coreDataReducer';

//TODO - consider renaming this file to just "store.tsx"

const rootReducer = (history: History) => combineReducers({
    //// reducers,
    // data: reducers.data,
    // app: reducers.app,
    // ui: reducers.ui,
    decks: combineReducers({
        data: decksDataReducer,
    }),

    inventory: combineReducers({
        overviews: inventoryOverviewAppReducer,
        data: inventoryDataReducer,
    }),

    core: combineReducers({
        data: coreDataReducer,
    }),

    router: connectRouter(history)
});

// export type AppState = ReturnType<typeof rootReducer>;

export interface AppState {
    //router state
    //
    decks: {
        data: DecksDataReducerState
    }
    inventory:{
        //or
        //app: {
        //  overviews
        //  detail
        //}
        overviews: InventoryOverviewState,
        data: InventoryDataReducerState
    }
    core: {
        data: CoreDataReducerState
    }
}

export default function configureStore(history: History, initialState?: AppState) {
    const middleware = [
        thunk,
        routerMiddleware(history)
    ];

    // const rootReducer = combineReducers({
    //     ...reducers,
    //     router: connectRouter(history)
    // });

    const enhancers: any[] = [];
    const windowIfDefined = typeof window === 'undefined' ? null : window as any;
    if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
        enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__());
    }

    return createStore(
        rootReducer(history),
        // initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}
