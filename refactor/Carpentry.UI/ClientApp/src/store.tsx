import {combineReducers} from 'redux';
import {configureStore} from "@reduxjs/toolkit";

import settingsReducer from './features/settings/settingsSlice';

const rootReducer = combineReducers({
  settings: settingsReducer,
});

export const store = configureStore({
  reducer: rootReducer,
})

export type RootState = ReturnType<typeof rootReducer>;

export type AppDispatch = typeof store.dispatch;
