import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'

import {
    apInitialized,
    apSetSelected,
    apLoadSelectedSet
} from '../actions/addPack.actions'

// import {
//     mtgApiRequestAddPackSearch
// } from '../actions/mgtApi.actions'

import { timingSafeEqual } from 'crypto';
import { stat } from 'fs';

interface apCardSection {
    name: string;
    cards: ICard[];
}

interface apCardSet {
    code: string;
    name: string;
}

interface PropsFromState {
    cardSets: apCardSet[];
    selectedSet: string | null;
    setCards: apCardSection[];
    searchInProgress: boolean;
}

type AddPackProps = PropsFromState & DispatchProp<ReduxAction>;

class AddPack extends React.Component<AddPackProps> {
    constructor(props: AddPackProps) {
        super(props)
        // this.handleFilterChanged = this.handleFilterChanged.bind(this);
        // this.handleSearchApplied = this.handleSearchApplied.bind(this);
        // this.handleCardSelected = this.handleCardSelected.bind(this);
        // this.handleActionApplied = this.handleActionApplied.bind(this);
        this.handleSetSelected = this.handleSetSelected.bind(this);
    }

    componentDidMount(): void {
        this.props.dispatch(apInitialized())
    }
    
    componentDidUpdate(){
        if(this.props.selectedSet != null ){
            console.log('there is a set');
            if(this.props.setCards != null && this.props.setCards.length == 0){
                console.log('There is an empty array of set cards');
                if(!this.props.searchInProgress){
                    console.log('There is no search in progress--------------------------');
                    this.props.dispatch(apLoadSelectedSet())
                }

            }
        }
    }

    

    handleSetSelected(setCode: string): void {
        this.props.dispatch(apSetSelected(setCode))
    }
    // handleFilterChanged(): void {
    //     this.props.dispatch(csFilterChanged())
    // }

    // handleSearchApplied(): void {
    //     this.props.dispatch(csSearchApplied())
    // }

    // handleCardSelected(): void {
    //     this.props.dispatch(csCardSelected())
    // }

    // handleActionApplied(): void {
    //     this.props.dispatch(csActionApplied())
    // }

    renderCard(): JSX.Element {
        return(
            <div className="flex-row">
                <div className="outline-section flex-section">Name | Type</div>
                <div className="outline-section">- | # | +</div>
            </div>
        )
    }

    renderSetButton(set: apCardSet): JSX.Element {
        return(
            <div className="" key={set.code}>
                <button onClick={() => this.handleSetSelected(set.code) }>
                    { set.code }
                </button>
                <span>{set.name}</span>
            </div>
        )
    }

    renderCardSection(collection: apCardSection): JSX.Element {
        return(
            <div className="flex-col">
                <div className="outline-section">{ collection.name }</div>
                <div className="flex-col">
                    {collection.cards.map((card) => {
                        return this.renderCard();
                    })}
                </div>
            </div>
        );
    }

    renderSetMenu(): JSX.Element {
        return(
            <div className="outline-section flex-col">
                    Pick a set
                    <div className="outline-section flex-col">
                        { //row-wrap
                            this.props.cardSets.map((set) => {
                                return this.renderSetButton(set)
                            })
                        }
                    </div>
                </div>
        );
    }

    renderSetContents(): JSX.Element {
        return(
            <>
                <div className="outline-section flex-row">
                    <div className="outline-section">Set</div>
                    <div className="outline-section">Location</div>
                    <div className="outline-section">Filters</div>
                </div>
                {
                    this.props.setCards.map((collection) => {
                        return(this.renderCardSection(collection))
                    })
                }
                <div className="outline-section">Save</div>
            </>
        )
    }

    renderSetLoadingScreen(): JSX.Element {
        return(
            <div className="flex-col flex-section">
                <div className="outline-section flex-row">
                    <div className="outline-section">Set</div>
                    <div className="outline-section">Location</div>
                    <div className="outline-section">Filters</div>
                </div>
                <div className="outline-section flex-section">Loading</div>
                <div className="outline-section">Save</div>
            </div>
        )
    }

    renderSetLoading(): JSX.Element {
        return(<div className="outline-section">
            <div className="outline-section">Loading set...</div>
        </div>)
    }

    render(){
        // console.log("THIGNS");
        // console.log(this.props.cardSets);
        return(
            <div className="flex-col stretch">
                <div className="outline-section">Add Pack</div>
                { (this.props.selectedSet == null) && this.renderSetMenu() }
                { (this.props.selectedSet != null && this.props.setCards.length > 0) && this.renderSetContents() }
                { (this.props.selectedSet != null && this.props.setCards.length == 0) && this.renderSetLoadingScreen() }
            </div>
        )
    }

}


function generateTestSets(): apCardSet[] {
    return([
        { name: "Dominaria", code: "DOM" },
        { name: "Amonkhet", code: "AKH" }, 
        { name: "Hour of Devastation", code: "HOU" },
        { name: "Theros", code: "THS" },
        { name: "Return to Ravnica", code: "RTR" },
        { name: "Dragon's Maze", code: "DGM" },
        { name: "Aether Revolt", code: "AER" },
        { name: "Fate Reforged", code: "FRF" },
        { name: "Tenth Edition", code: "10E" },
        { name: "Shadows over Innistrad", code: "SOI" },
        { name: "Eldritch Moon", code: "EMN" },
        { name: "Magic 2015 Core Set", code: "M15" },
        { name: "Ixalan", code: "XLN" },
        { name: "Rivals of Ixalan", code: "RIX" },
        { name: "Modern Masters 2017", code: "MM3" },
        { name: "Ravnica Allegiance", code: "RNA" }, 
        { name: "Guilds of Ravnica", code: "GRN" }
    ])
}

// function generateTestSetContents(): apCardSection[] {
//     return [
//         {
//             name: "Common Cards",
//             cards: ["card","card","card"]
//         },
//         {
//             name: "Uncommon Cards",
//             cards: ["card","card","card"]
//         },
//         {
//             name: "Rare Cards",
//             cards: ["card","card","card"]
//         },
//         {
//             name: "Mythic Cards",
//             cards: ["card","card","card"]
//         }
//     ]
// }


// function tryMapSearchResultsToLocal(apiState: IMtgApiSearchState): apCardSection[]{
//     // console.log('is there anything to map?')
//     if(apiState.searchInProgress){
//         // console.log('no')
//         return [];
//     // }
//     // else if (){
//     } else {
//         // console.log('yes??')
//         let cardsBySet: { [set: string]: ICard[] } = {}

//         apiState.searchResults.forEach((card: ICard) => {
//             if(!cardsBySet[card.rarity]){
//                 cardsBySet[card.rarity] = []
//             }
//             cardsBySet[card.rarity].push(card)
    
//             // console.log(card)
//         });

//         return(
//             Object.keys(cardsBySet).map((setKey) => {
//                 return{
//                     name: setKey,
//                     cards: cardsBySet[setKey].map((card: ICard) => (card))
//                 } as apCardSection;
//             })
    
//         )

//     }

    

// }

function mapNamedCardCollectionToLocal(stateData: INamedCardArray[] | null): apCardSection[] {
    if(stateData){
        const mapped = stateData.map((group: INamedCardArray) => {
            return {
                name: group.name,
                cards: group.cards
            } as apCardSection;
        });
        return mapped;
    }
    else{
        return [];
    }
}

function mapStateToProps(state: State): PropsFromState {

    /**
     * 
interface apCardSection {
    name: string;
    cards: string[];
}
     * 
     */
    // declare interface ICardIndex {
    //     [set: string]: {
    //         [name: string]: ICard;
    //     }
    // }
    
    /*
    interface groupedCardCollection { [key: string]: 
    {
        cards: IMagicCard[];
        isOpen: boolean;
        //count?
    } 
};
    */

    // if(state.mtgApiSearch.searchInProgress){
    //     console.log('a search is in progress');
    // } else {
    //     console.log(state)
    // }

    

    //let sets: apCardSection[] = [];//state.mtgApiSearch.searchResults.
   
    const result: PropsFromState = {
        cardSets: generateTestSets(),
        selectedSet: state.addPack.selectedSetCode,
        //setCards: tryMapSearchResultsToLocal(state.mtgApiSearch),
        setCards: mapNamedCardCollectionToLocal(state.addPack.groupedCards),
        //searchInProgress: ((state.mtgApiSearch && state.mtgApiSearch.searchInProgress) || false)
        searchInProgress: state.addPack.isLoadingSet
    }
    return result;
}

export default connect(mapStateToProps)(AddPack)