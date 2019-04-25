import React from 'react';
import Magic, { CardFilter, Cards, Card } from 'mtgsdk-ts';
// import { Card, Set, CardFilter, SetFilter, PaginationFilter } from "./IMagic";

import Redux, { Store, Dispatch } from 'redux'
import { stat } from 'fs';
import { unmountComponentAtNode } from 'react-dom';
import { string } from 'prop-types';
import { Lumberjack } from '../../carpentry.logic/lumberjack';
import { mapCardToICard } from '../../carpentry.data/lumberyard';
// import { lumberyardSaveState } from '../data/lumberyard'

//mtgApiRequestSearch




// export const SCOUT_SEARCH_FILTER_CHANGE = "SCOUT_SEARCH_FILTER_CHANGE";

// export const scoutSearchFilterChanged = (property: string, value: string): ReduxAction => ({
//     type: SCOUT_SEARCH_FILTER_CHANGE,
//     payload: {
//         property: property,
//         value: value
//     }
// });

// export const SCOUT_SEARCH_APPLIED = "SCOUT_SEARCH_APPLIED";

// export const scoutSearchApplied = (): ReduxAction => ({
//     type: SCOUT_SEARCH_APPLIED
// });




export const AP_INITIALIZED = 'AP_INITIALIZED';
export const apInitialized = (): ReduxAction => ({
    type: AP_INITIALIZED
});





// export const csSearchRequested = (setCode: string): ReduxAction => ({
//     type: AP_SET_SELECTED,
//     payload: setCode
// });


export function requestCardList(): any {
    return (dispatch: Dispatch, getState: any) => {
        return tryGetSetCards(dispatch, getState());
    }
}

function tryGetSetCards(dispatch: Dispatch, state: State): any{
    //fire off an action that returns the ReduxAction apSetSelected would normally return
    // dispatch(csSearchRequested())
    //csSearchRequested()    


    //if no search is in progress, request a new search
    // if(!state.addPack.searchIsFetching){
    //     fetchCards(dispatch, state.addPack.);
    // }

}

//This is supposed to return a function that takes in whatever TF Dispatch is
function fetchCards(dispatch: Dispatch, filter: SearchFilterProps): any {
    
    // dispatch(requestCardSearch());
    let cardFilter: CardFilter = {};
    cardFilter.name = filter.name;
    //parse sets
    const parsedSetList: string[] = filter.setFilterString.split(',');
    // if(parsedSetList && parsedSetList.length){
    //     cardFilter.set 
    // }
    cardFilter.set = filter.setFilterString;
    cardFilter.colorIdentity = filter.colorIdentity;
    // const parsedColorIdentity: string[] = [];
    // if(filter.includeRed) parsedColorIdentity.push('R');
    // if(filter.includeBlue) parsedColorIdentity.push('U');
    // if(filter.includeGreen) parsedColorIdentity.push('G');
    // if(filter.includeWhite) parsedColorIdentity.push('W');
    // if(filter.includeBlack) parsedColorIdentity.push('B');

    // cardFilter.colors
    console.log('about to search cards')
    console.log('parsed sets')
    console.log(parsedSetList);
    console.log('parsed color identity')
    // console.log(parsedColorIdentity)
    // cardFilter.colorIdentity = parsedColorIdentity.join('|')
    // console.log()

    // cardFilter.colorIdentity = "RW"

    // let cardFilter: CardFilter = {
    //     //name: "Nicol"
    //     name: filter.name
    // }   
    return Cards.where(cardFilter).then(results => {
        console.log('grand result dump');
        console.log(results);
        // return dispatch(receiveCardSearch(results));
    });
}

export const AP_SET_SELECTED = 'AP_SET_SELECTED';
export const apSetSelected = (setCode: string): ReduxAction => {
    return {
        type: AP_SET_SELECTED,
        payload: setCode
    }
};

export const AP_CLEAR_SELECTED_SET = 'AP_CLEAR_SELECTED_SET';
export const apClearSelectedSet = (): ReduxAction => {
    return {
        type: AP_CLEAR_SELECTED_SET
    }
}


//Container call action
export const apLoadSelectedSet = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return tryLoadSelectedSet(dispatch, getState());
    }
}
//start action
export const AP_LOAD_SET_STARTED = 'AP_LOAD_SET_STARTED';
export const apLoadSetStarted = (): ReduxAction => ({
    type: AP_LOAD_SET_STARTED
});
//end action
export const AP_LOAD_SET_COMPLETE = 'AP_LOAD_SET_COMPLETE';
export const apLoadSetComplete = (cards: ICardDictionary | null): ReduxAction => ({
    type: AP_LOAD_SET_COMPLETE,
    payload: cards
});
//emitter load action
export const AP_CARD_LOADED = 'AP_CARD_LOADED';
export const apCardLoaded = (card: ICard): ReduxAction => ({
    type: AP_CARD_LOADED,
    payload: card
});

function tryLoadSelectedSet(dispatch: Dispatch, state: State): any {
    if(!state.addPack.isLoadingSet && state.addPack.selectedSetCode){
        
        const selectedSetCode: string = state.addPack.selectedSetCode;

        //load start
        dispatch(apLoadSetStarted());

        //try to load from index (& cache?)
        //lumberjack.FindSetFromTheIndex() 
        const setFromIndex = Lumberjack.getAllCardsForSet(selectedSetCode);
        
        //try to load from API
        if(setFromIndex){
            //load end
            dispatch(apLoadSetComplete(setFromIndex));
        } else {
            const cachedCards = state.addPack.apiCache[selectedSetCode];
            if(cachedCards){
                dispatch(apLoadSetComplete(cachedCards));
            }
            else{
                let cardFilter: CardFilter = {};
                cardFilter.set = selectedSetCode;
                // cardFilter.pageSize = 100;

                //is it in that state?


                Cards.all(cardFilter).on("data", (card: Card) => {
                    dispatch(apCardLoaded(mapCardToICard(card)))
                }).on("end", () => {
                    console.log("done");
                    //console.log(state.addPack.apiCache[selectedSetCode]);
                    //load end
                    dispatch(apLoadSetComplete(null));
                });
            }
        }
    }
}
// function fetchCardsForSet(dispatch: Dispatch, setCode: string): any {
    
//     dispatch(mtgApiSearechRequested());

//     //dispatch(requestCardSearch());
//     let cardFilter: CardFilter = {};
//     cardFilter.set = setCode;
//     cardFilter.pageSize = 100;
    
//     return Cards.all(cardFilter)
//     .on("data", (card: Card) => {
//         // console.log(card.name);
//         return dispatch(mtgApiSearchItemReturned(mapCardToICard(card)));
//     })
//     .on("cancel", () => {
//         console.log("cancel");
//     })
//     .on("error", (err: Error) => {
//         console.log("error: "+err);
//     })
//     .on("end", () => {
//         console.log("done");
//         return dispatch(mtgApiSearechCompleted());
//     });
// }

// function tryFetchCardsForAddPack(dispatch: Dispatch, state: State) {
//     // console.log('state');
//     // console.log(state)
//     // console.log(state.mtgApiSearch)
//     if(!state.mtgApiSearch.searchInProgress){
//         //fetchCards(dispatch, state.mgtApiSearch.searchFilter);
//         fetchCardsForAddPack(dispatch, state.addPack.selectedSetCode);
//     }
// }

// function fetchCardsForAddPack(dispatch: Dispatch, setCode: string) {


// }
//function 


// function tryGetSetCards(): any {
//     return (dispatch: Dispatch, getState: any) => {
//         return _tryGetSetCards(dispatch, getState());
//     }
// }

//

// export const CS_FILTER_CHANGED = 'CS_FILTER_CHANGED';
// export const csFilterChanged = (): ReduxAction => ({
//     type: CS_FILTER_CHANGED
//     //payload
// });

// export const CS_SEARCH_APPLIED = 'CS_SEARCH_APPLIED';
// export const csSearchApplied = (): ReduxAction => ({
//     type: CS_SEARCH_APPLIED
// });

// export const CS_CARD_SELECTED = 'CS_CARD_SELECTED';
// export const csCardSelected = (): ReduxAction => ({
//     type: CS_CARD_SELECTED
//     //payload
// });

// export const CS_ACTION_APPLIED = 'CS_ACTION_APPLIED';
// export const csActionApplied = (): ReduxAction => ({
//     type: CS_ACTION_APPLIED
//     //payload
// });