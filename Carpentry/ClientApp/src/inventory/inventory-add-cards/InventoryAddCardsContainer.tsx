//This may be the ideal situation to figure out how to do [that second mapping for actions]


import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode } from 'react';
import {
    Button,
    Paper,
    Box,
} from '@material-ui/core';
// import SearchResultTable from './components/SearchResultTable';
// import SearchResultGrid from './components/SearchResultGrid';
// import PendingCardsSection from './components/PendingCardsSection';
// import { 
//     cardSearchClearPendingCards, 
//     cardSearchSearchMethodChanged, 
//     toggleCardSearchViewMode, 
//     cardSearchAddPendingCard, 
//     cardSearchRemovePendingCard, 
//     cardSearchSelectCard, 
//     requestCardSearchInventory,
//     requestCardSearch, 
//     requestAddDeckCard 
// } from '../../_actions/cardSearchActions';

// import {

// } from '';

// import { combineStyles, appStyles } from '../../styles/appStyles';
import { AppState } from '../../configureStore';
// import CardSearchLayout from './components/CardSearchLayout';
import { 
    cardSearchAddPendingCard, 
    cardSearchRemovePendingCard, 
    cardSearchFilterValueChanged, 
    toggleCardSearchViewMode, 
    cardSearchClearPendingCards, 
    cardSearchSearchMethodChanged, 
    // requestAddDeckCard, 
    cardSearchSelectCard, 
    cardSearchRequestSavePendingCards,
    requestCardSearch, 
} from './state/InventoryAddCardsActions';
import InventoryAddCardsLayout from './components/InventoryAddCardsLayout';

interface PropsFromState { 
    // cardSearchMethod: "set" | "web" | "inventory";
    cardSearchMethod: "set" | "web" | "inventory";
    
    pendingCards: { [key:number]: PendingCardsDto }

    searchContext: "deck" | "inventory";
    deckId: number;

    selectedCard: CardSearchResultDto | null;
    // selectedCardDetail: InventoryDetailDto | null;

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


type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class InventoryAddCardsContainer extends React.Component<ContainerProps>{
    constructor(props: ContainerProps) {
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
        // this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
        // this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
    }

    handleSaveClick(){
        this.props.dispatch(cardSearchRequestSavePendingCards());
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
    }

    handleSearchButtonClick(){
        this.props.dispatch(requestCardSearch())
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        // console.log(`search filter bar change filter: ${filter} val: ${value}`)
        this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
    }

    // handleAddExistingCardClick(inventoryCard: InventoryCard): void{
    //     // this.props.dispatch(requestAddDeckCard(inventoryCard));
    // }
    
    // //handleAddNewCardClick(multiverseId: number, isFoil: boolean, variant: string): void{
    // handleAddNewCardClick(cardId: number, isFoil: boolean): void{

    //     let deckCard: DeckCardDto = {
    //         categoryId: null,
    //         deckId: this.props.deckId,
    //         id: 0,
    //         inventoryCardId: 0,
    //         cardId: cardId,
    //         isFoil: isFoil,
    //         inventoryCardStatusId: 1, //in inventory
    //         // inventoryCard: {
    //         //     cardId: cardId,
    //         //     isFoil: isFoil,
    //         //     statusId: 1,

    //         //     collectorNumber: 0,
    //         //     deckCards: [],
    //         //     id: 0,
    //         //     name: '',
    //         //     set: '',
    //         // }

            
    //     }

    //     // let inventoryCard: InventoryCard = {
    //     //     id: 0,
    //     //     deckCards: [],
    //     //     isFoil: isFoil,
    //     //     // variantName: variant,
    //     //     // multiverseId: multiverseId,
    //     //     statusId: 1,
    //     //     name: '',
    //     //     set: '',
    //     //     cardId: 0,
    //     //     collectorNumber: 0,
    //     // }

    //     //is this an app or data action?
    //     //Maybe app so it can reroute after saving
    //     this.props.dispatch(requestAddDeckCard(deckCard));  
    // }

    render(){
        return(
            <InventoryAddCardsLayout 
                cardSearchMethod={this.props.cardSearchMethod}
                filterOptions={this.props.filterOptions}
                handleCancelClick={this.handleCancelClick}
                handleSaveClick={this.handleSaveClick}
                handleSearchMethodTabClick={this.handleSearchMethodTabClick}
                handleToggleViewClick={this.handleToggleViewClick}
                handleAddPendingCard={this.handleAddPendingCard}
                handleBoolFilterChange={this.handleBoolFilterChange}
                handleCardSelected={this.handleCardSelected}
                handleFilterChange={this.handleFilterChange}
                handleRemovePendingCard={this.handleRemovePendingCard}
                handleSearchButtonClick={this.handleSearchButtonClick}
                pendingCards={this.props.pendingCards}
                searchFilterProps={this.props.searchFilterProps}
                searchResults={this.props.searchResults}
                selectedCard={this.props.selectedCard}
                viewMode={this.props.viewMode}/>
            );
    }
}

// function selectInventoryDetail(state: AppState): InventoryDetailDto {
//     const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById } = state.cardSearch.data.inventoryDetail;
//     const result: InventoryDetailDto = {
//         name: "",
//         cards: allCardIds.map(id => cardsById[id]),
//         inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
//     }
//     return result;
// }

function selectSearchResults(state: AppState): CardSearchResultDto[] {
    const { allSearchResultIds, searchResultsById } = state.inventory.inventoryAddCards.searchResults;//state.cardSearch.data.searchResults;
    const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
    return result;
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //Notes: "visibleContainer" now needs to be determined by the route & "ownProps"

    //I'm going to need to map pending card totals to the inventory query result
    
    const containerState = state.inventory.inventoryAddCards;

    let mappedSearchResults: CardListItem[] = [];

    if(ownProps.searchContext === "deck") { // && state.deckEditor.selectedDeckDto != null){

        mappedSearchResults = selectSearchResults(state).map(card => {

            //Apparently THIS IS BAD but I can't figure out a better approach right now
            //Clarification, .Find() is BAD
            //const cardExistsInDeck = state.data.deckDetail.cardOverviewsByName[card.name];
            const { cardOverviewsById, allCardOverviewIds } = state.decks.data.detail;

            const cardExistsInDeck = Boolean(allCardOverviewIds.find(id => cardOverviewsById[id].name === card.name));

            return ({
                data: card,
                count: cardExistsInDeck ? 1 : 0
            }) as CardListItem;
        });

    } else {
        mappedSearchResults = selectSearchResults(state).map(card => ({
            data: card,
            //count: state.data.cardSearch.pendingCards[card.name] && state.data.cardSearch.pendingCards[card.name].cards.length
            //count: state.cardSearch.state.pendingCards[card.name] && state.cardSearch.state.pendingCards[card.name].cards.length
            count: containerState.pendingCards[card.name] && containerState.pendingCards[card.name].cards.length,
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

    //switch(state.cardSearch.state.cardSearchMethod){
    switch(containerState.cardSearchMethod){
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
        cardSearchMethod: containerState.cardSearchMethod,//state.cardSearch.state.cardSearchMethod,
        deckId: parseInt(ownProps.match.params.deckId) || 0,
        pendingCards: containerState.pendingCards,//state.cardSearch.state.pendingCards,
        searchContext: ownProps.searchContext,
        selectedCard: containerState.selectedCard,//state.cardSearch.state.selectedCard,
        // selectedCardDetail: selectInventoryDetail(state),
        searchResults: mappedSearchResults,
        viewMode: containerState.viewMode,//state.cardSearch.state.viewMode,
        filterOptions: state.core.data.filterOptions,
        searchFilterProps: containerState.searchFilter,//state.cardSearch.state.searchFilter,
        visibleFilters: visibleFilters,
    }

    return result;
}

export default connect(mapStateToProps)(InventoryAddCardsContainer);