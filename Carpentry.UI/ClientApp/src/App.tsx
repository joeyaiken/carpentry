//This is the core "app" component.
//I need some place to load core values from the API
//Because of that, it might make more sense for this to be a connected component, even if it just displays the routing functional component

import * as React from 'react';
import { Route, Switch } from 'react-router';
import AppLayout from './components/AppLayout';
import './styles/App.css';
import Backups from './components/Backups';
import InventoryContainer from './containers/Inventory/InventoryContainer';
import { ConnectedComponent } from 'react-redux';
import DeckEditorContainer from './containers/DeckEditor/DeckEditorContainer';
import HomeContainer from './home/HomeContainer';
import CardSearchContainer from './containers/CardSearch/CardSearchContainer';
import { connect, DispatchProp } from 'react-redux';
import { AppState } from './reducers'
import { requestCoreData } from './actions/coreActions';
import CardSetSettingsContainer from './containers/CardSetSettings/CardSetSettingsContainer';
import './styles/mana.css';

{/* <link href="styles/mana.min.css" rel="stylesheet" type="text/css" /> */}

interface PropsFromState {
}

type AppContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class AppContainer extends React.Component<AppContainerProps>{
    constructor(props: AppContainerProps) {
        super(props);
    }

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
        {
            path: '/Decks/:deckId/addCards',
            component: CardSearchContainer,
            name: 'Deck Editor - Add Cards',
            customProps: {
                searchContext: "deck"
            }
        },
        {
            path: '/Decks/:deckId',
            component: DeckEditorContainer,
            name: 'Carpentry - Deck Editor',
            customProps: {}
        },
        {
            path: '/Inventory/addCards',
            component: CardSearchContainer,
            name: 'Inventory - Add Cards',
            customProps: {
                searchContext: "inventory"
            }
        },
        {
            path: '/Inventory',
            component: InventoryContainer,
            name: 'Inventory',
            customProps: {}
        },
        {
            path: '/settings/sets',
            component: CardSetSettingsContainer,
            name: 'Carpentry',
            customProps: {}
        },
        {
            path: '/settings/backups',
            component: Backups,
            name: 'Carpentry',
            customProps: {}
        },
        // {
        //     path: '/settings',
        //     component: null,
        //     name: 'Settings'
        // },
        {
            path: '/',
            component: HomeContainer,
            name: 'Carpentry',
            customProps: {}
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
                { routes.map(route => <Route path={route.path} render={(props) => <route.component {...props} {...route.customProps} />} />) }


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