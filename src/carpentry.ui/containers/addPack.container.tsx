import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'

// import {
//     // csActionApplied,
//     // csCardSelected,
//     // csFilterChanged,
//     // csInitialized,
//     // csSearchApplied
// } from '../actions/addPack.actions'

interface apCardSection {
    name: string;
    cards: string[];
}

interface apCardSet {
    code: string;
    name: string;
}

interface PropsFromState {
    cardSets: apCardSet[];
    selectedSet: string | null;
    setCards: apCardSection[];
}

type AddPackProps = PropsFromState & DispatchProp<ReduxAction>;

class AddPack extends React.Component<AddPackProps> {
    constructor(props: AddPackProps) {
        super(props)
        // this.handleFilterChanged = this.handleFilterChanged.bind(this);
        // this.handleSearchApplied = this.handleSearchApplied.bind(this);
        // this.handleCardSelected = this.handleCardSelected.bind(this);
        // this.handleActionApplied = this.handleActionApplied.bind(this);
    }

    componentDidMount(): void {
        // this.props.dispatch(csInitialized())
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

    renderFilterButton(set: apCardSet): JSX.Element {
        return(
            <div className="">
                <button onClick={() => 
                        {/*props.onClick(props.value)*/}
                    }>
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
                                return this.renderFilterButton(set)
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

    renderSetLoading(): JSX.Element {
        return(<div className="outline-section">
            <div className="outline-section">Loading set...</div>
        </div>)
    }

    render(){
        
        return(
            <div>
                <div className="outline-section">Add Pack</div>
                { (this.props.selectedSet == null) && this.renderSetMenu() }
                { (this.props.selectedSet != null && this.props.cardSets.length > 0) && this.renderSetContents() }
                { (this.props.selectedSet != null && this.props.cardSets.length == 0) && this.renderSetContents() }
            </div>
        )
    }

}


function generateTestSets(): apCardSet[] {
    return([
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
        { name: "Guilds of Ravnica", code: "GRN" },
        { name: "Dominaria", code: "DOM" }
    ])
}

function generateTestSetContents(): apCardSection[] {
    return [
        {
            name: "Common Cards",
            cards: ["card","card","card"]
        },
        {
            name: "Uncommon Cards",
            cards: ["card","card","card"]
        },
        {
            name: "Rare Cards",
            cards: ["card","card","card"]
        },
        {
            name: "Mythic Cards",
            cards: ["card","card","card"]
        }
    ]
}

function mapStateToProps(state: State): PropsFromState {
    const result: PropsFromState = {
        cardSets: generateTestSets(),
        selectedSet: null,
        setCards: [] //generateTestSetContents()
    }
    return result;
}

export default connect(mapStateToProps)(AddPack)