import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'
import {
    selectDeck,
    // deckChanged,
    saveDeck,
    deckEditorSectionToggle,
    appSheetToggle,
    // selectDeck,
    // deckChanged
    deckEditorLandCountChange,
    deckEditorViewChange,
    deckEditorGroupChange,
    deckEditorSortChange,
    // cardDetailRequested,
    fetchCardsIfNeeded,


    deckEditorCardSelected,
    deckEditorDuplicateSelectedCard,
    deckEditorRemoveOneSelectedCard,
    deckEditorRemoveAllSelectedCards,
    toggleDeckEditorStatus
    //deckEditorHeaderToggle
    //deckEditorSectionToggle
} from '../actions'



// import DeckBuilder from '../components/DeckBuilder';
import DeckQuickStats from '../components/DeckQuickStats';

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
    // deckList: CardDeck[];
    selectedDeckId: number;
    sectionVisibilities: boolean[];
    
    selectedCard: IDeckCard | null;
    
    // dispatch?: any;

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

    isDeckUpToDate: boolean;

}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props)
        this.handleDeckSelected = this.handleDeckSelected.bind(this);
        // this.handleDeckChanged = this.handleDeckChanged.bind(this);
        this.handleDeckSaved = this.handleDeckSaved.bind(this);
        // this.handleSheetToggle = this.handleSheetToggle.bind(this);

        this.handleCardClick = this.handleCardClick.bind(this);
        // this.handleLandCountChange = this.handleLandCountChange.bind(this);

        this.handleViewChange = this.handleViewChange.bind(this);
        this.handleGroupChange = this.handleGroupChange.bind(this);
        this.handleSortChange = this.handleSortChange.bind(this);
        this.handleLandCountChange = this.handleLandCountChange.bind(this);

        this.handleHeaderToggle = this.handleHeaderToggle.bind(this);
        this.handleSectionToggle = this.handleSectionToggle.bind(this);

        this.handleDuplicateClick = this.handleDuplicateClick.bind(this);
        this.handleRemoveOneClick = this.handleRemoveOneClick.bind(this);
        this.handleRemoveAllClick = this.handleRemoveAllClick.bind(this);

        this.handleStatusChange = this.handleStatusChange.bind(this);
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

    // handleDeckChanged(event: any, selectedDeck: CardDeck){
        
    //     const updatedDeck: CardDeck = {
    //         ...selectedDeck, [event.target.name]: event.target.value
    //     }
    //     // console.log(updatedDeck)


    //     this.props.dispatch(deckChanged(updatedDeck))
    // }

    handleDeckSaved(){
        //localStorage.setItem('')
        this.props.dispatch(saveDeck())
    }

    handleHeaderToggle(){
        
    }

    handleSectionToggle(sectionIndex: string){
        console.log('sectionToggle: ' + sectionIndex)
        this.props.dispatch(deckEditorSectionToggle(sectionIndex));
    }

    // handleSheetToggle(section: string){
    //     this.props.dispatch(cardBinderSheetToggle(section))
    // }

    handleViewChange(view: string){
        // console.log('handling view change: '+view)
        this.props.dispatch(deckEditorViewChange(view));
    }

    handleGroupChange(group: string){
        // console.log('handling group change: '+group)
        this.props.dispatch(deckEditorGroupChange(group));
    }

    handleSortChange(sort: string){
        // console.log('handling sort change: '+sort)
        this.props.dispatch(deckEditorSortChange(sort));
    }

    handleLandCountChange(newValue: number, type: string){
        this.props.dispatch(deckEditorLandCountChange(newValue, type))
    }

    handleCardClick(cardName: string){
        this.props.dispatch(deckEditorCardSelected(cardName))
    }

    handleDuplicateClick(){
        this.props.dispatch(deckEditorDuplicateSelectedCard())
        //
    }

    handleRemoveOneClick(){
        this.props.dispatch(deckEditorRemoveOneSelectedCard())
        //
    }

    handleRemoveAllClick(){
        this.props.dispatch(deckEditorRemoveAllSelectedCards())
        //
    }
    
    handleStatusChange(){
        this.props.dispatch(toggleDeckEditorStatus())
    }
    

    
  

    render(){
        // console.log('rendering deckEditor control')
        // const { deckList, selectedDeckId } = this.props;
        // const selectedDeck = deckList.find((deck) => {
        //     return deck.id == selectedDeckId
        // }) || deckList[0];

        // console.log(selectedDeck);
        // let test = this.props;

        const selectedCardHeaderSection: JSX.Element = (
            <div className="header-section">
                <label>Selected:</label>
                <label>{ this.props.selectedCard }</label>
                <MaterialButton value="add" icon="add_circle" onClick={this.handleDuplicateClick} />
                <MaterialButton value="remove" icon="remove_circle" onClick={this.handleRemoveOneClick} />
                <MaterialButton value="clear" icon="cancel" onClick={this.handleRemoveAllClick} />
            </div>
        );


        const deckViewOptions: JSX.Element = (
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
                    <div className="header-section">
                        <label>Status:</label>
                        <MaterialButton value="clear" isSelected={!this.props.isDeckUpToDate} icon="clear" onClick={this.handleStatusChange} />
                        <MaterialButton value="done" isSelected={this.props.isDeckUpToDate} icon="done" onClick={this.handleStatusChange} />
                    </div>
                    {
                        (this.props.selectedCard != null) && selectedCardHeaderSection
                    }

                    <div className="pull-right">
                        <MaterialButton value="" isSelected={false} icon="expand_less" onClick={() => { } } />
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
                                        <MaterialButton value="manaCost" isSelected={false} icon="expand_less" onClick={() => { this.handleSectionToggle(name) } } />
                                    </div>
                                </div>
                                <div className="card-list">
                                    {
                                        collection.cards.map((card: IMagicCard, index: number) => {
                                            // const cardIsSelected = false; // (this.props.selectedSearchResult == card.id);
                                            // console.log(card)
                                            const cardIsSelected = this.props.selectedCard == card.data.name;
                                            // console.log(card.data)
                                            return (<MagicCard key={index} card={card.data} display={this.props.display} cardIsSelected={cardIsSelected} onClick={() => this.handleCardClick(card.data.name)} />);
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

    const result: PropsFromState = {
        // deckList: state.data.deckList,
        selectedDeckId: state.ui.selectedDeckId || 0,
        sectionVisibilities: state.deckEditor.sectionVisibilities,
        display: state.deckEditor.deckView,
        groupBy: state.deckEditor.deckGroup,
        sortBy: state.deckEditor.deckSort,
        cardCollections: groupedCards,
        // cardGroups: cardGroups,
        landCounts: landCounts,
        selectedCard: state.data.selectedCard,
        isDeckUpToDate: deckIsUpToDate
    }
    
    return result;
  }

export default connect(mapStateToProps)(DeckEditor)