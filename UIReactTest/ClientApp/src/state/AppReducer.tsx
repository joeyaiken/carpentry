 import { AppThunkAction } from '../store/configureStore';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AppReducerState {
    isLoading: boolean;
    appConfigStatus: 'Not Loaded'|'Loading...'|'Loaded'; //string;
    appConfig: AppConfigResult | null;
}

export interface AppConfigResult {
    configString: string;
    lastUpdated: Date;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface AppConfigRequested {
    type: 'APP_CONFIG_REQUESTED',
}

interface AppConfigReceived { 
    type: 'APP_CONFIG_RECEIVED',
    payload: AppConfigResult,
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = AppConfigRequested | AppConfigReceived;
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    getAppConfig: (): AppThunkAction<KnownAction> => (dispatch, getState): void => {
        const appState = getState();
        console.log('appState', appState);
        if(appState && appState.appConfig && !appState.appConfig.isLoading) {
            dispatch({ type: 'APP_CONFIG_REQUESTED'});
            fetch('api/AppConfig')
                .then(response => response.json() as Promise<AppConfigResult>)
                .then(data => {
                    console.log('data', data);
                    dispatch({ type: 'APP_CONFIG_RECEIVED', payload: data })
                });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const AppReducer = (state = initialState, action: ReduxAction): AppReducerState => {
    switch(action.type) {
        case 'APP_CONFIG_REQUESTED': {
            return {
                ...state,
                isLoading: true,
                appConfigStatus: 'Loading...',
            };
        }
        case 'APP_CONFIG_RECEIVED': {
            console.log('reducer received', action);
            return {
                ...state,
                isLoading: false,
                appConfigStatus: 'Loaded',
                appConfig: action.payload,
            };
        }
        default: return (state);
    }
}

const initialState: AppReducerState = {
    isLoading: false,
    appConfigStatus: 'Not Loaded',
    appConfig: null,
}