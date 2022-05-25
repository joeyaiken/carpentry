// This is the core "app" component.
// I need some place to load core values from the API, there might be a better place to load that data...
// ...but it will just exist here for now
import React, {useEffect} from "react";
import {BrowserRouter,Switch,Route} from 'react-router-dom';
import './styles/App.css';
import HomeContainer from './home/HomeContainer';
import './styles/mana.css';
import DecksLayout from './decks/DecksLayout';
import InventoryLayout from './inventory/InventoryLayout';
import NewDeckContainer from './decks/new-deck/NewDeckContainer';
import ImportDeckContainer from './decks/import-deck/ImportDeckContainer';
import {Settings} from './settings/Settings';
import {useAppDispatch, useAppSelector} from "./hooks";
import {ApiStatus} from "./enums";
import {getCoreData} from "./common/coreDataSlice";

export const App = (): JSX.Element  => {
  const coreDataStatus = useAppSelector(state => state.core.filterDataStatus);
  const dispatch = useAppDispatch();
  useEffect(() => {
    if(coreDataStatus === ApiStatus.uninitialized) dispatch(getCoreData());
  });

  /*
  /decks/add/
  /decks/import/
  /decks/:id/props/
  /decks/:id/addCards/
  /decks/:id/
  /inventory/addCards/
  /inventory/:detail/
  /inventory/import/
  /inventory/
  /settings/???
  /settings/cardData|trackedSets
  /settings/backups
  */

  return(
    <BrowserRouter>
      <Switch>
        <Route path="/decks/" component={DecksLayout} />
        <Route path="/inventory" component={InventoryLayout} />
        <Route path="/settings" component={Settings} />

        <Route path="/add-deck">
          <NewDeckContainer />
          <HomeContainer />
        </Route>

        <Route path="/import-deck">
          <ImportDeckContainer />
          <HomeContainer />
        </Route>

        <Route path="/" component={HomeContainer} />
      </Switch>
    </BrowserRouter>
  )
}
