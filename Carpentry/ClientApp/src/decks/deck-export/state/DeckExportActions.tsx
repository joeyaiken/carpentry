import { Dispatch } from "redux";
import { decksApi } from "../../../api/decksApi";
import { RootState } from "../../../configureStore";

export const OPEN_EXPORT_DIALOG = 'DECK_EXPORT.OPEN_EXPORT_DIALOG';
export const openExportDialog = (): ReduxAction => ({
    type: OPEN_EXPORT_DIALOG,
});

export const CLOSE_DECK_DIALOG = 'DECK_EXPORT.CLOSE_DECK_DIALOG';
export const closeExportDialog = (): ReduxAction => ({
    type: CLOSE_DECK_DIALOG,
});

export const EXPORT_TYPE_CHANGED = 'DECK_EXPORT.EXPORT_TYPE_CHANGED';
export const exportTypeChanged = (exportType: string): ReduxAction => ({
    type: EXPORT_TYPE_CHANGED,
    payload: exportType,
});

export const requestDeckExport = (deckId: number, exportType: DeckExportType): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryGetDeckExport(dispatch, getState(), deckId, exportType);
    }
}

function tryGetDeckExport(dispatch: Dispatch, state: RootState, deckId: number, exportType: DeckExportType): void {
    var isLoading = state.decks.deckExport.isLoading;
    if(isLoading) return;
    
    dispatch(deckExportRequested());

    decksApi.exportDeckList(deckId, exportType).then((payload) => {
        dispatch(deckExportReceived(payload));
    });
}

export const DECK_EXPORT_REQUESTED = 'DECK_EXPORT.DECK_EXPORT_REQUESTED';
export const deckExportRequested = (): ReduxAction => ({
    type: DECK_EXPORT_REQUESTED,
});

export const DECK_EXPORT_RECEIVED = 'DECK_EXPORT.DECK_EXPORT_RECEIVED';
export const deckExportReceived = (resultPayload: string): ReduxAction => ({
    type: DECK_EXPORT_RECEIVED,
    payload: resultPayload,
});