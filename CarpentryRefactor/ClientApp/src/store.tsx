import {combineReducers} from 'redux';
import {configureStore} from "@reduxjs/toolkit";

//TODO - consider renaming this file to just "store.tsx"
const rootReducer = combineReducers({
  
});

export const store = configureStore({
  reducer: rootReducer,
})

export type RootState = ReturnType<typeof rootReducer>;

export type AppDispatch = typeof store.dispatch;
