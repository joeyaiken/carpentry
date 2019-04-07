import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'


import {
    // selectDeck,
    // // deckChanged,
    // saveDeck,
    // deckEditorSectionToggle,
    // appSheetToggle,
    // // selectDeck,
    // // deckChanged
    // deckEditorLandCountChange,
    // deckEditorViewChange,
    // deckEditorGroupChange,
    // deckEditorSortChange,
    // // cardDetailRequested,
    // fetchCardsIfNeeded,


    // deckEditorCardSelected,
    // deckEditorDuplicateSelectedCard,
    // deckEditorRemoveOneSelectedCard,
    // deckEditorRemoveAllSelectedCards,
    // toggleDeckEditorStatus
    // //deckEditorHeaderToggle
    // //deckEditorSectionToggle
} from '../actions'



/**
 * 
 * The Deck Editor is basically a fancy data table
 * 
 */

interface binderCardGroup { 
    name: string;
    isOpen: boolean;
    cards: IMagicCard[];
};
// interface binderCardCollection { [key: string]: IMagicCard[] };
//interface groupedCardCollection { [key: string]: IMagicCard[] };
interface groupedCardCollection { [key: string]: 
    {
        cards: IMagicCard[];
        isOpen: boolean;
        //count?
    } 
};

interface CardScoutDeck {
    name: string;
}

interface PropsFromState {


    pinnedDecks: CardScoutDeck[];
    relatedDecks: CardScoutDeck[];
    otherDecks: CardScoutDeck[];

    // deckList: CardDeck[];
    // selectedDeckId: number;
    // sectionVisibilities: boolean[];
    
    // selectedCard: IDeckCard | null;
    
    // // dispatch?: any;

    // display: string | null; // card | list

    // groupBy: string; // spellType | rarity | none
    // //spell type icon   flash_on
    // //rarity icon       grade
    // //name icon         text-format
    // //mana cost icon    battery-unknown | signal_cellular_alt
    // sortBy: string | null; //manaCost | name | rarity

    // landCounts: ILandCount;
    // //cardCollections: binderCardCollection;//{ [key: string]: IMagicCard[] };
    // cardCollections: groupedCardCollection;//{ [key: string]: IMagicCard[] };

    // isDeckUpToDate: boolean;

}

function generateTestData(): PropsFromState {
    return {
        pinnedDecks: [
            {
                name:"Pinned Deck"
            },
            {
                name:"Pinned Deck"
            }
        ],
        relatedDecks: [
            {
                name:"Related Deck"
            },
            {
                name:"Related Deck"
            }
        ],
        otherDecks: [
            {
                name:"Other Deck"
            },
            {
                name:"Other Deck"
            },
            {
                name:"Other Deck"
            }
        ]
    }
}


type CardScoutProps = PropsFromState & DispatchProp<ReduxAction>;

class CardScout extends React.Component<CardScoutProps> {
    constructor(props: CardScoutProps) {
        super(props)
        this.handleDeckSelected = this.handleDeckSelected.bind(this);
        // this.handleDeckChanged = this.handleDeckChanged.bind(this);
        this.handleDeckSaved = this.handleDeckSaved.bind(this);
        // this.handleSheetToggle = this.handleSheetToggle.bind(this);

        // this.handleCardClick = this.handleCardClick.bind(this);
        // this.handleLandCountChange = this.handleLandCountChange.bind(this);

        // this.handleViewChange = this.handleViewChange.bind(this);
        // this.handleGroupChange = this.handleGroupChange.bind(this);
        // this.handleSortChange = this.handleSortChange.bind(this);
        // this.handleLandCountChange = this.handleLandCountChange.bind(this);

        this.handleHeaderToggle = this.handleHeaderToggle.bind(this);
        // this.handleSectionToggle = this.handleSectionToggle.bind(this);

        // this.handleDuplicateClick = this.handleDuplicateClick.bind(this);
        // this.handleRemoveOneClick = this.handleRemoveOneClick.bind(this);
        // this.handleRemoveAllClick = this.handleRemoveAllClick.bind(this);

        // this.handleStatusChange = this.handleStatusChange.bind(this);
    }

    // componentDidMount() {
    //     // const { dispatch, selectedSubreddit } = this.props
    //     // dispatch(fetchPostsIfNeeded(selectedSubreddit))
    // }

    // componentDidUpdate(prevProps: any) {
    //     // if (this.props.selectedSubreddit !== prevProps.selectedSubreddit) {
    //     //   const { dispatch, selectedSubreddit } = this.props
    //     //   dispatch(fetchPostsIfNeeded(selectedSubreddit))
    //     // }
    // }
    
    // handleChange(nextSubreddit: any) {
    //     // this.props.dispatch(selectSubreddit(nextSubreddit))
    //     // this.props.dispatch(fetchPostsIfNeeded(nextSubreddit))
    // }
    
    // handleRefreshClick(e: any) {
    //     // e.preventDefault()

    //     // const { dispatch, selectedSubreddit } = this.props
    //     // dispatch(invalidateSubreddit(selectedSubreddit))
    //     // dispatch(fetchPostsIfNeeded(selectedSubreddit))
    // }

    handleDeckSelected(id: number) {
        // this.props.dispatch(selectDeck(id))
    }

    // handleDeckChanged(event: any, selectedDeck: CardDeck){
        
    //     const updatedDeck: CardDeck = {
    //         ...selectedDeck, [event.target.name]: event.target.value
    //     }
    //     // console.log(updatedDeck)


    //     this.props.dispatch(deckChanged(updatedDeck))
    // }

    handleDeckSaved(){
        //localStorage.setItem('')
        // this.props.dispatch(saveDeck())
    }

    handleHeaderToggle(){
        
    }

    
    renderCardScoutDeck(deck: CardScoutDeck): JSX.Element{
        return(
            <div className="outline-section flex-row">
                <div className="outline-section">[+]</div>
                {/* <div className="outline-section">[??/4]</div> */}
                <div className="outline-section">{ deck.name }</div>
                <div className="outline-section">[Pin]</div>
                <div className="outline-section">[Options: View Deck]</div>
                {/* 
                    What if, instead of a count, it was just a + / - based on if the specific card was recommend
                    That way, only 1 instance of a card is ever recommended for any specific deck
                    Maybe recomended cards should be in a different table than deck cards?
                */}
            </div>
        );
    }
    
  

    render(){
        

        // const selectedCardHeaderSection: JSX.Element = (
        //     <div className="header-section">
        //         <label>Selected:</label>
        //         <label>{ this.props.selectedCard }</label>
        //         <MaterialButton value="add" icon="add_circle" onClick={this.handleDuplicateClick} />
        //         <MaterialButton value="remove" icon="remove_circle" onClick={this.handleRemoveOneClick} />
        //         <MaterialButton value="clear" icon="cancel" onClick={this.handleRemoveAllClick} />
        //     </div>
        // );

        // const groupNames = Object.keys(this.props.cardCollections);
        /*
        <div className="">
        </div>
        


        <div className="outline-section">[]</div>
        */

        return(
            <div className="card-scout-container flex-col">
                <div className="outline-section">Card Scout</div>
                <div className="flex-row">
                    <div className="flex-section">
                        <div className="outline-section">Card Search</div>
                        <div className="outline-section">
                            <div className="outline-section">Search Filters</div>
                            <div className="outline-section flex-row">    
                                <div className="outline-section">[Set]</div>
                                <div className="outline-section">[Colors]</div>
                                <div className="outline-section">[More Filters]</div>
                                <div className="outline-section">[Search Button]</div>
                            </div>
                        </div>
                        
                        <div className="outline-section">
                            <div className="outline-section">Search Results</div>
                            <div className="outline-section flex-row">
                                <div className="outline-section">result</div>
                                <div className="outline-section">result</div>
                                <div className="outline-section">result</div>
                                <div className="outline-section">result</div>
                                <div className="outline-section">result</div>
                            </div>
                        </div>
                    </div>
                    <div className="flex-section">
                        <div className="outline-section">Decks</div>
                        <div className="outline-section">Pinned</div>
                        { this.props.pinnedDecks.map((deck) => this.renderCardScoutDeck(deck)) }
                        
                        <div className="outline-section">Related</div>
                        { this.props.relatedDecks.map((deck) => this.renderCardScoutDeck(deck)) }
                        {/* Related decks are decks that contain the colors of the selected card */}

                        <div className="outline-section">Other</div>
                        { this.props.otherDecks.map((deck) => this.renderCardScoutDeck(deck)) }
                        
                    </div>
                </div>
                
                <div className="CardScout-card-section outline-section">
                    [Card search notes]
                    <div className="notes">
                        <p>This component will allow the user to search for cards, and propose cards for decks</p>
                        <p>Q: Should the "Set" filter be optional?</p>
                        <p>It shouldn't be able to directly add cards to decks</p>
                        <p>If a user wants to add specific cards to specific deck s, the QuickAdd component from the deck editor is more appropriate</p>
                        <p>Maybe provide an expanded & collapsed version of the deck list</p>
                        <p>Compact could have an summary, card count, and a + icon (when a card is selected)</p>
                        <p>It shouldn't show that pesky QuickAdd component</p>
                        <p>This component isn't going to rely on some local index, it's going to always query the MTG API</p>
                        <p>Q: Should the queries continue to log to an index to keep building out a local library?</p>
                        <p>Does this get its own reducer?</p>
                        <p></p>
                    </div>
                </div>
            </div>
        )
    }

}

function mapStateToProps(state: State): PropsFromState {
    //This function can map the deck list from the data/store/whatever reducer
        //let something = state.data.
    

    
    
    
    
    
    
    
    
    
    // const { selectedSubreddit, postsBySubreddit } = state
    // const { isFetching, lastUpdated, items: posts } = postsBySubreddit[
    //   selectedSubreddit
    // ] || {
    //   isFetching: true,
    //   items: []
    // }


    // console.log(state.actions.sectionVisibilities)
    //test
    // console.log('breakin things')
    const selectedDeckId = state.ui.selectedDeckId || 0;
    const activeDeck = state.data.deckList[selectedDeckId];
    // const activeDeckDetail = state.data.detailList[selectedDeckId];
    // const activeDeckCardList = state.data.cardLists[selectedDeckId];
    let visibleCards: IMagicCard[] = [];
    if(activeDeck){
        visibleCards = activeDeck.cards.map((deckCard) =>{

            // const indexData = state.data.cardIndex[cardId];
            let data = {};
            let cardSet = state.data.cardIndex[deckCard.set];
            if(cardSet){
                data = cardSet[deckCard.name];
            }
            const card: IMagicCard = {
                cardId: deckCard.name,
                data: data
            }
            return card;
        });
    }
    
    

    let groupedCards: groupedCardCollection = {};
    // let cardGroups: binderCardGroup[] = [];
    //group visible cards
    switch(state.deckEditor.deckGroup){// spellType | rarity | none
        case 'spellType':
            visibleCards.forEach((card: IMagicCard) => {
                
                const cardType: string = card.data.types[0];


                if(!groupedCards[cardType]){
                    // groupedCards[cardType].cards = [];
                    groupedCards[cardType] = {
                        cards: [],
                        isOpen: true
                    }
                }
                groupedCards[cardType].cards.push(card);
            });
            break;
        case 'rarity':
            // console.log('sorting by rarity');
            visibleCards.forEach((card: IMagicCard) => {
                // const cardType: string = card.data.types[0];
                if(!groupedCards[card.data.rarity]){
                    //groupedCards[card.data.rarity] = [];
                    groupedCards[card.data.rarity] = {
                        cards: [],
                        isOpen: true
                    };
                }
                groupedCards[card.data.rarity].cards.push(card);
            });
            break;
        default: 
            groupedCards['Cards'] = {
                cards: visibleCards,
                isOpen: true
            };
        break;
    }

    let landCounts: ILandCount = {B:0,G:0,R:0,U:0,W:0}
    let deckIsUpToDate = false;
    if (activeDeck){
        landCounts = activeDeck.basicLands;
        deckIsUpToDate = activeDeck.details.isUpToDate;
    }

    const result: PropsFromState = generateTestData();

    
    
    return result;
}



export default connect(mapStateToProps)(CardScout)