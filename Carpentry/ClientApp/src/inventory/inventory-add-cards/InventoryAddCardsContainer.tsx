//This may be the ideal situation to figure out how to do [that second mapping for actions]
import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../configureStore';
import InventoryAddCardsLayout from './components/InventoryAddCardsLayout';
import { 
    addPendingCard,
    removePendingCard,
    cardSearchFilterValueChanged,
    toggleCardSearchViewMode,
    cardSearchClearPendingCards,
    cardSearchSearchMethodChanged,
    cardSearchSelectCard,
    requestSavePendingCards,
    requestSearch,
} from './state/InventoryAddCardsActions';

interface PropsFromState { 
    cardSearchMethod: "set" | "web" | "inventory";
    pendingCards: { [key:number]: PendingCardsDto }
    deckId: number;
    selectedCard: CardSearchResultDto | null;
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
    }

    handleSaveClick(){
        this.props.dispatch(requestSavePendingCards());
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

    handleAddPendingCard(name: string, cardId: number, isFoil: boolean){
        this.props.dispatch(addPendingCard(name, cardId, isFoil));
    }

    handleRemovePendingCard(name: string, cardId: number, isFoil: boolean){
        this.props.dispatch(removePendingCard(name, cardId, isFoil));
    }

    handleCardSelected(item: CardListItem){
        this.props.dispatch(cardSearchSelectCard(item.data));
    }

    handleSearchButtonClick(){
        this.props.dispatch(requestSearch())
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
    }

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

function selectSearchResults(state: AppState): CardSearchResultDto[] {
    const { allSearchResultIds, searchResultsById } = state.inventory.inventoryAddCards.searchResults;//state.cardSearch.data.searchResults;
    const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
    return result;
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //Notes: "visibleContainer" now needs to be determined by the route & "ownProps"

    //I'm going to need to map pending card totals to the inventory query result
    
    const containerState = state.inventory.inventoryAddCards;

    let mappedSearchResults: CardListItem[] = selectSearchResults(state).map(card => ({
        data: card,
        count: containerState.pendingCards[card.name] && containerState.pendingCards[card.name].cards.length,
    }) as CardListItem);

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
        cardSearchMethod: containerState.cardSearchMethod,
        deckId: parseInt(ownProps.match.params.deckId) || 0,
        pendingCards: containerState.pendingCards,
        selectedCard: containerState.selectedCard,
        searchResults: mappedSearchResults,
        viewMode: containerState.viewMode,
        filterOptions: state.core.data.filterOptions,
        searchFilterProps: containerState.searchFilter,
        visibleFilters: visibleFilters,
    }

    return result;
}

export default connect(mapStateToProps)(InventoryAddCardsContainer);