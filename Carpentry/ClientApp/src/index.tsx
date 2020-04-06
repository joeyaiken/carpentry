import React from 'react';
import ReactDOM from 'react-dom';
import './css/mana.min.css';
//import manaB from '../img/B.svg'
// import 'mana-font';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux'
import thunkMiddleware from 'redux-thunk'
import rootReducer from './reducers'

// import App from './carpentry.ui/containers/App';
import App from './containers/App';
import * as serviceWorker from './serviceWorker';
// import './index.css';
// import '../node_modules/material-icons/iconfont/material-icons.css';

/* <link href="css/mana.min.css" rel="stylesheet" type="text/css" /> */

//Eventually this should call something that initializes the store as well...
const store = createStore(rootReducer, applyMiddleware(thunkMiddleware));

ReactDOM.render(
    <>
        {/* <link rel="stylesheet" href="/path/to/material-icons/iconfont/material-icons.css"></link> */}
        <link href="css/mana.min.css" rel="stylesheet" type="text/css" />
        <Provider store={store}>
            <App />
        </Provider>
    </>, 
    document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();
