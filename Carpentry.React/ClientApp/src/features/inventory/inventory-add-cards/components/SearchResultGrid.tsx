import React from 'react';
import {
  Box,
  Card,
  CardMedia,
} from '@material-ui/core';
import { appStyles } from '../../../styles/appStyles';

interface SearchResultGridProps {
  searchResults: CardListItem[];
  onCardSelected: (item: CardListItem) => void;
}

export default function SearchResultGrid(props: SearchResultGridProps): JSX.Element {
  const { outlineSection, flexRowWrap } = appStyles();
  return (
    <Box className={flexRowWrap}>
      {props.searchResults.map((card) => (
        <Card
          key={card.data.name}
          className={outlineSection}
          onClick={() => props.onCardSelected(card)}>
          <CardMedia
            style={{height:"310px", width: "223px"}}
            image={card.data.details[0].imageUrl}
            title={card.data.name} />
        </Card>
      ))}
    </Box>
  );
}