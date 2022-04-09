import React from 'react';
import { Route, Switch } from 'react-router-dom';
import DeckListContainer from './deck-list/DeckListContainer';
import DeckEditorContainer from './deck-editor/DeckEditorContainer';
import AppLayout from '../common/components/AppLayout';
import DeckAddCardsContainer from './deck-add-cards/DeckAddCardsContainer';

export default function DecksLayout(): JSX.Element {
  return (
    <Switch>
      <Route path="/decks/:deckId/addCards" component={DeckAddCardsContainer} />
      <Route path="/decks/:deckId" component={DeckEditorContainer} />
      <Route path="/decks/" render={(props) => <AppLayout title={"Deck List"}><DeckListContainer {...props} /></AppLayout>} />
    </Switch>
  );
}