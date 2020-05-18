import React from 'react';
import { Box } from '@material-ui/core';
import VisualCard from './VisualCard';

interface ComponentProps{
    cardOverviews: DeckCardOverview[];
    onCardSelected: (card: DeckCardOverview) => void;
}

export default function DeckCardGrid(props: ComponentProps): JSX.Element {
    return (
        <React.Fragment>
            <Box className="flex-row-wrap">
                {props.cardOverviews.map((card) => (
                    <VisualCard key={card.name} cardOverview={card} onCardSelected={() => {props.onCardSelected(card)}} />
                ))}
            </Box>
        </React.Fragment>
    );
}