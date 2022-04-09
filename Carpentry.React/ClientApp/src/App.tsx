import * as React from 'react';
import { Switch, Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect, DispatchProp } from 'react-redux';
import DecksLayout from "../../../Carpentry/ClientApp/src/decks/DecksLayout";
import InventoryLayout from "../../../Carpentry/ClientApp/src/inventory/InventoryLayout";
import TrackedSetsContainer from "../../../Carpentry/ClientApp/src/settings/tracked-sets/TrackedSetsContainer";
import NewDeckContainer from "../../../Carpentry/ClientApp/src/decks/new-deck/NewDeckContainer";
import HomeContainer from "../../../Carpentry/ClientApp/src/home/HomeContainer";
import ImportDeckContainer from "../../../Carpentry/ClientApp/src/decks/import-deck/ImportDeckContainer";
import {AppState} from "../../../Carpentry/ClientApp/src/configureStore";

// import './custom.css'

interface PropsFromState { }

type AppContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class AppContainer extends  React.Component<AppContainerProps>{
  
  componentDidMount() {
    //this.props.dispatch(requestCoreData());
  }
  
  render() {
    return(<AppLayout />);
  }
}

function AppLayout(): JSX.Element {
  return (
    <Switch>
      <Route exact path="/" component={HomeContainer} />
      <Route path="/decks/" component={DecksLayout} />
      <Route path="/inventory" component={InventoryLayout} />
      <Route path="/settings/sets" component={TrackedSetsContainer} />
      <Route path="/add-deck">
        <NewDeckContainer />
        <HomeContainer />
      </Route>
      <Route path="/import-deck">
        <ImportDeckContainer />
        <HomeContainer />
      </Route>
    </Switch>
  );
}

function mapStateToProps(state: AppState): PropsFromState {
  const result: PropsFromState = {
  }
  return result;
}
export default connect(mapStateToProps)(AppContainer);



// export default () => (
//     <Layout>
//         <Route exact path='/' component={Home} />
//         <Route path='/counter' component={Counter} />
//         <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
//     </Layout>
// );
