import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../reducers';

import { 
    cardSearchSearchMethodChanged,
    cardSearchClearPendingCards,
} from '../actions/cardSearch.actions';

import CardSearchPendingCards from './CardSearchPendingCards'
import { 
    requestAddCardsFromSearch
} from '../actions/inventory.actions';

import {
    Button,
    AppBar,
    Toolbar,
    Typography,
    Paper,
    Box,
    Tabs,
    Tab,
} from '@material-ui/core';

import CardSearchFilterBar from './CardSearchFilterBar';
import CardSearchResultDetail from './CardSearchResultDetail';
import CardSearchResults from './CardSearchResults';

interface PropsFromState { 
    cardSearchMethod: "set" | "web" | "inventory";
}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearch extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
        this.handleSaveClick = this.handleSaveClick.bind(this);
        this.handleCancelClick = this.handleCancelClick.bind(this);
        this.handleSearchMethodTabClick = this.handleSearchMethodTabClick.bind(this);
    }

    handleSaveClick(){
        this.props.dispatch(requestAddCardsFromSearch());
    }

    handleCancelClick(){
        this.props.dispatch(cardSearchClearPendingCards());
    }

    handleSearchMethodTabClick(name: string): void {
        this.props.dispatch(cardSearchSearchMethodChanged(name));
    }

    render() {
        return (
        <React.Fragment>
            <div className="flex-col">
                {/* Region - component app bar */}
                <AppBar color="default" position="relative">
                    <Toolbar>
                        <Typography variant="h6">
                            Card Search
                        </Typography>
                        <Tabs value={this.props.cardSearchMethod} onChange={(e, value) => {this.handleSearchMethodTabClick(value)}} >
                            <Tab value="set" label="Set" />
                            <Tab value="web" label="Web" />
                            <Tab value="inventory" label="Inventory" />
                        </Tabs>
                    </Toolbar>
                </AppBar>
                {/* EndRegion - component app bar */}
                <div>
                <CardSearchFilterBar />       

                {/* Region - Search Results */}
        
                {/* <CardList 
                    handleAddPendingCard={this.handleAddPendingCard}
                    handleRemovePendingCard={this.handleRemovePendingCard}
                    layout="list"
                    cards={this.props.searchResults}
                /> */}
                <Box className="flex-row">
                    <CardSearchResults />
                    <CardSearchResultDetail />
                </Box>
                
                {/* EndRegion - Search Results */}
                
                <CardSearchPendingCards />

                {/* Region - Button Bar */}
                <Paper className="outline-section flex-row">
                    <Button onClick={this.handleCancelClick}>
                        Cancel
                    </Button>
                    <Button color="primary" variant="contained" onClick={this.handleSaveClick}>
                        Save
                    </Button>
                </Paper>
                {/* EndRegion - Button Bar */}

                </div>
            </div>
        </React.Fragment>);
    }
}

function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        cardSearchMethod: state.app.cardSearch.cardSearchMethod,
    }

    return result;
}

export default connect(mapStateToProps)(CardSearch);
