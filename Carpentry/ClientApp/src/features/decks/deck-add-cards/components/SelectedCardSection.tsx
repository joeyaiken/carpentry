import React from 'react';
import {
  Box,
  Card,
  CardHeader,
  CardMedia,
  CardContent,
  Typography,
  Button,
  TableHead,
  TableRow, TableCell, TableBody, Table
} from '@material-ui/core';
import styles from "../../../../app/App.module.css";
// import {requestAddDeckCard} from "../state/DeckAddCardsActions";
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";

interface ComponentProps {
  // selectedCard: CardSearchResultDto;
  // inventoryCardsById: { [id: number]: InventoryCard };
  // inventoryCardsAllIds: number[];
  
  deckId: number;
  // handleAddInventoryCard: (inventoryCard: InventoryCard) => void;
  // handleAddNewCard: (cardName: string, cardId: number, isFoil: boolean) => void;
  // handleAddEmptyCard: (cardName: string) => void;
}

export default function SelectedCardSection(props: ComponentProps): JSX.Element {
  
  const selectedCard = useAppSelector(state => state.decks.deckAddCards.selectedCard);
  const inventoryCardsById = useAppSelector(state => state.decks.deckAddCards.inventoryDetail.inventoryCardsById);
  const inventoryCardsAllIds = useAppSelector(state => state.decks.deckAddCards.inventoryDetail.inventoryCardsAllIds);
  
  const isSaving = useAppSelector(state => !state.decks.deckAddCards.isSaving);
  
  const dispatch = useAppDispatch();
  
  const handleAddInventoryCard = (inventoryCard: InventoryCard): void => {
    if(isSaving) return;
    const newDeckCard: DeckCardDto = {
      id: 0,
      deckId: props.deckId,
      cardName: inventoryCard.name,
      categoryId: null, //TODO - Should cards added this way default to the sideboard if the deck is full?
      inventoryCardId: inventoryCard.id,
      //TODO - Find a way to exclude these fields from what's sent to the api, as they aren't used
      cardId: inventoryCard.cardId,
      isFoil: inventoryCard.isFoil,
      inventoryCardStatusId: inventoryCard.statusId,
    }
    dispatch(requestAddDeckCard(newDeckCard));
  };
  
  const handleAddNewCard = (cardName: string, cardId: number, isFoil: boolean): void => {
    if(isSaving) return;
    let deckCard: DeckCardDto = {
      categoryId: null,
      deckId: props.deckId,
      cardName: cardName,
      id: 0,
      inventoryCardId: 0,
      cardId: cardId,
      isFoil: isFoil,
      inventoryCardStatusId: 1,
    }
    dispatch(requestAddDeckCard(deckCard));
  };
  
  const handleAddEmptyCard = (cardName: string): void => {
    if(isSaving) return;
    let deckCard: DeckCardDto = {
      categoryId: null,
      deckId: props.deckId,
      cardName: cardName,
      id: 0,
      inventoryCardId: 0,
      cardId: 0,
      isFoil: false,
      inventoryCardStatusId: 0,
    }
    dispatch(requestAddDeckCard(deckCard));
  };
  
  return(
    <Box className={styles.flexCol}>
      <Card className={[styles.outlineSection, styles.flexCol].join(' ')}>
        <CardHeader titleTypographyProps={{variant:"body1"}} title="Inventory"/>
        {
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>Set</TableCell>
                <TableCell>Style</TableCell>
                <TableCell colSpan={2}>Status</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {
                inventoryCardsAllIds.map(id => {
                  const inventoryCard = inventoryCardsById[id];
                  return(
                    <TableRow key={inventoryCard.id}>
                      {/* TODO - Include the card image in an on-hover style (like I kinda implemented in inventory overview)*/}
                      <TableCell>{inventoryCard.set}</TableCell>
                      <TableCell>{inventoryCard.collectorNumber}{inventoryCard.isFoil &&" foil"}</TableCell>
                      <TableCell>
                        { inventoryCard.deckId && inventoryCard.deckName }
                        { !inventoryCard.deckId && "Inventory" }
                      </TableCell>
                      <TableCell>
                        { !inventoryCard.deckId && inventoryCard.statusId === 1 &&
                          <Button size="small" 
                                  variant="contained"
                                  onClick={()=> handleAddInventoryCard(inventoryCard)}
                                  className="add-inventory-card-button"
                          >Add</Button>
                        }
                      </TableCell>
                    </TableRow>
                  )
                })
              }
            </TableBody>
          </Table>
        }
      </Card>
      <Card className={[styles.outlineSection, styles.flexCol].join(' ')}>
        <CardHeader
          titleTypographyProps={{variant:"body1"}}
          title="Add New"
          action={
            //TODO - find a better way to handle this specific function call (don't really want to pass in an empty string)
            <Button variant="outlined" onClick={() => handleAddEmptyCard(selectedCard?.name ?? "")}>
              Add Empty
            </Button>
          }/>
        
        { selectedCard &&
          selectedCard.details.map(detail => {
            return (
              <Card key={detail.cardId} className={[styles.outlineSection, styles.flexRow].join(' ')}>
                <CardHeader titleTypographyProps={{variant:"body1"}} title={`${detail.setCode}-${detail.collectionNumber}`} />
                <CardMedia
                  style={{height:"310px", width: "223px"}}
                  image={detail.imageUrl || undefined} />
                <CardContent>
                  <Box className={styles.flexCol}>
                    <Box className={styles.flexCol}>
                      <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
                    </Box>
                    <Box className={styles.flexCol}>
                      <Button variant="outlined" onClick={() => handleAddNewCard(detail.name, detail.cardId, false)}>
                        Add Normal
                      </Button>
                      <Button variant="outlined" onClick={() => handleAddNewCard(detail.name, detail.cardId, true)}>
                        Add Foil
                      </Button>
                    </Box>
                  </Box>
                </CardContent>
              </Card>
            )}
          )
        }
      </Card>
    </Box>
  );
}