import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../reducers'
import DeckStatsBar from '../components/DeckStatsBar';

/**
 * The Deck Editor is basically a fancy data table
 */

interface PropsFromState {
    deckStats: DeckStats | null;
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    // constructor(props: DeckEditorProps) {
    //     super(props);
    // }

    render() {
        return(
            <React.Fragment>
                {this.props.deckStats && <DeckStatsBar deckStats={this.props.deckStats} />}
            </React.Fragment>
        )
    }
}

function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        deckStats: state.data.deckDetail.deckStats,
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor)