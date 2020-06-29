import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../../reducers';
import { Typography, Box, CardContent, Card, CardHeader, Paper, Button } from '@material-ui/core';
import DeckList from '../DeckList/DeckListContainer';
import { Link } from 'react-router-dom';
import CardSetSettingsContainerLayout from './CardSetSettingsContainerLayout';

//Should this manage the modal?

interface PropsFromState {

}

type CardSetSettingsContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSetSettingsContainer extends React.Component<CardSetSettingsContainerProps> {
    constructor(props: CardSetSettingsContainerProps) {
        super(props);
    }

    componentDidMount() {
        // this.props.dispatch(ensureDeckOverviewsLoaded())
        //load 
    }

    render() {
        return (
            <CardSetSettingsContainerLayout />
        );
    }
}

function mapStateToProps(state: AppState, ownProps): PropsFromState {

    // console.log('attempting to check ownProps');
    // console.log(ownProps);

    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(CardSetSettingsContainer);