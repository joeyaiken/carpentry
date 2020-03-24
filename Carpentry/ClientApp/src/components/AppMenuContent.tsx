 //TODO Review and verify if actually used
 
import React, { ReactNode } from 'react';

import { AppBar, Typography, Toolbar, IconButton, Container, Box, CardHeader, CardActions, Button, Card, Paper
} from '@material-ui/core';

import {
  Add, AddBox, Menu
} from '@material-ui/icons';
// import CoreData from '../containers/CoreData';
import DeckList from '../containers/DeckList';

interface ComponentProps {
  onNavigationClick: (option: 'inventory' | 'buyList' | 'cardSearch' | 'newDeck') => void;
  onDeckClick: (deckId: number) => void;
  // decks: DeckProperties[];
}

export default function AppMenuContent(props: ComponentProps): JSX.Element {
  return(<Box>
    <Box>
      <Card>
        {/* <CardHeader 
          titleTypographyProps={{variant:"h6"}} 
          title={"Navigation"} 
        />          */}
        <CardActions>
          <Button 
            onClick={() => {props.onNavigationClick('inventory')}} 
            color="primary" 
            variant="contained"
            >Inventory</Button>
          <Button 
            onClick={() => {props.onNavigationClick('buyList')}} 
            color="primary" 
            variant="contained"
            >Buy List</Button>
          <Button 
            onClick={() => {props.onNavigationClick('cardSearch')}} 
            color="primary" 
            variant="contained"
            >Card Search</Button>
          <Button
            color="secondary"
            variant="contained"
            onClick={() => {props.onNavigationClick('newDeck')}}>New Deck</Button>
        </CardActions>
      </Card>
    </Box>
    <Box>
      <DeckList />
    </Box>
  </Box>);
}
