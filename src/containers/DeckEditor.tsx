import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'
import {
    selectDeck,
    deckChanged,
    saveDeck,
    onSectionToggle,
    cardBinderSheetToggle,
    // selectDeck,
    // deckChanged
    searchApplied,
    searchValueChange,
    searchCardSelected,
    addCardToDeck,
    cardBinderLandCountChange,
    cardBinderViewChange,
    cardBinderGroupChange,
    cardBinderSortChange,
    // cardDetailRequested,
    fetchCardsIfNeeded
} from '../actions'


import EditorOptions from './EditorOptions';
import DeckList from '../components/DeckList';
import DeckDetail from '../components/DeckDetail';
import DeckBuilder from '../components/DeckBuilder';
import DeckQuickStats from '../components/DeckQuickStats';
import CardBinder from '../components/CardBinder';
import CardQuickAdd from './CardQuickAdd';
import RareBinder from '../components/RareBinder';
import LandCounter from '../components/LandCounter';
import MagicCard from '../components/MagicCard';

import MaterialButton from '../components/MaterialButton'

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

interface PropsFromState {
    deckList: CardDeck[];
    selectedDeckId: number;
    sectionVisibilities: boolean[];
    // dispatch?: any;


    isSearchOpen: boolean;
    isRareBinderOpen: boolean;
    isDetailOpen: boolean;


    display: string | null; // card | list

    groupBy: string; // spellType | rarity | none
    //spell type icon   flash_on
    //rarity icon       grade
    //name icon         text-format
    //mana cost icon    battery-unknown | signal_cellular_alt
    sortBy: string | null; //manaCost | name | rarity

    landCounts: ILandCount;
    //cardCollections: binderCardCollection;//{ [key: string]: IMagicCard[] };
    cardCollections: groupedCardCollection;//{ [key: string]: IMagicCard[] };



}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props)
        this.handleDeckSelected = this.handleDeckSelected.bind(this);
        this.handleDeckChanged = this.handleDeckChanged.bind(this);
        this.handleDeckSaved = this.handleDeckSaved.bind(this);
        // this.handleSheetToggle = this.handleSheetToggle.bind(this);

        this.handleCardClick = this.handleCardClick.bind(this);
        // this.handleLandCountChange = this.handleLandCountChange.bind(this);

        this.handleViewChange = this.handleViewChange.bind(this);
        this.handleGroupChange = this.handleGroupChange.bind(this);
        this.handleSortChange = this.handleSortChange.bind(this);
        this.handleLandCountChange = this.handleLandCountChange.bind(this);
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
        this.props.dispatch(selectDeck(id))
    }

    handleDeckChanged(event: any, selectedDeck: CardDeck){
        
        const updatedDeck: CardDeck = {
            ...selectedDeck, [event.target.name]: event.target.value
        }
        // console.log(updatedDeck)


        this.props.dispatch(deckChanged(updatedDeck))
    }

    handleDeckSaved(){
        //localStorage.setItem('')
        this.props.dispatch(saveDeck())
    }

    handleSectionToggle(sectionIndex: number){
        console.log('sectionToggle: ' + sectionIndex)
        this.props.dispatch(onSectionToggle(sectionIndex));
    }

    // handleSheetToggle(section: string){
    //     this.props.dispatch(cardBinderSheetToggle(section))
    // }

    handleViewChange(view: string){
        // console.log('handling view change: '+view)
        this.props.dispatch(cardBinderViewChange(view));
    }

    handleGroupChange(group: string){
        // console.log('handling group change: '+group)
        this.props.dispatch(cardBinderGroupChange(group));
    }

    handleSortChange(sort: string){
        // console.log('handling sort change: '+sort)
        this.props.dispatch(cardBinderSortChange(sort));
    }

    handleLandCountChange(newValue: number, type: string){
        this.props.dispatch(cardBinderLandCountChange(newValue, type))
    }

    handleCardClick(cardId: string){

    }

    render(){
        console.log('rendering deckEditor control')
        const { deckList, selectedDeckId } = this.props;
        const selectedDeck = deckList.find((deck) => {
            return deck.id == selectedDeckId
        }) || deckList[0];

        // console.log(selectedDeck);
        // let test = this.props;


        let deckViewOptions: JSX.Element = (
            <div className="card-header app-bar">
                    {/* <div className="header-section">
                        <label>Card Binder</label>
                    </div> */}
                    <div className="header-section">
                        <label>View:</label>
                        <MaterialButton value="card" isSelected={(this.props.display == "card")} icon="view_module" onClick={this.handleViewChange} />
                        <MaterialButton value="list" isSelected={(this.props.display == "list")} icon="view_headline" onClick={this.handleViewChange} />
                    </div>
                    <div className="header-section">
                        <label>Group:</label>
                        <MaterialButton value="none" isSelected={(this.props.groupBy == "none")} icon="view_headline" onClick={this.handleGroupChange} />
                        <MaterialButton value="spellType" isSelected={(this.props.groupBy == "spellType")} icon="flash_on" onClick={this.handleGroupChange} />
                        <MaterialButton value="rarity" isSelected={(this.props.groupBy == "rarity")} icon="grade" onClick={this.handleGroupChange} />
                    </div>
                    <div className="header-section">
                        <label>Sort:</label>
                        <MaterialButton value="name" isSelected={(this.props.sortBy == "name")} icon="text_format" onClick={this.handleSortChange} />
                        <MaterialButton value="rarity" isSelected={(this.props.sortBy == "rarity")} icon="grade" onClick={this.handleSortChange} />
                        <MaterialButton value="manaCost" isSelected={(this.props.sortBy == "manaCost")} icon="signal_cellular_alt" onClick={this.handleSortChange} />
                    </div>
                    <div className="pull-right">
                        <MaterialButton value="manaCost" isSelected={false} icon="expand_less" onClick={() => { } } />
                    </div>
                    {/* <label className="pull-right">(icon)</label> */}
                    {/* <div className="section-header pull-right">
                        <label>(icon)</label>
                    </div> */}
                </div>
        );

        const groupNames = Object.keys(this.props.cardCollections);



        return(
            <div className="sd-deck-editor card">
                { 
                    deckViewOptions
                }
                <div className="sd-card-binder card">
                    <div className="binder-section">
                        <div className="section-header">
                            <label>Basic Lands</label>
                            <label>(count)</label>
                            {/* <label className="pull-right">(icon)</label> */}
                            <div className="pull-right">
                                <MaterialButton value="manaCost" isSelected={false} icon="expand_less" onClick={() => { } } />
                            </div>
                        </div>
                        <div className="flex-row card-body">
                            <LandCounter manaType="R" count={this.props.landCounts.R} onCountChange={(newValue: number) => this.handleLandCountChange(newValue, "R")} />
                            <LandCounter manaType="U" count={this.props.landCounts.U} onCountChange={(newValue: number) => this.handleLandCountChange(newValue, "U")} />
                            <LandCounter manaType="G" count={this.props.landCounts.G} onCountChange={(newValue: number) => this.handleLandCountChange(newValue, "G")} />
                            <LandCounter manaType="W" count={this.props.landCounts.W} onCountChange={(newValue: number) => this.handleLandCountChange(newValue, "W")} />
                            <LandCounter manaType="B" count={this.props.landCounts.B} onCountChange={(newValue: number) => this.handleLandCountChange(newValue, "B")} />
                        </div>
                    </div>
                    {
                    groupNames.map((name: string) => {
                        const collection = this.props.cardCollections[name];
                        return(
                            <div className="binder-section">
                                <div className="section-header">
                                    <label>{name}</label>
                                    <label>(count)</label>
                                    {/* <label className="pull-right">(icon)</label> */}
                                    <div className="pull-right">
                                        <MaterialButton value="manaCost" isSelected={false} icon="expand_less" onClick={() => { } } />
                                    </div>
                                </div>
                                <div className="card-list">
                                    {
                                        collection.cards.map((card: IMagicCard, index: number) => {
                                            const cardIsSelected = false; // (this.props.selectedSearchResult == card.id);

                                            return (<MagicCard key={index} card={card} display={this.props.display} cardIsSelected={cardIsSelected} onClick={() => this.handleCardClick("")} />);
                                        })
                                    }
                                </div>
                  
                            </div>
                        )
                    })
                }

                
            </div>
                <DeckQuickStats />
            </div>
        )
    }

}

function mapStateToProps(state: State): PropsFromState {
    // const { selectedSubreddit, postsBySubreddit } = state
    // const { isFetching, lastUpdated, items: posts } = postsBySubreddit[
    //   selectedSubreddit
    // ] || {
    //   isFetching: true,
    //   items: []
    // }

    // console.log(state.actions.sectionVisibilities)

    const selectedDeckId = state.actions.selectedDeckId;
    const activeDeck = state.actions.deckList[selectedDeckId];

    console.log('card binder state')
    console.log(state);

    //Get collection of visible magic cards
    let visibleCards: IMagicCard[] = activeDeck.cards.map((cardId) =>{

        const indexData = state.actions.cardIndex[cardId];
        
        

        const card: IMagicCard = {
            cardId: cardId,
            data: state.actions.cardIndex[cardId]
        }
        return card;
    });
    
    // console.log()

    //once we have the set of visible cards, lets log those card names (in case this garbage breaks again)
    let visibleCardNames: string[] = visibleCards.map((card) => {
        return card.data.name
    })

    // console.log('visibleCardNames')
    // console.log(visibleCardNames);

    //Group cards (only by type for now)
    // let groupedCards: groupedCardCollection = {};

    // visibleCards.forEach((card: IMagicCard) => {
    //     const cardType: string = card.data.types[0];
    //     if(!groupedCards[cardType]){
    //         groupedCards[cardType] = [];
    //     }
    //     groupedCards[cardType].push(card);
    // });

    

    let groupedCards: groupedCardCollection = {};
    let cardGroups: binderCardGroup[] = [];
    //group visible cards
    switch(state.ui.deckGroup){// spellType | rarity | none
        case 'spellType':
            visibleCards.forEach((card: IMagicCard) => {
                const cardType: string = card.data.types[0];
                if(!groupedCards[cardType]){
                    groupedCards[cardType].cards = [];
                }
                groupedCards[cardType].cards.push(card);
            });
            break;
        case 'rarity':
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

    const result: PropsFromState = {
        deckList: state.actions.deckList,
        selectedDeckId: state.actions.selectedDeckId,
        sectionVisibilities: state.actions.sectionVisibilities,
        isDetailOpen: state.actions.isDetailOpen,
        isRareBinderOpen: state.actions.isRareBinderOpen,
        isSearchOpen: state.actions.isSearchOpen,
        display: state.ui.deckView,
        groupBy: state.ui.deckGroup,
        sortBy: state.ui.deckSort,
        cardCollections: groupedCards,
        // cardGroups: cardGroups,
        landCounts: activeDeck.basicLands,
    }
    
    return result;
  }

export default connect(mapStateToProps)(DeckEditor)