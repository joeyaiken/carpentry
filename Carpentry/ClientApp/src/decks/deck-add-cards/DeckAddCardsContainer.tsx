//This may be the ideal situation to figure out how to do [that second mapping for actions]
import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import {RootState} from '../../configureStore';
import {
  cardSearchFilterValueChanged,
  toggleCardSearchViewMode,
  // cardSearchSearchMethodChanged, 
  requestAddDeckCard,
  cardSearchSelectCard,
  requestCardSearch,
  requestCardSearchInventory
} from './state/DeckAddCardsActions';
import DeckAddCardsLayout from './components/DeckAddCardsLayout';
import { push } from 'react-router-redux';
import {DeckCardDto} from "../../../../../Carpentry.Angular/ClientApp/src/app/decks/models";

interface PropsFromState {
  // cardSearchMethod: "set" | "web" | "inventory";
  deckId: number;
  selectedCard: CardSearchResultDto | null;

  // selectedCardDetail: InventoryDetailDto | null;
  inventoryCardsById: { [id: number]: InventoryCard };
  inventoryCardsAllIds: number[];

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

class DeckAddCardsContainer extends React.Component<ContainerProps>{
  constructor(props: ContainerProps) {
    super(props);
    this.handleCloseClick = this.handleCloseClick.bind(this);
    this.handleToggleViewClick = this.handleToggleViewClick.bind(this);
    this.handleCardSelected = this.handleCardSelected.bind(this);
    this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
    this.handleFilterChange = this.handleFilterChange.bind(this);
    this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
    this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
    this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
    this.handleAddEmptyCardClick = this.handleAddEmptyCardClick.bind(this);
  }

  handleCloseClick(){
    this.props.dispatch(push(`/decks/${this.props.deckId}`));
  }

  handleToggleViewClick(): void {
    this.props.dispatch(toggleCardSearchViewMode());
  }

  handleCardSelected(item: CardListItem){
    this.props.dispatch(cardSearchSelectCard(item.data));
    this.props.dispatch(requestCardSearchInventory(item.data));
  }

  handleSearchButtonClick(){
    this.props.dispatch(requestCardSearch())
  }

  handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
    this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
  }

  handleBoolFilterChange(filter: string, value: boolean): void {
    this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
  }

  handleAddExistingCardClick(inventoryCard: InventoryCard): void {
    const newDeckCard: DeckCardDto = {
      id: 0,
      deckId: this.props.deckId,
      cardName: inventoryCard.name,
      categoryId: null, //TODO - Should cards added this way default to the sideboard if the deck is full?
      inventoryCardId: inventoryCard.id,
      //TODO - Find a way to exclude these fields from what's sent to the api, as they aren't used
      cardId: inventoryCard.cardId,
      isFoil: inventoryCard.isFoil,
      inventoryCardStatusId: inventoryCard.statusId,
    }
    this.props.dispatch(requestAddDeckCard(newDeckCard));
  }

  handleAddNewCardClick(cardName: string, cardId: number, isFoil: boolean): void {

    let deckCard: DeckCardDto = {
      categoryId: null,
      deckId: this.props.deckId,
      cardName: cardName,
      id: 0,
      inventoryCardId: 0,
      cardId: cardId,
      isFoil: isFoil,
      inventoryCardStatusId: 1,
    }

    this.props.dispatch(requestAddDeckCard(deckCard));
  }

  //specify add mainboard vs sidebiard/maybeboard ?
  handleAddEmptyCardClick(cardName: string): void {
    let deckCard: DeckCardDto = {
      categoryId: null,
      deckId: this.props.deckId,
      cardName: cardName,
      id: 0,
      inventoryCardId: 0,
      cardId: 0,
      isFoil: false,
      inventoryCardStatusId: 0,
    }
    // console.log('ping!');
    this.props.dispatch(requestAddDeckCard(deckCard));
  }

  render(){
    return(
      <DeckAddCardsLayout
        // cardSearchMethod={this.props.cardSearchMethod}
        filterOptions={this.props.filterOptions}
        handleCloseClick={this.handleCloseClick}
        // handleSearchMethodTabClick={this.handleSearchMethodTabClick}
        handleToggleViewClick={this.handleToggleViewClick}
        handleAddExistingCardClick={this.handleAddExistingCardClick}
        handleAddNewCardClick={this.handleAddNewCardClick}
        handleAddEmptyCard={this.handleAddEmptyCardClick}
        handleBoolFilterChange={this.handleBoolFilterChange}
        handleCardSelected={this.handleCardSelected}
        handleFilterChange={this.handleFilterChange}
        handleSearchButtonClick={this.handleSearchButtonClick}
        searchFilterProps={this.props.searchFilterProps}
        searchResults={this.props.searchResults}
        selectedCard={this.props.selectedCard}
        // selectedCardDetail={this.props.selectedCardDetail}
        inventoryCardsById={this.props.inventoryCardsById}
        inventoryCardsAllIds={this.props.inventoryCardsAllIds}
        isLoading={this.props.isLoading}
        viewMode={this.props.viewMode}/>
    );
  }
}

function selectInventoryDetail(state: RootState): InventoryDetailDto {
  const { allCardIds, cardsById, inventoryCardsAllIds, inventoryCardsById } = state.decks.deckAddCards.inventoryDetail;//state.cardSearch.data.inventoryDetail;

  //Are you fucking kidding me?
  //I'm taking the list mapped to a dictionary and mapping it back to a fucking list?
  //...wow

  const result: InventoryDetailDto = {
    cardId: 0,
    name: "",
    cards: allCardIds.map(id => cardsById[id]),
    inventoryCards: inventoryCardsAllIds.map(id => inventoryCardsById[id]),
  }
  return result;
}

function selectSearchResults(state: RootState): CardSearchResultDto[] {
  const { allSearchResultIds, searchResultsById } = state.decks.deckAddCards.searchResults; //state.cardSearch.data.searchResults;
  const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
  return result;
}

function mapStateToProps(state: RootState, ownProps: OwnProps): PropsFromState {
  //I'm going to need to map pending card totals to the inventory query result

  let mappedSearchResults: CardListItem[] = [];

  mappedSearchResults = selectSearchResults(state).map(card => {

    //Apparently THIS IS BAD but I can't figure out a better approach right now
    //Clarification, .Find() is BAD
    //const cardExistsInDeck = state.data.deckDetail.cardOverviewsByName[card.name];
    const { cardOverviews } = state.decks.data.detail;
    const cardExistsInDeck = Boolean(cardOverviews.allIds.find(id => cardOverviews.byId[id].name === card.name));

    return ({
      data: card,
      count: cardExistsInDeck ? 1 : 0
    }) as CardListItem;
  });

  const containerState = state.decks.deckAddCards;

  const result: PropsFromState = {
    // cardSearchMethod: containerState.cardSearchMethod,
    deckId: parseInt(ownProps.match.params.deckId) || 0,
    selectedCard: containerState.selectedCard,

    // selectedCardDetail: selectInventoryDetail(state),
    inventoryCardsById: containerState.inventoryDetail.inventoryCardsById,
    inventoryCardsAllIds: containerState.inventoryDetail.inventoryCardsAllIds,

    searchResults: mappedSearchResults,
    viewMode: containerState.viewMode,
    filterOptions: state.core.filterOptions,
    searchFilterProps: containerState.searchFilterProps,
    isLoading: containerState.inventoryDetail.isLoading || containerState.searchResults.isLoading || containerState.addCardIsSaving,
  }

  return result;
}

export default connect(mapStateToProps)(DeckAddCardsContainer);