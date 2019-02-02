//export function 

// import db from 'diskdb'

// import React from 'react';

const store: string = './store/';


export function generateDefaultDecks(): void {



}






export default class lumberyard {
    constructor(props: any){

    }

}



export function tryLoadData(): AppState {
    // const cachedData: string|null = localStorage.getItem('deck-cache');
    const cachedIndexData: string|null = localStorage.getItem('card-index-cache');

    const cachedData: string | null = '[{"id":0,"name":"Green/White Exalted","description":"Big tokens","cards":["7a1287974bf5f3bb619d53752c6ec698de891f37","7a1287974bf5f3bb619d53752c6ec698de891f37","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","e48254b4aff80e5eda8640f8a32d1b1bfdb7264a","c5c76762e46f29b20df8ac59f3800e34ca5fb727","2948a004e967dd228734b74377105a7a3bf39732","2948a004e967dd228734b74377105a7a3bf39732","869ee41a0d1867fdd3a069a056742909e96d5470","869ee41a0d1867fdd3a069a056742909e96d5470","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","1517852607b6182707e98a1a0dbd08e5aab6fd19","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","13b2e582c6074c5df03865000291b2a66f76ce1c","a1def1c22021107451f84d7007ff2405f6406c5c","a1def1c22021107451f84d7007ff2405f6406c5c","4b956d115e5fc9f2bd68b64b5e5f3ca9622d1134","4b956d115e5fc9f2bd68b64b5e5f3ca9622d1134","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","747af463d7cf4075df76832ef20cb759a5d958d0","f698e1257973400d06ee2423aa6d80159f22a776","9e8e4997e84311ce2371eb15753e45ed6c2c93c1","089692ce590f004172e86c984ba478eed479d6f4","670d0364f5b9bd40f70ff682ca2ab82e3411bc0d","d242e7f3187da8b254e0caa5936e58cfbb961a9b","d242e7f3187da8b254e0caa5936e58cfbb961a9b","bc68e621a65bef9eed328af437ca72755f8c8825","bc68e621a65bef9eed328af437ca72755f8c8825","80030d4aaaff044fb5be7dbc03b418b9d6af5aff","80030d4aaaff044fb5be7dbc03b418b9d6af5aff","3ffc7ba3e5163791f5ef3c599f1d750a345ae4db","4e0590d622a5076a748647c7692b0c0fb56ed3de","5c74c58794ef0de94b12fdc90ce32cd777d3c4d3","094b47ac41744e30f456fad8f2ea025be1507cf0","0d01ef93a63150628b80d5745ea34b30c30e8eb5","ae41c5bd1f4a97149e222fe1f49d32aea223ea33","f9ba80fc8aff2670208afaadb1934cd8d25c554c"],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":8,"W":8,"B":0}},{"id":1,"name":"B/R/Bu Bolas","description":"","cards":["e5a85451def764645578a9628ba060814fc88794","c422203e03ef9c8e7ef3118980f3c0e2e42bc525","fd34bd9ae0b4545f6f268c7f3af5b2dec35aec65","fd34bd9ae0b4545f6f268c7f3af5b2dec35aec65","a279c34c4f07792e423cb0286191e80358c9174e","a279c34c4f07792e423cb0286191e80358c9174e","9e9d6d78ed80dd8f092ef373cb034f8efd034a9b","9e9d6d78ed80dd8f092ef373cb034f8efd034a9b","542ea64fcac69334e3870f4492796db6d1fd265f","542ea64fcac69334e3870f4492796db6d1fd265f","eccf98d80c08c3a06727565f1b1d834e4d6c8627","8a20ca25c580a3c123119dcef12ee03add1c291f","054092a64d39584c8227f064d6cab0ad99d9aafe","592a576deeb5254e598d4347f37215c3cb4a0d98","bef35e93bfa3506ef548786f772d30cd6fd7c180","e522e844b7d341f2c166ef4d3593ff21eec01dd4","cbae24fe2ce943cc7fa98f87931868f90adcb4db","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","9a11aef6749517c117fe8d8d6a035d2f32bf4abf","d5872ccbcc68c5c2c3ea3be02d92d8a6068d93d6","d5872ccbcc68c5c2c3ea3be02d92d8a6068d93d6","7e920f2d08947384e1cb27340e30b9c000d3f606","ed4cc781203aafb783a5918f02b97ea2a3744b24","4c0461c45258a32f367903901db11a1ae8ce9744","4c0461c45258a32f367903901db11a1ae8ce9744","e374f7dc27c2f0239cc2d984185ec0120974d155","9a7964a56881982e117bcdfdc997aa024bf69392","dfcd3c65509bc0e3a5f2f7a6a1246dfd0f027629","bc6e94213b8017aaef16109b21fc6c877c9d3ee5","964c7dfb079e2f9fa9f6a35bbc5a6842cf50cb36","8ca8fed73c4aa9e88164ec8394ab55f8d19b3b45","8ca8fed73c4aa9e88164ec8394ab55f8d19b3b45","1d21b1b2afd3d42cff43b7aac3f4ca55099ed7db","1d21b1b2afd3d42cff43b7aac3f4ca55099ed7db","7e00388e1cc243bdff641d7c810dfc6c7b783b22","7e00388e1cc243bdff641d7c810dfc6c7b783b22","d379e0e90d255d2c84d2883fb4c144c2346b06f0","21cabe83e6cfd0aa1e5976af8821829eb8081de7","21cabe83e6cfd0aa1e5976af8821829eb8081de7","ef6990940d8d6ab4ccbf8bfd694824ccc04e750c","973baefe2990e0b20fd1f1b4d329b81f04f18a66","a3183251ec69290dd0cad081c260a9fd787572d8","6d0096024acc8c6847ac9968acc1ad47a5bebe20","6d0096024acc8c6847ac9968acc1ad47a5bebe20","93b679b31dabc5c254bcc47bd05f805621649a50","93b679b31dabc5c254bcc47bd05f805621649a50","0394e48ccf31dc1926657ebb36996181865b2ed1","0394e48ccf31dc1926657ebb36996181865b2ed1","1c9337a6ba1d0ca54fb5f801ea7aeb681daf1654","29d59026393a90d5d8fa6ad25ae01ec11c3bf67b","8f754724dc2ce27cf40f97f434f18dc6a3256f2c","ef566ace53de05808c77c1ee00b67935d8aac4b1","fd76dc19ca17adec9693b2d19cbff189f142b4b7","c296e737311de2a17d061d7453d330a900595e13"],"type":"","colors":"","basicLands":{"R":4,"U":4,"G":0,"W":0,"B":8}},{"id":2,"name":"Burn","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":0,"B":0}},{"id":3,"name":"Sphynx Mill?","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":0,"B":0}},{"id":4,"name":"pirates","description":"Yarrr","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":0,"W":0,"B":1}},{"id":5,"name":"Dino red/white","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},{"id":6,"name":"dino green/white","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":1,"W":1,"B":0}},{"id":7,"name":"vamp","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":0,"W":1,"B":1}},{"id":8,"name":"merfolk","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":1,"G":1,"W":0,"B":0}},{"id":9,"name":"cat","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":1,"B":0}},{"id":10,"name":"exert","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":1,"B":0}},{"id":11,"name":"minotaur","description":"","cards":[],"type":"","colors":"","basicLands":{"R":1,"U":0,"G":0,"W":0,"B":1}},{"id":12,"name":"green/black ??","description":"","cards":[],"type":"","colors":"","basicLands":{"R":0,"U":0,"G":1,"W":0,"B":1}}]';
    

    let initialState: AppState = generateAppStateInstance();

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

    if(!deckList || !deckList.length){
        deckList = generateDeckData();
    }
    initialState.deckList = deckList;// || generateDeckData();

    if(cachedIndexData){
        // var rawCache = JSON.parse(cachedIndexData);
        let cardIndex: ICardIndex = JSON.parse(cachedIndexData);
        initialState.cardIndex = cardIndex || {};
    }
    
    //Check for any cards that need to be loaded in the initial deck
    let activeDeck = initialState.deckList[initialState.selectedDeckId];
    activeDeck.cards.forEach((cardId: string) => {
        let cardFromIndex = initialState.cardIndex[cardId];
        if(!cardFromIndex){
            initialState.requestedCards.push(cardId);
        }
    });
    // console.log('audit result');
    // console.log(initialState.requestedCards)
    

    // if(deckList && deckList.length){
    //     initialState.deckList = deckList;
    // } else {
    //     initialState.deckList = generateDeckData();
    // }

    return initialState;
}

function generateAppStateInstance(): AppState {
    return {
        // isNavOpen: true,
        isDetailOpen: false,
        isRareBinderOpen: false,
        isSearchOpen: false,

        deckList: [],
        selectedDeckId: 0,
        // ui: {
        //     isFindModalVisible: false
        // },
        searchFilter: {
            name: '',
            // isFetching: false,
            results: []
        },
        cardIndex: {},
        sectionVisibilities: [true,true,true,true,true,true],
        // cardBinderFiter: '',
        // cardBinderGroup: 'none',
        // cardBinderSort: 'name',
        // cardBinderView: 'card',
        searchIsFetching: false,
        requestedCards: [],
        activeDeckVisibleCards: []
        // activeDeck: undefined,

        
        // activeDeck: {},
        // activeDeckVisibleCards: {}

    }
}

function generateLandCounterInstance(): ILandCount {
    return {
        R: 0,
        U: 0,
        G: 0,
        W: 0,
        B: 0
    }
}

function generateDeckData(): CardDeck[] {

    const generateDeckInstance = (): CardDeck => {
        return {
            id: 0,
            name: '',
            description: '',
            cards: [],
            type: '',
            colors: '',
            basicLands: generateLandCounterInstance()
        }
    }

    let deckList: CardDeck[] = [
        //exist
        {
            ...generateDeckInstance(),
            id: 0,
            name: "Green/White Exalted"
        },
        {
            ...generateDeckInstance(),
            id: 1,
            name: "B/R/Bu Bolas"
        },
        {
            ...generateDeckInstance(),
            id: 2,
            name: "Mono Red Burn"
        },
        {
            ...generateDeckInstance(),
            id: 3,
            name: "Mono Blue Sphynx Mill?"
        },
        {
            ...generateDeckInstance(),
            id: 4,
            name: "pirates"
        },
        {
            ...generateDeckInstance(),
            id: 5,
            name: "Dino red/white"
        },
        {
            ...generateDeckInstance(),
            id: 6,
            name: "dino green/white"
        },
        {
            ...generateDeckInstance(),
            id: 7,
            name: "vamp"
        },
        {
            ...generateDeckInstance(),
            id: 8,
            name: "merfolk"
        },
        {
            ...generateDeckInstance(),
            id: 9,
            name: "cat"
        },
        {
            ...generateDeckInstance(),
            id: 10,
            name: "exert"
        },
        {
            ...generateDeckInstance(),
            id: 11,
            name: "minotaur"
        },
        {
            ...generateDeckInstance(),
            id: 12,
            name: "green/black ??"
        },
    ];

    return deckList;
}


export function loadDefaultUIState(): IUIState {
    return {
        // isFindModalVisible: true
        isNavOpen: false,
        isSideSheetOpen: false,
        visibleSideSheet: '',
        selectedDeckId: ''

        // deckView: 'card',
        // deckGroup: 'none',
        // deckSort: 'name',
        // deckFilter: ''
    }// a
}