import {
  Box,
  Button,
  Card,
  CardHeader,
  CardMedia,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle, Table, TableBody, TableCell, TableHead, TableRow, Typography
} from "@material-ui/core";
import React, {useEffect} from "react";
import styles from "../../../App.module.css";
import {useHistory} from "react-router";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {combineStyles} from "../../../styles/appStyles";
import {ApiStatus} from "../../../enums";
import {loadInventoryDetail} from "./InventoryDetailSlice";

interface OwnProps {
  match: {
    params: {
      cardId: string;
    }
  }
}

const InventoryDetailCard = (props: { cardId: number }): JSX.Element => {
  const card = useAppSelector(state => state.inventory.detail.data.cardsById[props.cardId]);
  const inventoryCardIds = useAppSelector(state => state.inventory.detail.data.cardGroups[props.cardId]);
  const img = card.imageUrl;
  const cardTitle = `${card.set} (${card.collectionNumber}) - $${card.price} | $${card.priceFoil}`;

  // TODO - Inventory Card Rows should probably be their own component
  const inventoryCardsById = useAppSelector(state => state.inventory.detail.data.inventoryCardsById);

  return (
    <Card key={card.cardId} className={combineStyles(styles.outlineSection, styles.flexCol)}>
      <CardHeader titleTypographyProps={{variant:"body1"}} style={{textTransform:"uppercase"}} title={cardTitle} />
      <Box className={combineStyles(styles.flexRow, styles.flexSection)}>
        <Box className={styles.staticSection}>
          <CardMedia style={{height:"310px", width: "223px"}} image={img} />
        </Box>

        <Box className={combineStyles(styles.flexSection, styles.flexCol)}>
          <Box className={styles.scrollSection} style={{overflow:"auto"}}>
            <Table size="small" >
              <TableHead>
                <TableRow>
                  <TableCell>Style</TableCell>
                  <TableCell>Status</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {
                  inventoryCardIds.map(inventoryCardId => {
                    const item = inventoryCardsById[inventoryCardId];
                    return(
                      <TableRow key={item.id}>
                        <TableCell>
                          <Typography>{(item.isFoil && " foil") || "normal"}</Typography>
                        </TableCell>
                        <TableCell>
                          <Typography>
                            { item.statusId === 1 && "Inventory/Deck" }
                            { item.statusId === 2 && "Wish List" }
                            { item.statusId === 3 && "Sell List" }
                          </Typography>
                        </TableCell>
                      </TableRow>
                    )
                  })
                }
              </TableBody>
            </Table>
          </Box>
        </Box>
      </Box>
    </Card>
  )
}

export const InventoryDetail = (props: OwnProps): JSX.Element => {

  const selectedCardId = +props.match.params.cardId;
  
  const modalIsOpen = Boolean(selectedCardId != null);

  const allCardIds = useAppSelector(state => state.inventory.detail.data.allCardIds);
  
  // TODO - This should really be a selector in the slice, or just be smarter...
  const shouldLoad = useAppSelector(state => {

    const apiStatus = state.inventory.detail.data.status
    
    if(apiStatus === ApiStatus.uninitialized) return true;
    
    if(apiStatus === ApiStatus.initialized){
      const idsMatch = state.inventory.detail.selectedCardId === selectedCardId;
      if(!idsMatch) return true;
    }
    return false;
  });
  
  const history = useHistory();
  
  const handleCloseModalClick = (): void => {
    history.push('/inventory');
  }
  
  const dispatch = useAppDispatch();
  
  useEffect(() => {
    if(shouldLoad) dispatch(loadInventoryDetail(selectedCardId));
  });
  
  const cardName = useAppSelector(state => state.inventory.detail.data.selectedCardName);
  
  return (
    <React.Fragment>
      <Dialog open={modalIsOpen} onClose={() => {handleCloseModalClick()}} >
        <DialogTitle>{`Inventory Detail - ${cardName}`}</DialogTitle>
        <DialogContent>
          <Box className={combineStyles(styles.flexCol, styles.flexSection)}>
            {
              allCardIds.map(cardId => <InventoryDetailCard key={cardId} cardId={cardId} />)
            }
          </Box>
        </DialogContent>
        <DialogActions>
          <Button size="medium" onClick={() => handleCloseModalClick()}>Close</Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  )
}

