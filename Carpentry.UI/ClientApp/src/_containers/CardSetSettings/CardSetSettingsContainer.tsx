import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../../_reducers';
// import { Typography, Box, CardContent, Card, CardHeader, Paper, Button } from '@material-ui/core';
// import DeckList from '../DeckList/DeckListContainer';
// import { Link } from 'react-router-dom';
import CardSetSettingsContainerLayout from './CardSetSettingsContainerLayout';
import { requestTrackedSets, requestAddTrackedSet, requestUpdateTrackedSet, requestRemoveTrackedSet } from '../../_actions/coreActions';

//Should this manage the modal?

interface PropsFromState {
    trackedSetDetails: SetDetailDto[];

    showUntracked: boolean;
    isLoading: boolean;
}

type CardSetSettingsContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSetSettingsContainer extends React.Component<CardSetSettingsContainerProps> {
    constructor(props: CardSetSettingsContainerProps) {
        super(props);
        this.handleRefreshClick = this.handleRefreshClick.bind(this);
        this.handleShowUntrackedClick = this.handleShowUntrackedClick.bind(this);
        this.handleAddTrackedSetClick = this.handleAddTrackedSetClick.bind(this);
        this.handleUpdateTrackedSetClick = this.handleUpdateTrackedSetClick.bind(this);
        this.handleRemoveTrackedSetClick = this.handleRemoveTrackedSetClick.bind(this);
    }

    componentDidMount() {
        //showUntracked, update
        this.props.dispatch(requestTrackedSets(this.props.showUntracked, false));
    }

    handleRefreshClick(): void {
        this.props.dispatch(requestTrackedSets(this.props.showUntracked, true));
    }

    handleShowUntrackedClick(): void {
        this.props.dispatch(requestTrackedSets(!this.props.showUntracked, false));
    }

    handleAddTrackedSetClick(setId: number): void {
        this.props.dispatch(requestAddTrackedSet(setId))
    }

    handleUpdateTrackedSetClick(setId: number): void {
        this.props.dispatch(requestUpdateTrackedSet(setId))
    }

    handleRemoveTrackedSetClick(setId: number): void {
        this.props.dispatch(requestRemoveTrackedSet(setId))
    }

    render() {
        return (
            <CardSetSettingsContainerLayout
                onRefreshClick={this.handleRefreshClick}
                onShowUntrackedClick={this.handleShowUntrackedClick}
                trackedSetDetails={this.props.trackedSetDetails}
                showUntrackedValue={this.props.showUntracked}
                onAddSetClick={this.handleAddTrackedSetClick}
                onRemoveSetClick={this.handleRemoveTrackedSetClick}
                onUpdateSetClick={this.handleUpdateTrackedSetClick}
                isLoading={this.props.isLoading}
            />
        );
    }
}

// function selectDeckList(state: AppState): DeckOverviewDto[] {
//     //state.data.
//     //const someting = state.
//     const { deckIds, decksById } = state.data.deckOverviews;
//     const result: DeckOverviewDto[] = deckIds.map( id => decksById[id]);
//     return result;
// }

function selectTrackedSets(state: AppState): SetDetailDto[] {
    const { setsById, setIds } = state.data.trackedSets;
    const result: SetDetailDto[] = setIds.map((setId) => setsById[setId]);
    return result;
}

////
function mapStateToProps(state: AppState, ownProps): PropsFromState {

    // console.log('attempting to check ownProps');
    // console.log(ownProps);

    const result: PropsFromState = {
        trackedSetDetails: selectTrackedSets(state),
        showUntracked: state.data.trackedSets.showUntracked,
        isLoading: state.data.trackedSets.isLoading,
    }
    return result;
}

export default connect(mapStateToProps)(CardSetSettingsContainer);