import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../../reducers';

// import { 
//     cardSearchSearchMethodChanged,
//     cardSearchClearPendingCards,
//     toggleCardSearchViewMode,
// } from '../actions/cardSearch.actions';

// import CardSearchPendingCards from './CardSearchPendingCards'
import { 
    requestAddCardsFromSearch
} from '../../actions/inventoryActions';

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
import DeckSelectedCardSection from './DeckSelectedCardSection';
import SelectedCardSection from './SelectedCardSection';
import { cardSearchClearPendingCards, cardSearchSearchMethodChanged, toggleCardSearchViewMode, cardSearchAddPendingCard, cardSearchRemovePendingCard, cardSearchSelectCard, requestCardSearchInventory, requestCardSearch, requestAddDeckCard } from '../../actions/cardSearchActions';
import CardFilterBar from '../Inventory/CardFilterBar';
import FilterBarSearchButton from '../../components/FilterBarSearchButton';
import { filterValueChanged } from '../../actions/ui.actions';
import { combineStyles, appStyles } from '../../styles/appStyles';

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

    
    filterOptions: AppFiltersDto;
    searchFilterProps: CardFilterProps;
    visibleFilters: CardFilterVisibilities;
}

interface OwnProps {
    searchContext: "deck" | "inventory";
    
    // match: {
    //     params: {
    //         deckId: number
    //     }
    // }
    // viewMode: DeckEditorViewMode;//"list" | "grid";
    // deckProperties: DeckDetailDto | null;
    // cardOverviews: InventoryOverviewDto[];
    // cardMenuAnchor: HTMLButtonElement | null;
    // deckPropsModalOpen: boolean;
    // selectedCard: InventoryOverviewDto | null;
    // selectedInventoryCards: InventoryCard[];
    // deckStats: DeckStats | null;
}


type CardSearchContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearchContainer extends React.Component<CardSearchContainerProps>{
    constructor(props: CardSearchContainerProps) {
        super(props);
        this.handleSaveClick = this.handleSaveClick.bind(this);
        this.handleCancelClick = this.handleCancelClick.bind(this);
        this.handleSearchMethodTabClick = this.handleSearchMethodTabClick.bind(this);
        this.handleToggleViewClick = this.handleToggleViewClick.bind(this);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
        this.handleCardSelected = this.handleCardSelected.bind(this);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
        this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
        this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
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
        // const { flexCol, flexRow, outlineSection } = appStyles();
        return (
            <ContainerLayout
                appBar={this.renderAppBar()}
                filterBar={this.renderFilterBar()}
                handleCancelClick={this.handleCancelClick}
                handleSaveClick={this.handleSaveClick}
                pendingCards={ this.renderPendingCards() }
                searchResults={<React.Fragment>
                    {this.renderSearchResults()}
                    {this.renderSearchResultDetail()}
                </React.Fragment>}


            />






        
        );
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
        // const {  flexRow, outlineSection } = appStyles();
        return(
            <FilterBar 
                filterOptions={this.props.filterOptions}
                    handleBoolFilterChange={this.handleBoolFilterChange}
                    handleFilterChange={this.handleFilterChange}
                    searchFilterProps={this.props.searchFilterProps}
                    visibleFilters={this.props.visibleFilters}
            
                    handleSearchButtonClick={this.handleSearchButtonClick}
            />);
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
            </React.Fragment>
        );
    }

    renderPendingCards(){
        return(
        <React.Fragment>
            <PendingCardsSection pendingCards={this.props.pendingCards} />
        </React.Fragment>);
    }
}

interface FilterBarProps {
    filterOptions: AppFiltersDto;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;

    searchFilterProps: CardFilterProps;

    visibleFilters: CardFilterVisibilities;
    handleSearchButtonClick: () => void;

}

function FilterBar(props: FilterBarProps): JSX.Element{
        const {  flexRow, outlineSection } = appStyles();
        return(<React.Fragment>
            <Paper className={combineStyles(outlineSection, flexRow)}>
                <CardFilterBar 
                    filterOptions={props.filterOptions}
                    handleBoolFilterChange={props.handleBoolFilterChange}
                    handleFilterChange={props.handleFilterChange}
                    
                    searchFilter={props.searchFilterProps}
                    visibleFilters={props.visibleFilters}
                />
                <FilterBarSearchButton handleSearchButtonClick={props.handleSearchButtonClick}/>

            </Paper>
        </React.Fragment>);
    }



interface ContainerLayoutProps {
    appBar: ReactNode;
    filterBar: ReactNode;
    pendingCards: ReactNode;
    searchResults: ReactNode;
    handleCancelClick: () => void;
    handleSaveClick: () => void;
}

function ContainerLayout(props: ContainerLayoutProps): JSX.Element {
    const {  flexRow, outlineSection, flexCol } = appStyles();
    return(<React.Fragment>
        <div className={flexCol}>
            { props.appBar }
            <div>
                { props.filterBar }
            <Box className={flexRow}>
                {props.searchResults}
            </Box>
            {props.pendingCards}
            <Paper className={combineStyles(outlineSection, flexRow)}>
                <Button onClick={props.handleCancelClick}>
                    Cancel
                </Button>
                <Button color="primary" variant="contained" onClick={props.handleSaveClick}>
                    Save
                </Button>
            </Paper>

            </div>
        </div>
    </React.Fragment>);


}

function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById } = state.data.cardSearch.inventoryDetail;
    const result: InventoryDetailDto = {
        cards: allCardIds.map(id => cardsById[id]),
        inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
    }
    return result;
}

function selectSearchResults(state: AppState): MagicCard[] {
    const { allSearchResultIds, searchResultsById } = state.data.cardSearch.searchResults;
    const result: MagicCard[] = allSearchResultIds.map(mid => searchResultsById[mid])
    return result;
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //Notes: "visibleContainer" now needs to be determined by the route & "ownProps"



    // console.log(state.cardSearch.inventoryDetail);

    //I'm going to need to map pending card totals to the inventory query result
    
    let mappedSearchResults: CardListItem[] = [];

    if(ownProps.searchContext === "deck") { // && state.deckEditor.selectedDeckDto != null){

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
            count: state.data.cardSearch.pendingCards[card.multiverseId] && state.data.cardSearch.pendingCards[card.multiverseId].cards.length
        }) as CardListItem);
    }

    let visibleFilters: CardFilterVisibilities = {
        name: false,
        color: false,
        rarity: false,
        set: false,
        type: false,
        count: false,
        format: false,
        text: false,
    }

    switch(state.app.cardSearch.cardSearchMethod){
        case "inventory":
            visibleFilters = {
                ...visibleFilters,
                format: true,
                color: true,
                type: true,
                set: true,
                rarity: true,
                text: true,
            }
            break;
        case "set":
            visibleFilters = {
                ...visibleFilters,
                color: true,
                rarity: true,
                set: true,
                type: true,
            }
            break;
        case "web":
            visibleFilters = {
                ...visibleFilters,
                name: true,
            }
            break;
    }

    const result: PropsFromState = {
        // cardSearchMethod: state.app.cardSearch.cardSearchMethod,
        cardSearchMethod: state.app.cardSearch.cardSearchMethod,
        
        pendingCards: state.data.cardSearch.pendingCards,

        //searchContext: (state.app.core.visibleContainer === "deckEditor") ? "deck":"inventory",
        searchContext: ownProps.searchContext,
        selectedCard: state.app.cardSearch.selectedCard,
        selectedCardDetail: selectInventoryDetail(state),

        searchResults: mappedSearchResults,
        viewMode: state.app.cardSearch.viewMode,
        filterOptions: state.data.appFilterOptions.filterOptions,
        searchFilterProps: state.ui.cardSearchFilterProps,
        visibleFilters: visibleFilters,
    }

    return result;
}

export default connect(mapStateToProps)(CardSearchContainer);