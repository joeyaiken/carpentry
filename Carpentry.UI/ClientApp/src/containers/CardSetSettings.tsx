import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../reducers';
import { Typography, Box, CardContent, Card, CardHeader, Paper, Button } from '@material-ui/core';
import DeckList from './DeckList/DeckListContainer';
import { Link } from 'react-router-dom';

//Should this manage the modal?

interface PropsFromState {

}

type CardSetSettingsProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSetSettings extends React.Component<CardSetSettingsProps> {
    constructor(props: CardSetSettingsProps) {
        super(props);
    }

    componentDidMount() {
        // this.props.dispatch(ensureDeckOverviewsLoaded())
        //load 
    }

    render() {
        return (
            <Box>
                
                <Box>
                    <Typography variant="h4">
                        Tracked Sets
                    </Typography>
                    [Add] [Update All]
                </Box>
                <Box>
                    [Table of set results]
                </Box>
            </Box>
        );
    }
}

function mapStateToProps(state: AppState, ownProps): PropsFromState {

    console.log('attempting to check ownProps');
    console.log(ownProps);

    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(CardSetSettings);