import DeckList from "../components/DeckList";
import DeckDetail from "../components/DeckDetail";
import { stat } from "fs";
import { Card } from 'mtgsdk-ts'


const CACHED_UI_STATE_IDENTIFIER = 'CARPENTRY_CACHED_UI_STATE_IDENTIFIER';


// import React from 'react';


//So I don't really know WHERE I should be loading the default states, but here are the methods that will be called!
export function loadInitialUIState(): IUIState {
    
    //try to cache a loaded UI state?
    //What do we REALLY want to cache besides selected deck ID?
    //Would it hurt to cache everything and only apply certain settings?
    const cachedUIState = loadUIStateCache();

    const initialUIState: IUIState = loadUIStateCache() || {
        isNavOpen: false,
        isSideSheetOpen: false,
        // selectedDeckId: 0,
        selectedDeckId: (cachedUIState && cachedUIState.selectedDeckId) || 0,
        visibleSideSheet: ''
    }

    return initialUIState;
}

export function loadInitialDeckEditorState(): IDeckEditorState {

    const initialDeckEditorState: IDeckEditorState = {
        deckView: 'card',
        deckGroup: 'none',
        deckSort: 'name',
        deckFilter: '',
        activeDeckVisibleCards: [],
        sectionVisibilities: [true,true,true,true,true,true],
        selectedCard: null
    }

    return initialDeckEditorState;

}

export function loadInitialCardSearchState(): ICardSearch {

    let initialCardSearchState: ICardSearch = {
        requestedCards: [],
        searchFilter: {
            name: '',
            results: []
        },
        searchIsFetching: false
    }

    return initialCardSearchState;
}

// export function loadInitialGeneralAppState(): AppState {
//     const initialAppState = tryLoadData();
//     // const initialAppState: AppState = {

//     // }
//     return initialAppState;
// }

export function loadInitialDataStore(): IDataStore {

    // const cachedData: string|null = localStorage.getItem('deck-cache');
    const cachedIndexData: string|null = localStorage.getItem('card-index-cache');

    //const cachedData: string | null = '[{"id":0,"name":"Green/White Exalted","description":"Big tokens","cards":["7a1287974bf5f3bb619d53752c6ec698de891f37","7a1287974bf5f3bb619d53752c6ec698de891f37","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","c5c76762e46f29b20df8ac59f3800e34ca5fb727","2948a004e967dd228734b74377105a7a3bf39732","2948a004e967dd228734b74377105a7a3bf39732","869ee41a0d1867fdd3a069a056742909e96d5470","869ee41a0d1867fdd3a069a056742909e96d5470","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","a1def1c22021107451f84d7007ff2405f6406c5c","a1def1c22021107451f84d7007ff2405f6406c5c","4b956d115e5fc9f2bd68b64b5e5f3ca9622d1134","4b956d115e5fc9f2bd68b64b5e5f3ca9622d1134","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","f698e1257973400d06ee2423aa6d80159f22a776","9e8e4997e84311ce2371eb15753e45ed6c2c93c1","089692ce590f004172e86c984ba478eed479d6f4","670d0364f5b9bd40f70ff682ca2ab82e3411bc0d","d242e7f3187da8b254e0caa5936e58cfbb961a9b","d242e7f3187da8b254e0caa5936e58cfbb961a9b","bc68e621a65bef9eed328af437ca72755f8c8825","bc68e621a65bef9eed328af437ca72755f8c8825","80030d4aaaff044fb5be7dbc03b418b9d6af5aff","80030d4aaaff044fb5be7dbc03b418b9d6af5aff","3ffc7ba3e5163791f5ef3c599f1d750a345ae4db","4e0590d622a5076a748647c7692b0c0fb56ed3de","5c74c58794ef0de94b12fdc90ce32cd777d3c4d3","094b47ac41744e30f456fad8f2ea025be1507cf0","0d01ef93a63150628b80d5745ea34b30c30e8eb5","ae41c5bd1f4a97149e222fe1f49d32aea223ea33","f9ba80fc8aff2670208afaadb1934cd8d25c554c"],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":8,"W":8,"B":0}},{"id":1,"name":"B/R/Bu Bolas","description":"","cards":["e5a85451def764645578a9628ba060814fc88794","c422203e03ef9c8e7ef3118980f3c0e2e42bc525","fd34bd9ae0b4545f6f268c7f3af5b2dec35aec65","fd34bd9ae0b4545f6f268c7f3af5b2dec35aec65","a279c34c4f07792e423cb0286191e80358c9174e","a279c34c4f07792e423cb0286191e80358c9174e","9e9d6d78ed80dd8f092ef373cb034f8efd034a9b","9e9d6d78ed80dd8f092ef373cb034f8efd034a9b","542ea64fcac69334e3870f4492796db6d1fd265f","542ea64fcac69334e3870f4492796db6d1fd265f","eccf98d80c08c3a06727565f1b1d834e4d6c8627","8a20ca25c580a3c123119dcef12ee03add1c291f","054092a64d39584c8227f064d6cab0ad99d9aafe","592a576deeb5254e598d4347f37215c3cb4a0d98","bef35e93bfa3506ef548786f772d30cd6fd7c180","e522e844b7d341f2c166ef4d3593ff21eec01dd4","cbae24fe2ce943cc7fa98f87931868f90adcb4db","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","d5872ccbcc68c5c2c3ea3be02d92d8a6068d93d6","d5872ccbcc68c5c2c3ea3be02d92d8a6068d93d6","7e920f2d08947384e1cb27340e30b9c000d3f606","ed4cc781203aafb783a5918f02b97ea2a3744b24","4c0461c45258a32f367903901db11a1ae8ce9744","4c0461c45258a32f367903901db11a1ae8ce9744","e374f7dc27c2f0239cc2d984185ec0120974d155","9a7964a56881982e117bcdfdc997aa024bf69392","dfcd3c65509bc0e3a5f2f7a6a1246dfd0f027629","bc6e94213b8017aaef16109b21fc6c877c9d3ee5","964c7dfb079e2f9fa9f6a35bbc5a6842cf50cb36","8ca8fed73c4aa9e88164ec8394ab55f8d19b3b45","8ca8fed73c4aa9e88164ec8394ab55f8d19b3b45","1d21b1b2afd3d42cff43b7aac3f4ca55099ed7db","1d21b1b2afd3d42cff43b7aac3f4ca55099ed7db","7e00388e1cc243bdff641d7c810dfc6c7b783b22","7e00388e1cc243bdff641d7c810dfc6c7b783b22","d379e0e90d255d2c84d2883fb4c144c2346b06f0","21cabe83e6cfd0aa1e5976af8821829eb8081de7","21cabe83e6cfd0aa1e5976af8821829eb8081de7","ef6990940d8d6ab4ccbf8bfd694824ccc04e750c","973baefe2990e0b20fd1f1b4d329b81f04f18a66","a3183251ec69290dd0cad081c260a9fd787572d8","6d0096024acc8c6847ac9968acc1ad47a5bebe20","6d0096024acc8c6847ac9968acc1ad47a5bebe20","93b679b31dabc5c254bcc47bd05f805621649a50","93b679b31dabc5c254bcc47bd05f805621649a50","0394e48ccf31dc1926657ebb36996181865b2ed1","0394e48ccf31dc1926657ebb36996181865b2ed1","1c9337a6ba1d0ca54fb5f801ea7aeb681daf1654","29d59026393a90d5d8fa6ad25ae01ec11c3bf67b","8f754724dc2ce27cf40f97f434f18dc6a3256f2c","ef566ace53de05808c77c1ee00b67935d8aac4b1","fd76dc19ca17adec9693b2d19cbff189f142b4b7","c296e737311de2a17d061d7453d330a900595e13"],"type":"","colors":"","basicLands":{"R":4,"U":4,"G":0,"W":0,"B":8}},{"id":2,"name":"Burn","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":0,"B":0}},{"id":3,"name":"Sphynx Mill?","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":0,"B":0}},{"id":4,"name":"pirates","description":"Yarrr","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":0,"B":1}},{"id":5,"name":"Dino red/white","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},{"id":6,"name":"dino green/white","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":1,"W":1,"B":0}},{"id":7,"name":"vamp","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":0,"W":1,"B":1}},{"id":8,"name":"merfolk","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":1,"W":0,"B":0}},{"id":9,"name":"cat","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":1,"B":0}},{"id":10,"name":"exert","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},{"id":11,"name":"minotaur","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":0,"B":1}},{"id":12,"name":"green/black ??","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":0,"B":1}}]';
    const cachedData: string | null = '['
        +'{"id":0,"name":"Green/White Exalted","description":"Big tokens","cards":["7a1287974bf5f3bb619d53752c6ec698de891f37","7a1287974bf5f3bb619d53752c6ec698de891f37","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","c5c76762e46f29b20df8ac59f3800e34ca5fb727","2948a004e967dd228734b74377105a7a3bf39732","2948a004e967dd228734b74377105a7a3bf39732","869ee41a0d1867fdd3a069a056742909e96d5470","869ee41a0d1867fdd3a069a056742909e96d5470","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","a1def1c22021107451f84d7007ff2405f6406c5c","a1def1c22021107451f84d7007ff2405f6406c5c","4b956d115e5fc9f2bd68b64b5e5f3ca9622d1134","4b956d115e5fc9f2bd68b64b5e5f3ca9622d1134","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","f698e1257973400d06ee2423aa6d80159f22a776","9e8e4997e84311ce2371eb15753e45ed6c2c93c1","089692ce590f004172e86c984ba478eed479d6f4","670d0364f5b9bd40f70ff682ca2ab82e3411bc0d","d242e7f3187da8b254e0caa5936e58cfbb961a9b","d242e7f3187da8b254e0caa5936e58cfbb961a9b","bc68e621a65bef9eed328af437ca72755f8c8825","bc68e621a65bef9eed328af437ca72755f8c8825","80030d4aaaff044fb5be7dbc03b418b9d6af5aff","80030d4aaaff044fb5be7dbc03b418b9d6af5aff","3ffc7ba3e5163791f5ef3c599f1d750a345ae4db","4e0590d622a5076a748647c7692b0c0fb56ed3de","5c74c58794ef0de94b12fdc90ce32cd777d3c4d3","094b47ac41744e30f456fad8f2ea025be1507cf0","0d01ef93a63150628b80d5745ea34b30c30e8eb5","ae41c5bd1f4a97149e222fe1f49d32aea223ea33","f9ba80fc8aff2670208afaadb1934cd8d25c554c"],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":8,"W":8,"B":0}},'
        +'{"id":1,"name":"B/R/Bu Bolas","description":"","cards":["e5a85451def764645578a9628ba060814fc88794","c422203e03ef9c8e7ef3118980f3c0e2e42bc525","fd34bd9ae0b4545f6f268c7f3af5b2dec35aec65","fd34bd9ae0b4545f6f268c7f3af5b2dec35aec65","a279c34c4f07792e423cb0286191e80358c9174e","a279c34c4f07792e423cb0286191e80358c9174e","9e9d6d78ed80dd8f092ef373cb034f8efd034a9b","9e9d6d78ed80dd8f092ef373cb034f8efd034a9b","542ea64fcac69334e3870f4492796db6d1fd265f","542ea64fcac69334e3870f4492796db6d1fd265f","eccf98d80c08c3a06727565f1b1d834e4d6c8627","8a20ca25c580a3c123119dcef12ee03add1c291f","054092a64d39584c8227f064d6cab0ad99d9aafe","592a576deeb5254e598d4347f37215c3cb4a0d98","bef35e93bfa3506ef548786f772d30cd6fd7c180","e522e844b7d341f2c166ef4d3593ff21eec01dd4","cbae24fe2ce943cc7fa98f87931868f90adcb4db","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","d5872ccbcc68c5c2c3ea3be02d92d8a6068d93d6","d5872ccbcc68c5c2c3ea3be02d92d8a6068d93d6","7e920f2d08947384e1cb27340e30b9c000d3f606","ed4cc781203aafb783a5918f02b97ea2a3744b24","4c0461c45258a32f367903901db11a1ae8ce9744","4c0461c45258a32f367903901db11a1ae8ce9744","e374f7dc27c2f0239cc2d984185ec0120974d155","9a7964a56881982e117bcdfdc997aa024bf69392","dfcd3c65509bc0e3a5f2f7a6a1246dfd0f027629","bc6e94213b8017aaef16109b21fc6c877c9d3ee5","964c7dfb079e2f9fa9f6a35bbc5a6842cf50cb36","8ca8fed73c4aa9e88164ec8394ab55f8d19b3b45","8ca8fed73c4aa9e88164ec8394ab55f8d19b3b45","1d21b1b2afd3d42cff43b7aac3f4ca55099ed7db","1d21b1b2afd3d42cff43b7aac3f4ca55099ed7db","7e00388e1cc243bdff641d7c810dfc6c7b783b22","7e00388e1cc243bdff641d7c810dfc6c7b783b22","d379e0e90d255d2c84d2883fb4c144c2346b06f0","21cabe83e6cfd0aa1e5976af8821829eb8081de7","21cabe83e6cfd0aa1e5976af8821829eb8081de7","ef6990940d8d6ab4ccbf8bfd694824ccc04e750c","973baefe2990e0b20fd1f1b4d329b81f04f18a66","a3183251ec69290dd0cad081c260a9fd787572d8","6d0096024acc8c6847ac9968acc1ad47a5bebe20","6d0096024acc8c6847ac9968acc1ad47a5bebe20","93b679b31dabc5c254bcc47bd05f805621649a50","93b679b31dabc5c254bcc47bd05f805621649a50","0394e48ccf31dc1926657ebb36996181865b2ed1","0394e48ccf31dc1926657ebb36996181865b2ed1","1c9337a6ba1d0ca54fb5f801ea7aeb681daf1654","29d59026393a90d5d8fa6ad25ae01ec11c3bf67b","8f754724dc2ce27cf40f97f434f18dc6a3256f2c","ef566ace53de05808c77c1ee00b67935d8aac4b1","fd76dc19ca17adec9693b2d19cbff189f142b4b7","c296e737311de2a17d061d7453d330a900595e13"],"type":"","colors":"","basicLands":{"R":4,"U":4,"G":0,"W":0,"B":8}},'
        +'{"id":2,"name":"Burn","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":0,"B":0}},'
        +'{"id":3,"name":"Sphynx Mill?","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":0,"B":0}},'
        +'{"id":4,"name":"pirates","description":"Yarrr","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":0,"B":1}},'
        +'{"id":5,"name":"Dino red/white","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},'
        +'{"id":6,"name":"dino green/white","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":1,"W":1,"B":0}},'
        +'{"id":7,"name":"vamp","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":0,"W":1,"B":1}},'
        +'{"id":8,"name":"merfolk","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":1,"W":0,"B":0}},'
        +'{"id":9,"name":"cat","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":1,"B":0}},'
        +'{"id":10,"name":"exert","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},'
        +'{"id":11,"name":"minotaur","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":0,"B":1}},'
        +'{"id":12,"name":"green/black poison","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":0,"B":1}},'
        +'{"id":13,"name":"green/black graveyard","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":0,"B":1}},'
        //Ravinica 1
        +'{"id":14,"name":"r/w bolstering","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},'
        +'{"id":15,"name":"g/w convoke","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"1":0,"W":1,"B":0}},'
        +'{"id":16,"name":"r/u dragon","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":1,"G":0,"W":0,"B":0}},'
        //Ravinica 2
        +'{"id":17,"name":"r/g riot","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":1,"W":0,"B":0}},'
        +'{"id":18,"name":"w/b spirit?","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":0,"W":1,"B":1}},'
        +'{"id":19,"name":"U/G adapt","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":1,"W":0,"B":0}},'
        +'{"id":20,"name":"W/U artifact nonsense","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":1,"B":0}},'
        +'{"id":21,"name":"Gate boosted deck","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":1,"G":1,"W":1,"B":1}}'
    +']';

    let deckList: CardDeck[] = []
    if(cachedData && cachedData.length > 0){
        deckList = JSON.parse(cachedData);

        //Should only really need to do this once
        deckList.forEach((deck) => {
            if(!deck.basicLands){
                deck.basicLands = generateLandCounterInstance();
            }
        })
    }

    let cardIndex: ICardIndex = {}
    if(cachedIndexData){
        // var rawCache = JSON.parse(cachedIndexData);
        cardIndex = JSON.parse(cachedIndexData);
    }



    const initialDataStore: IDataStore = {
        deckList: deckList,
        cardIndex: cardIndex
    }

    return initialDataStore;
}



////////cache access
//I'd really like to wrap this in a class or some other object




export function cacheUIState(state: IUIState): void {
    const UIStateString: string = JSON.stringify(state);
    localStorage.setItem(CACHED_UI_STATE_IDENTIFIER, UIStateString);
}

function loadUIStateCache(): IUIState | null {
    const cachedUIStateData: string|null = localStorage.getItem(CACHED_UI_STATE_IDENTIFIER)
    if(cachedUIStateData){
        return JSON.parse(cachedUIStateData);  //what if this parse fails?
    } else {
        return null;
    }
}

// export function cacheDeckEditorState(state: IDeckEditorState): void {



// }

//don't think we want to try caching the deck editor state right?


//
//  Methods for caching states
//


//export function loadInitialActionsState(): 

//More states will need to be loaded ofc

//What abt a load default app state?













///////////////

export default class lumberyard {
    constructor(props: any){

    }

}



// export function tryLoadData(): AppState {
    

    
//     // console.log('trying to load some of the new parsed data');
//     // let storedDeckDetailInstance = loadDeckDetailIndex();
//     // console.log(storedDeckDetailInstance);

//     // console.log(loadLandCountIndex())
//     // console.log(loadCardListIndex())

//     //also need to cache the rare binder
//     // let initialState: AppState = generateAppStateInstance();

    

//     // if(!deckList || !deckList.length){
//     //     deckList = generateDeckData();
//     // }
//     // initialState.deckList = deckList;// || generateDeckData();

//     // if(cachedIndexData){
//     //     // var rawCache = JSON.parse(cachedIndexData);
//     //     let cardIndex: ICardIndex = JSON.parse(cachedIndexData);
//     //     initialState.cardIndex = cardIndex || {};
//     // }
    
//     //Check for any cards that need to be loaded in the initial deck
//     // let activeDeck = initialState.deckList[initialState.selectedDeckId];
//     // activeDeck.cards.forEach((cardId: string) => {
//     //     let cardFromIndex = initialState.cardIndex[cardId];
//     //     if(!cardFromIndex){
//     //         initialState.requestedCards.push(cardId);
//     //     }
//     // });
//     // console.log('audit result');
//     // console.log(initialState.requestedCards)
    

//     // if(deckList && deckList.length){
//     //     initialState.deckList = deckList;
//     // } else {
//     //     initialState.deckList = generateDeckData();
//     // }

//     return initialState;
// }

// function generateAppStateInstance(): AppState {
//     return {

//         deckList: [],
//         selectedDeckId: 0,
//         searchFilter: {
//             name: '',
//             results: []
//         },
//         cardIndex: {},
//         sectionVisibilities: [true,true,true,true,true,true],
//         searchIsFetching: false,
//         requestedCards: [],
//         activeDeckVisibleCards: []
//     }
// }

function generateLandCounterInstance(): ILandCount {
    return {
        R: 0,
        U: 0,
        G: 0,
        W: 0,
        B: 0
    }
}



export function loadDefaultUIState(): IUIState {
    return {
        // isFindModalVisible: true
        isNavOpen: false,
        isSideSheetOpen: false,
        visibleSideSheet: '',
        selectedDeckId: 0

        // deckView: 'card',
        // deckGroup: 'none',
        // deckSort: 'name',
        // deckFilter: ''
    }// a
}

/*
export function lumberyardSaveState(state: State) {
    console.log('saving the app state?');
    console.log(state);

    // console.log('detail index');
    let deckDetailString = '';
    state.actions.deckList.forEach((deck) => {
        let deckDtail: IDeckDetail = {
            id: deck.id,
            colors: deck.colors,
            description: deck.description,
            name: deck.name,
            type: deck.type
        }
        deckDetailString +=  "+'"+JSON.stringify(deckDtail)+",'\n"
    });
    // console.log(deckDetailString);

    console.log('land count index');
    let landCountIndex: IDeckLandCountIndex = {};
    let landCountString = '';
    state.actions.deckList.forEach((deck) => {
        let deckLandCount: IDeckLandCount = {
            id: deck.id,
            basicLands: deck.basicLands
        }
        landCountString += "+'"+JSON.stringify(deckLandCount)+",'"
        landCountIndex[deck.id] = deckLandCount;
    });
    console.log(landCountIndex)
    console.log(landCountString);

    // console.log('card list index');
    let cardListString = '';
    state.actions.deckList.forEach((deck) => {
        let deckCardIndex: IDeckCardList = {
            id: deck.id,
            cards: []
        }
        deck.cards.forEach((cardId) => {
            let thisCard: Card  = state.actions.cardIndex[cardId];
            deckCardIndex.cards.push(thisCard.name)
        })
        // cardListIndex[deck.id] = deckCardIndex;
        cardListString +=  "+'"+JSON.stringify(deckCardIndex)+",'"
    })
    // console.log(cardListString)

}
*/
const loadDeckDetailIndex = (): IDeckDetail[] =>  {
    let detailStr = '['
        +'{"id":0,"colors":"","description":"Big tokens","name":"Green/White Exalted","type":""},'
        +'{"id":1,"colors":"","description":"","name":"B/R/Bu Bolas","type":""},'
        +'{"id":2,"colors":"","description":"","name":"Burn","type":""},'
        +'{"id":3,"colors":"","description":"","name":"Sphynx Mill?","type":""},'
        +'{"id":4,"colors":"","description":"Yarrr","name":"pirates","type":""},'
        +'{"id":5,"colors":"","description":"","name":"Dino red/white","type":""},'
        +'{"id":6,"colors":"","description":"","name":"dino green/white","type":""},'
        +'{"id":7,"colors":"","description":"","name":"vamp","type":""},'
        +'{"id":8,"colors":"","description":"","name":"merfolk","type":""},'
        +'{"id":9,"colors":"","description":"","name":"cat","type":""},'
        +'{"id":10,"colors":"","description":"","name":"exert","type":""},'
        +'{"id":11,"colors":"","description":"","name":"minotaur","type":""},'
        +'{"id":12,"colors":"","description":"","name":"green/black poison","type":""},'
        +'{"id":13,"colors":"","description":"","name":"green/black graveyard","type":""},'
        +'{"id":14,"colors":"","description":"","name":"r/w bolstering","type":""},'
        +'{"id":15,"colors":"","description":"","name":"g/w convoke","type":""},'
        +'{"id":16,"colors":"","description":"","name":"r/u dragon","type":""},'
        +'{"id":17,"colors":"","description":"","name":"r/g riot","type":""},'
        +'{"id":18,"colors":"","description":"","name":"w/b spirit?","type":""},'
        +'{"id":19,"colors":"","description":"","name":"U/G adapt","type":""},'
        +'{"id":20,"colors":"","description":"","name":"W/U artifact nonsense","type":""},'
        +'{"id":21,"colors":"","description":"","name":"Gate boosted deck","type":""}'
    +']';

    console.log('some weird things parsing the detail str');
    console.log(JSON.parse(detailStr))

    let detailIndex: IDeckDetail[] = JSON.parse(detailStr);
    return detailIndex;
}

/*
const loadLandCountIndex = (): IDeckLandCountIndex =>  {
    let landCountStr = `[
        +'{"id":0,"basicLands":{"R":0,"U":0,"G":8,"W":8,"B":0}},'
        +'{"id":1,"basicLands":{"R":4,"U":4,"G":0,"W":0,"B":8}},'
        +'{"id":2,"basicLands":{"R":1,"U":0,"G":0,"W":0,"B":0}},'
        +'{"id":3,"basicLands":{"R":0,"U":1,"G":0,"W":0,"B":0}},'
        +'{"id":4,"basicLands":{"R":0,"U":1,"G":0,"W":0,"B":1}},'
        +'{"id":5,"basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},'
        +'{"id":6,"basicLands":{"R":1,"U":0,"G":1,"W":1,"B":0}},'
        +'{"id":7,"basicLands":{"R":0,"U":0,"G":0,"W":1,"B":1}},'
        +'{"id":8,"basicLands":{"R":0,"U":1,"G":1,"W":0,"B":0}},'
        +'{"id":9,"basicLands":{"R":0,"U":0,"G":1,"W":1,"B":0}},'
        +'{"id":10,"basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},'
        +'{"id":11,"basicLands":{"R":1,"U":0,"G":0,"W":0,"B":1}},'
        +'{"id":12,"basicLands":{"R":0,"U":0,"G":1,"W":0,"B":1}},'
        +'{"id":13,"basicLands":{"R":0,"U":0,"G":1,"W":0,"B":1}},'
        +'{"id":14,"basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},'
        +'{"id":15,"basicLands":{"1":0,"R":0,"U":0,"W":1,"B":0}},'
        +'{"id":16,"basicLands":{"R":1,"U":1,"G":0,"W":0,"B":0}},'
        +'{"id":17,"basicLands":{"R":1,"U":0,"G":1,"W":0,"B":0}},'
        +'{"id":18,"basicLands":{"R":0,"U":0,"G":0,"W":1,"B":1}},'
        +'{"id":19,"basicLands":{"R":0,"U":1,"G":1,"W":0,"B":0}},'
        +'{"id":20,"basicLands":{"R":0,"U":1,"G":0,"W":1,"B":0}},'
        +'{"id":21,"basicLands":{"R":1,"U":1,"G":1,"W":1,"B":1}}'
    +]`
    ;
    console.log(landCountStr)
    let landCountIndex: IDeckLandCountIndex = {};
    return landCountIndex;
}
*/
/*
const loadCardListIndex = (): IDeckCardListIndex =>  {
    let cardListStr = '['
    +'{"id":0,"cards":["Anointed Procession","Anointed Procession","Resilient Khenra","Resilient Khenra","Resilient Khenra","Resilient Khenra","Sylvan Caryatid","Vitu-Ghazi Guildmage","Vitu-Ghazi Guildmage","Steadfast Sentinel","Steadfast Sentinel","Sunscourge Champion","Sunscourge Champion","Sunscourge Champion","Sunscourge Champion","Adorned Pouncer","Adorned Pouncer","Adorned Pouncer","Adorned Pouncer","Desert of the True","Desert of the True","Desert of the Indomitable","Desert of the Indomitable","Selesnya Guildgate","Selesnya Guildgate","Selesnya Guildgate","Selesnya Guildgate","Lifecrafter\'s Bestiary","Embalmer\'s Tools","Rhonas\'s Monument","Oketra\'s Monument","Sundering Growth","Sundering Growth","Life Goes On","Life Goes On","Sandblast","Sandblast","Rootborn Defenses","Druid\'s Deliverance","Oketra\'s Last Mercy","Hour of Revelation","Hour of Promise","Collective Blessing","Abundance"]},'+'{"id":1,"cards":["Submerged Boneyard","Fetid Pools","Izzet Guildgate","Izzet Guildgate","Rakdos Guildgate","Rakdos Guildgate","Evolving Wilds","Evolving Wilds","Crypt of the Eternals","Crypt of the Eternals","Nicol Bolas, God-Pharaoh","The Scorpion God","The Locust God","The Scarab God","To the Slaughter","Thoughtflare","Cyclonic Rift","Razaketh\'s Rite","Razaketh\'s Rite","Razaketh\'s Rite","Torment of Hailfire","Torment of Hailfire","Hour of Devastation","Psychic Intrusion","Torment of Scarabs","Torment of Scarabs","Liliana\'s Mastery","Imprisoned in the Moon","Trial of Knowledge","Trial of Zeal","Trial of Ambition","Cartouche of Knowledge","Cartouche of Knowledge","Cartouche of Ambition","Cartouche of Ambition","Frontline Devastator","Frontline Devastator","Neheb, the Eternal","Spellweaver Eternal","Spellweaver Eternal","Eternal of Harsh Truths","Proven Combatant","Champion of Wits","Gravedigger","Gravedigger","Dreamstealer","Dreamstealer","Ammit Eternal","Ammit Eternal","Dread Wanderer","Lord of the Accursed","Indulgent Tormentor","Sultai Emissary","Razaketh, the Foulblooded","Bontu the Glorified"]},'+'{"id":2,"cards":[]},'+'{"id":3,"cards":[]},'+'{"id":4,"cards":[]},'+'{"id":5,"cards":[]},'+'{"id":6,"cards":[]},'+'{"id":7,"cards":[]},'+'{"id":8,"cards":[]},'+'{"id":9,"cards":[]},'+'{"id":10,"cards":[]},'+'{"id":11,"cards":[]},'+'{"id":12,"cards":[]},'+'{"id":13,"cards":[]},'+'{"id":14,"cards":[]},'+'{"id":15,"cards":[]},'+'{"id":16,"cards":[]},'+'{"id":17,"cards":[]},'+'{"id":18,"cards":[]},'+'{"id":19,"cards":[]},'+'{"id":20,"cards":[]},'+'{"id":21,"cards":[]}'
    +']';
    let cardListIndex: IDeckCardListIndex = {};
    return cardListIndex;
}
*/