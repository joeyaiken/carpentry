//table of deck cards
import React, { ReactNode } from 'react';
import { CardHeader, CardMedia, Card } from '@material-ui/core';
import styles from '../../../App.module.css';

interface ComponentProps{
  cardOverview: DeckCardOverview;
  onCardSelected?: () => void;
  children?: ReactNode;
}

export const VisualCard = (props: ComponentProps): JSX.Element => {
  return (
    <React.Fragment>
      <Card
        key={props.cardOverview.name}
        className={styles.outlineSection}
        onClick={props.onCardSelected}
      >
        <CardHeader titleTypographyProps={{variant:"body1"}} title={ `${props.cardOverview.name} - (${props.cardOverview.count})` } />
        <CardMedia
          style={{height:"310px", width: "223px"}}
          image={props.cardOverview.img}
          title={props.cardOverview.name} />
        {props.children}
      </Card>
    </React.Fragment>
  );
}