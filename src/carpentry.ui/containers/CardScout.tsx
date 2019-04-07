import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'

import InputField from '../components/InputField';

import {
    scoutSearchFilterChanged,
    scoutSearchApplied
} from '../actions/cardScoutActions'

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

interface CardScoutSearchFilter {
    set: string;
    name: string;
    type: string;
    colorIdentity: string;
}

interface PropsFromState {
    searchFilter: CardScoutSearchFilter;
    searchIsInProgress: boolean;
    searchResults?: ICard[];

    pinnedDecks: CardScoutDeck[];
    relatedDecks: CardScoutDeck[];
    otherDecks: CardScoutDeck[];
}

function generateTestData(state: State): PropsFromState {
    return {
        // searchFilter:{
        //     set:"DOM", //RNA, guessing on the DOM bit
        //     colors:"",
        //     name: "",
        //     type:"wiz"
        // },
        searchFilter: state.cardScoutCardSearch.filter,
        searchIsInProgress: state.cardScoutCardSearch.searchIsInProgress,
        searchResults: state.cardScoutCardSearch.searchResults,
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
        super(props);
        this.handleSetFilterChanged = this.handleSetFilterChanged.bind(this);
        this.handleSearchFilterChanged = this.handleSearchFilterChanged.bind(this);
        this.handleTypeFilterChanged = this.handleTypeFilterChanged.bind(this);
        this.handleColorIdentityChanged = this.handleColorIdentityChanged.bind(this);
        this.handleSearchClick = this.handleSearchClick.bind(this);
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


    handleSetFilterChanged(event: any){
        this.props.dispatch(scoutSearchFilterChanged('set', event.target.value));
    }

    handleSearchFilterChanged(event: any){
        this.props.dispatch(scoutSearchFilterChanged('name', event.target.value));
    }

    handleTypeFilterChanged(event: any){
        this.props.dispatch(scoutSearchFilterChanged('type', event.target.value));
    }

    handleColorIdentityChanged(event: any){
        this.props.dispatch(scoutSearchFilterChanged('colorIdentity', event.target.value));
    }

    handleSearchClick(event: any){
        this.props.dispatch(scoutSearchApplied());
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
        
        return(
            <div className="card-scout-container flex-col">
                <div className="outline-section">Card Scout</div>
                <div className="flex-row">
                    <div className="flex-section">
                        <div className="outline-section">Card Search</div>
                        <div className="outline-section">
                            <div className="outline-section">Search Filters</div>
                            <div className="flex-row">
                                <InputField label="Set" value={this.props.searchFilter.set} property="" onChange={this.handleSetFilterChanged} />
                                <InputField label="Name" value={this.props.searchFilter.name} property="" onChange={this.handleSearchFilterChanged} />
                                <InputField label="Type" value={this.props.searchFilter.type} property="" onChange={this.handleTypeFilterChanged} />
                                <InputField label="Colors" value={this.props.searchFilter.colorIdentity} property="" onChange={this.handleColorIdentityChanged} />
                                {/* <div className="outline-section">[Search Button]</div> */}
                                <div className="outline-section flex-col">
                                    <label>Search</label>
                                    <button onClick={this.handleSearchClick}>GO</button>
                                </div>
                            </div>
                        </div>
                        
                        <div className="outline-section">
                            <div className="outline-section">Search Results</div>
                            {
                                (this.props.searchResults && this.props.searchResults.length > 0) && (
                                    <div className="outline-section flex-row">
                                        <div className="outline-section">result</div>
                                        <div className="outline-section">result</div>
                                        <div className="outline-section">result</div>
                                        <div className="outline-section">result</div>
                                        <div className="outline-section">result</div>
                                    </div>
                                )
                            }
                            {
                                (this.props.searchResults && this.props.searchResults.length == 0) && <div className="outline-section">[No Results]</div>
                            }
                            {
                                (!this.props.searchResults) && <div className="outline-section">[No Search Performed]</div>
                            }
                            {
                                this.props.searchIsInProgress && <div className="outline-section">[Search In Progress]</div>
                            }
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
    
    const result: PropsFromState = generateTestData(state);

    return result;
}



export default connect(mapStateToProps)(CardScout)