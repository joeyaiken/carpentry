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
    CardHeader,
    CardMedia,
    Card,
} from '@material-ui/core';

import { Star } from '@material-ui/icons';
import VisualCard from '../components/VisualCard';

interface PropsFromState { 
    searchContext: "deck" | "inventory";
    searchResults: CardListItem[];
    viewMode: CardSearchViewMode;
}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearch extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
        this.handleCardSelected = this.handleCardSelected.bind(this);
    }

    handleAddPendingCard(data: MagicCard, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchAddPendingCard(data,  isFoil, variant));
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
            {
                this.props.viewMode === "list" && 
                    <SearchResultTable 
                        searchContext={this.props.searchContext} 
                        searchResults={this.props.searchResults}
                        handleAddPendingCard={this.handleAddPendingCard}
                        handleRemovePendingCard={this.handleRemovePendingCard}
                        onCardSelected={this.handleCardSelected}
                        />
            }
            {
                this.props.viewMode === "grid" &&
                    <SearchResultGrid 
                        searchResults={this.props.searchResults}
                        onCardSelected={this.handleCardSelected}
                        />
            }
        </React.Fragment>);
    }
}

interface SearchResultTableProps {
    searchContext: "deck" | "inventory";
    searchResults: CardListItem[];
    handleAddPendingCard: (data: MagicCard, isFoil: boolean, variant: string) => void;
    handleRemovePendingCard: (multiverseId: number, isFoil: boolean, variant: string) => void;
    onCardSelected: (item: CardListItem) => void;
}
function SearchResultTable(props: SearchResultTableProps): JSX.Element {

    return (
        <Paper className="flex-section">
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        {
                            props.searchContext === "inventory" &&
                            (   <>
                                    <TableCell># Pending</TableCell>
                                    <TableCell>Actions</TableCell>
                                </>
                            )
                        }
                        {
                            props.searchContext === "deck" &&
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
                        props.searchResults.map(result => (
                            <TableRow 
                                onClick={() => { props.onCardSelected(result) }}
                                key={result.data.multiverseId}>
                                <TableCell>{result.data.name}</TableCell>
                                {
                                        props.searchContext === "inventory" &&
                                        (   <>
                                                <TableCell>{result.count}</TableCell>
                                                <TableCell>
                                                    <Box className="flex-row">
                                                        <Button variant="contained" size="small" onClick={() => {props.handleRemovePendingCard(result.data.multiverseId, false, "normal")} } >-</Button>
                                                        {/* <Typography>({result.count})</Typography> */}
                                                        <Button variant="contained" size="small" onClick={() => {props.handleAddPendingCard(result.data, false, "normal")} } >+</Button>       
                                                    </Box>
                                                </TableCell>
                                            </>
                                        )
                                    }
                                    {
                                        props.searchContext === "deck" &&
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
    );
}

interface SearchResultGridProps {
    searchResults: CardListItem[];
    onCardSelected: (item: CardListItem) => void;
}
function SearchResultGrid(props: SearchResultGridProps): JSX.Element {
    // <VisualCard key={card.data.name} cardOverview={card} onCardSelected={() => {props.onCardSelected(card)}} />

    return (
        <Box className="flex-row-wrap">
            {props.searchResults.map((card) => (            
                <Card 
                    key={card.data.name} 
                    className="outline-section"
                    onClick={() => props.onCardSelected(card)}
                    >
                    {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={ `${card.data.name} - (${props.cardOverview.count})` } /> */}
                    {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={`${card.name} (${card.count})`}/> */}
                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        className="item-image"
                        image={card.data.variants['normal'] || ''}
                        title={card.data.name} />
                    {/* {props.children} */}

                </Card>
            ))}
        </Box>
    );
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

    if(state.app.core.visibleContainer === "deckEditor") { // && state.deckEditor.selectedDeckDto != null){

        mappedSearchResults = selectSearchResults(state).map(card => {

            //Apparently THIS IS BAD but I can't figure out a better approach right now
            //Clarification, .Find() is BAD
            //const cardExistsInDeck = state.data.deckDetail.cardOverviewsByName[card.name];
            const { cardOverviewsById, allCardOverviewIds } = state.data.deckDetail;

            const cardExistsInDeck = Boolean(allCardOverviewIds.find(id => cardOverviewsById[id].name === card.name));

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
        searchContext: (state.app.core.visibleContainer === "deckEditor") ? "deck":"inventory",
        searchResults: mappedSearchResults,
        viewMode: state.app.cardSearch.viewMode
    }

    return result;
}

export default connect(mapStateToProps)(CardSearch);
