import React from 'react';
import {
    // Typography,
    // Box,
    // Card,
    // CardHeader,
    // CardContent,
    // IconButton,
    // Button,
    // makeStyles,
} from '@material-ui/core';
// import DeckList from '../containers/DeckList/DeckListContainer';
import { Route, Switch } from 'react-router-dom';
import DeckListContainer from './deck-list/DeckListContainer';
// import { ArrowForward } from '@material-ui/icons';
// import { appStyles } from '../styles/appStyles';
// import DeckEditorContainer from './deck-editor/DeckEditorContainer';
// import CardSearchContainer from '../_containers/CardSearch/CardSearchContainer';
// import CardSearchContainer from '../containers/CardSearch/CardSearchContainer';
// import DeckEditorContainer from '../containers/DeckEditor/DeckEditorContainer';

interface LayoutProps {
}

export default function DecksLayout(props: LayoutProps): JSX.Element {
    return (
        <React.Fragment>
            <Switch>
                {/* <Route path="/decks/:deckId/addCards" render={(props) => <CardSearchContainer {...props} searchContext="deck" />} />
                <Route path="/decks/:deckId" component={DeckEditorContainer} /> */}
                <Route path="/decks/" component={DeckListContainer} />
                
            </Switch>
        </React.Fragment>
    );
}
