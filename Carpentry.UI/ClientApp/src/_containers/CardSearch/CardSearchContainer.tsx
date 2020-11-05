import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../../_reducers';

// import { 
//     cardSearchSearchMethodChanged,
//     cardSearchClearPendingCards,
//     toggleCardSearchViewMode,
// } from '../actions/cardSearch.actions';

// import CardSearchPendingCards from './CardSearchPendingCards'
import { 
    requestAddCardsFromSearch
} from '../../_actions/inventoryActions';

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
import SearchResultTable from './components/SearchResultTable';
import SearchResultGrid from './components/SearchResultGrid';
import PendingCardsSection from './components/PendingCardsSection';
import DeckSelectedCardSection from './components/DeckSelectedCardSection';
import SelectedCardSection from './components/SelectedCardSection';
import { cardSearchClearPendingCards, cardSearchSearchMethodChanged, toggleCardSearchViewMode, cardSearchAddPendingCard, cardSearchRemovePendingCard, cardSearchSelectCard, requestCardSearchInventory, requestCardSearch, requestAddDeckCard } from '../../_actions/cardSearchActions';
import SetSearchFilterBar from './components/SetSearchFilterBar';
import WebSearchFilterBar from './components/WebSearchFilterBar';
import FilterBarSearchButton from '../../_components/FilterBarSearchButton';
import { filterValueChanged } from '../../_actions/ui.actions';
import { combineStyles, appStyles } from '../../styles/appStyles';

// import CardSearchFilterBar from './CardSearchFilterBar';
// import CardSearchResultDetail from './CardSearchResultDetail';
// import CardSearchResults from './CardSearchResults';

interface PropsFromState { 
    // cardSearchMethod: "set" | "web" | "inventory";
    cardSearchMethod: "set" | "web" | "inventory";
    
    pendingCards: { [key:number]: PendingCardsDto }

    searchContext: "deck" | "inventory";
    deckId: number;

    selectedCard: CardSearchResultDto | null;
    selectedCardDetail: InventoryDetailDto | null;

    searchResults: CardListItem[];
    viewMode: CardSearchViewMode;

    
    filterOptions: AppFiltersDto;
    searchFilterProps: CardFilterProps;
    visibleFilters: CardFilterVisibilities;
}

interface OwnProps {
    searchContext: "deck" | "inventory";
    match: {
        params: {
            deckId: string
        }
    }
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

    //handleAddPendingCard(data: CardSearchResultDto, isFoil: boolean, variant: string){
    handleAddPendingCard(name: string, cardId: number, isFoil: boolean){
    //name: string, cardId: number, isFoil: boolean
        this.props.dispatch(cardSearchAddPendingCard(name, cardId, isFoil));
    }

    //handleRemovePendingCard(multiverseId: number, isFoil: boolean, variant: string){
    handleRemovePendingCard(name: string, cardId: number, isFoil: boolean){
        this.props.dispatch(cardSearchRemovePendingCard(name, cardId, isFoil));
    }

    handleCardSelected(item: CardListItem){
        this.props.dispatch(cardSearchSelectCard(item.data));
        //also search for that selected card
        //Maybe dispatch a second request to load dat detail
        if(this.props.cardSearchMethod !== "set"){
            this.props.dispatch(requestCardSearchInventory(item.data));
        }
    }

    handleSearchButtonClick(){
        this.props.dispatch(requestCardSearch())
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(filterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        // console.log(`search filter bar change filter: ${filter} val: ${value}`)
        this.props.dispatch(filterValueChanged("cardSearchFilterProps", filter, value));
    }

    handleAddExistingCardClick(inventoryCard: InventoryCard): void{
        // this.props.dispatch(requestAddDeckCard(inventoryCard));
    }
    
    //handleAddNewCardClick(multiverseId: number, isFoil: boolean, variant: string): void{
    handleAddNewCardClick(cardId: number, isFoil: boolean): void{

        let deckCard: DeckCardDto = {
            categoryId: null,
            deckId: this.props.deckId,
            id: 0,
            inventoryCardId: 0,
            cardId: cardId,
            isFoil: isFoil,
            inventoryCardStatusId: 1, //in inventory
            // inventoryCard: {
            //     cardId: cardId,
            //     isFoil: isFoil,
            //     statusId: 1,

            //     collectorNumber: 0,
            //     deckCards: [],
            //     id: 0,
            //     name: '',
            //     set: '',
            // }

            
        }

        // let inventoryCard: InventoryCard = {
        //     id: 0,
        //     deckCards: [],
        //     isFoil: isFoil,
        //     // variantName: variant,
        //     // multiverseId: multiverseId,
        //     statusId: 1,
        //     name: '',
        //     set: '',
        //     cardId: 0,
        //     collectorNumber: 0,
        // }
        this.props.dispatch(requestAddDeckCard(deckCard));
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
                    // visibleFilters={this.props.visibleFilters}
                    cardSearchMethod={this.props.cardSearchMethod}
                    handleSearchButtonClick={this.handleSearchButtonClick}
            />);
    }

//<div className={classes.flexSection} style={{ overflow:'auto', flex:'1 1 70%' }} >
//  { props.children }
// </div>

    renderSearchResults(){
        return(<Paper 
                style={{ overflow:'auto', flex:'1 1 70%' }} >

        
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
        </Paper>);
    }

    renderSearchResultDetail(){
        // console.log('search context')
        // console.log(this.props.searchContext)
        return(
            //<Paper className={staticSection}>
        //     <div className={classes.flexSection} style={{ overflow:'auto', flex:'1 1 30%' }} >
        //     { props.children }
        // </div>

            <Paper style={{ overflow:'auto', flex:'1 1 30%' }} > 
            {
                this.props.selectedCard && this.props.searchContext === "inventory" &&
                <SelectedCardSection 
                    selectedCard={this.props.selectedCard}
                    pendingCards={this.props.pendingCards[this.props.selectedCard.name]}
                    handleAddPendingCard={this.handleAddPendingCard}
                    handleRemovePendingCard={this.handleRemovePendingCard}
                    selectedCardDetail={null} />
            }
            {
                this.props.selectedCard && this.props.searchContext === "deck" &&
                <DeckSelectedCardSection 
                    selectedCard={this.props.selectedCard}
                    // pendingCards={this.props.pendingCards[this.props.selectedCard.multiverseId]}
                    
                    //But decks don't support pending cards?...
                    handleAddPendingCard={this.handleAddPendingCard}
                    
                    handleRemovePendingCard={this.handleRemovePendingCard} 
                    selectedCardDetail={this.props.selectedCardDetail}
                    handleAddInventoryCard={this.handleAddExistingCardClick}
                    handleAddNewCard={this.handleAddNewCardClick}
                    // handleMoveCard={this.handleMoveCardClick}
                    
                    />
            }
            </Paper>
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
    cardSearchMethod: "set" | "web" | "inventory";
    handleSearchButtonClick: () => void;
}

function FilterBar(props: FilterBarProps): JSX.Element{
    const {  flexRow, outlineSection } = appStyles();
    return(<React.Fragment>
        <Paper className={combineStyles(outlineSection, flexRow)}>
            {   (props.cardSearchMethod === "set") && 
                    <SetSearchFilterBar 
                        filterOptions={props.filterOptions}
                        handleBoolFilterChange={props.handleBoolFilterChange}
                        handleFilterChange={props.handleFilterChange}
                        searchFilter={props.searchFilterProps} />
            }
            {   (props.cardSearchMethod === "web") && 
                    <WebSearchFilterBar 
                        filterOptions={props.filterOptions}
                        handleBoolFilterChange={props.handleBoolFilterChange}
                        handleFilterChange={props.handleFilterChange}
                        searchFilter={props.searchFilterProps} />
            }
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
    const {  flexRow, outlineSection, flexSection } = appStyles();
    return(
    <React.Fragment>
        {/* <div className={flexCol}> */}
            { props.appBar }
            {/* <div> */}
            { props.filterBar }
            <Box  // className={flexRow}
                className={ `${flexRow} ${flexSection}` }
                style={{ overflow:'auto', alignItems:'stretch' }}
                >

            {/* className={ `${classes.flexRow} ${classes.flexSection}` } style={{ overflow:'auto', alignItems:'stretch' }} */}


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

            {/* </div> */}
        {/* </div> */}
    </React.Fragment>);
}

function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById } = state.data.cardSearch.inventoryDetail;
    const result: InventoryDetailDto = {
        name: "",
        cards: allCardIds.map(id => cardsById[id]),
        inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
    }
    return result;
}

function selectSearchResults(state: AppState): CardSearchResultDto[] {
    const { allSearchResultIds, searchResultsById } = state.data.cardSearch.searchResults;
    const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
    return result;
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //Notes: "visibleContainer" now needs to be determined by the route & "ownProps"


    // console.log('card search own props');
    // console.log(ownProps);

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
            count: state.data.cardSearch.pendingCards[card.name] && state.data.cardSearch.pendingCards[card.name].cards.length
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
        deckId: parseInt(ownProps.match.params.deckId) || 0,
        
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