import React from 'react';
import { Box, Card, CardMedia, CardContent, Typography, Button } from '@material-ui/core';
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface SelectedCardDetailSectionProps {
  selectedCard: CardSearchResultDto;
  pendingCards?: PendingCardsDto;
  selectedCardDetail: InventoryDetailDto | null;
  handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
  handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
}

export default function SelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
  const { outlineSection, flexRow, flexCol } = appStyles();
  return(
    <Box className={flexCol}>
      {
        props.selectedCard.details.map((detail) => {
          let countNormal = 0;
          let countFoil = 0;
          if(props.pendingCards)
          {
            countNormal = props.pendingCards.cards.filter(c => c.cardId === detail.cardId && !c.isFoil).length;
            countFoil = props.pendingCards.cards.filter(c => c.cardId === detail.cardId && c.isFoil).length;
          }
          return (
            <Card key={detail.cardId} className={combineStyles(outlineSection, flexRow, 'search-result-card')}>
              <CardMedia
                style={{height:"310px", width: "223px"}}
                image={detail.imageUrl || undefined} />
              <CardContent>
                <Box className={flexCol}>
                  <Box className={flexCol}>
                    <Box className={flexCol}>
                      <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
                    </Box>
                    <Box className={flexCol}>
                      <Typography>Normal ({countNormal})</Typography>
                      <Box className={flexRow}>
                        <Button variant="outlined" className="remove-button-normal" onClick={() => {props.handleRemovePendingCard(detail.name, detail.cardId, false)} } >-</Button>
                        <Button variant="outlined" className="add-button-normal" onClick={() => {props.handleAddPendingCard(detail.name, detail.cardId, false)} } >+</Button>
                      </Box>
                    </Box>
                    <Box className={flexCol}>
                      <Typography>Foil ({countFoil})</Typography>
                      <Box className={flexRow}>
                        <Button variant="outlined" className="remove-button-foil" onClick={() => {props.handleRemovePendingCard(detail.name, detail.cardId, true)} } >-</Button>
                        <Button variant="outlined" className="add-button-foil" onClick={() => {props.handleAddPendingCard(detail.name, detail.cardId, true)} } >+</Button>
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