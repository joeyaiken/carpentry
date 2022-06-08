import React, {useState} from 'react';
import {Box, CardContent, Typography, CardMedia, CardActions, Button, Card, IconButton, TableRow, Table, TableCell, TableBody} from '@material-ui/core';
import {InfoOutlined} from '@material-ui/icons';
import styles from "../../../../App.module.css";
import {useHistory} from "react-router";
import {useAppSelector} from "../../../../hooks";
import {CardImagePopper} from "../../../common/components/CardImagePopper";

interface InventoryCardGridItemProps {
  cardId: number;
  onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onInfoButtonLeave: () => void;
}
const InventoryCardGridItem = (props: InventoryCardGridItemProps): JSX.Element => {
  const cardItem = useAppSelector(state => state.inventory.overviews.data.byId[props.cardId]);
  const history = useHistory();
  const onCardSelected = (cardId: number): void => 
    history.push(`/inventory/${cardId}`);

  return(
    <React.Fragment>
      <Card key={cardItem.id} className={[styles.outlineSection, "card-result"].join(' ')}>
        <CardMedia
          className="card-result-image"
          style={{height:"310px", width: "223px"}}
          image={cardItem.imageUrl}
          title={cardItem.name} />
        <Box className={styles.flexCol}>
          <CardContent className={styles.flexSection}>
            <Box className={styles.flexCol}>
              { cardItem.totalCount === 0 &&
              <Typography>
                no cards
              </Typography>
              }
              { Boolean(cardItem.totalCount) &&
              <Table size="small">
                <TableBody>
                  { Boolean(cardItem.inventoryCount) &&
                  <TableRow>
                    <TableCell size="small">Inventory</TableCell>
                    <TableCell>{cardItem.inventoryCount}</TableCell>
                  </TableRow>
                  }
                  { Boolean(cardItem.deckCount) &&
                  <TableRow>
                    <TableCell>Decks</TableCell>
                    <TableCell>{cardItem.deckCount}</TableCell>
                  </TableRow>
                  }
                  { Boolean(cardItem.sellCount) &&
                  <TableRow>
                    <TableCell>Sell</TableCell>
                    <TableCell>{cardItem.sellCount}</TableCell>
                  </TableRow>
                  }
                  { Boolean(cardItem.totalCount) &&
                  <TableRow>
                    <TableCell>Total</TableCell>
                    <TableCell>{cardItem.totalCount}</TableCell>
                  </TableRow>
                  }
                  <TableRow>
                    <TableCell>${cardItem.price}</TableCell>
                    <TableCell>{ cardItem.isFoil && <Typography>*F</Typography> }</TableCell>
                  </TableRow>
                </TableBody>
              </Table>
              }
            </Box>
          </CardContent>
          <CardActions className={styles.flexSection}>
            <Button color="primary" size="small" onClick={() => {onCardSelected(cardItem.cardId)}} >
              Details
            </Button>
            <IconButton
              value={cardItem.id}
              color="primary"
              size="small"
              onMouseEnter={props.onInfoButtonEnter}
              onMouseLeave={props.onInfoButtonLeave} >
              <InfoOutlined />
            </IconButton>
          </CardActions>
        </Box>
      </Card>
    </React.Fragment>
  )
}

export default function InventoryCardGrid(): JSX.Element {
  
  const [cardImageMenuAnchor, setCardImageMenuAnchor] = useState<HTMLButtonElement | null>(null);
  
  const cardOverviewIds = useAppSelector(state => state.inventory.overviews.data.allIds)

  const selectedOverviewImage: string = useAppSelector(state => {
    const cardImageAnchorId = +(cardImageMenuAnchor?.value ?? "0");
    const card = state.inventory.overviews.data.byId[cardImageAnchorId];
    return card?.imageUrl ?? "";
  });

  const onInfoButtonEnter = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void =>
    setCardImageMenuAnchor(event.currentTarget);

  const onInfoButtonLeave = (): void =>
    setCardImageMenuAnchor(null);

  return (
    <React.Fragment>
      <CardImagePopper
        menuAnchor={cardImageMenuAnchor}
        onClose={onInfoButtonLeave}
        image={selectedOverviewImage} />
      <Box className={styles.flexRowWrap}>
        {cardOverviewIds.map(overviewId => 
          <InventoryCardGridItem 
            key={overviewId} 
            cardId={overviewId}
            onInfoButtonEnter={onInfoButtonEnter}
            onInfoButtonLeave={onInfoButtonLeave}
          />)}
      </Box>
    </React.Fragment>
  );
}