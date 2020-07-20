import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../../reducers';
import { Typography, Box, CardContent, Card, CardHeader, Paper, Button } from '@material-ui/core';
import DeckList from '../DeckList/DeckListContainer';
import { Link } from 'react-router-dom';
import CardSetSettingsContainerLayout from './CardSetSettingsContainerLayout';
import { requestTrackedSets } from '../../actions/coreActions';

//Should this manage the modal?

interface PropsFromState {
    trackedSetDetails: SetDetailDto[];
}

type CardSetSettingsContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSetSettingsContainer extends React.Component<CardSetSettingsContainerProps> {
    constructor(props: CardSetSettingsContainerProps) {
        super(props);
    }

    componentDidMount() {
        //showUntracked, update
        this.props.dispatch(requestTrackedSets(false, false));
    }

    handleRefreshClick(): void {

    }

    handleShowUntrackedClick(): void {
        
    }

    render() {
        return (
            <CardSetSettingsContainerLayout
                onRefreshClick={this.handleRefreshClick}
                onShowUntrackedClick={this.handleShowUntrackedClick}
                trackedSetDetails={this.props.trackedSetDetails}
                showUntrackedValue={true}
            />
        );
    }
}

function mapStateToProps(state: AppState, ownProps): PropsFromState {

    // console.log('attempting to check ownProps');
    // console.log(ownProps);

    const result: PropsFromState = {
        trackedSetDetails: [],
    }
    return result;
}

export default connect(mapStateToProps)(CardSetSettingsContainer);