import { connect, DispatchProp } from 'react-redux'
import React from 'react';

import '../App.css';
import DeckEditor from './DeckEditor';
import RareBinder from '../components/RareBinder';
import CardQuickAdd from './CardQuickAdd';
import AppData from './AppData';

import MaterialButton from '../components/MaterialButton'
import AppNav from '../components/AppNav'

import AppIcon from '../components/AppIcon'

import {
    appNavClick,
    addDeck,
    selectDeck,
    appSheetToggle
} from '../actions'


interface PropsFromState { 
    isNavOpen: boolean;
    selectedDeckId: number | null;
    deckList: ICardDeck[];
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
        // this.handleSaveStateClick = this.handleSaveStateClick.bind(this);
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

    // handleSaveStateClick() {
    //     this.props.dispatch(appNavSave());
    // }

    // handleDeckSelected(id: number) {
    //     this.props.dispatch(selectDeck(id))
    // }

    render() {
        // flex-container-vertical
        //static-section
        // const description: string = "a MTG deck management tooklit";

        // const navContainer: JSX.Element = <div>NAVIGATION</div>;

        // const { deckList } = props;

        //const { ui } = props;


        const deckDataSheet: JSX.Element = (<AppData />);
        const deckDetailSheet: JSX.Element = (<div className="app-deck-detail">DECK DETAIL SHOULD GO HERE EVENTUALLY</div>);
        const cardSearchSheet: JSX.Element = (<CardQuickAdd />);
        const rareBinderSheet: JSX.Element = (<RareBinder />);

        const sideSheet: JSX.Element = (
            <div className="app-side-sheet grid-col-2">
                {this.props.visibleSideSheet == 'data' && deckDataSheet}
                {this.props.visibleSideSheet == 'detail' && deckDetailSheet}
                {this.props.visibleSideSheet == 'search' && cardSearchSheet}
                {this.props.visibleSideSheet == 'rare' && rareBinderSheet}
            </div>
        )
            //
        const navOverlay: JSX.Element = (
            <div className="app-nav-overlay">
                <AppNav deckList={this.props.deckList} 
                    onAddDeckClick={this.handleAddDeck} 
                    onItemSelected={this.handleSelectDeck}
                    selectedDeckId={this.props.selectedDeckId} 
                    handleNavClick={this.handleNavClick}/>
                <div className="overlay" onClick={this.handleOverlayClick} />
            </div>
        );

        const appHeader: JSX.Element = (
            <div className="app-header app-bar bar-dark">
                <div className="header-section">
                    <MaterialButton value="" isSelected={this.props.isNavOpen} onClick={this.handleNavClick} icon="menu"  />
                    <AppIcon />
                </div>
                <div className="header-section pull-right">
                    <MaterialButton value="data" icon="save" onClick={this.handleSheetToggle} />
                    <MaterialButton value="detail" icon="list" onClick={this.handleSheetToggle} />
                    <MaterialButton value="search" icon="search" onClick={this.handleSheetToggle} />
                    <MaterialButton value="rare" icon="grade" onClick={this.handleSheetToggle} />
                </div>
            </div>
        );
        
        const appBody: JSX.Element = (
            <div className={"app-contents"+(this.props.isSideSheetOpen ? " contents-short" : " contents-full")}>
                <DeckEditor />
            </div>
        );

        return (
            <div className="app">
                { appHeader }
                { appBody }
                { this.props.isSideSheetOpen && sideSheet }
                { this.props.isNavOpen && navOverlay }
            </div>
        );
    }
}

function mapStateToProps(state: State): PropsFromState {
    // const searchFilterName = state.actions.searchFilter.name;
    // const searchResults: Card[] = state.actions.searchFilter.results.slice();
    // const selectedSearchResult = state.actions.searchFilter.selectedCardId;
    // console.log('can we hit thisss?')

    //
    //state.ui.visibleSideSheet

    // console.log('app mapping things')
    // console.log('app mapping things');
    // console.log(state.data)

    // const localCardList: ICardDeck_Legacy[] = state.data.deckList.map((deck) => {

    //     // let deckCardList: IDeckCard[] = deck.cards.map((cardName) => {
    //     //     return {
    //     //         name: cardName,
    //     //         set: ''
    //     //     }
            
    //     // })

    //     let returningCardDeck: ICardDeck_Legacy = {
    //         // basicLands: detail.basicLands,
    //         // cards: state.data.cardLists[detail.id].cards,
    //         // colors: detail.colors,
    //         // description: detail.description,
    //         id: detail.id,
    //         details: detail,
    //         cards: deckcard,
    //         stats: null
    //     //  
    //         // name: detail.name,
    //         // type: detail.type
    //     }
    //     return returningCardDeck;
    // })

    const result: PropsFromState = {
        isNavOpen: state.ui.isNavOpen,
        selectedDeckId: state.ui.selectedDeckId,
        deckList: state.data.deckList,
        isSideSheetOpen: state.ui.isSideSheetOpen,
        visibleSideSheet: state.ui.visibleSideSheet
    }
    return result;
}

export default connect(mapStateToProps)(App);


