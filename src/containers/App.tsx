import { connect, DispatchProp } from 'react-redux'
import React from 'react';

import '../App.css';
import DeckEditor from './DeckEditor';
import RareBinder from '../components/RareBinder';
import CardQuickAdd from './CardQuickAdd';

import DeckDetail from '../components/DeckDetail';

import MaterialButton from '../components/MaterialButton'
import AppNav from '../components/AppNav'

import AppIcon from '../components/AppIcon'

import {
    appNavClick,
    addDeck,
    selectDeck,
    deckChanged,
    saveDeck,
    onSectionToggle,
    appSheetToggle,
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


interface PropsFromState { 
    isNavOpen: boolean;
    selectedDeckId: number;
    deckList: CardDeck[];
    isSideSheetOpen: boolean;
    visibleSideSheet: string;
}

type AppProps = PropsFromState & DispatchProp<ReduxAction>;

class App extends React.Component<AppProps>{
    constructor(props: AppProps) {
        super(props);

        this.handleNavClick = this.handleNavClick.bind(this);
        this.handleAddDeck = this.handleAddDeck.bind(this);
        this.handleSelectDeck = this.handleSelectDeck.bind(this);
        this.handleOverlayClick = this.handleOverlayClick.bind(this);

        this.handleSheetToggle = this.handleSheetToggle.bind(this);
        this.handleSaveStateClick = this.handleSaveStateClick.bind(this);
    }

    handleNavClick(){
        this.props.dispatch(appNavClick())
    }

    handleAddDeck(){
        this.props.dispatch(addDeck());
    }

    handleSelectDeck(index: number){
        this.props.dispatch(selectDeck(index));
    }

    handleSheetToggle(section: string){
        this.props.dispatch(appSheetToggle(section))
    }

    handleOverlayClick() {
        this.props.dispatch(appNavClick())
    }

    handleSaveStateClick() {

    }

    // handleDeckSelected(id: number) {
    //     this.props.dispatch(selectDeck(id))
    // }

    render() {
        // flex-container-vertical
        //static-section
        const description: string = "a MTG deck management tooklit";

        // const navContainer: JSX.Element = <div>NAVIGATION</div>;

        //const { deckList } = props;

        //const { ui } = props;


        const deckDetailSheet: JSX.Element = (<div className="app-deck-detail"></div>);
        const cardSearchSheet: JSX.Element = (<CardQuickAdd />);
        const rareBinderSheet: JSX.Element = (<RareBinder />);

        const sideSheet: JSX.Element = (
            <div className="app-side-sheet grid-col-2">
                {this.props.visibleSideSheet == 'detail' && deckDetailSheet}
                {this.props.visibleSideSheet == 'search' && cardSearchSheet}
                {this.props.visibleSideSheet == 'rare' && rareBinderSheet}
            </div>
        )

        const navOverlay: JSX.Element = (
            <div className="app-nav-overlay">
                <AppNav deckList={this.props.deckList} 
                    selectedDeckId={this.props.selectedDeckId} 
                    onAddDeckClick={this.handleAddDeck} 
                    onItemSelected={this.handleSelectDeck}
                    handleNavClick={this.handleNavClick}/>
                <div className="overlay" onClick={this.handleOverlayClick} />
            </div>
        );

        return (
            <div className="app">
                <div className="app-header app-bar bar-dark">
                    <div className="header-section">
                        <MaterialButton value="" isSelected={this.props.isNavOpen} onClick={this.handleNavClick} icon="menu"  />
                        <AppIcon />
                    </div>
                    <div className="header-section pull-right">
                        <MaterialButton value="save" icon="save" onClick={this.handleSaveStateClick} />
                        <MaterialButton value="detail" icon="list" onClick={this.handleSheetToggle} />
                        <MaterialButton value="search" icon="search" onClick={this.handleSheetToggle} />
                        <MaterialButton value="rare" icon="grade" onClick={this.handleSheetToggle} />
                    </div>
                </div>
               
                <div className={"app-contents"+(this.props.isSideSheetOpen ? " contents-short" : " contents-full")}>
                    <DeckEditor />
                </div>
                
                { this.props.isSideSheetOpen && sideSheet }
                { this.props.isNavOpen && navOverlay }
            </div>
        );
    }
}


// class App extends Component {
//     //Will either be showing the DeckManager or RareBinder container components


//     render() {
//         // flex-container-vertical
//         //static-section
//         const description: string = "Deck Manager";
//         return (
//             <div className="app">
//                 <div className="app-header">
//                     <h1>carpentry</h1><h2>{description}</h2>
//                 </div>
//                 <div className="app-contents">
                    
//                     {/* <ViewSelector />  flex-section */}
//                     <DeckManager />
//                     {/* <RareBinder /> */}
//                 </div>
//             </div>
//         );
//     }
// }

// export default App;

    

function mapStateToProps(state: State): PropsFromState {
    // const searchFilterName = state.actions.searchFilter.name;
    // const searchResults: Card[] = state.actions.searchFilter.results.slice();
    // const selectedSearchResult = state.actions.searchFilter.selectedCardId;
    // console.log('can we hit thisss?')

    //
    //state.ui.visibleSideSheet

    const result: PropsFromState = {
        isNavOpen: state.ui.isNavOpen,
        selectedDeckId: state.actions.selectedDeckId,
        deckList: state.actions.deckList,
        isSideSheetOpen: state.ui.isSideSheetOpen,
        visibleSideSheet: state.ui.visibleSideSheet
        // searchValue: searchFilterName,
        // searchResults: searchResults,
        // selectedSearchResult: selectedSearchResult
    }
    return result;
}

export default connect(mapStateToProps)(App);


