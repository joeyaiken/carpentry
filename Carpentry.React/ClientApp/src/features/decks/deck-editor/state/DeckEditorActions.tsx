// import { Dispatch } from 'redux';
import { push } from "connected-react-router";
import { Dispatch } from "redux";
import { decksApi } from "../../../api/decksApi";
import { AppState } from "../../../configureStore";
import { reloadDeckDetail, requestDeckOverviews } from "../../state/decksDataActions";

export const TOGGLE_DECK_VIEW_MODE = 'TOGGLE_DECK_VIEW_MODE';
export const toggleDeckViewMode = (): ReduxAction => ({
    type: TOGGLE_DECK_VIEW_MODE,
});

export const DECK_EDITOR_CARD_SELECTED = 'DECK_EDITOR_CARD_SELECTED';
export const deckEditorCardSelected = (cardOverview: DeckCardOverview): ReduxAction => ({
    type: DECK_EDITOR_CARD_SELECTED,
    payload: cardOverview
});

export const OPEN_DECK_PROPS_MODAL = 'OPEN_DECK_PROPS_MODAL';
export const openDeckPropsModal = (deckProps: DeckPropertiesDto | null): ReduxAction => ({
    type: OPEN_DECK_PROPS_MODAL,
    payload: deckProps,
});

export const CLOSE_DECK_PROPS_MODAL = 'CLOSE_DECK_PROPS_MODAL';
export const closeDeckPropsModal = (): ReduxAction => ({
    type: CLOSE_DECK_PROPS_MODAL,
});

export const requestSavePropsModal = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        trySaveDeckProps(dispatch, getState());
    }
}

export const DECK_EDITOR_SAVE_REQUESTED = 'DECK_EDITOR_SAVE_REQUESTED';
export const deckEditorSaveRequested = (): ReduxAction => ({
    type: DECK_EDITOR_SAVE_REQUESTED,
});

export const DECK_EDITOR_SAVE_RECEIVED = 'DECK_EDITOR_SAVE_RECEIVED';
export const deckEditorSaveReceived = (): ReduxAction => ({
    type: DECK_EDITOR_SAVE_RECEIVED,
});

// export const DECK_PROPS_SAVE_REQUESTED = 'DECK_PROPS_SAVE_REQUESTED';
// export const deckPropsSaveRequested = (): ReduxAction => ({
//     type: DECK_PROPS_SAVE_REQUESTED,
// });

// export const DECK_PROPS_SAVE_RECEIVED = 'DECK_PROPS_SAVE_RECEIVED';
// export const deckPropsSaveReceived = (): ReduxAction => ({
//     type: DECK_PROPS_SAVE_RECEIVED,
// });

function trySaveDeckProps(dispatch: Dispatch, state: AppState): void {
    var isSaving = state.decks.deckEditor.isSaving;
    const deckPropsToUpdate = state.decks.deckEditor.deckModalProps;

    if(isSaving || deckPropsToUpdate === null){
        return
    }
    
    dispatch(deckEditorSaveRequested());

    deckPropsToUpdate.basicW = +deckPropsToUpdate.basicW;
    deckPropsToUpdate.basicU = +deckPropsToUpdate.basicU;
    deckPropsToUpdate.basicB = +deckPropsToUpdate.basicB;
    deckPropsToUpdate.basicR = +deckPropsToUpdate.basicR;
    deckPropsToUpdate.basicG = +deckPropsToUpdate.basicG;

    decksApi.updateDeck(deckPropsToUpdate).then(() => {
        dispatch(deckEditorSaveReceived());
        dispatch(closeDeckPropsModal());
        dispatch(reloadDeckDetail(deckPropsToUpdate.id));
    });
}

export const DECK_PROPS_MODAL_CHANGED = 'DECK_PROPS_MODAL_CHANGED';
export const deckPropsModalChanged = (name: string, value: string | number): ReduxAction => ({
    type: DECK_PROPS_MODAL_CHANGED,
    payload: {
        name: name,
        value: value
    }
});

export const requestDisassembleDeck = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryDisassembleDeck(dispatch, getState());
    }
}
//Not going to implement this until I'm done removing the .ui and .legacy project
function tryDisassembleDeck(dispatch: Dispatch, state: AppState): void {
    //Not going to implement this until I'm done removing the .ui and .legacy project
    console.log('Prentending to disassemble deck');
    dispatch(closeDeckPropsModal());
    //is[Saving|Loading]Check?
    //dispatch
    //api
    //dispatch
}

export const requestDeleteDeck = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryDeleteDeck(dispatch, getState());
    }
}
function tryDeleteDeck(dispatch: Dispatch, state: AppState): void {
    const isSaving = state.decks.deckEditor.isSaving;
    if(isSaving) return;
    dispatch(deckEditorSaveRequested());
    const idToDelete = state.decks.data.detail.deckId;
    decksApi.deleteDeck(idToDelete).then(() => {
        dispatch(deckEditorSaveReceived());
        dispatch(push('/'));
        dispatch(requestDeckOverviews());
    });
}

export const CARD_MENU_BUTTON_CLICKED = 'CARD_MENU_BUTTON_CLICKED';
export const cardMenuButtonClicked = (cardMenuAnchor: HTMLElement | null): ReduxAction => ({
    type: CARD_MENU_BUTTON_CLICKED,
    payload: cardMenuAnchor
});


////////////
export const requestUpdateDeckCard = (detail: DeckCardDetail): any => {
    return (dispatch: Dispatch, getState: any) => {
        return updateDeckCard(dispatch, getState(), detail);
    }
}

//"status" should be "category" but w/e
function updateDeckCard(dispatch: Dispatch, state: AppState, cardDetail: DeckCardDetail): any {

    // const cardDetail = state.decks.data.detail.cardDetails.byId[deckCardId];


    // const something = state.data.deckDetail.cardDetailsById[deckCardId];

    // declare interface InventoryCard {
    //     id: number;
    //     multiverseId: number;
    //     name: string;
    //     set: string;
    //     isFoil: boolean;
    //     variantName: string;
    //     statusId: number; //normal === 1, buylist === 2, sellList === 3
    //     deckCards: InventoryDeckCardDto[];
    // }

    let categoryId: string | null = null;

    switch(cardDetail.category){
        case "mainboard":
        //     categoryId = null;
            break;
        case "sideboard":
            categoryId = 's';
            break;
        case "commander":
            categoryId = 'c';
            break;
    }


    let dto: DeckCardDto = {
        id: cardDetail.id,
        deckId: cardDetail.deckId,
        cardName: cardDetail.name,
        categoryId: categoryId,
        inventoryCardId: cardDetail.inventoryCardId,



        cardId: cardDetail.cardId ?? 0,
        isFoil: cardDetail.isFoil,
        inventoryCardStatusId: cardDetail.inventoryCardStatusId ?? 0,

        // inventoryCard: {
        //     deckCards: [],
        //     id: 0,
        //     isFoil: false,
        //     multiverseId: 0,
        //     name: "",
        //     set: "",
        //     statusId: 0,
        //     variantName: ""
        // },
        // //inventoryCard: something, 
        // deckId: state.data.deckDetail.deckId,

    }
    // console.log('submitting DTO');
    // console.log(dto);
    // console.log(state);


    decksApi.updateDeckCard(dto).then(() => {
        dispatch(reloadDeckDetail(dto.deckId));
    });
}

export const requestDeleteDeckCard = (deckCardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return deleteDeckCard(dispatch, getState(), deckCardId);
    }
}

function deleteDeckCard(dispatch: Dispatch, state: AppState, deckCardId: number): any {
    const currentDeckId = state.decks.data.detail.deckId;
    decksApi.removeDeckCard(deckCardId).then(() => {
        dispatch(reloadDeckDetail(currentDeckId));
    });
}