//This may be the ideal situation to figure out how to do [that second mapping for actions]
import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../configureStore';
import { 
    cardSearchFilterValueChanged, 
    toggleCardSearchViewMode, 
    cardSearchSearchMethodChanged, 
    requestAddDeckCard, 
    cardSearchSelectCard, 
    requestCardSearch, 
    requestCardSearchInventory
} from './state/DeckAddCardsActions';
import DeckAddCardsLayout from './components/DeckAddCardsLayout';

interface PropsFromState { 
    cardSearchMethod: "set" | "web" | "inventory";
    
    // pendingCards: { [key:number]: PendingCardsDto }

    // searchContext: "deck" | "inventory";
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
    match: {
        params: {
            deckId: string
        }
    }
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckAddCardsContainer extends React.Component<ContainerProps>{
    constructor(props: ContainerProps) {
        super(props);
        // this.handleSaveClick = this.handleSaveClick.bind(this);
        this.handleCancelClick = this.handleCancelClick.bind(this);
        this.handleSearchMethodTabClick = this.handleSearchMethodTabClick.bind(this);
        this.handleToggleViewClick = this.handleToggleViewClick.bind(this);
        this.handleCardSelected = this.handleCardSelected.bind(this);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
        this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
        this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
    }

    // handleSaveClick(){
    //     this.props.dispatch(cardSearchRequestSavePendingCards());
    // }

    handleCancelClick(){
        // this.props.dispatch(cardSearchClearPendingCards());
    }

    handleSearchMethodTabClick(name: string): void {
        this.props.dispatch(cardSearchSearchMethodChanged(name));
    }

    handleToggleViewClick(): void {
        this.props.dispatch(toggleCardSearchViewMode());
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
        this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        // console.log(`search filter bar change filter: ${filter} val: ${value}`)
        this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
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

        //is this an app or data action?
        //Maybe app so it can reroute after saving
        this.props.dispatch(requestAddDeckCard(deckCard));  
    }

    render(){
        return(
            <DeckAddCardsLayout
                cardSearchMethod={this.props.cardSearchMethod}
                filterOptions={this.props.filterOptions}
                handleCancelClick={this.handleCancelClick}
                // handleSaveClick={this.handleSaveClick}
                handleSearchMethodTabClick={this.handleSearchMethodTabClick}
                handleToggleViewClick={this.handleToggleViewClick}
                handleAddExistingCardClick={this.handleAddExistingCardClick}
                handleAddNewCardClick={this.handleAddNewCardClick}
                // handleAddPendingCard={this.handleAddPendingCard}
                handleBoolFilterChange={this.handleBoolFilterChange}
                handleCardSelected={this.handleCardSelected}
                handleFilterChange={this.handleFilterChange}
                // handleRemovePendingCard={this.handleRemovePendingCard}
                handleSearchButtonClick={this.handleSearchButtonClick}
                // pendingCards={this.props.pendingCards}
                // searchContext={this.props.searchContext}
                searchFilterProps={this.props.searchFilterProps}
                searchResults={this.props.searchResults}
                selectedCard={this.props.selectedCard}
                selectedCardDetail={this.props.selectedCardDetail}
                viewMode={this.props.viewMode}/>
                
        );
    }
}

function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById } = state.decks.deckAddCards.inventoryDetail;//state.cardSearch.data.inventoryDetail;
    const result: InventoryDetailDto = {
        name: "",
        cards: allCardIds.map(id => cardsById[id]),
        inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
    }
    return result;
}

function selectSearchResults(state: AppState): CardSearchResultDto[] {
    const { allSearchResultIds, searchResultsById } = state.decks.deckAddCards.searchResults; //state.cardSearch.data.searchResults;
    const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
    return result;
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //I'm going to need to map pending card totals to the inventory query result
    
    let mappedSearchResults: CardListItem[] = [];

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
    switch(state.decks.deckAddCards.cardSearchMethod){
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

    const containerState = state.decks.deckAddCards;

    const result: PropsFromState = {
        cardSearchMethod: containerState.cardSearchMethod,//state.cardSearch.state.cardSearchMethod,
        deckId: parseInt(ownProps.match.params.deckId) || 0,
        selectedCard: containerState.selectedCard,//state.cardSearch.state.selectedCard,
        selectedCardDetail: selectInventoryDetail(state),
        searchResults: mappedSearchResults,
        viewMode: containerState.viewMode,//state.cardSearch.state.viewMode,
        filterOptions: state.core.data.filterOptions,
        searchFilterProps: containerState.searchFilterProps,//state.cardSearch.state.searchFilter,
        visibleFilters: visibleFilters,
    }

    return result;
}

export default connect(mapStateToProps)(DeckAddCardsContainer);