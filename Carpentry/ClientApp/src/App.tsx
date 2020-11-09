//This is the core "app" component.
//I need some place to load core values from the API
//Because of that, it might make more sense for this to be a connected component, even if it just displays the routing functional component

import * as React from 'react';
import { Route, Switch } from 'react-router';
// import AppLayout from './_components/AppLayout';
import './styles/App.css';
// import Backups from './_components/Backups';
// import InventoryContainer from './containers/Inventory/InventoryContainer';
import { ConnectedComponent } from 'react-redux';
// import DeckEditorContainer from './_containers/DeckEditor/DeckEditorContainer';
import HomeContainer from './home/HomeContainer';
// import CardSearchContainer from './_containers/CardSearch/CardSearchContainer';
import { connect, DispatchProp } from 'react-redux';
import { AppState } from './configureStore'
// import { requestCoreData } from './_actions/coreActions';
// import CardSetSettingsContainer from './_containers/CardSetSettings/CardSetSettingsContainer';
import './styles/mana.css';
// import { Typography } from '@material-ui/core';
import AppLayout from './common/components/AppLayout';
import DecksLayout from './decks/DecksLayout';
import InventoryLayout from './inventory/InventoryLayout';
import { requestCoreData } from './common/state/coreDataActions';

// import InventoryLayout from './inventory/InventoryLayout';
// import DecksLayout from './decks/DecksLayout';

interface PropsFromState {
}

type AppContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class AppContainer extends React.Component<AppContainerProps>{
    // constructor(props: AppContainerProps) {
    //     super(props);
    // }

    componentDidMount() {
        // console.log('calling core data')
        this.props.dispatch(requestCoreData());
    }

    render() {
        return (<App />);
    }
}

function App(): JSX.Element {

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
    /
    
    */
    const routes: {
        path: string,
        component: ConnectedComponent<any, any>,
        name: string;
        customProps: any;
    }[] = [
        // {
        //     path: '/Decks/:deckId/addCards',
        //     component: CardSearchContainer,
        //     name: 'Deck Editor - Add Cards',
        //     customProps: {
        //         searchContext: "deck"
        //     }
        // },
        // {
        //     path: '/Decks/:deckId',
        //     component: DeckEditorContainer,
        //     name: 'Carpentry - Deck Editor',
        //     customProps: {}
        // },
        // {
        //     path: '/settings/sets',
        //     component: CardSetSettingsContainer,
        //     name: 'Carpentry',
        //     customProps: {}
        // },
        // {
        //     path: '/settings/backups',
        //     component: Backups,
        //     name: 'Carpentry',
        //     customProps: {}
        // },
        // {
        //     path: '/',
        //     component: HomeContainer,
        //     name: 'Carpentry',
        //     customProps: {}
        // },

    ];



    return(
        <AppLayout routes={routes}>
            <Switch>
                <Route path="/decks" component={DecksLayout} />
                <Route path="/inventory" component={InventoryLayout} />
                {/* <Route path="/settings" component={SettingsLayout} /> */}
                <Route path="/" component={HomeContainer} />

                {/* <Route path="/inventory/addCards" render={(props) => <CardSearchContainer {...props} searchContext="inventory" />} />    
                <Route path="/inventory" component={InventoryContainer} /> */}


                


                {/* { routes.map(route => <Route key={route.path} path={route.path} render={(props) => <route.component {...props} {...route.customProps} />} />) } */}


                {/* <Route path='/Inventory/addCards' render={(props) => <CardSearchContainer {...props} searchContext="inventory" />} /> */}

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

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
    }
    return result;
}
export default connect(mapStateToProps)(AppContainer);

