import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../configureStore';
import { 
//     cardSearchFilterValueChanged, 
//     toggleCardSearchViewMode, 
//     // cardSearchSearchMethodChanged, 
    requestAddDeckCard, 
//     cardSearchSelectCard, 
    // requestCardSearch, 
//     requestCardSearchInventory
} from '../deck-add-cards/state/DeckAddCardsActions';
import CardDetailLayout from './components/CardDetailLayout';
import { ensureCardDetailLoaded } from './state/CardDetailActions';
import CardMenu from '../deck-editor/components/CardMenu';
// import { requestAddDeckCard } from '../deck-add-cards/state/DeckAddCardsActions';
// import { push } from 'react-router-redux';

interface PropsFromState { 
    deckId: number;

    selectedCardId: number; //selected card [CardId]
    cardsById: { [cardId: number]: MagicCard } //magic cards by CardId ?
    allCardIds: number[]; //card definition IDs, will itterate over for bottom

    inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
    cardGroups: { [cardId: number]: number[] }

    deckCardDetailsById: { [deckCardId: number]: DeckCardDetail };
    activeDeckCardIds: number[];
}

interface OwnProps {
    selectedCardId: number;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardDetailContainer extends React.Component<ContainerProps>{
    constructor(props: ContainerProps) {
        super(props);
        this.handleAddEmptyCardClick = this.handleAddEmptyCardClick.bind(this);
        this.handleDeckCardMenuClick = this.handleDeckCardMenuClick.bind(this);
        this.handleInventoryCardMenuClick = this.handleInventoryCardMenuClick.bind(this);
        this.handleCardMenuSelected = this.handleCardMenuSelected.bind(this);
        this.handleCardMenuClose = this.handleCardMenuClose.bind(this);
    }

    componentDidMount() {
        this.props.dispatch(ensureCardDetailLoaded(this.props.selectedCardId));
    }
  
    // //specify add mainboard vs sidebiard/maybeboard ?
    handleAddEmptyCardClick(): void {
        const selectedCard = this.props.cardsById[this.props.selectedCardId];

        
        let deckCard: DeckCardDto = {
            categoryId: null,
            deckId: this.props.deckId,
            cardName: selectedCard.name,

            id: 0,
            inventoryCardId: 0,
            cardId: 0,
            isFoil: false,
            inventoryCardStatusId: 0,
        }
        console.log('ping!');
        this.props.dispatch(requestAddDeckCard(deckCard));
    }

    handleDeckCardMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        // this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", event.currentTarget));
        // this.props.dispatch(cardMenuButtonClicked(event.currentTarget));
    }


    handleInventoryCardMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        // this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", event.currentTarget));
        // this.props.dispatch(cardMenuButtonClicked(event.currentTarget));
    }

    handleCardMenuSelected(name: DeckEditorCardMenuOption){
        // // console.log('card anchor');
        // // console.log(this.props.cardMenuAnchor);
        // switch (name){
        //     // case "search":
        //     //     if(this.props.cardMenuAnchor != null){
        //     //         // this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
        //     //     }
        //     //     break;
        //     // case "delete":
        //     //         if(this.props.cardMenuAnchor != null){
        //     //             const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
        //     //             if(window.confirm(confirmText)){
        //     //                 // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
        //     //             }
        //     //         }
        //     //         break;
        //     case "sideboard":
        //         if(this.props.cardMenuAnchor != null){
        //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "sideboard"));
        //         }
        //         break;
        //     case "mainboard":
        //         if(this.props.cardMenuAnchor != null){
        //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "mainboard"));
        //         }
        //         break;
        //     case "commander":
        //         if(this.props.cardMenuAnchor != null){
        //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "commander"));
        //         }
        //         break;
        // }

        // this.props.dispatch(cardMenuButtonClicked(null));
    }

    handleCardMenuClose(): void {
        // this.props.dispatch(cardMenuButtonClicked(null));
        // this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", null));
    }

    render(){
        //does the layout really need to controll things like the modal? I may have lost sense of things in refactoring
        //TODO - consider wrapping this in a React.Fragment or something, and pulling out the card menu
        return(
            <React.Fragment>
                <CardMenu 
                    cardMenuAnchor={null} 
                    onCardMenuSelect={() => {}} 
                    onCardMenuClose={() => {}} 
                    cardCategoryId={''}
                    hasInventoryCard={false}
                    // cardCategoryId={props.selectedCard?.category || ''}
                    // hasInventoryCard={Boolean(props.cardDetailsById[props.cardMenuAnchorId]?.inventoryCardId)}
                    />
                
            <CardDetailLayout
                selectedCardId={this.props.selectedCardId}
                cardGroups={this.props.cardGroups}
                cardsById={this.props.cardsById}
                allCardIds={this.props.allCardIds}
                inventoryCardsById={this.props.inventoryCardsById}
                // handleAddExistingCardClick={this.handleAddExistingCardClick}
                // handleAddNewCardClick={this.handleAddNewCardClick}
                onAddEmptyCardClick={this.handleAddEmptyCardClick}
                
                // selectedCard={this.props.selectedCard}
                // selectedCardDetail={this.props.selectedCardDetail}

                deckCardDetailsById={this.props.deckCardDetailsById}
                activeDeckCardIds={this.props.activeDeckCardIds}

                
                // menuAnchor={null}
                // menuAnchorId={0}
                // onCardMenuSelected={() => {}}
                onDeckCardMenuClick={() => {}}
                onInventoryCardMenuClick={() => {}}
                // onCardMenuClosed={() => {}}
                />
                
            </React.Fragment>
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


function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //I need to get all deck card details for (the name mapping to this ID)

    var matchingIds = state.decks.data.detail.cardDetails.allIds.filter(id => {
        const card = state.decks.data.detail.cardDetails.byId[id];
        return (card.name === state.decks.cardDetail.activeCardName);
    });




    //I'm going to need to map pending card totals to the inventory query result    
    // let mappedSearchResults: CardListItem[] = [];

    // mappedSearchResults = selectSearchResults(state).map(card => {

    //     //Apparently THIS IS BAD but I can't figure out a better approach right now
    //     //Clarification, .Find() is BAD (should have been a dict)
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
        deckId: state.decks.data.detail.deckId,

        selectedCardId: ownProps.selectedCardId,
        cardGroups: containerState.cardGroups,
        cardsById: containerState.cards.byId,
        allCardIds: containerState.cards.allIds,
        inventoryCardsById: containerState.inventoryCards.byId,

        deckCardDetailsById: state.decks.data.detail.cardDetails.byId,
        activeDeckCardIds: matchingIds,
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