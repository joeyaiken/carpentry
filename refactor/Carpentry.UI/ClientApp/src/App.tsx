import * as React from 'react';
// import { Route } from 'react-router';
// import Layout from './components/Layout';
//import Home from './components/Home';
// import Counter from './components/Counter';
// import FetchData from './components/FetchData';
import {BrowserRouter, Redirect, Route, Switch} from "react-router-dom";
import {Home} from "./features/home/Home";
import {Settings} from "./features/settings/Settings";
import './styles/App.css';

export default () => (
  <BrowserRouter>
    <Switch>

      <Route path="/settings" component={Settings} />
      
      <Route exact path='/' component={Home} />
      {/*<Route path='/' component={Home} />*/}
      
      {/*<Route path='/counter' component={Counter} />*/}
      {/*<Route path='/fetch-data/:startDateIndex?' component={FetchData} />*/}

      <Redirect to="/" />
    </Switch>
  </BrowserRouter>
);
