//////////////////////
//  State interfaces/
////////////////////

declare interface State {
    //actions need to be removed completely
    // actions: AppState;
    //core
    data: IDataStore;
    ui: IUIState;
    //components
    deckEditor: IDeckEditorState;
    cardSearch: ICardSearch;
    cardScoutCardSearch: ICardScoutCardSearch;
    cardInventory: ICardInventoryState;
}

declare interface ICardInventoryState {
    groupedCards: INamedCardArray[] | null;
}



//ui - activeView: inventory | deck | scout