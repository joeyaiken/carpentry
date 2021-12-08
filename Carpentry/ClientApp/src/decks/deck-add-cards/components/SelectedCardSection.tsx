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
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface ComponentProps {
  selectedCard: CardSearchResultDto;
  inventoryCardsById: { [id: number]: InventoryCard };
  inventoryCardsAllIds: number[];
  
  handleAddInventoryCard: (inventoryCard: InventoryCard) => void;
  handleAddNewCard: (cardName: string, cardId: number, isFoil: boolean) => void;
  handleAddEmptyCard: (cardName: string) => void;
  // handleMoveCard?: (inventoryCard: InventoryCard) => void;
}

export default function SelectedCardSection(props: ComponentProps): JSX.Element {
  const { outlineSection, flexCol, flexRow, } = appStyles();
  return(
    <Box className={flexCol}>
      <Card className={combineStyles(outlineSection, flexCol)}>
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
                props.inventoryCardsAllIds.map(id => {
                  const inventoryCard = props.inventoryCardsById[id];
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
                                  onClick={()=> props.handleAddInventoryCard(inventoryCard)}
                                  className="add-inventory-card-button"
                          >Add</Button>
                        }
                        {/*{*/}
                        {/*  inventoryCard.deckId &&*/}
                        {/*  <Button size="small" variant="contained"*/}
                        {/*    // onClick={() => props.handleAddCardClick && props.handleAddCardClick(item)}*/}
                        {/*  >Move</Button>*/}
                        {/*}*/}
                      </TableCell>
                    </TableRow>
                  )
                })
              }
            </TableBody>
          </Table>
        }
      </Card>
      <Card className={combineStyles(outlineSection, flexCol)}>
        <CardHeader
          titleTypographyProps={{variant:"body1"}}
          title="Add New"
          action={
            <Button variant="outlined" onClick={() => props.handleAddEmptyCard(props.selectedCard.name)}>
              Add Empty
            </Button>
          }/>
        {
          props.selectedCard.details.map(detail => {
            return (
              <Card key={detail.cardId} className={combineStyles(outlineSection, flexRow)}>
                <CardHeader titleTypographyProps={{variant:"body1"}} title={`${detail.setCode}-${detail.collectionNumber}`} />
                <CardMedia
                  style={{height:"310px", width: "223px"}}
                  image={detail.imageUrl || undefined} />
                <CardContent>
                  <Box className={flexCol}>
                    <Box className={flexCol}>
                      <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
                    </Box>
                    <Box className={flexCol}>
                      <Button variant="outlined" onClick={() => props.handleAddNewCard(detail.name, detail.cardId, false)}>
                        Add Normal
                      </Button>
                      <Button variant="outlined" onClick={() => props.handleAddNewCard(detail.name, detail.cardId, true)}>
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