import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../configureStore';
import { 
    requestAddDeckCard, //TODO - This should be moved to the Deck Data Reducer
} from '../deck-add-cards/state/DeckAddCardsActions';
// import CardDetailLayout from './components/CardDetailLayout';
// import { deckCardMenuButtonClicked, ensureCardDetailLoaded, forceLoadCardDetail, inventoryCardMenuButtonClicked } from './state/CardDetailActions';
import CardMenu from '../deck-editor/components/CardMenu';
import { Menu, MenuItem } from '@material-ui/core';
import { requestDeleteDeckCard, requestUpdateDeckCard } from '../deck-editor/state/DeckEditorActions'; //This should ALSO be moved to the Deck Data Reducer
import { push } from 'react-router-redux';
import CardTagsLayout from './components/CardTagsLayout';
import { ensureTagDetailLoaded, newTagChange, requestAddCardTag } from './state/CardTagsActions';
// import { requestAddDeckCard } from '../deck-add-cards/state/DeckAddCardsActions';
// import { push } from 'react-router-redux';

interface PropsFromState { 
    // deckId: number;

    selectedDeckId: number;

    selectedCardId: number; //selected card [CardId]
    selectedCardName: string;

    newTagName: string;
    // cardsById: { [cardId: number]: MagicCard } //magic cards by CardId ?
    // allCardIds: number[]; //card definition IDs, will itterate over for bottom

    // inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
    // cardGroups: { [cardId: number]: number[] }

    // deckCardDetailsById: { [deckCardId: number]: DeckCardDetail };
    // activeDeckCardIds: number[];

    // deckCardMenuAnchor: HTMLButtonElement | null;
    // deckCardMenuAnchorId: number;

    // inventoryCardMenuAnchor: HTMLButtonElement | null;
    // inventoryCardMenuAnchorId: number;
}

interface OwnProps {
    selectedCardId: number;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardDetailContainer extends React.Component<ContainerProps>{
    constructor(props: ContainerProps) {
        super(props);
        this.handleAddTagButtonClick = this.handleAddTagButtonClick.bind(this);
        this.handleNewTagChange = this.handleNewTagChange.bind(this);
        // this.handleAddEmptyCardClick = this.handleAddEmptyCardClick.bind(this);
        // this.handleDeckCardMenuClick = this.handleDeckCardMenuClick.bind(this);
        // this.handleInventoryCardMenuClick = this.handleInventoryCardMenuClick.bind(this);
        // this.handleDeckCardMenuSelected = this.handleDeckCardMenuSelected.bind(this);
        // this.handleInventoryCardMenuSelected = this.handleInventoryCardMenuSelected.bind(this);
        // this.handleDeckCardMenuClose = this.handleDeckCardMenuClose.bind(this);
        // this.handleInventoryCardMenuClose = this.handleInventoryCardMenuClose.bind(this);
    }

    componentDidMount() {
        // this.props.dispatch(ensureCardDetailLoaded(this.props.selectedCardId));
        this.props.dispatch(ensureTagDetailLoaded(this.props.selectedCardId))
    }
  
    handleAddTagButtonClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        const dto: CardTagDto = {
            cardName: this.props.selectedCardName,
            deckId: this.props.selectedDeckId,
            tag: this.props.newTagName,
        }
        this.props.dispatch(requestAddCardTag(dto));
    }

    handleNewTagChange(newValue: string) {
        this.props.dispatch(newTagChange(newValue));
    }
    // //specify add mainboard vs sidebiard/maybeboard ?
    // handleAddEmptyCardClick(): void {
    //     const selectedCard = this.props.cardsById[this.props.selectedCardId];

    //     let deckCard: DeckCardDto = {
    //         categoryId: null,
    //         deckId: this.props.deckId,
    //         cardName: selectedCard.name,

    //         id: 0,
    //         inventoryCardId: 0,
    //         cardId: 0,
    //         isFoil: false,
    //         inventoryCardStatusId: 0,
    //     }
        
    //     this.props.dispatch(requestAddDeckCard(deckCard));
    // }

    // handleDeckCardMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
    //     this.props.dispatch(deckCardMenuButtonClicked(event.currentTarget));
    // }

    // handleInventoryCardMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
    //     this.props.dispatch(inventoryCardMenuButtonClicked(event.currentTarget));
    // }

    // handleDeckCardMenuSelected(name: DeckEditorCardMenuOption){

    //     const deckCardDetail = this.props.deckCardDetailsById[this.props.deckCardMenuAnchorId];

    //     switch(name){
    //         case "sideboard":
    //             deckCardDetail.category = name;
    //             this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
    //             break;
    //         case "mainboard":
    //             deckCardDetail.category = "";
    //             this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
    //             break;
    //         case "commander":
    //             deckCardDetail.category = name;
    //             this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
    //             break;
    //         case "inventory":
    //             deckCardDetail.inventoryCardId = null;
    //             this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
    //             break;
    //         case "delete":
    //             const confirmText = `Are you sure you want to remove ${this.props.deckCardMenuAnchor?.name} from the deck?`;
    //             if(window.confirm(confirmText)){
    //                 deckCardDetail.inventoryCardId = null;
    //                 this.props.dispatch(requestDeleteDeckCard(deckCardDetail.id));
    //             }
    //             break;
    //     }

    //     this.props.dispatch(deckCardMenuButtonClicked(null));
    // }

    // handleInventoryCardMenuSelected(name: InventoryCardMenuOption){
    //     //inventoryCardMenuAnchorId
    //     const inventoryCard = this.props.inventoryCardsById[this.props.inventoryCardMenuAnchorId];
    //     switch(name){
    //         case "add": 

    //             //is there an empty card in the deck that can be filled?
    //             var firstEmptyId = this.props.activeDeckCardIds.find(id => !Boolean(this.props.deckCardDetailsById[id].inventoryCardId));

    //             if(firstEmptyId){
    //                 //If so, update it
    //                 var thisDeckCard = this.props.deckCardDetailsById[firstEmptyId];
    //                 thisDeckCard.inventoryCardId = inventoryCard.id;
    //                 this.props.dispatch(requestUpdateDeckCard(thisDeckCard));

    //             } else {
    //                 //if not, a new should be added
                    
    //                 let newDeckCard: DeckCardDto = {
    //                     id: 0,
    //                     deckId: this.props.deckId,
    //                     cardName: inventoryCard.name,
    //                     categoryId: null,
    //                     inventoryCardId: inventoryCard.id,

    //                     cardId: inventoryCard.cardId,
    //                     isFoil: inventoryCard.isFoil,
    //                     inventoryCardStatusId: 1,
    //                 }

    //                 this.props.dispatch(requestAddDeckCard(newDeckCard));
    //                 this.props.dispatch(forceLoadCardDetail(this.props.selectedCardId));
    //             }
    //             break;
    //         case "remove": 
    //             // const confirmText = `Are you sure you want to remove ${this.props.deckCardMenuAnchor?.name} from the deck?`;
    //             // if(window.confirm(confirmText)){
    //             //     deckCardDetail.inventoryCardId = null;
    //             //     this.props.dispatch(requestDeleteDeckCard(deckCardDetail.id));
    //             // }
    //             break;
    //         case "view": 
    //             this.props.dispatch(push(`/decks/${inventoryCard.deckId}?cardId=${inventoryCard.cardId}`));
    //             break;
    //     }

    //     // // console.log('card anchor');
    //     // // console.log(this.props.cardMenuAnchor);
    //     // switch (name){
    //     //     // case "search":
    //     //     //     if(this.props.cardMenuAnchor != null){
    //     //     //         // this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
    //     //     //     }
    //     //     //     break;
    //     //     // case "delete":
    //     //     //         if(this.props.cardMenuAnchor != null){
    //     //     //             const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
    //     //     //             if(window.confirm(confirmText)){
    //     //     //                 // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
    //     //     //             }
    //     //     //         }
    //     //     //         break;
    //     //     case "sideboard":
    //     //         if(this.props.cardMenuAnchor != null){
    //     //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "sideboard"));
    //     //         }
    //     //         break;
    //     //     case "mainboard":
    //     //         if(this.props.cardMenuAnchor != null){
    //     //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "mainboard"));
    //     //         }
    //     //         break;
    //     //     case "commander":
    //     //         if(this.props.cardMenuAnchor != null){
    //     //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "commander"));
    //     //         }
    //     //         break;
    //     // }

    //     this.props.dispatch(inventoryCardMenuButtonClicked(null));
    // }

    // handleDeckCardMenuClose(): void {
    //     this.props.dispatch(deckCardMenuButtonClicked(null));
    // }

    // handleInventoryCardMenuClose(): void {
    //     this.props.dispatch(inventoryCardMenuButtonClicked(null));
    // }

    

    render(){
        //does the layout really need to controll things like the modal? I may have lost sense of things in refactoring
        //TODO - consider wrapping this in a React.Fragment or something, and pulling out the card menu

        //selected deck card category id
        // const categoryId = getCategoryId(this.props.deckCardDetailsById[this.props.deckCardMenuAnchorId]?.category);

        // //selected inventory card deck id
        // const invCardDeckId = this.props.inventoryCardsById[this.props.inventoryCardMenuAnchorId]?.deckId;
        

        return(
            <React.Fragment>
                {/* <CardMenu 
                    cardMenuAnchor={this.props.deckCardMenuAnchor} 
                    onCardMenuSelect={this.handleDeckCardMenuSelected} 
                    onCardMenuClose={this.handleDeckCardMenuClose} 
                    cardCategoryId={categoryId}
                    hasInventoryCard={Boolean(this.props.deckCardDetailsById[this.props.deckCardMenuAnchorId]?.inventoryCardId)}
                    />
                <React.Fragment>
            <Menu open={Boolean(this.props.inventoryCardMenuAnchor)} onClose={this.handleInventoryCardMenuClose} anchorEl={this.props.inventoryCardMenuAnchor} >

                { //Only show if not in a deck
                    !invCardDeckId &&
                    <MenuItem onClick={() => { this.handleInventoryCardMenuSelected("add") }} value="">Add to Deck</MenuItem> }
                
                { //Only show if in this deck
                    invCardDeckId &&  invCardDeckId === this.props.deckId &&
                    <MenuItem onClick={() => { this.handleInventoryCardMenuSelected("remove") }} value="">Remove from Deck</MenuItem> }
                
                { //Only show if in another deck
                    invCardDeckId && invCardDeckId !== this.props.deckId &&
                    <MenuItem onClick={() => { this.handleInventoryCardMenuSelected("view") }} value="">View Deck</MenuItem> }

            </Menu>
        </React.Fragment>
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

                onDeckCardMenuClick={this.handleDeckCardMenuClick}
                onInventoryCardMenuClick={this.handleInventoryCardMenuClick}
                />
                 */}

                 <CardTagsLayout
                    newTagName={this.props.newTagName}
                    selectedCardName={this.props.selectedCardName}
                    onAddTagButtonClick={this.handleAddTagButtonClick}
                    onNewTagChange={this.handleNewTagChange} />
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

function getCategoryId(category: string): string {
    switch(category){
        case "Sideboard": return 's';
        case "Commander": return 'c'
        default: return '';
    }
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    //I need to get all deck card details for (the name mapping to this ID)

    // var matchingIds = state.decks.data.detail.cardDetails.allIds.filter(id => {
    //     const card = state.decks.data.detail.cardDetails.byId[id];
    //     return (card.name === state.decks.cardDetail.activeCardName);
    // });




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

    const containerState = state.decks.cardTags;

    const result: PropsFromState = {
        // deckId: state.decks.data.detail.deckId,

        selectedDeckId: state.decks.data.detail.deckId,
        selectedCardId: ownProps.selectedCardId,
        selectedCardName: containerState.cardName,
        newTagName: containerState.newTagName,
        // cardGroups: containerState.cardGroups,
        // cardsById: containerState.cards.byId,
        // allCardIds: containerState.cards.allIds,
        // inventoryCardsById: containerState.inventoryCards.byId,

        // deckCardDetailsById: state.decks.data.detail.cardDetails.byId,
        // activeDeckCardIds: matchingIds,

        // deckCardMenuAnchor: containerState.deckCardMenuAnchor,
        // deckCardMenuAnchorId: containerState.deckCardMenuAnchorId,

        // inventoryCardMenuAnchor: containerState.inventoryCardMenuAnchor,
        // inventoryCardMenuAnchorId: containerState.inventoryCardMenuAnchorId,
        // // cardSearchMethod: containerState.cardSearchMethod,
        // // deckId: parseInt(ownProps.match.params.deckId) || 0,
        // // selectedCard: containerState.selectedCard,
        // // selectedCardDetail: selectInventoryDetail(state),
        // // searchResults: mappedSearchResults,
        // // viewMode: containerState.viewMode,
        // // filterOptions: state.core.data.filterOptions,
        // // searchFilterProps: containerState.searchFilterProps,
    }

    return result;
}

export default connect(mapStateToProps)(CardDetailContainer);