import React from 'react';
import { Box } from '@material-ui/core';
import { appStyles } from '../../../styles/appStyles';
import VisualCard from '../../../common/components/VisualCard';

interface ComponentProps{
    cardOverviews: DeckCardOverview[];
    onCardSelected: (card: DeckCardOverview) => void;
}

export default function DeckCardGrid(props: ComponentProps): JSX.Element {
    const { flexRowWrap } = appStyles();
    return (
        <React.Fragment>
            <Box className={flexRowWrap}>
                {props.cardOverviews.map((card) => (
                    <VisualCard key={card.name} cardOverview={card} onCardSelected={() => {props.onCardSelected(card)}} />
                ))}
            </Box>
        </React.Fragment>
    );
}