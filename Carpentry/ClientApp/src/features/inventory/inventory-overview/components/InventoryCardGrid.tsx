import React, {useState} from 'react';
import {Box, CardContent, Typography, CardMedia, CardActions, Button, Card, IconButton, TableRow, Table, TableCell, TableBody} from '@material-ui/core';
import CardGridContainer from './CardGridContainer';
import {InfoOutlined} from '@material-ui/icons';
import styles from "../../../../app/App.module.css";
import {useAppSelector} from "../../../../app/hooks";
import {useHistory} from "react-router";
import {CardImagePopper} from "../../../../common/components/CardImagePopper";

const InventoryCardGridItem = (props: {cardId: number}): JSX.Element => {
  const cardItem = useAppSelector(state => state.inventory.overviews.data.byId[props.cardId]);
  const [cardMenuAnchor, setCardMenuAnchor] = useState<HTMLButtonElement | null>(null);
  const history = useHistory();
  
  const onCardSelected = (cardId: number): void => 
    history.push(`/inventory/${cardId}`);
  const onInfoButtonEnter = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => 
    setCardMenuAnchor(event.currentTarget);
  const onInfoButtonLeave = (): void =>
    setCardMenuAnchor(null);
  
  return(
    <React.Fragment>
      <CardImagePopper
        menuAnchor={cardMenuAnchor}
        onClose={onInfoButtonLeave}
        image={cardItem?.imageUrl} />
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
              onMouseEnter={onInfoButtonEnter}
              onMouseLeave={onInfoButtonLeave} >
              <InfoOutlined />
            </IconButton>
          </CardActions>
        </Box>
      </Card>
    </React.Fragment>
  )
}

export default function InventoryCardGrid(): JSX.Element {
  const cardOverviewIds = useAppSelector(state => state.inventory.overviews.data.allIds) 
  return (
    <React.Fragment>
      <CardGridContainer layout="grid">
        {cardOverviewIds.map(overviewId => <InventoryCardGridItem key={overviewId} cardId={overviewId} />)}
      </CardGridContainer>
    </React.Fragment>
  );
}