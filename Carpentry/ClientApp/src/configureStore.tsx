import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import { History } from 'history';
import { 
    ////AppState, 
    // reducers 
} from './';

//TODO - consider renaming this file to just "store.tsx"

const rootReducer = (history: History) => combineReducers({
    //// reducers,
    // data: reducers.data,
    // app: reducers.app,
    // ui: reducers.ui,
    router: connectRouter(history)
});

export type AppState = ReturnType<typeof rootReducer>;


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
