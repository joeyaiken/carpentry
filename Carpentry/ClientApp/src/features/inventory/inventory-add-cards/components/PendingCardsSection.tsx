import React from 'react';
import { Typography, Paper } from '@material-ui/core';
import styles from '../../../../App.module.css';
import {combineStyles} from "../../../../styles/appStyles";
import {useAppSelector} from "../../../../hooks"; 

export const PendingCardsSection = (): JSX.Element => {
  const pendingCards = useAppSelector(state => state.inventory.inventoryAddCards.pendingCards.byName);
  return (<Paper className={combineStyles(styles.outlineSection, styles.flexRow)}>
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