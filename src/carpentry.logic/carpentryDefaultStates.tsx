
///////////////////////////////
//default instance generators//
///////////////////////////////

//This class makes empty state instances

export class carpentryDefaultStates {
    // static cardInventory = (): any
    static cardInventory = (): ICardInventoryState => {
        const initialState: ICardInventoryState = {
            groupedCards: null
            // requestedCards: [],
            // searchFilter: {
            //     name: '',
            //     setFilterString: '',
            //     includeRed: false,
            //     includeBlue: false,
            //     includeGreen: false,
            //     includeWhite: false,
            //     includeBlack: false,
            //     colorIdentity: '',
            //     results: []
            // },
            // searchIsFetching: false
        }
        return initialState;
    }


    
}