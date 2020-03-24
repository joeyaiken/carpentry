import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../reducers';

// import SectionLayout from '../components/SectionLayout';

import { 
    cardSearchSelectCard,
    requestCardSearchInventory,
    cardSearchAddPendingCard,
    cardSearchRemovePendingCard,

} from '../actions/cardSearch.actions';

import {
    Button,
    Paper,
    Box,
    TableHead,
    TableRow,
    TableBody,
    Table,
    TableCell,
    IconButton,
} from '@material-ui/core';

import { Star } from '@material-ui/icons';

interface PropsFromState { 
    searchContext: "deck" | "inventory";
    searchResults: CardListItem[];
}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearch extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
        this.handleCardSelected = this.handleCardSelected.bind(this);
    }

    handleAddPendingCard(multiverseId: number, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchAddPendingCard(multiverseId, isFoil, variant));
    }

    handleRemovePendingCard(multiverseId: number, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchRemovePendingCard(multiverseId, isFoil, variant));
    }

    handleCardSelected(item: CardListItem){
        this.props.dispatch(cardSearchSelectCard(item.data));
        //also search for that selected card
        //Maybe dispatch a second request to load dat detail
        this.props.dispatch(requestCardSearchInventory(item.data));

    }

    render() {
        return (
        <React.Fragment>
            <Paper className="flex-section">
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            {
                                this.props.searchContext == "inventory" &&
                                (   <>
                                        <TableCell># Pending</TableCell>
                                        <TableCell>Actions</TableCell>
                                    </>
                                )
                            }
                            {
                                this.props.searchContext == "deck" &&
                                (   <>
                                        {/* <TableCell>Set</TableCell> */}
                                        <TableCell>Type</TableCell>
                                        <TableCell>Cost</TableCell>
                                        <TableCell></TableCell>
                                    </>
                                )
                            }
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            this.props.searchResults.map(result => (
                                <TableRow 
                                    onClick={() => { this.handleCardSelected(result) }}
                                    key={result.data.multiverseId}>
                                    <TableCell>{result.data.name}</TableCell>
                                    {
                                            this.props.searchContext == "inventory" &&
                                            (   <>
                                                    <TableCell>{result.count}</TableCell>
                                                    <TableCell>
                                                        <Box className="flex-row">
                                                            <Button variant="contained" size="small" onClick={() => {this.handleRemovePendingCard(result.data.multiverseId, false, "normal")} } >-</Button>
                                                            {/* <Typography>({result.count})</Typography> */}
                                                            <Button variant="contained" size="small" onClick={() => {this.handleAddPendingCard(result.data.multiverseId, false, "normal")} } >+</Button>       
                                                        </Box>
                                                    </TableCell>
                                                </>
                                            )
                                        }
                                        {
                                            this.props.searchContext == "deck" &&
                                            (   <>
                                                    {/* <TableCell>{result.data.set}</TableCell> */}
                                                    <TableCell>{result.data.type}</TableCell>
                                                    <TableCell>{result.data.manaCost}</TableCell>
                                                    <TableCell>{Boolean(result.count) && 
                                                        <IconButton color="inherit" disabled={true} size="small">
                                                            <Star />
                                                        </IconButton> 
                                                    }</TableCell>
                                                </>
                                            )
                                        }                                            
                                </TableRow>
                            ))
                        }
                    </TableBody>
                </Table>
            </Paper>
        </React.Fragment>);
    }
}

function selectSearchResults(state: AppState): MagicCard[] {
    const { allSearchResultIds, searchResultsById } = state.data.cardSearchResults;
    const result: MagicCard[] = allSearchResultIds.map(mid => searchResultsById[mid])
    return result;
}

function mapStateToProps(state: AppState): PropsFromState {
    // console.log(state.cardSearch.inventoryDetail);

    //I'm going to need to map pending card totals to the inventory query result
    
    let mappedSearchResults: CardListItem[] = [];

    if(state.app.core.visibleContainer == "deckEditor") { // && state.deckEditor.selectedDeckDto != null){

        mappedSearchResults = selectSearchResults(state).map(card => {

            const cardExistsInDeck = state.data.deckDetail.cardOverviewsByName[card.name];

            return ({
                data: card,
                count: cardExistsInDeck ? 1 : 0
            }) as CardListItem;
        });

    } else {
        mappedSearchResults = selectSearchResults(state).map(card => ({
            data: card,
            count: state.data.cardSearchPendingCards.pendingCards[card.multiverseId] && state.data.cardSearchPendingCards.pendingCards[card.multiverseId].cards.length
        }) as CardListItem);
    }

    const result: PropsFromState = {
        searchContext: (state.app.core.visibleContainer == "deckEditor") ? "deck":"inventory",
        searchResults: mappedSearchResults,
    }

    return result;
}

export default connect(mapStateToProps)(CardSearch);
