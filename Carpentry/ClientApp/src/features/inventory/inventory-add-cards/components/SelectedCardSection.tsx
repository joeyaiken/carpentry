import React from 'react';
import { Box, Card, CardMedia, CardContent, Typography, Button } from '@material-ui/core';
import {combineStyles} from '../../../../styles/appStyles';
import styles from '../../../../App.module.css';
import {useAppDispatch, useAppSelector} from "../../../../hooks";
import {addPendingCard, removePendingCard} from "../inventoryAddCardsSlice";

export default function SelectedCardSection(): JSX.Element {
  const selectedCard = useAppSelector(state => state.inventory.inventoryAddCards.selectedCard);
  const pendingCards = useAppSelector(state => selectedCard && state.inventory.inventoryAddCards.pendingCards.byName[selectedCard!.name]);

  const dispatch = useAppDispatch();
  
  const handleAddPendingCard = (name: string, cardId: number, isFoil: boolean): void => 
    dispatch(addPendingCard({name: name, cardId: cardId, isFoil: isFoil}));
  
  const handleRemovePendingCard = (name: string, cardId: number, isFoil: boolean): void => 
    dispatch(removePendingCard({name: name, cardId: cardId, isFoil: isFoil}));
  
  if(!selectedCard) return (<React.Fragment />);
  
  return(
    <Box className={styles.flexCol}>
      {
        selectedCard!.details.map((detail) => {
          let countNormal = 0;
          let countFoil = 0;
          if(pendingCards)
          {
            countNormal = pendingCards.cards.filter(c => c.cardId === detail.cardId && !c.isFoil).length;
            countFoil = pendingCards.cards.filter(c => c.cardId === detail.cardId && c.isFoil).length;
          }
          return (
            <Card key={detail.cardId} className={combineStyles(styles.outlineSection, styles.flexRow, 'search-result-card')}>
              <CardMedia
                style={{height:"310px", width: "223px"}}
                image={detail.imageUrl || undefined} />
              <CardContent>
                <Box className={styles.flexCol}>
                  <Box className={styles.flexCol}>
                    <Box className={styles.flexCol}>
                      <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
                    </Box>
                    <Box className={styles.flexCol}>
                      <Typography>Normal ({countNormal})</Typography>
                      <Box className={styles.flexRow}>
                        <Button variant="outlined" className="remove-button-normal" onClick={() => {handleRemovePendingCard(detail.name, detail.cardId, false)} } >-</Button>
                        <Button variant="outlined" className="add-button-normal" onClick={() => {handleAddPendingCard(detail.name, detail.cardId, false)} } >+</Button>
                      </Box>
                    </Box>
                    <Box className={styles.flexCol}>
                      <Typography>Foil ({countFoil})</Typography>
                      <Box className={styles.flexRow}>
                        <Button variant="outlined" className="remove-button-foil" onClick={() => {handleRemovePendingCard(detail.name, detail.cardId, true)} } >-</Button>
                        <Button variant="outlined" className="add-button-foil" onClick={() => {handleAddPendingCard(detail.name, detail.cardId, true)} } >+</Button>
                      </Box>
                    </Box>
                  </Box>
                </Box>
              </CardContent>
            </Card>
          )})
      }
    </Box>
  );
}