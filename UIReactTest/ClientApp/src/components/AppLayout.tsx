import { AppBar, Button, LinearProgress, Toolbar, Typography, Container } from '@material-ui/core';
import React from 'react';
// import { Container } from 'reactstrap';
import { AppConfigResult } from '../state/AppReducer';
import NavMenu from './NavMenu';

export interface ComponentProps {
    appConfigStatus: string;
    isLoading: boolean;
    appConfig: AppConfigResult | null;
    onRefreshClick: () => void;
}

export default (props: ComponentProps): JSX.Element => (
  <React.Fragment>
    <AppBar position="static">
      <Toolbar>
        <Container>
          <Typography variant="h5">
            Test App - React
          </Typography>
        </Container>
      </Toolbar>
    </AppBar>
    { props.isLoading && <LinearProgress/> }
    {/* <NavMenu/> */}
    <Container>
      <div>
        <h1>Hello, world!</h1>
        <p>This is a simple example app written with React and Redux!</p>
      </div>
      <div>
        <h4>App Config: <span id="app-config-status">{props.appConfigStatus}</span></h4>
        { !props.isLoading && props.appConfig &&
          <h4>Configured Value: <span id="config-string">{props.appConfig.configString}</span></h4>
        }
        {
          !props.isLoading && props.appConfig &&
          <h4>Last Updated: <span id="config-last-updated">{props.appConfig.lastUpdated}</span></h4>
        }
        <Button variant="contained" color="primary" id="refresh-button" onClick={() => { props.onRefreshClick() }} disabled={props.isLoading}>
          Refresh
        </Button>
      </div>
    </Container>
  </React.Fragment>
);