import React from 'react';
import LandIcon from './LandIcon'


export interface DeckListItemProps{
    deckData: CardDeck;
    isSelected: boolean;
    onClick: () => void;

    //Eventually this should contain a binding for 'display-mode' or something
}

export function DeckListItem(props: DeckListItemProps): JSX.Element {
    
    
    
    
    return(
        <div className="sd-deck-card">
            <div className={ props.isSelected ? "selected-card compact-card" : "compact-card"} onClick={ props.onClick }>
                {/* <label>{ props.deckData.description}</label> */}
                <div className="deck-mana-types">
                    {props.deckData.basicLands.R > 0 && <LandIcon manaType={'R'} />}
                    {props.deckData.basicLands.U > 0 && <LandIcon manaType={'U'} />}
                    {props.deckData.basicLands.G > 0 && <LandIcon manaType={'G'} />}
                    {props.deckData.basicLands.W > 0 && <LandIcon manaType={'W'} />}
                    {props.deckData.basicLands.B > 0 && <LandIcon manaType={'B'} />}
                </div>
            </div>
            <label>{ props.deckData.name}</label>
        </div>
        
    );
}
