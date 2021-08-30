import * as React from 'react';
import { connect } from 'react-redux';
import './custom.css'
import AppLayout from './components/AppLayout';
import { actionCreators, AppReducerState } from './state/AppReducer'
import { ApplicationState } from './store/configureStore';

type ContainerProps = AppReducerState & typeof actionCreators;

class AppContainer extends React.PureComponent<ContainerProps> {
    render(){
        return(<AppLayout 
            appConfig={this.props.appConfig}
            appConfigStatus={this.props.appConfigStatus}
            isLoading={this.props.isLoading}
            onRefreshClick={() => { this.loadAppConfig() }} />);
    }

    private loadAppConfig() {
        this.props.getAppConfig();
    }
}

export default connect(
    (state: ApplicationState) => state.appConfig, 
    actionCreators
)(AppContainer as any);