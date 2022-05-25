import React, { ReactNode } from 'react';
import { Container, LinearProgress } from '@material-ui/core';
import {Navigation} from "./Navigation";
import styles from '../../App.module.css'

interface LayoutProps {
  children: NonNullable<ReactNode>;
    title: string;
    isLoading?: boolean;
}

export const AppLayout = (props: LayoutProps): JSX.Element => {
  return(
    <div className={[styles.stretch, styles.flexCol].join(' ')}>
      <Navigation />
      {props.isLoading && <LinearProgress id='progress-bar' />}
      <Container maxWidth="xl" className={[styles.flexSection, styles.flexCol].join(' ')} style={{overflow:'auto'}}>
        {props.children}
      </Container>
    </div>
  );
}
