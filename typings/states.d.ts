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
    addPack: IAddPackState;
    deckEditor: IDeckEditorState;
    cardSearch: ICardSearch;
    cardScoutCardSearch: ICardScoutCardSearch;
    cardInventory: ICardInventoryState;
    mtgApiSearch: IMtgApiSearchState;
}

declare interface ICardInventoryState {
    groupedCards: INamedCardArray[] | null;
}


declare interface IAddPackState {
    selectedSetCode: string | null;
    visibleSetFilters: ICardSet[] | null;
    isLoadingSet: boolean;
    groupedCards: INamedCardArray[] | null;
    
    //pendingCards: ICard[] | null;
    //pending cards is a dictionary of ints that represents the number of cards in a set that should eventually be added to the inventory
    pendingCards: IntDictionary;

    //apiCache  //needs to be a dictionary of dictionaries 
    // dict { [key: setName] : {[key: cardName] : ICard  }   }
    apiCache: ICardIndex;
    
    
}

declare interface IMtgApiSearchState {
    searchInProgress: boolean;
    searchResults: ICard[];
}

//ui - activeView: inventory | deck | scout