import { Dispatch } from 'react';
import { Action, Reducer } from 'redux';
 import { AppThunkAction } from '../store';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

// export interface WeatherForecastsState {
//     isLoading: boolean;
//     startDateIndex?: number;
//     forecasts: WeatherForecast[];
// }

// export interface WeatherForecast {
//     date: string;
//     temperatureC: number;
//     temperatureF: number;
//     summary: string;
// }

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


// interface RequestWeatherForecastsAction {
//     type: 'REQUEST_WEATHER_FORECASTS';
//     startDateIndex: number;
// }

// interface ReceiveWeatherForecastsAction {
//     type: 'RECEIVE_WEATHER_FORECASTS';
//     startDateIndex: number;
//     forecasts: WeatherForecast[];
// }

// export const APP_CONFIG_REQUESTED = 'APP_CONFIG_REQUESTED';
//interface APP_CONFIG_REQUESTED: 'APP_CONFIG_REQUESTED'
// type APP_CONFIG_RECEIVED = 'APP_CONFIG_REQUESTED';
// type APP_CONFIG_REQUESTED = 'APP_CONFIG_REQUESTED';
interface AppConfigRequested {
    type: 'APP_CONFIG_REQUESTED',
}

// type APP_CONFIG_RECEIVED = 'APP_CONFIG_RECEIVED'
interface AppConfigReceived { 
    type: 'APP_CONFIG_RECEIVED',
    payload: AppConfigResult,
}

// export const appConfigRequested = (): ReduxAction => ({
//     type: APP_CONFIG_REQUESTED,
// });

// export const APP_CONFIG_RECEIVED = 'APP_CONFIG_RECEIVED';

// export const appConfigReceived = (result: AppConfigResult): ReduxAction => ({
//     type: APP_CONFIG_RECEIVED,
//     payload: result,
// })


// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
// type KnownAction = RequestWeatherForecastsAction | ReceiveWeatherForecastsAction;
type KnownAction = AppConfigRequested | AppConfigReceived;
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

// export const requestAppConfig = (): AppThunkAction< => (dispatch, getState) =>


// {
//     // return (dispatch, getState: any) => {
//     //     // return getAppConfig(dispatch, getState());
//     //     const appState = getState();
//     //     const queryIsLoading = false;
//     //     if(queryIsLoading) return;
    
//     //     dispatch(appConfigRequested());
    
//     //     appConfigService.getAppConfig().then((result) => {
//     //         dispatch(appConfigReceived(result));
//     //     });

//     //     ////
//     // }
// }

// function getAppConfig(dispatch: Dispatch, appState: AppState): any {
//     const queryIsLoading = false;
//     if(queryIsLoading) return;

//     dispatch(appConfigRequested());

//     appConfigService.getAppConfig().then((result) => {
//         dispatch(appConfigReceived(result));
//     });
// }

export const actionCreators = {
    // requestWeatherForecasts: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
    //     // Only load data if it's something we don't already have (and are not already loading)
    //     const appState = getState();
    //     if (appState && appState.weatherForecasts && startDateIndex !== appState.weatherForecasts.startDateIndex) {
    //         fetch(`weatherforecast`)
    //             .then(response => response.json() as Promise<WeatherForecast[]>)
    //             .then(data => {
    //                 dispatch({ type: 'RECEIVE_WEATHER_FORECASTS', startDateIndex: startDateIndex, forecasts: data });
    //             });

    //         dispatch({ type: 'REQUEST_WEATHER_FORECASTS', startDateIndex: startDateIndex });
    //     }
    // },

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

// const unloadedState: WeatherForecastsState = { forecasts: [], isLoading: false };

export const reducer: Reducer<AppReducerState> = (state = initialState, incomingAction: Action): AppReducerState => {
//     if (state === undefined) {
//         return unloadedState;
//     }

    const action = incomingAction as KnownAction;
    switch (action.type) {
//         case 'REQUEST_WEATHER_FORECASTS':
//             return {
//                 startDateIndex: action.startDateIndex,
//                 forecasts: state.forecasts,
//                 isLoading: true
//             };
//         case 'RECEIVE_WEATHER_FORECASTS':
//             // Only accept the incoming data if it matches the most recent request. This ensures we correctly
//             // handle out-of-order responses.
//             if (action.startDateIndex === state.startDateIndex) {
//                 return {
//                     startDateIndex: action.startDateIndex,
//                     forecasts: action.forecasts,
//                     isLoading: false
//                 };
//             }
//             break;
    }

    return state;
};



export const AppReducer = (state: AppReducerState | undefined, action: ReduxAction): AppReducerState => {
    if(state === undefined) return initialState;

    switch(action.type) {
        case 'APP_CONFIG_REQUESTED': { //TODO - Magic strings are bad
            return {
                ...state,
                isLoading: true,
                appConfigStatus: 'Loading...',
            };
        }

        case 'APP_CONFIG_RECEIVED': { //TODO - Magic strings are bad
            console.log('reducer received', action);
            return {
                ...state,
                isLoading: false,
                appConfigStatus: 'Loaded',
                appConfig: action.payload,
            };
        }

        default: return (state);

//         export const APP_CONFIG_REQUESTED = 'APP_CONFIG_REQUESTED';
// export const appConfigRequested = (): ReduxAction => ({
//     type: APP_CONFIG_REQUESTED,
// });

// export const APP_CONFIG_RECEIVED = 'APP_CONFIG_RECEIVED';

    }
}

const initialState: AppReducerState = {
    isLoading: false,
    appConfigStatus: 'Not Loaded',
    appConfig: null,
}