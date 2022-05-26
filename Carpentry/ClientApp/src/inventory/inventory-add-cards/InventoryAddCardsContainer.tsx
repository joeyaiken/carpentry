//This may be the ideal situation to figure out how to do [that second mapping for actions]
import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { RootState } from '../../configureStore';
import InventoryAddCardsLayout from './components/InventoryAddCardsLayout';
import {
  addPendingCard,
  removePendingCard,
  cardSearchFilterValueChanged,
  toggleCardSearchViewMode,
  cardSearchClearPendingCards,
  cardSearchSelectCard,
  requestSavePendingCards,
  requestSearch,
} from './state/InventoryAddCardsActions';

interface PropsFromState {
  pendingCards: { [key:number]: PendingCardsDto }
  deckId: number;
  selectedCard: CardSearchResultDto | null;
  searchResults: CardListItem[];
  viewMode: CardSearchViewMode;
  filterOptions: AppFiltersDto;
  searchFilterProps: CardFilterProps;
  isLoading: boolean;
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
        filterOptions={this.props.filterOptions}
        pendingCards={this.props.pendingCards}
        searchFilterProps={this.props.searchFilterProps}
        searchResults={this.props.searchResults}
        selectedCard={this.props.selectedCard}
        viewMode={this.props.viewMode}
        isLoading={this.props.isLoading}
        handleCancelClick={this.handleCancelClick}
        handleSaveClick={this.handleSaveClick}
        handleToggleViewClick={this.handleToggleViewClick}
        handleAddPendingCard={this.handleAddPendingCard}
        handleBoolFilterChange={this.handleBoolFilterChange}
        handleCardSelected={this.handleCardSelected}
        handleFilterChange={this.handleFilterChange}
        handleRemovePendingCard={this.handleRemovePendingCard}
        handleSearchButtonClick={this.handleSearchButtonClick}
      />);
  }
}

function selectSearchResults(state: RootState): CardSearchResultDto[] {
  const { allSearchResultIds, searchResultsById } = state.inventory.inventoryAddCards.searchResults;
  const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
  return result;
}

function mapStateToProps(state: RootState, ownProps: OwnProps): PropsFromState {
  const containerState = state.inventory.inventoryAddCards;

  let mappedSearchResults: CardListItem[] = selectSearchResults(state).map(card => ({
    data: card,
    count: containerState.pendingCards[card.name] && containerState.pendingCards[card.name].cards.length,
  }) as CardListItem);

  const result: PropsFromState = {
    deckId: parseInt(ownProps.match.params.deckId) || 0,
    pendingCards: containerState.pendingCards,
    selectedCard: containerState.selectedCard,
    searchResults: mappedSearchResults,
    viewMode: containerState.viewMode,
    filterOptions: state.core.filterOptions,
    searchFilterProps: containerState.searchFilter,
    isLoading: containerState.searchResults.isLoading,
  }
  return result;
}

export default connect(mapStateToProps)(InventoryAddCardsContainer);