import React, {ReactNode} from 'react';
import {AppBar, Button, Container, LinearProgress, Toolbar, Typography} from '@material-ui/core';
import styles from '../../../App.module.css'
import {Link} from "react-router-dom";
import {useHistory} from "react-router";

interface LayoutProps {
  children: NonNullable<ReactNode>;
    title?: string;
    isLoading?: boolean;
}

export const AppLayout = (props: LayoutProps): JSX.Element => {

  const navigationTitle = "Carpentry" +  (!props.title ? "" : " - "+props.title);

  const history =  useHistory();
  const Navigate = (route: string): void => {
    history.push(route);
  }
  
  return(
    <div className={[styles.stretch, styles.flexCol].join(' ')}>
      <AppBar id="app-nav-menu" position="static">
        <Toolbar>
          <Typography variant="h5" className={styles.flexSection}>
            {navigationTitle}
          </Typography>
          {/* TODO - figure out how to get the link to stop being blue on-hover (like with css) */}
          <Button onClick={() => Navigate('/')} className="nav-button" color="inherit">Home</Button>

          <Button component={Link} to={"/"} className="nav-button" color="inherit">Home</Button>
        </Toolbar>
      </AppBar>
      
      {props.isLoading && <LinearProgress id='progress-bar' />}
      <Container maxWidth="xl" className={[styles.flexSection, styles.flexCol].join(' ')} style={{overflow:'auto'}}>
        {props.children}
      </Container>
    </div>
  );
}
