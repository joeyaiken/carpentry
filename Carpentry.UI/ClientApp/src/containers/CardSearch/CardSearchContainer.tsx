import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../../reducers';

// import { 
//     cardSearchSearchMethodChanged,
//     cardSearchClearPendingCards,
//     toggleCardSearchViewMode,
// } from '../actions/cardSearch.actions';

// import CardSearchPendingCards from './CardSearchPendingCards'
// import { 
//     requestAddCardsFromSearch
// } from '../actions/inventory.actions';

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
import SearchResultTable from './SearchResultTable';
import SearchResultGrid from './SearchResultGrid';
import PendingCardsSection from './PendingCardsSection';

// import CardSearchFilterBar from './CardSearchFilterBar';
// import CardSearchResultDetail from './CardSearchResultDetail';
// import CardSearchResults from './CardSearchResults';

interface PropsFromState { 
    // cardSearchMethod: "set" | "web" | "inventory";
    cardSearchMethod: "set" | "web" | "inventory";
    
    pendingCards: { [key:number]: PendingCardsDto }

    searchContext: "deck" | "inventory";
    selectedCard: MagicCard | null;
    selectedCardDetail: InventoryDetailDto | null;

    searchResults: CardListItem[];
    viewMode: CardSearchViewMode;

}

type CardSearchContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearchContainer extends React.Component<CardSearchContainerProps>{
    constructor(props: CardSearchContainerProps) {
        super(props);
        // this.handleSaveClick = this.handleSaveClick.bind(this);
        // this.handleCancelClick = this.handleCancelClick.bind(this);
        // this.handleSearchMethodTabClick = this.handleSearchMethodTabClick.bind(this);
        // this.handleToggleViewClick = this.handleToggleViewClick.bind(this);
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

    handleToggleViewClick(): void {
        this.props.dispatch(toggleCardSearchViewMode());
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

    handleSearchButtonClick(){
        this.props.dispatch(requestCardSearch())
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(filterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        console.log(`search filter bar change filter: ${filter} val: ${value}`)
        this.props.dispatch(filterValueChanged("cardSearchFilterProps", filter, value));
    }

    handleAddExistingCardClick(inventoryCard: InventoryCard): void{
        this.props.dispatch(requestAddDeckCard(inventoryCard));
    }
    
    handleAddNewCardClick(multiverseId: number, isFoil: boolean, variant: string): void{
        let inventoryCard: InventoryCard = {
            id: 0,
            deckCards: [],
            isFoil: isFoil,
            variantName: variant,
            multiverseId: multiverseId,
            statusId: 1,
            name: '',
            set: '',
        }
        this.props.dispatch(requestAddDeckCard(inventoryCard));
    }

    render() {
        return (
        <React.Fragment>
            <div className="flex-col">
                { this.renderAppBar() }

                <div>
                
                    { this.renderFilterBar() }

                <Box className="flex-row">
                    { this.renderSearchResults() }
                    { this.renderSearchResultDetail() }
                </Box>

                { this.renderPendingCards() }

                <Paper className="outline-section flex-row">
                    <Button onClick={this.handleCancelClick}>
                        Cancel
                    </Button>
                    <Button color="primary" variant="contained" onClick={this.handleSaveClick}>
                        Save
                    </Button>
                </Paper>

                </div>
            </div>
        </React.Fragment>);
    }

    renderAppBar(){
        return(
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
                    <Button onClick={this.handleToggleViewClick} color="primary" variant="contained">
                        Toggle View
                    </Button>
                </Toolbar>
            </AppBar>
        );
    }

    renderFilterBar(){
        return(<React.Fragment>
            <Paper className="outline-section flex-row">
                <CardFilterBar 
                    filterOptions={this.props.filterOptions}
                    handleBoolFilterChange={this.handleBoolFilterChange}
                    handleFilterChange={this.handleFilterChange}
                    
                    searchFilter={this.props.searchFilterProps}
                    visibleFilters={this.props.visibleFilters}
                />
                <FilterBarSearchButton handleSearchButtonClick={this.handleSearchButtonClick}/>

            </Paper>
        </React.Fragment>);
    }

    renderSearchResults(){
        return(<React.Fragment>
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

    renderSearchResultDetail(){
        return(
            <CardSearchResultDetail />
        );
    }

    renderPendingCards(){
        return(
        <React.Fragment>
            <PendingCardsSection pendingCards={this.props.pendingCards} />
        </React.Fragment>);
    }
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
        // cardSearchMethod: state.app.cardSearch.cardSearchMethod,
        cardSearchMethod: state.app.cardSearch.cardSearchMethod,
        
        pendingCards: state.data.cardSearchPendingCards.pendingCards,

        searchContext: (state.app.core.visibleContainer === "deckEditor") ? "deck":"inventory",
        selectedCard: state.app.cardSearch.selectedCard,
        selectedCardDetail: selectInventoryDetail(state),

        searchResults: mappedSearchResults,
        viewMode: state.app.cardSearch.viewMode
    }

    return result;
}

export default connect(mapStateToProps)(CardSearchContainer);


//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////

class CardSearch_selectedCardSection extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
        this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
        this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
    }

    //handleAddPendingCard(multiverseId: number, isFoil: boolean, variant: string){
    handleAddPendingCard(data: MagicCard, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchAddPendingCard(data, isFoil, variant));
    }

    handleRemovePendingCard(multiverseId: number, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchRemovePendingCard(multiverseId, isFoil, variant));
    }

    handleAddExistingCardClick(inventoryCard: InventoryCard): void{
        this.props.dispatch(requestAddDeckCard(inventoryCard));
    }
    
    handleAddNewCardClick(multiverseId: number, isFoil: boolean, variant: string): void{
        let inventoryCard: InventoryCard = {
            id: 0,
            deckCards: [],
            isFoil: isFoil,
            variantName: variant,
            multiverseId: multiverseId,
            statusId: 1,
            name: '',
            set: '',
        }
        this.props.dispatch(requestAddDeckCard(inventoryCard));
    }

    render() {
        return (
        <React.Fragment>
            {
                this.props.selectedCard && this.props.searchContext === "inventory" &&
                <SelectedCardSection 
                    selectedCard={this.props.selectedCard}
                    pendingCards={this.props.pendingCards[this.props.selectedCard.multiverseId]}
                    handleAddPendingCard={this.handleAddPendingCard}
                    handleRemovePendingCard={this.handleRemovePendingCard}
                    selectedCardDetail={null} />
            }
            {
                this.props.selectedCard && this.props.searchContext === "deck" &&
                <DeckSelectedCardSection 
                    selectedCard={this.props.selectedCard}
                    pendingCards={this.props.pendingCards[this.props.selectedCard.multiverseId]}
                    
                    //But decks don't support pending cards?...
                    handleAddPendingCard={this.handleAddPendingCard}
                    
                    handleRemovePendingCard={this.handleRemovePendingCard} 
                    selectedCardDetail={this.props.selectedCardDetail}
                    handleAddInventoryCard={this.handleAddExistingCardClick}
                    handleAddNewCard={this.handleAddNewCardClick}
                    // handleMoveCard={this.handleMoveCardClick}
                    
                    />
            }
        </React.Fragment>);
    }
}

function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById } = state.data.cardSearchInventoryDetail;
    const result: InventoryDetailDto = {
        cards: allCardIds.map(id => cardsById[id]),
        inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
    }
    return result;
}

function selectSearchResults(state: AppState): MagicCard[] {
    const { allSearchResultIds, searchResultsById } = state.data.cardSearchResults;
    const result: MagicCard[] = allSearchResultIds.map(mid => searchResultsById[mid])
    return result;
}
