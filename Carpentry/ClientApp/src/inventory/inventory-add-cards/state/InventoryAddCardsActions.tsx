import { Dispatch } from "redux";
import { cardSearchApi } from "../../../api/cardSearchApi";
import { inventoryApi } from "../../../api/inventoryApi";
import { AppState } from "../../../configureStore";

export const requestCardSearch = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCards(dispatch, getState());
    }   
}

export const CARD_SEARCH_REQUESTED = 'INVENTORY_ADD_CARDS.CARD_SEARCH_REQUESTED';
export const cardSearchRequested = (): ReduxAction => ({
    type: CARD_SEARCH_REQUESTED,
});

export const CARD_SEARCH_RECEIVED = 'INVENTORY_ADD_CARDS.CARD_SEARCH_RECEIVED'
export const cardSearchReceived = (results: CardSearchResultDto[]): ReduxAction => ({
    type: CARD_SEARCH_RECEIVED,
    payload: results,

});

function searchCards(dispatch: Dispatch, state: AppState): any{
    const containerState = state.inventory.inventoryAddCards;
    const searchInProgress: boolean = state.inventory.inventoryAddCards.searchResults.isLoading;
    if(searchInProgress){
        return;
    }
    dispatch(cardSearchRequested());
    //why TF am I treating this like a bool?
    if(containerState.cardSearchMethod === "web"){
        const { cardName, exclusiveName } = containerState.searchFilter;
        
        cardSearchApi.searchWeb(cardName, exclusiveName).then((results) =>{
            dispatch(cardSearchReceived(results));
        });
    // }else if(cardSearchMethod === "inventory"){

    //     const param: InventoryQueryParameter = {
    //         groupBy: "mid",
    //         text: state.ui.cardSearchFilterProps.text,
    //         colors: state.ui.cardSearchFilterProps.colorIdentity,
    //         types: [],
    //         skip: 0,
    //         take: 500,
    //         format: state.ui.cardSearchFilterProps.format,
    //         sort: '',
    //         set: state.ui.cardSearchFilterProps.set,
    //         // setId: state.ui.cardSearchFilterProps.setId,
    //         exclusiveColorFilters: state.ui.cardSearchFilterProps.exclusiveColorFilters,
    //         multiColorOnly: state.ui.cardSearchFilterProps.multiColorOnly,
    //         maxCount:0,
    //         minCount:0,
    //         type: state.ui.cardSearchFilterProps.type,
    //         rarity: state.ui.cardSearchFilterProps.rarity,
    //         sortDescending: false,
    //     }

    //     api.cardSearch.searchInventory(param).then((results) => {
    //         dispatch(apiDataReceived(_localApiScope, results));
    //     });

    } else {
        const currentFilterProps = containerState.searchFilter;
        //CardSearchQueryParameter
        const param: CardSearchQueryParameter = {
            colorIdentity: currentFilterProps.colorIdentity,
            exclusiveColorFilters: currentFilterProps.exclusiveColorFilters,
            multiColorOnly: currentFilterProps.multiColorOnly,
            rarity: currentFilterProps.rarity,
            // set: currentFilterProps.set,
            set: currentFilterProps.set,
            // setId: currentFilterProps.setId,
            type: currentFilterProps.type,
            searchGroup: currentFilterProps.group,
            excludeUnowned: false,
        }

        // console.log('serch by set')
        // console.log(param);

        cardSearchApi.searchInventory(param).then((results) => {
            dispatch(cardSearchReceived(results));
        })

        // api.cardSearch.searchSet(param).then((results) => {
        //     dispatch(apiDataReceived(_localApiScope, results));
        // })
    }
}

//Add pending cards
export const CARD_SEARCH_ADD_PENDING_CARD = 'CARD_SEARCH_ADD_PENDING_CARD'
//export const cardSearchAddPendingCard = (data: MagicCard, isFoil: boolean, variant: string) =>({
export const cardSearchAddPendingCard = (name: string, cardId: number, isFoil: boolean) =>({
    type: CARD_SEARCH_ADD_PENDING_CARD,
    payload: {
        name: name,
        cardId: cardId,
        isFoil: isFoil,
    }
});
//Remove pending cards
export const CARD_SEARCH_REMOVE_PENDING_CARD = 'CARD_SEARCH_REMOVE_PENDING_CARD'
export const cardSearchRemovePendingCard = (name: string, cardId: number, isFoil: boolean) =>({
    type: CARD_SEARCH_REMOVE_PENDING_CARD,
    payload: {
        name: name,
        cardId: cardId,
        isFoil: isFoil,
    }
});

export const CARD_SEARCH_FILTER_VALUE_CHANGED = 'CARD_SEARCH_FILTER_VALUE_CHANGED';
export const cardSearchFilterValueChanged = (type: 'inventoryFilterProps' | 'cardSearchFilterProps', filter: string, value: string | boolean): ReduxAction => ({
    type: CARD_SEARCH_FILTER_VALUE_CHANGED,
    payload: {
        type: type,
        filter: filter,
        value: value,
    }
});

export const TOGGLE_CARD_SEARCH_VIEW_MODE = 'TOGGLE_CARD_SEARCH_VIEW_MODE';
export const toggleCardSearchViewMode = (): ReduxAction => ({
    type: TOGGLE_CARD_SEARCH_VIEW_MODE
});


export const CARD_SEARCH_SEARCH_METHOD_CHANGED = 'CARD_SEARCH_SEARCH_METHOD_CHANGED';
export const cardSearchSearchMethodChanged = (method: string): ReduxAction => ({
    type: CARD_SEARCH_SEARCH_METHOD_CHANGED,
    payload: method
});

//clear pending cards
export const CARD_SEARCH_CLEAR_PENDING_CARDS = 'CARD_SEARCH_CLEAR_PENDING_CARDS'
export const cardSearchClearPendingCards = () =>({
    type: CARD_SEARCH_CLEAR_PENDING_CARDS
});

//Select search result (to see variant / foil options)
export const CARD_SEARCH_SELECT_CARD = 'CARD_SEARCH_SELECT_CARD';
export const cardSearchSelectCard = (card: CardSearchResultDto): ReduxAction => ({
    type: CARD_SEARCH_SELECT_CARD,
    payload: card
});


//TODO - maybe delete, this isn't referenced in container
// export const requestAddDeckCard = (deckCardDto: DeckCardDto): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         return addDeckCard(dispatch, getState(), deckCardDto);
//     }
// }

// function addDeckCard(dispatch: Dispatch, state: AppState, deckCardDto: DeckCardDto): any{
//     dispatch(cardSearchAddingDeckCard());

//     // alert('broken code hit - cardSearchActions - addDeckCard')

//     // const deckCardDto: DeckCardDto = {
//     //     deckId: state.app.core.selectedDeckId || 0,
//     //     //deckId: state.de,
//     //     id: 0,
//     //     inventoryCard: inventoryCard,
//     //     categoryId: null,
//     // }
//     //console.log

    
//     decksApi.addDeckCard(deckCardDto).then(() => {


        
//         //After response, need to re-request inventory

//         //!! This should be re-added eventually
//         // if(state.app.cardSearch.selectedCard){
//         //     dispatch(requestCardSearchInventory(state.app.cardSearch.selectedCard));
//         // }

//         // alert('broken code hit - CardSearchActions - addDeckCard (should be added, wont nav)');

//         // if(state.app.core.visibleContainer === "deckEditor" && state.data.deckDetail.deckProps != null){
            
//         //     //This should be a "navigate to deck detail"
//         //     // dispatch(requestDeckDetail(state.data.deckDetail.deckProps.id));
//         // }

//     })
// }

// export const CARD_SEARCH_ADDING_DECK_CARD = 'CARD_SEARCH_ADDING_DECK_CARD';
// export const cardSearchAddingDeckCard = (): ReduxAction => ({
//     type: CARD_SEARCH_ADDING_DECK_CARD
// });

//////
export const cardSearchRequestSavePendingCards = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        addCardsFromSearch(dispatch, getState());
    }
}

// export const requestAddCardsFromSearch = (): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         addCardsFromSearch(dispatch, getState());
//     }
// }
export const CARD_SEARCH_SAVE_PENDING_CARDS = 'CARD_SEARCH_SAVE_PENDING_CARDS';
export const cardSearchSavingPendingCards = (): ReduxAction => ({
    type: CARD_SEARCH_SAVE_PENDING_CARDS
});

function addCardsFromSearch(dispatch: Dispatch, state: AppState){
    const isSaving = state.inventory.inventoryAddCards.pendingCardsSaving;
    if(isSaving){
        return;
    }

    dispatch(cardSearchSavingPendingCards())

    // console.log('addingCards');
    // console.log(state.cardSearch.pendingCards);

    //cards to add from pending
    //var cardsToTryAdding = state.cardSearch.cardSearchPendingCards

    //for each of the keys in pending cards...

    let newCards: InventoryCard[] = [];

    //Object.keys(state.cardSearch.pendingCards).forEach((key: string) => {
    //Object.keys(state.data.cardSearch.pendingCards).forEach((key: string) => {
    Object.keys(state.inventory.inventoryAddCards.pendingCards).forEach((key: string) => {
        //need to rethink this

        //see if it exists in the current inventory

        let itemToAdd: PendingCardsDto = state.inventory.inventoryAddCards.pendingCards[key];

        itemToAdd.cards.forEach(card => {
            const newCard: InventoryCard = {
                id: 0,
                isFoil: card.isFoil,
                // multiverseId: card.multiverseId,
                statusId: card.statusId,
                // variantName: card.variantName,
                cardId: card.cardId,
                collectorNumber: card.collectorNumber,
                deckCards: [],
                name: card.name,
                set: card.set,
            }

            newCards.push(newCard);

        })
    });
    inventoryApi.addInventoryCardBatch(newCards).then(() => {
        // dispatch(inventoryAddComplete());
        // dispatch(requestInventoryOverviews());
        console.log('!! Need to route to inventory overviews !!'); // TODO - do routing to '/inventory/'
    });

}
