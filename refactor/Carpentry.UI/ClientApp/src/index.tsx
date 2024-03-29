import 'bootstrap/dist/css/bootstrap.css';

import React from 'react';
import ReactDOM from 'react-dom';
import {Provider} from 'react-redux';
// import { ConnectedRouter } from 'connected-react-router';
// import { createBrowserHistory } from 'history';
// import configureStore from './store/configureStore';
import App from './App';
// import registerServiceWorker from './registerServiceWorker';

// Create browser history to use in the Redux store
// const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
// const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
// const store = configureStore(history);
import {store} from './store';

ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
        {/*<ConnectedRouter history={history}>*/}
            <App />
        {/*</ConnectedRouter>*/}
    </Provider>
  </React.StrictMode>,
  document.getElementById('root'));

// registerServiceWorker();
