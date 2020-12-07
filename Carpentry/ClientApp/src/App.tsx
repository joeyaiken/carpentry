//This is the core "app" component.
//I need some place to load core values from the API
//Because of that, it might make more sense for this to be a connected component, even if it just displays the routing functional component
import * as React from 'react';
import { Route, Switch } from 'react-router';
import './styles/App.css';
import HomeContainer from './home/HomeContainer';
import { connect, DispatchProp } from 'react-redux';
import { AppState } from './configureStore'
import './styles/mana.css';
import DecksLayout from './decks/DecksLayout';
import InventoryLayout from './inventory/InventoryLayout';
import { requestCoreData } from './common/state/coreDataActions';
import TrackedSetsContainer from './settings/tracked-sets/TrackedSetsContainer';
import NewDeckContainer from './decks/new-deck/NewDeckContainer';
import { push } from 'react-router-redux';

interface PropsFromState {
}

type AppContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class AppContainer extends React.Component<AppContainerProps>{
    // constructor(props: AppContainerProps) {
    //     super(props);
    // }

    componentDidMount() {
        this.props.dispatch(requestCoreData());
        // this.props.dispatch(push('/decks/61'));
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
    */


    return(
        // <AppLayout>
            <Switch>
                <Route path="/decks/" component={DecksLayout} />
                <Route path="/inventory" component={InventoryLayout} />
                {/* <Route path="/settings" component={SettingsLayout} /> */}
                <Route path="/settings/sets" component={TrackedSetsContainer} />

                <Route path="/add-deck">
                    <NewDeckContainer />
                    <HomeContainer />
                </Route>

                <Route path="/" component={HomeContainer} />

                {/* <Route path='/settings/backups' >
                    <Backups />
                </Route> */}
                {/* <Route path='/settings' >
                    Settings
                </Route> */}
            </Switch>
        // </AppLayout>
    )
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
    }
    return result;
}
export default connect(mapStateToProps)(AppContainer);

