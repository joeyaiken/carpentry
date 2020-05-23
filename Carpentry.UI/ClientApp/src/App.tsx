import * as React from 'react';
import { Route, Switch } from 'react-router';
import AppLayout from './components/AppLayout';
// import AppHomeLayout from './components/AppHomeLayout';

// import Home from './components/Home';
// import Counter from './components/Counter';
// import FetchData from './components/FetchData';

import './styles/App.css';
import Backups from './containers/Backups';
import CardSetSettings from './containers/CardSetSettings';
import InventoryContainer from './containers/Inventory/InventoryContainer';
import { ConnectedComponent } from 'react-redux';
import DeckEditorContainer from './containers/DeckEditor/DeckEditorContainer';
import HomeContainer from './containers/Home/HomeContainer';

export default function App(): JSX.Element {

    /*
    
    /decks/add/
    /decks/import/
    /decks/:id/props/
    /decks/:id/addCards/
    /decks/:id/
    /inventory/addCards/
    /inventory/:detail/
    /inventory/
    /
    
    */
    const routes: {
        path: string,
        component: ConnectedComponent<any, any>,
        name: string;
    }[] = [
        {
            path: '/Decks/:deckId',
            component: DeckEditorContainer,
            name: 'Carpentry - Deck Editor'
        },
        {
            path: '/Inventory',
            component: InventoryContainer,
            name: 'Inventory'
        },
        {
            path: '/settings/sets',
            component: CardSetSettings,
            name: 'Carpentry'
        },
        {
            path: '/settings/backups',
            component: Backups,
            name: 'Carpentry'
        },
        // {
        //     path: '/settings',
        //     component: null,
        //     name: 'Settings'
        // },
        {
            path: '/',
            component: HomeContainer,
            name: 'Carpentry'
        },
        // {
        //     path: '',
        //     component: null,
        //     name: 'Carpentry'
        // },
    ];



    return(
        <AppLayout routes={routes}>
            <Switch>
                {
                    routes.map(route => <Route path={route.path} component={route.component} />)
                }

                {/* <Route path='/Decks/:deckId' component={DeckEditor} /> */}
                
                {/* <Route path='/Decks' >
                    Deck List
                </Route> */}

                {/* <Route path='/Inventory' component={Inventory} /> */}
                
                {/* <Route path='/settings/sets' component={CardSetSettings} /> */}
                    {/* <CardSetSettings />
                </Route> */}

                {/* <Route path='/settings/backups' >
                    <Backups />
                </Route> */}
{/* 
                <Route path='/settings' >
                    Settings
                </Route> */}
{/* 
                <Route path='/' >
                    <Home />
                </Route> */}
            </Switch>
            {/* <Route exact path='/' component={Home} />
            <Route path='/counter' component={Counter} />
            <Route path='/fetch-data/:startDateIndex?' component={FetchData} /> */}
        </AppLayout>
    )
}