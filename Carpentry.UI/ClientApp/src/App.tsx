import * as React from 'react';
import { Route, Switch } from 'react-router';
import AppLayout from './components/AppLayout';
import AppHomeLayout from './components/AppHomeLayout';
import Home from './containers/Home';
// import Home from './components/Home';
// import Counter from './components/Counter';
// import FetchData from './components/FetchData';

import './styles/App.css';
import DeckEditor from './containers/DeckEditor';

export default function App(): JSX.Element {
    return(
        <AppLayout>
            <Switch>
                <Route path='/Decks/:deckId' >
                    <DeckEditor />
                </Route>
                
                <Route path='/Decks' >
                    Deck List
                </Route>

                <Route path='/Inventory' >
                    Inventory
                </Route>

                <Route path='/' >
                    <Home />
                </Route>
            </Switch>
            {/* <Route exact path='/' component={Home} />
            <Route path='/counter' component={Counter} />
            <Route path='/fetch-data/:startDateIndex?' component={FetchData} /> */}
        </AppLayout>
    )
}