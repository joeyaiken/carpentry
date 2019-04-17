import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'

import InputField from '../components/InputField';

import {
    // scoutSearchFilterChanged,
    // scoutSearchApplied
    ciInitialized
} from '../actions/cardInventory.actions'

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

interface CardInventoryDeck {
    name: string;
}

interface CardInventorySearchFilter {
    set: string;
    name: string;
    type: string;
    colorIdentity: string;
}

interface CardInventoryGroup {
    name: string;
    cards: CardInventoryItem[];
}

interface CardInventoryItem {
    name: string;
    cmc: number;
    type: string;
}

interface PropsFromState {
    cardGroups: CardInventoryGroup[] | null;

    // searchFilter: CardInventorySearchFilter;
    // searchIsInProgress: boolean;
    // searchResults?: ICard[];

    // pinnedDecks: CardInventoryDeck[];
    // relatedDecks: CardInventoryDeck[];
    // otherDecks: CardInventoryDeck[];
}

// function generateTestData(state: State): PropsFromState {
//     return {
//         cardGroups: [
//             {name: "Set 1",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]},
//             {name: "Set 2",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]},
//             {name: "Set 3",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]},
//             {name: "Set 4",cards:[{name:"Card"},{name:"Card"},{name:"Card"}]}
//         ]
//         // searchFilter:{
//         //     set:"DOM", //RNA, guessing on the DOM bit
//         //     colors:"",
//         //     name: "",
//         //     type:"wiz"
//         // },
//         // searchFilter: state.CardInventoryCardSearch.filter,
//         // searchIsInProgress: state.CardInventoryCardSearch.searchIsInProgress,
//         // searchResults: state.CardInventoryCardSearch.searchResults,
//     }
// }


type CardInventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class CardInventory extends React.Component<CardInventoryProps> {
    constructor(props: CardInventoryProps) {
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

    componentDidMount() {
        this.props.dispatch(ciInitialized());
    }

    handleSetFilterChanged(event: any){
        // this.props.dispatch(scoutSearchFilterChanged('set', event.target.value));
    }

    handleSearchFilterChanged(event: any){
        // this.props.dispatch(scoutSearchFilterChanged('name', event.target.value));
    }

    handleTypeFilterChanged(event: any){
        // this.props.dispatch(scoutSearchFilterChanged('type', event.target.value));
    }

    handleColorIdentityChanged(event: any){
        // this.props.dispatch(scoutSearchFilterChanged('colorIdentity', event.target.value));
    }

    handleSearchClick(event: any){
        // this.props.dispatch(scoutSearchApplied());
    }
    
    renderFilters(): JSX.Element {
        return (
            <div className="flex-row outline-section">
                <div className="outline-section">Set: All</div>
                <div className="outline-section">Sort By: Name</div>
                <div className="outline-section">Group By: Set</div>
                <div className="outline-section">Rarity: All</div>
                <div className="outline-section">Display: Names</div>
                {/* <InputField label="Set" value={this.props.searchFilter.set} property="" onChange={this.handleSetFilterChanged} />
                <InputField label="Name" value={this.props.searchFilter.name} property="" onChange={this.handleSearchFilterChanged} />
                <InputField label="Type" value={this.props.searchFilter.type} property="" onChange={this.handleTypeFilterChanged} />
                <InputField label="Colors" value={this.props.searchFilter.colorIdentity} property="" onChange={this.handleColorIdentityChanged} /> */}
                
            </div>
        )
    }

    renderCardGroups(cardGroups: CardInventoryGroup[] | null): JSX.Element {
        if(cardGroups != null){
            return(<>{cardGroups.map((group) => this.renderCardGroup(group))}</>);
        } else {
            return (<div className="outline-section">No cards found</div>);
        }
    }

    renderCardGroup(cardGroup: CardInventoryGroup): JSX.Element {
        return(
            <div className="flex-col">
                <div className="outline-section">{cardGroup.name}</div>
                <div className="outline-section flex-row-wrap">
                    {
                        cardGroup.cards.map((card) => this.renderCard(card))
                    }
                </div>
            </div>
        );
    }

    renderCard(card: CardInventoryItem): JSX.Element {
        return(
            <div className="outline-section flex-col">
                <div className="flex-row">
                    <div className="outline-section">{card.name}</div>
                    <div className="outline-section">{card.cmc}</div>
                </div>
                <div className="outline-section">{card.type}</div>
                <div className="flex-row">
                    <div className="outline-section">[-]</div>
                    <div className="outline-section">0</div>
                    <div className="outline-section">[+]</div>
                </div>
            </div>
        );
    }

    renderCardNotes(): JSX.Element {
        return(
            <div className="CardInventory-card-section outline-section">
                [Card inventory notes]
                <div className="notes">
                    <p>Eventually this should limit to a single set at a time, loading a lot at once might take a while?</p>
                    <p>Lets load all index cards for now, then start building up the DB system</p>
                    <p></p>
                    <p></p>
                    <p></p>
                </div>
            </div>
        )
    }

    render(){
        
        return(
            <div className="card-scout-container flex-col">
                <div className="outline-section">Card Inventory - Can we add cards here?</div>
                <div className="flex-row">
                    <div className="flex-section">
                        { this.renderFilters() }
                        { this.renderCardGroups(this.props.cardGroups) }
                    </div>
                </div>
                {/* { this.renderCardNotes() } */}
            </div>
        )
    }

}

function mapStateCardGroupToLocal(group: INamedCardArray): CardInventoryGroup {
    return{
        name: group.name,
        cards: group.cards.map((card) => mapStateCardToLocal(card))
    }
}

function mapStateCardToLocal(card: ICard): CardInventoryItem {
    return {
        name: card.name,
        cmc: card.cmc,
        type: card.type
    }
}

function mapStateToProps(state: State): PropsFromState {
    let groupedCards: CardInventoryGroup[] = [];
    if(state.cardInventory && state.cardInventory.groupedCards){
        groupedCards = state.cardInventory.groupedCards.map((group) => mapStateCardGroupToLocal(group))
    }
    return {
        cardGroups: groupedCards
    };
}

export default connect(mapStateToProps)(CardInventory)