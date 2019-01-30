import React from 'react';
import { DeckListItem } from './DeckListItem';

export interface DeckListProps {
    deckList: CardDeck[];
    selectedDeck: number;
    onDeckItemClicked: (id: number) => void;
    isOpen: boolean;
    onHeaderToggle: () => void;
}

export default function DeckList(props: DeckListProps): JSX.Element {

    const cardContents: JSX.Element = 
        <div className="card-body flex-row">
            {  // (props.deckList && props.deckList.length) ? (
                (props.deckList && props.deckList.length) ? (
                    props.deckList.map((deck, index) => {
                        return(
                            <DeckListItem 
                                key={index} 
                                deckData={ deck } 
                                onClick={() => { props.onDeckItemClicked(deck.id) }} 
                                isSelected={ deck.id == props.selectedDeck  }
                                />)
                    })
                ) : (
                    <div>No data found</div>
                )
                
            }
        </div>
    

    return(
        <div className="sd-deck-list card">
            <div className="card-header" onClick={props.onHeaderToggle}>
                <label>Navigation</label>
            </div>
            {
                (props.isOpen) && cardContents
            }
            
        </div>
    );
}

