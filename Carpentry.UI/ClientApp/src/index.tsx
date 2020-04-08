import React from 'react';
import ReactDOM from 'react-dom';
//import './css/mana.min.css';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux'
import * as serviceWorker from './serviceWorker';
import thunkMiddleware from 'redux-thunk'
import rootReducer from './reducers'




const store = createStore(rootReducer, applyMiddleware(thunkMiddleware));

ReactDOM.render(
    <>
        {/* <link rel="stylesheet" href="/path/to/material-icons/iconfont/material-icons.css"></link> */}
        <link href="css/mana.min.css" rel="stylesheet" type="text/css" />
        <Provider store={store}>
            {/*<App />*/}
            <h1>Hello, World!</h1>
        </Provider>
    </>,
    document.getElementById('root')
);