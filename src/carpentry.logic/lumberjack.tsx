//A Lumberjack works in the Lumberyard, building decks

//Until this gets replaced with something more fancy, it'll be a collection of functions

//Each function name will be prefixed with "Lumberjack_"

//A Lumberjack should have EXCLUSIVE access to the Lumberyard







import { Lumberyard } from '../carpentry.data/lumberyard'


export class Lumberjack {
    

    static addCardToPending = (cardName: string, pendingCards: IntDictionary, isFoil?: boolean): IntDictionary => {
        let result = {
            ...pendingCards
        } as IntDictionary;

        if(!result[cardName]){
            result[cardName] = [0,0]
        }
        if(isFoil){
            result[cardName][1]++;
        } else {
            result[cardName][0]++;
        }
        
        console.log('adding cards')
        return result;
    }

    static removeCardFromPending = (cardName: string, pendingCards: IntDictionary, isFoil?: boolean): IntDictionary => {
        let result = {
            ...pendingCards
        } as IntDictionary;

        if(!result[cardName]){
            result[cardName] = [0,0]
        }

        if(!result[cardName]){
            result[cardName] = [0,0]
        }
        if(isFoil){
            result[cardName][1]--;
        } else {
            result[cardName][0]--;
        }

        console.log('removing cards')
        return result;
    }

    static addCardsToInventory = (setCode: string, cards: IntDictionary): INamedCardArray[] => {

        let currentCache = Lumberyard.cache_load_cardInventory();

        Object.keys(cards).forEach((cardName: string) => {

            let existingCard = currentCache.find((dataCard) => (dataCard.name == cardName));

            let newCardCounts = cards[cardName];

            if(existingCard){
                existingCard.norm = existingCard.norm + newCardCounts[0]
                existingCard.foil = existingCard.foil + newCardCounts[1]
            } else {
                currentCache.push({
                    name: cardName,
                    set: setCode,
                    norm: newCardCounts[0],
                    foil: newCardCounts[1]
                } as DataInventoryCard);
            }
        });

        //This would be the spot to remove any 0/0 items

        Lumberyard.cache_save_cardInventory(currentCache);


        //init should load current cache then call this same logic:
        //{
        let mappedResult = Lumberjack.mapInventoryCacheToNamedCardArray(currentCache);
        
        return mappedResult;
    }

    static mapInventoryCacheToNamedCardArray(data: DataInventoryCard[]): INamedCardArray[] {
        let mappedThings: {
            [set: string]: INamedCardArray
        }= {

        }

        //return an obj that's mapped back to a...named card array? I guess each name is a set
        //So, this section is generating an object that the UI can load
        //Really, it should be it's own function that the initial load also calls


        data.forEach((card: DataInventoryCard) => {
            if(!mappedThings[card.set]){
                mappedThings[card.set] = {
                    name: card.set,
                    count: 0,
                    cards: []
                }
            }

            mappedThings[card.set].cards.push({
                
            } as ICard)


        })

        return [];

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

    static getAllOwnedCardsBySet = (): NamedInventoryCardArray[] => {
        const indexData: ICardIndex = Lumberyard.Collections_All_BySet();

        //filter this shit by owned cards?

        //should start by loading cache AND/OR hard data

        let cachedInventory: DataInventoryCard[] = Lumberyard.cache_load_cardInventory();



        const setKeys = Object.keys(indexData);

        const groupedResults: NamedInventoryCardArray[] = setKeys.map((key) => {
            //Grouping saved card sata per set
            //Originally, this was mapping all index cards to the results
            //Instead, we should itterate over the inventory, and pull cards out of the index as needed


            //getting an array of cards for this specific set

            /*
                name: string;
                cards: InventoryCard[];
            */
            const thisCardIndex: ICardDictionary = indexData[key];

            let unnamed: InventoryCard[] = cachedInventory.map((item: DataInventoryCard) => {
                return {
                    data: thisCardIndex[item.name],
                    inDecks: 0,
                    inInventory: item.norm,
                    inInventoryFoil: item.foil
                }  as InventoryCard;//
            })
        

            let theseCards: ICard[] = [];

            let thisIndex = indexData[key];


            let thisIndexNames = Object.keys(thisIndex);


            theseCards = thisIndexNames.map(cardName => thisIndex[cardName]);




            let result: NamedInventoryCardArray = {
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

