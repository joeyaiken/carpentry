import React from 'react';
import ReactDOM from 'react-dom';
//import './css/mana.min.css';
import { Provider } from 'react-redux';
// import { createStore, applyMiddleware } from 'redux'
// import * as serviceWorker from './serviceWorker';
// import thunkMiddleware from 'redux-thunk'
// import rootReducer from './reducers'
import App from './App';
import { createBrowserHistory } from 'history';

import configureStore from './_reducers/configureStore';
import { ConnectedRouter } from 'connected-react-router';

// const store = createStore(rootReducer, applyMiddleware(thunkMiddleware));


// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
const history = createBrowserHistory({ basename: baseUrl });


const store = configureStore(history)

ReactDOM.render(

        <Provider store={store}>
            <ConnectedRouter history={history}>
                <App />

            </ConnectedRouter>
        {/* <link rel="stylesheet" href="/path/to/material-icons/iconfont/material-icons.css"></link> */}
            {/* <link href="styles/mana.min.css" rel="stylesheet" type="text/css" /> */}
        
            {/* <App /> */}
            {/* <h1>Hello, World!</h1> */}
        </Provider>
    ,
    document.getElementById('root')
);