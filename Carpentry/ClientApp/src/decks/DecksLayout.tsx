import React from 'react';
import { Route, Switch } from 'react-router-dom';
import DeckEditorContainer from './deck-editor/DeckEditorContainer';
import {AppLayout} from '../features/common/components/AppLayout';
import DeckAddCardsContainer from './deck-add-cards/DeckAddCardsContainer';
import {DeckList} from "../features/decks/deck-list/DeckList";

export default function DecksLayout(): JSX.Element {
  return (
    <Switch>
      <Route path="/decks/:deckId/addCards" component={DeckAddCardsContainer} />
      <Route path="/decks/:deckId" component={DeckEditorContainer} />
      <Route path="/decks" render={() => <AppLayout title={"Deck List"} ><DeckList /></AppLayout>} />
    </Switch>
  );
}