import React, { ReactNode } from 'react';
import { Container, LinearProgress } from '@material-ui/core';
import { appStyles, combineStyles } from '../../styles/appStyles';
import NavigationContainer from "../../navigation/NavigationContainer";

interface LayoutProps {
    children: ReactNode;
    title: string;
    isLoading?: boolean;
}

export default function AppLayout(props: LayoutProps): JSX.Element {
  const { stretch, flexCol, flexSection } = appStyles();
  return(
  <div className={combineStyles(stretch, flexCol)}>
    <NavigationContainer />
    {props.isLoading && <LinearProgress id='progress-bar' />}
    <Container maxWidth="xl" className={combineStyles(flexSection, flexCol)} style={{overflow:'auto'}}>
      {props.children}
    </Container>
  </div>
  );
}
