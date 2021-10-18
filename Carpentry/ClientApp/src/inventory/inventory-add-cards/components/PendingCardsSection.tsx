import React from 'react';

import {
    Typography,
    Paper,
} from '@material-ui/core';
import { combineStyles, appStyles } from '../../../styles/appStyles';

interface CardSearchPendingCardsProps {
    pendingCards: { [key:number]: PendingCardsDto }
}

export default function PendingCardsSection(props: CardSearchPendingCardsProps): JSX.Element {
    const { outlineSection, flexRow } = appStyles();
    return (<Paper className={combineStyles(outlineSection, flexRow)}>
        {
            Object.keys(props.pendingCards).map((id: string) => {
                let thisCard: PendingCardsDto = props.pendingCards[id];
                return(
                <Paper className="pending-card" key={id}>
                    <Typography variant="h5">{ thisCard.name }</Typography>
                    <Typography variant="h6">{ thisCard.cards.length }</Typography>
                    {/* <Typography variant="h6">{ thisCard.countFoil }</Typography> */}
                </Paper>
            )
        })
        }
    </Paper>);
}