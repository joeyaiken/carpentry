import React from 'react';

import {
    Typography,
    Paper,
} from '@material-ui/core';

interface CardSearchPendingCardsProps {
    pendingCards: { [key:number]: PendingCardsDto }
}

export default function PendingCardsSection(props: CardSearchPendingCardsProps): JSX.Element {
    return (<Paper className="outline-section flex-row">
        {
            Object.keys(props.pendingCards).map((id: string) => {
                let thisCard: PendingCardsDto = props.pendingCards[id];
                return(
                <Paper key={id}>
                    <Typography variant="h5">{ thisCard.name }</Typography>
                    <Typography variant="h6">{ thisCard.cards.length }</Typography>
                    {/* <Typography variant="h6">{ thisCard.countFoil }</Typography> */}
                </Paper>
            )
        })
        }
    </Paper>);
}