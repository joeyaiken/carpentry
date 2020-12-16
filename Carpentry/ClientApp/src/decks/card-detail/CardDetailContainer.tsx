//This may be the ideal situation to figure out how to do [that second mapping for actions]
import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../configureStore';
// import { 
//     cardSearchFilterValueChanged, 
//     toggleCardSearchViewMode, 
//     // cardSearchSearchMethodChanged, 
//     requestAddDeckCard, 
//     cardSearchSelectCard, 
//     requestCardSearch, 
//     requestCardSearchInventory
// } from './state/DeckAddCardsActions';
import CardDetailLayout from './components/CardDetailLayout';
import { ensureCardDetailLoaded } from './state/CardDetailActions';
// import { push } from 'react-router-redux';


//This 'new state' probably actually belongs on the app reducer
//That way I'm not recalculating this info everytime an action occurrs
// interface newState {
//     selectedCardId: number;
//     cardsById: { [cardId: number]: MagicCard }
//     inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
//     cardGroups: { [cardId: number]: number[] }
//     // cardGroupIds: number[];
//     cards: NamedCardGroup[];
// }


interface PropsFromState { 
    selectedCardId: number;
    cardsById: { [cardId: number]: MagicCard }
    allCardIds: number[];
    inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
    cardGroups: { [cardId: number]: number[] }

    // cardGroupIds: number[];
    // cards: NamedCardGroup[];

    // cardSearchMethod: "set" | "web" | "inventory";
    // deckId: number;
    // selectedCardId: number;
    // selectedCard: CardSearchResultDto | null;
    // selectedCardDetail: InventoryDetailDto | null;
    // searchResults: CardListItem[];
    // viewMode: CardSearchViewMode;
    // filterOptions: AppFiltersDto;
    // searchFilterProps: CardFilterProps;
}

interface OwnProps {
    selectedCardId: number;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardDetailContainer extends React.Component<ContainerProps>{
    constructor(props: ContainerProps) {
        super(props);
        // this.handleCloseClick = this.handleCloseClick.bind(this);
        // this.handleToggleViewClick = this.handleToggleViewClick.bind(this);
        // this.handleCardSelected = this.handleCardSelected.bind(this);
        // this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        // this.handleFilterChange = this.handleFilterChange.bind(this);
        // this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
        // this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
        // this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
        // this.handleAddEmptyCardClick = this.handleAddEmptyCardClick.bind(this);
    }

    //on-mount: ensure name loaded
    componentDidMount() {
        // this.props.dispatch(ensureDeckDetailLoaded(this.props.deckId));
        // if(this.props.selectedCardId )
        this.props.dispatch(ensureCardDetailLoaded(this.props.selectedCardId));
    }
    // handleCloseClick(){
    //     this.props.dispatch(push(`/decks/${this.props.deckId}`));
    // }

    // handleToggleViewClick(): void {
    //     this.props.dispatch(toggleCardSearchViewMode());
    // }

    // handleCardSelected(item: CardListItem){
    //     this.props.dispatch(cardSearchSelectCard(item.data));
    //     this.props.dispatch(requestCardSearchInventory(item.data));
    // }

    // handleSearchButtonClick(){
    //     this.props.dispatch(requestCardSearch())
    // }

    // handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
    //     this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    // }

    // handleBoolFilterChange(filter: string, value: boolean): void {
    //     this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
    // }

    // handleAddExistingCardClick(inventoryCard: InventoryCard): void {
    //     // this.props.dispatch(requestAddDeckCard(inventoryCard));
    // }
    
    // handleAddNewCardClick(cardName: string, cardId: number, isFoil: boolean): void {

    //     // let deckCard: DeckCardDto = {
    //     //     categoryId: null,
    //     //     deckId: this.props.deckId,
    //     //     cardName: cardName,
    //     //     id: 0,
    //     //     inventoryCardId: 0,
    //     //     cardId: cardId,
    //     //     isFoil: isFoil,
    //     //     inventoryCardStatusId: 1,
    //     // }
        
    //     // this.props.dispatch(requestAddDeckCard(deckCard));  
    // }

    // // //specify add mainboard vs sidebiard/maybeboard ?
    // handleAddEmptyCardClick(cardName: string): void {
    //     // let deckCard: DeckCardDto = {
    //     //     categoryId: null,
    //     //     deckId: this.props.deckId,
    //     //     cardName: cardName,
    //     //     id: 0,
    //     //     inventoryCardId: 0,
    //     //     cardId: 0,
    //     //     isFoil: false,
    //     //     inventoryCardStatusId: 0,
    //     // }
    //     // // console.log('ping!');
    //     // this.props.dispatch(requestAddDeckCard(deckCard));
    // }

    render(){
        return(
            <CardDetailLayout
                selectedCardId={this.props.selectedCardId}
                cardGroups={this.props.cardGroups}
                cardsById={this.props.cardsById}
                allCardIds={this.props.allCardIds}
                inventoryCardsById={this.props.inventoryCardsById}
                // handleAddExistingCardClick={this.handleAddExistingCardClick}
                // handleAddNewCardClick={this.handleAddNewCardClick}
                // handleAddEmptyCard={this.handleAddEmptyCardClick}
                
                // selectedCard={this.props.selectedCard}
                // selectedCardDetail={this.props.selectedCardDetail}
                />
        );
    }
}

// function selectInventoryDetail(state: AppState): InventoryDetailDto {
//     const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById, activeCardId } = state.decks.cardDetail.inventoryDetail;//state.cardSearch.data.inventoryDetail;
//     const result: InventoryDetailDto = {
//         cardId: activeCardId,
//         name: "",
//         cards: allCardIds.map(id => cardsById[id]),
//         inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
//     }
//     return result;
// }

// function selectSearchResults(state: AppState): CardSearchResultDto[] {
//     const { allSearchResultIds, searchResultsById } = state.decks.cardDetail.searchResults; //state.cardSearch.data.searchResults;
//     const result: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
//     return result;
// }



//old model, used to group in inv detail
interface InventoryDetailCardProps {
    card: MagicCard;
    inventoryCards: InventoryCard[];
}




function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //I'm going to need to map pending card totals to the inventory query result
    
    let mappedSearchResults: CardListItem[] = [];






    // mappedSearchResults = selectSearchResults(state).map(card => {

    //     //Apparently THIS IS BAD but I can't figure out a better approach right now
    //     //Clarification, .Find() is BAD
    //     //const cardExistsInDeck = state.data.deckDetail.cardOverviewsByName[card.name];
    //     const { cardOverviews } = state.decks.data.detail;
    //     const cardExistsInDeck = Boolean(cardOverviews.allIds.find(id => cardOverviews.byId[id].name === card.name));

    //     return ({
    //         data: card,
    //         count: cardExistsInDeck ? 1 : 0
    //     }) as CardListItem;
    // });

    const containerState = state.decks.cardDetail;

    const result: PropsFromState = {
        selectedCardId: ownProps.selectedCardId,
        cardGroups: containerState.cardGroups,
        cardsById: containerState.cards.byId,
        allCardIds: containerState.cards.allIds,
        inventoryCardsById: containerState.inventoryCards.byId,
        // cardSearchMethod: containerState.cardSearchMethod,
        // deckId: parseInt(ownProps.match.params.deckId) || 0,
        // selectedCard: containerState.selectedCard,
        // selectedCardDetail: selectInventoryDetail(state),
        // searchResults: mappedSearchResults,
        // viewMode: containerState.viewMode,
        // filterOptions: state.core.data.filterOptions,
        // searchFilterProps: containerState.searchFilterProps,
    }

    return result;
}

export default connect(mapStateToProps)(CardDetailContainer);