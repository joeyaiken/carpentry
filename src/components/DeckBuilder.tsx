import React from 'react';
import CardBinder from './CardBinder';
import CardQuickAdd from '../containers/CardQuickAdd';
import RareBinder from './RareBinder';

export interface DeckBuilderProps {

}

export default function DeckBuilder(props: DeckBuilderProps): JSX.Element {
    return(
        <div className="sd-deck-builder outline-section">
            <label>Deck Builder</label>
            <div className="outline-section flex-row">
                <div className="outline-section flex-col">
                    <CardBinder />
                </div> 
                <div className="outline-section flex-col">
                    <CardQuickAdd />
                    <RareBinder />
                </div>
            </div>
        </div>
    );
}