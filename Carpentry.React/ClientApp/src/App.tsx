import * as React from 'react';
// import {Switch, Route, Redirect} from 'react-router';
import Layout from './components/Layout';
// import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect, DispatchProp } from 'react-redux';
import {Home} from "./features/home/Home";
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from 'react-router-dom'
import DecksLayout from "./features/decks/DecksLayout";
import {Settings} from "./features/settings/Settings";


// import './custom.css'

// interface PropsFromState { }
//
// type AppContainerProps = PropsFromState & DispatchProp<ReduxAction>;

// class AppContainer extends  React.Component<AppContainerProps>{
//  
//   componentDidMount() {
//     //this.props.dispatch(requestCoreData());
//   }
//  
//   render() {
//     return(<App />);
//   }
// }

function App(): JSX.Element {
  return (
    <Router>
      
    
    
      <Switch>
        
        <Route path="/decks/" component={DecksLayout} />
        {/*<Route exact path="/inventory" component={InventoryLayout} />*/}
        <Route path="/settings" component={Settings} />
        {/*<Route exact path="/add-deck">*/}
        {/*  <NewDeckContainer />*/}
        {/*  <HomeContainer />*/}
        {/*</Route>*/}
        {/*<Route exact path="/import-deck">*/}
        {/*  <ImportDeckContainer />*/}
        {/*  <HomeContainer />*/}
        {/*</Route>*/}
        <Route exact path="/" component={Home} />
        <Redirect to="/" />
      </Switch>
    </Router>
  );
}

// function mapStateToProps(state: AppState): PropsFromState {
//   const result: PropsFromState = {
//   }
//   return result;
// }
// export default connect(mapStateToProps)(AppContainer);
//

export default App;

// export default () => (
//     <Layout>
//         <Route exact path='/' component={Home} />
//         <Route path='/counter' component={Counter} />
//         <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
//     </Layout>
// );
