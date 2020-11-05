import { connect, DispatchProp } from 'react-redux'
import React from 'react'
// import { AppState } from '../reducers';
// import { Typography, Box, CardContent, Card, CardHeader, Paper } from '@material-ui/core';
// import DeckList from '../containers/DeckList/DeckListContainer';
// import { Link } from 'react-router-dom';
import HomeLayout from './HomeLayout';
import { AppState } from '../_reducers';

interface PropsFromState {

}

type HomeProps = PropsFromState & DispatchProp<ReduxAction>;

class HomeContainer extends React.Component<HomeProps> {
    // constructor(props: HomeProps) {
    //     super(props);
    // }

    // componentDidMount() {
    //     this.props.dispatch(ensureDeckOverviewsLoaded())
    //  eventually this should ensure data for all cards is loaded
    // }

    render() {
        return (
            <HomeLayout />
           
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(HomeContainer);