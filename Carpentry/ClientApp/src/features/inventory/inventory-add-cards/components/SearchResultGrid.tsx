import React from 'react';
import {Box,Card,CardMedia} from '@material-ui/core';
import styles from '../../../../app/App.module.css';
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
import {cardSearchSelectCard, selectSearchResultItem} from "../inventoryAddCardsSlice";

const SearchResultGridCell = (props:{cardId: number}): JSX.Element => {
  const dispatch = useAppDispatch();
  const onCardSelected = (item: CardListItem): void =>
    dispatch(cardSearchSelectCard(item.data));

  const card = useAppSelector(state => selectSearchResultItem(state, props.cardId))

  return (
    <Card
      key={card.data.name}
      className={styles.outlineSection}
      onClick={() => onCardSelected(card)}>
      <CardMedia
        style={{height:"310px", width: "223px"}}
        image={card.data.details[0].imageUrl}
        title={card.data.name} />
    </Card>
  )
}

export const SearchResultGrid = (): JSX.Element => {
  const searchResultIds = useAppSelector(state =>
    state.inventory.inventoryAddCards.searchResults.allSearchResultIds);
  
  return (
    <Box className={styles.flexRowWrap}>
      { searchResultIds.map(cardId => <SearchResultGridCell key={cardId} cardId={cardId} />) }
    </Box>
  );
}