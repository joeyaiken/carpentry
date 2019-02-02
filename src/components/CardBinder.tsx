
import { connect, DispatchProp } from 'react-redux'
import React from 'react';
import {
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
import InputField from '../components/InputField';
import { Card } from 'mtgsdk-ts';
import MagicCard from './MagicCard';
import LandCounter from './LandCounter';
import MaterialButton from './MaterialButton'


//import Magic = require('mtgsdk-ts');
// import Magic from 'mtgsdk-ts';

interface binderCardGroup { 
    name: string;
    isOpen: boolean;
    cards: IMagicCard[];
};

interface binderCardCollection { [key: string]: IMagicCard[] };


interface groupedCardCollection { [key: string]: IMagicCard[] };

interface PropsFromState {
    // searchValue: string;
    //need a dictionary of cards instead of array of cards
    visibleCards: IMagicCard[];
    // cardCollections: bindarCardGroup;
    cardCollections: binderCardCollection;//{ [key: string]: IMagicCard[] };

    cardGroups: binderCardGroup[];
    // selectedSearchResult?: string;
    landCounts: ILandCount;

    display: string | null; // card | list

    groupBy: string; // spellType | rarity | none
    //spell type icon   flash_on
    //rarity icon       grade
    //name icon         text-format
    //mana cost icon    battery-unknown | signal_cellular_alt
    sortBy: string | null; //manaCost | name | rarity
}

type CardBinderProps = PropsFromState & DispatchProp<ReduxAction>;

class CardBinder extends React.Component<CardBinderProps> {
    constructor(props: CardBinderProps) {
        super(props);
        // //this.handleDeckSelected = this.handleDeckSelected.bind(this);
        // this.handleSearchFilterChanged = this.handleSearchFilterChanged.bind(this);
        // this.handleSearchClick = this.handleSearchClick.bind(this);
        // //this.handleAddToRaresClick = this.handleAddToRaresClick(bind)
        this.handleCardClick = this.handleCardClick.bind(this);
        this.handleLandCountChange = this.handleLandCountChange.bind(this);

    }

    componentDidMount() {
        // const { dispatch, selectedSubreddit } = this.props
        // const { visibleCards } = this.props
        // visibleCards.forEach(card => {
        //     if(!card.data && !card.updateRequested){
        //         console.log('requesting card ' + card.cardId)
        //         this.props.dispatch(cardDetailRequested(card.cardId))
        //     }
        // })
        // this.props.dispatch(fetchCardsIfNeeded())
    }

    componentDidUpdate(){
        // this.props.dispatch(fetchCardsIfNeeded())
    }

    handleCardClick(cardId: string){

    }

    handleLandCountChange(newValue: number, type: string){
        this.props.dispatch(cardBinderLandCountChange(newValue, type))
    }

    render(){

        // console.log('card binder render');
        // console.log(this.props.cardCollections);
        // console.log(this.props.cardGroups);
        // console.log('keys');
        // console.log(Object.keys(this.props.cardCollections));

        //Can I get all of they keys from the dictionary?
        

        //There's either a collection of groups or nothing to group by

        //How about this:
        //If there's nothing to group by, put everything in a group named "cards"
        //Otherwise, group by (collection key)
        



        // for (groupName in this.props.cardCollections){

        // }

        const cardSection = 
            <div className="card-list">
                {
                    this.props.visibleCards.map((card: IMagicCard, index: number) => {
                        const cardIsSelected = false; // (this.props.selectedSearchResult == card.id);
                        // console.log('attempting to display card');
                        // console.log(card);
                        return (<MagicCard key={index} card={card} display={this.props.display} cardIsSelected={cardIsSelected} onClick={() => this.handleCardClick("")} />);
                    })
                }
            </div>;



        //const newSection = this.props
        

        const groupNames = Object.keys(this.props.cardCollections);
        // groupNames.forEach((name: string) => {


        // })
        // for (let key in this.props.cardCollections) {

        // const detailSection =
        //     <div className="">
            
        //     </div>;  

        // {/* <CardList cards={deck.cards} /> */}

        //What properties would this button class need?
        //onClick
        //material icon
        //isSelected
        //value

        return(
            <div className="sd-card-binder card">

                <div className="binder-section">
                    <div className="section-header">
                        <label>Basic Lands</label>
                        <label>(count)</label>
                        <label className="pull-right">(icon)</label>
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
                                    <label className="pull-right">(icon)</label>
                                </div>
                                <div className="card-list">
                                    {
                                        collection.map((card: IMagicCard, index: number) => {
                                            const cardIsSelected = false; // (this.props.selectedSearchResult == card.id);
                                            // console.log('attempting to display card');
                                            // console.log(card);
                                            return (<MagicCard key={index} card={card} display={this.props.display} cardIsSelected={cardIsSelected} onClick={() => this.handleCardClick("")} />);
                                        })
                                    }
                                </div>
                                {/*
                                
                                <div className="card-list">
                                        {
                                            this.props.visibleCards.map((card: IMagicCard, index: number) => {
                                                const cardIsSelected = false; // (this.props.selectedSearchResult == card.id);
                                                // console.log('attempting to display card');
                                                // console.log(card);
                                                return (<MagicCard key={index} card={card} display={this.props.display} cardIsSelected={cardIsSelected} onClick={() => this.handleCardClick("")} />);
                                            })
                                        }
                                    </div>;
                                
                                */}
                            </div>
                        )
                    })
                }
                {/* <div className="binder-section">
                    <div className="section-header">
                        Cards
                    </div>
                    {
                        cardSection
                    }
                </div> */}
                
            </div>
        );
    }
}

function mapStateToProps(state: State): PropsFromState {

    //get selected deck
    const selectedDeckId = state.actions.selectedDeckId;
    const activeDeck = state.actions.deckList[selectedDeckId];

    // console.log('card binder state')
    // console.log(state);

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
    switch(state.deckEditor.deckGroup){// spellType | rarity | none
        case 'spellType':
            visibleCards.forEach((card: IMagicCard) => {
                const cardType: string = card.data.types[0];
                if(!groupedCards[cardType]){
                    groupedCards[cardType] = [];
                }
                groupedCards[cardType].push(card);
            });
            break;
        case 'rarity':
            visibleCards.forEach((card: IMagicCard) => {
                // const cardType: string = card.data.types[0];
                if(!groupedCards[card.data.rarity]){
                    groupedCards[card.data.rarity] = [];
                }
                groupedCards[card.data.rarity].push(card);
            });
            break;
        default: 
            groupedCards['Cards'] = visibleCards;
        break;
    }

    // console.log('grouped cards');
    // console.log(groupedCards)

    let defaultBinder: binderCardGroup = {
        name: 'Cards',
        isOpen: true,
        cards: visibleCards
    }

    // let defaultCollection: binderCardCollection = {
    //     ["Cards"]:visibleCards
    // }

    
    // console.log('visibleCards');
    // console.log(visibleCards);
    const result: PropsFromState = {
        visibleCards: visibleCards,
        // cardCollections: orderedCardCollection,
        cardCollections: groupedCards,
        cardGroups: cardGroups,
        landCounts: activeDeck.basicLands,
        display: state.deckEditor.deckView,
        groupBy: state.deckEditor.deckGroup,
        sortBy: state.deckEditor.deckSort
        // searchValue: searchFilterName,
        // searchResults: searchResults,
        // selectedSearchResult: selectedSearchResult
    }
    return result;
}

export default connect(mapStateToProps)(CardBinder);



