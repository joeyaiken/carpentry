import {App} from './App';
// import configureStore from './configureStore';
// import { ConnectedRouter } from 'connected-react-router';
// import { createBrowserHistory } from 'history';
import { Provider } from 'react-redux';
import React from 'react';
import ReactDOM from 'react-dom';
import {store} from './configureStore'

// console.log('wut');
// Create browser history to use in the Redux store
// const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
// const history = createBrowserHistory({ basename: baseUrl });
// const store = configureStore(history)

// console.log('wut');
ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      {/*<ConnectedRouter history={history}>*/}
        <App />
      {/*</ConnectedRouter>*/}
    </Provider>
  </React.StrictMode>
  , document.getElementById('root')
);