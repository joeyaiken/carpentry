import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../reducers';

import {
    Typography,
    Paper,
} from '@material-ui/core';

interface PropsFromState { 
    pendingCards: { [key:number]: PendingCardsDto }
}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

interface CardSearchPendingCardsProps {
    pendingCards: { [key:number]: PendingCardsDto }
}

function CardSearchPendingCardsSection(props: CardSearchPendingCardsProps): JSX.Element {
    return (<Paper className="outline-section flex-row">
        {
            Object.keys(props.pendingCards).map((id: string) => (
                <Paper key={id}>
                    <Typography variant="h5">{ props.pendingCards[id].data.name }</Typography>
                    <Typography variant="h6">{ props.pendingCards[id].countNormal }</Typography>
                    <Typography variant="h6">{ props.pendingCards[id].countFoil }</Typography>
                </Paper>
            ))
        }
    </Paper>);
}

class CardSearch extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
    }

    render() {
        return (
        <React.Fragment>
            <CardSearchPendingCardsSection pendingCards={this.props.pendingCards} />
        </React.Fragment>);
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    
    const result: PropsFromState = {
        pendingCards: state.data.cardSearchPendingCards.pendingCards,
    }

    return result;
}

export default connect(mapStateToProps)(CardSearch);
