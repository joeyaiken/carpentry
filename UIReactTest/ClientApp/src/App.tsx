import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect, DispatchProp } from 'react-redux';
import './custom.css'
import AppLayout from './components/AppLayout';
import { AppState } from './store/configureStore';
import { actionCreators } from './state/AppReducer'

interface PropsFromState {

}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class AppContainer extends React.Component<ContainerProps> {
    render(){
        // return(<AppLayout />)
        return(<Layout>
            <Home />
            {/* <Route path='/' component={Home} /> */}
        </Layout>)
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        
    }
    return result;
}
export default connect(mapStateToProps)(AppContainer); //, actionCreators

// export default () => (
//     <Layout>
//         <Route exact path='/' component={Home} />
//         <Route path='/counter' component={Counter} />
//         <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
//     </Layout>
// );
