import React from 'react';
import {Route,Switch} from 'react-router-dom';
import {DeckList} from "./deck-list/DeckList";
import {AppLayout} from "../../common/components/AppLayout";
import {DeckEditor} from "./deck-editor/DeckEditor";
import {DeckAddCards} from "./deck-add-cards/DeckAddCards";

export default function DecksLayout(): JSX.Element {
  return (
    <Switch>
      <Route path="/decks/:deckId/addCards" component={DeckAddCards} />
      <Route path="/decks/:deckId" component={DeckEditor} />
      <Route path="/decks/" render={(props) => <AppLayout title={"Deck List"}><DeckList /></AppLayout>} />
    </Switch>
  );
}