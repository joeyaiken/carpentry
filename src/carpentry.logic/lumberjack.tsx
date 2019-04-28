//A Lumberjack works in the Lumberyard, building decks

//Until this gets replaced with something more fancy, it'll be a collection of functions

//Each function name will be prefixed with "Lumberjack_"

//A Lumberjack should have EXCLUSIVE access to the Lumberyard







import { Lumberyard } from '../carpentry.data/lumberyard'


export class Lumberjack {
    

    static addCardToPending = (cardName: string, pendingCards: IntDictionary): IntDictionary => {
        let result = {
            ...pendingCards
        } as IntDictionary;

        if(!result[cardName]){
            result[cardName] = 0
        }
        result[cardName]++;
        console.log('adding cards')
        return result;
    }

    static removeCardFromPending = (cardName: string, pendingCards: IntDictionary): IntDictionary => {
        let result = {
            ...pendingCards
        } as IntDictionary;

        if(!result[cardName]){
            result[cardName] = 0
        }
        result[cardName]--;
        console.log('removing cards')
        return result;
    }

    // constructor(){
    //     // let defaultStates = new DefaultInstanceStateGenerator();
    //     //this.handleSectionToggle = this.handleSectionToggle.bind(this);
    //     this.defaultSateInstance_dataStore = this.defaultSateInstance_dataStore.bind(this);
    // }
    //defaultStates
    //defaultStates: DefaultInstanceStateGeneratorl

    static getAllCardsForSet = (setCode: string): ICardDictionary | null => {
        return Lumberyard.getAllCardsForSet(setCode);
    }

    //for now, always groups by rarity
    static mapCardDictionaryToGroupedNamedCardArray(dict: ICardDictionary): INamedCardArray[]{
        //const mappedCards: INamedCardArray = cardsFromAction.map
            //let groupedCards: ICardIndex = {}
            let groupedCards: { [grouping: string]: ICard[] } = { };
            //     "Mythic": [],
            //     "Rare": [],
            //     "Uncommon": [],
            //     "Common": []
            // }

            Object.keys(dict).forEach((cardName: string) => {
                let card = dict[cardName];
                groupedCards = {
                    [card.rarity]: [],
                    ...groupedCards
                }
                groupedCards[card.rarity].push(card);
            });

            //This is going to specifically sort cards by rarity
            //Rare
            
            let groupedCardArray = [
                Lumberjack.namedCardArray("Mythic", groupedCards["Mythic"]),
                Lumberjack.namedCardArray("Rare", groupedCards["Rare"]),
                Lumberjack.namedCardArray("Uncommon", groupedCards["Uncommon"]),
                Lumberjack.namedCardArray("Common", groupedCards["Common"]),
            ];


            // let groupedCardArray = Object.keys(groupedCards).map((group) => {
            //     return {
            //         name: group,
            //         cards: groupedCards[group]
            //     } as INamedCardArray;
            // })
            return groupedCardArray;
    }

    static namedCardArray(name: string, cards: ICard[]): INamedCardArray{

        // let sortedCard = cards.sort((a,b) => {
        //         return ((+a.number) - (+b.number))
        //     } )

        let sortedCard = cards.sort((a,b) => {
            
            if(a.name > b.name){
                return 1
            }
            if(a.name < b.name){
                return -1
            }
            return 0
            //(+a.number) - (+b.number))
        });
        console.log('sorted')
        console.log(sortedCard)
        return {
            name: name,
            cards: sortedCard
            // cards.sort((a,b) => {
            //     console.log('sorting...')
            //     if(a.name.toUpperCase() > b.name.toUpperCase()){
            //         return 1
            //     }
            //     if(a.name > b.name){
            //         return -1
            //     }
            //     return 0
            //     //(+a.number) - (+b.number))
            // }) 
        } as INamedCardArray;
    }

    //getGroupedCards(filter): cardGroup[]

    static getAllOwnedCardsBySet = (): INamedCardArray[] => {
        const indexData: ICardIndex = Lumberyard.Collections_All_BySet();

        //filter this shit by owned cards?

        



        const dataKeys = Object.keys(indexData);

        const groupedResults: INamedCardArray[] = dataKeys.map((key) => {
            let theseCards: ICard[] = [];

            let thisIndex = indexData[key];
            let thisIndexNames = Object.keys(thisIndex);
            theseCards = thisIndexNames.map(cardName => thisIndex[cardName])
            let result: INamedCardArray = {
                name:key,
                cards: theseCards
            };
            return result;
        })


        // declare interface ICardIndex {
        //     [set: string]: {
        //         [name: string]: ICard;
        //     }
        // }



        return groupedResults;
    }

    static getGroupedCards = (): INamedCardArray[] => {
        // const dummyData: INamedCardArray[] = [
        //     {name: "Set 1",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]},
        //     {name: "Set 2",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]},
        //     {name: "Set 3",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]},
        //     {name: "Set 4",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]}
        // ];
        
        
        const indexData: ICardIndex = Lumberyard.Collections_All_BySet();

        const dataKeys = Object.keys(indexData);

        const groupedResults: INamedCardArray[] = dataKeys.map((key) => {
            let theseCards: ICard[] = [];

            let thisIndex = indexData[key];
            let thisIndexNames = Object.keys(thisIndex);
            theseCards = thisIndexNames.map(cardName => thisIndex[cardName])
            let result: INamedCardArray = {
                name:key,
                cards: theseCards
            };
            return result;
        })


        // declare interface ICardIndex {
        //     [set: string]: {
        //         [name: string]: ICard;
        //     }
        // }



        return (groupedResults);
    }
    
    static defaultStateInstance_cardSearch = (): ICardSearch => {
        const initialCardSearchState: ICardSearch = {
            requestedCards: [],
            searchFilter: {
                name: '',
                setFilterString: '',
                includeRed: false,
                includeBlue: false,
                includeGreen: false,
                includeWhite: false,
                includeBlack: false,
                colorIdentity: '',
                results: []
            },
            searchIsFetching: false
        }
        return initialCardSearchState;
    }

    static defaultStateInstance_dataStore = (): IDataStore => {
        return {
            deckList: [],
            cardIndex: {},
            selectedCard: null,
            selectedDeckId: null
        }
    }

    static defaultStateInstance_deckEditor = (): IDeckEditorState => {
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

    //misc - need to be refactored probs
    static legacy_loadInitialUIState = (): IUIState => {
        return Lumberyard.legacy_loadInitialUIState();
    }

    static legacy_cacheUIState = (state: IUIState): void => {
        Lumberyard.legacy_cacheUIState(state);
    }

    static legacy_cacheDeckData = (data: ICardDeck[]): void => {
        Lumberyard.legacy_cacheDeckData(data);
    }

    static legacy_saveCardIndexCache = (index: ICardIndex): void => {
        Lumberyard.legacy_saveCardIndexCache(index);
    }

    static legacy_loadInitialDataStore = (): IDataStore => {
        return Lumberyard.legacy_loadInitialDataStore();
    }
    
}

