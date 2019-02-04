//export function 
import React from 'react';
// import Magic, { CardFilter, Cards, Card } from 'mtgsdk-ts';
// import { Card, Set, CardFilter, SetFilter, PaginationFilter } from "./IMagic";

import Redux, { Store, Dispatch } from 'redux'

import Datastore from 'nedb'
// var db = require('diskdb')
// var fs = require('fs')
// import fs from 'fs'

// import {} from 'diskdb';


// export function generateDefaultDecks(): void {
// }

// function getAppState(): State {
//     let appState: State;

//     let something: State = (getState) => {

//         let state = getState();

//         return state;
//     }
// }

// function tryGetAppState(): any {
//     return (getState: any) => {
//         return getState();

        
//     }
// }


// function returnState(state: State) {
//     return state;
// }


const returnState = (state: State): State => {
    console.log('state is:');
    console.log(state)
    return state;
}

const tryGetAppState = (): any => {
    console.log('tryGetAppState init')
    return (getState: any) => {
        console.log('tryGetAppState 1')
        return returnState(getState());
    }
}

// export const fetchCardsIfNeeded = (): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         if(shouldFetchCards(getState())){
//             console.log('WE SHOULD FETCH A CARD');
//             return tryFetchCardDetail(dispatch, getState());
//         }
        
//     }
// }


// if(shouldFetchCards(getState())){
//     console.log('WE SHOULD FETCH A CARD');
//     return tryFetchCardDetail(dispatch, getState());
// }

export function lumberyardSaveState(state: State) {
    console.log('saving the app state?');
    console.log(state);

    // var Datastore = require('nedb'), db = new Datastore();

    // Type 2: Persistent datastore with manual loading
    // var Datastore = require('nedb'), db = new Datastore({ filename: 'path/to/datafile' });
    // var db = new Datastore({ filename: 'path/to/datafile' });
    var db = new Datastore({ filename: './persistedstate.db' });
    db.loadDatabase(function (err: any) {
            // Callback is optional
        // Now commands will be executed
        console.log('trying to save this state')
        // db.insert(state.ui);
        db.find({}, function (err: any, docs: any) {
            console.log('existing docs')
            console.log(docs);
        });
        

        //

        db.insert(state.ui, function (err, newDoc) {
            console.log('did anything happen?')
            console.log(newDoc)
            // Callback is optional
            // newDoc is the newly inserted document, including its _id
            // newDoc has no key called notToBeSaved since its value was undefined
          });
    });

    // var datastore = nedb, new Datastore()

    // db = db.connect('/',['store']);

    // console.log('maybe connected to a db?')

    // console.log(db);
}

export function trySaveState(): void {
    // let appState: State = getState();
    console.log('saving the app state?');
    // tryGetAppState();
    let currentState = tryGetAppState();

    let test = currentState();


    console.log('Current state is: ');
    // console.log(currentState);

    // console.log('what is the current state')

    // (getState: any) => {
    //     let appState: State = getState();
    //     // console.log('gonna call tryFetchCards')
    //     // return tryFetchCards(dispatch, getState());
    //     console.log('trying to save the state. Current state is: ');
    //     console.log(appState);

    //     db = db.connect('/',['store']);

    //     console.log('maybe connected to a db?')

    //     console.log(db);

    // }
}