import {Table, TableHead, TableRow, TableCell, TableBody, Box, CardActions, Button, Card, CardHeader, CardMedia, CardContent, Typography, IconButton} from '@material-ui/core';
import React, {useState} from 'react';
import {MoreVert} from '@material-ui/icons';
import {useHistory} from "react-router";
import {useAppSelector} from "../../../hooks";
import styles from '../../../App.module.css';
import {getSelectedDeckId} from "../deckDetailSlice";
import {getSelectedDeckDetails} from "../../../features/decks/deck-editor/deckEditorSlice";
import {CardMenu} from "./CardMenu";

export const DeckCardDetail = (): JSX.Element => {
  
  const deckId = useAppSelector(getSelectedDeckId);

  //TODO - make slice selector
  const selectedCard: DeckCardOverview | null = useAppSelector(state => {
    const selectedOverviewCardId = state.decks.deckEditor.selectedOverviewCardId;
    if(selectedOverviewCardId){
      return state.decks.deckDetailData.cardOverviews.byId[selectedOverviewCardId];
    }
    return null;
  });

  const inventoryCards = useAppSelector(getSelectedDeckDetails);

  const [menuAnchor, setMenuAnchor] = useState<HTMLButtonElement | null>(null);
  
  const history = useHistory();

  const onCardDetailClick = (cardId: number): void => {
    history.push(`/decks/${deckId}?cardId=${cardId}&show=detail`);
  };

  const onCardTagsClick = (cardId: number): void => {
    history.push(`/decks/${deckId}?cardId=${cardId}&show=tags`);
  };

  const onMenuClick = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
    setMenuAnchor(event.currentTarget);
  }

  const onMenuClose = (): void => {
    setMenuAnchor(null)
  }
  
  if(selectedCard === null){
    return (
      <Box className={styles.staticSection}>
        <Card>
          <CardHeader titleTypographyProps={{variant:"body1"}} title={"no card selected"}/>
          <CardContent>
            <Typography>select a card</Typography>
          </CardContent>
        </Card>
      </Box>
    );
  }
  else {
    return (
      <React.Fragment>
        <CardMenu
          cardMenuAnchor={menuAnchor}
          cardCategoryId={selectedCard?.category || ''}
          onCardMenuClose={onMenuClose} />
        <Box className={styles.staticSection}>

          <Card>
            {
              selectedCard &&
              <CardMedia style={{height:"310px", width: "223px"}} image={selectedCard.img} />
            }
            <CardContent>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Card Detail</TableCell>
                    <TableCell />
                  </TableRow>
                </TableHead>
                <TableBody>
                  {
                    inventoryCards.map(item => {

                      const rowDeckCardId = item.id;
                      return(
                        <TableRow key={item.id}>
                          {
                            item.inventoryCardId &&
                            <TableCell style={{textTransform:"uppercase"}}>
                              {`${item.set} - ${item.collectorNumber}`}{item.isFoil &&" (foil)"}
                            </TableCell>
                          }
                          {
                            !item.inventoryCardId &&
                            <TableCell>
                                Empty
                            </TableCell>
                          }
                          <TableCell size="small">
                            <IconButton size="small" onClick={onMenuClick} name={item.name} value={rowDeckCardId}>
                              <MoreVert />
                            </IconButton>
                          </TableCell>
                        </TableRow>
                      )
                    })
                  }
                </TableBody>
              </Table>
            </CardContent>
            <CardActions>
              <Button
                onClick={() => selectedCard && onCardDetailClick(selectedCard.cardId)}
                color="primary"
                variant="contained">Details</Button>
              <Button
                onClick={() => selectedCard && onCardTagsClick(selectedCard.cardId)}
                color="primary"
                variant="contained">Tags</Button>
            </CardActions>
          </Card>
        </Box>
      </React.Fragment>
    );
  }
}