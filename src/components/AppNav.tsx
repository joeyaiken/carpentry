import React from 'react';
import DeckList from './DeckList'
import AppNavItem from './AppNavItem'
import AppIcon from './AppIcon'
import MaterialButton from './MaterialButton'

export interface AppNavProps {
    deckList: CardDeck[];
    selectedDeckId: number;
    onItemSelected: (index: number) => void;
    onAddDeckClick: () => void;
    handleNavClick: () => void;
}

export default function AppNav(props: AppNavProps): JSX.Element {
    //rare binder is always an option

    //add deck is always an option

    //binder | <decks> | add (where decks scrolls if needed)
    
    const { deckList } = props;
    // console.log('app nav deck list')
    // console.log(deckList)
    
    return(
    <div className="cc-app-nav flex-col">
        <div className="app-nav-header app-bar bar-dark">
            <MaterialButton value="" isSelected={false} onClick={props.handleNavClick} icon="close" />
            <AppIcon />
        </div>
        {/* <div className="app-nav-section">
            <AppNavItem label="Rare Binder" icon="star_border" isSelected={false} onClick= {() => { props.onItemSelected(-1) }}  />
        </div> */}
        <div className="app-nav-section flex-col">
            {/* <DeckList deckList={deckList}
                onDeckItemClicked={this.handleDeckSelected}
                selectedDeck={selectedDeckId}
                isOpen={this.props.sectionVisibilities[0]}
                onHeaderToggle={() => this.handleSectionToggle(0)} /> */}
            {
                deckList.map((item, index) => {
                    return <AppNavItem key={index} label={item.name} onClick= {() => { props.onItemSelected(index) }} isSelected={ item.id == props.selectedDeckId } deckData={item} />
                })
            }
        </div>
        {/* <div className="app-nav-section">
            <AppNavItem label="Add Deck" icon="add" isSelected={false} onClick= {() => { props.onAddDeckClick() }} />
        </div> */}
    </div>
    );
}