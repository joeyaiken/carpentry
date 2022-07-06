import React from 'react';
import {BrowserRouter,Route, Switch} from "react-router-dom";
import {Home} from "./features/home/Home";
// import 'App.module.css'
import './styles/App.css';
// import HomeContainer from './home/HomeContainer';
// import './styles/mana.css';

export const App = () => {
  return(
    <BrowserRouter>
      <Switch>
        <Route path="/" component={Home} />
      </Switch>
    </BrowserRouter>
  )
}