import React from 'react';
import { Route, Switch } from 'react-router-dom';
import DeckListContainer from './deck-list/DeckListContainer';
import DeckEditorContainer from './deck-editor/DeckEditorContainer';
import CardSearchContainer from '../common/card-search/CardSearchContainer';
import AppLayout from '../common/components/AppLayout';

// interface LayoutProps {
// }

export default function DecksLayout(
    // props: LayoutProps
    ): JSX.Element {
    return (
        <AppLayout title="Decks">
            <Switch>
                <Route path="/decks/:deckId/addCards" render={(props) => <CardSearchContainer {...props} searchContext="deck" />} />
                <Route path="/decks/:deckId" component={DeckEditorContainer} />
                <Route path="/decks/" component={DeckListContainer} />
            </Switch>
        </AppLayout>
    );
}
