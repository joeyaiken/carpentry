import React from 'react';
import { Typography, Paper } from '@material-ui/core';
import styles from '../../../../app/App.module.css';
import {useAppSelector} from "../../../../app/hooks";

// const PendingCardsSectionItem = (props: {}): JSX.Element => {
//   return (<React.Fragment/>)
// }

export const PendingCardsSection = (): JSX.Element => {
  
  const pendingCards = useAppSelector(state => state.inventory.inventoryAddCards.pendingCards);
  
  return (<Paper className={[styles.outlineSection, styles.flexRow].join(' ')}>
    {
      Object.keys(pendingCards).map((id: string) => {
        let thisCard: PendingCardsDto = pendingCards[id];
        return(
          <Paper className="pending-card" key={id}>
            <Typography variant="h5">{ thisCard.name }</Typography>
            <Typography variant="h6">{ thisCard.cards.length }</Typography>
          </Paper>
        )
      })
    }
  </Paper>);
}