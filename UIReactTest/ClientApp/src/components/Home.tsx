import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { actionCreators, AppReducerState } from '../state/AppReducer';
import { ApplicationState } from '../store';

type HomeProps = AppReducerState & typeof actionCreators;

class Home extends React.PureComponent<HomeProps> {
  public render() {
    return (
      <React.Fragment>
        <div>
          <h1>Hello, world!</h1>
          <p>This is a simple example app written with React and Redux!</p>
        </div>
        <div>
          <h4>App Config: <span id="app-config-status">{this.props.appConfigStatus}</span></h4>
          { !this.props.isLoading && this.props.appConfig &&
            <h4>Configured Value: <span id="config-string">{this.props.appConfig.configString}</span></h4>
          }
          {
            !this.props.isLoading && this.props.appConfig &&
            <h4>Last Updated: <span id="config-last-updated">{this.props.appConfig.lastUpdated}</span></h4>
          }
          <button id="refresh-button" onClick={() => { this.getAppConfig() }} disabled={this.props.isLoading}>
            Refresh
          </button>
        </div>
      </React.Fragment>
    );
  }

  private getAppConfig() {
    this.props.getAppConfig();
  }
}

export default connect(
  (state: ApplicationState) => state.appConfig,
  actionCreators
)(Home as any);
