import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../reducers';
import { Typography, Box, CardContent, Card, CardHeader, Paper } from '@material-ui/core';
import DeckList from '../containers/DeckList';
import { Link } from 'react-router-dom';

interface PropsFromState {

}

type HomeProps = PropsFromState & DispatchProp<ReduxAction>;

class Home extends React.Component<HomeProps> {
    constructor(props: HomeProps) {
        super(props);
    }

    // componentDidMount() {
    //     this.props.dispatch(ensureDeckOverviewsLoaded())
    // }

    render() {
        return (
            <Box>
                <Typography variant="h4">
                    Carpentry
                </Typography>
                <Typography variant="h6">
                    A deck & inventory management tool for Magic the Gathering
                </Typography>
                <br/>
                <Card>
                    <CardHeader
                        //titleTypographyProps={{variant:"body1"}}
                        title={"Dev Links"}
                    />
                    <CardContent>
                        <ul>
                            <li><Link to="/decks/39?view=grid">grid - standard</Link></li>
                            <li><Link to="/decks/38?view=grid">grid - singleton</Link></li>
                            <li><Link to="/decks/38?view=grouped">grouped - singleton</Link></li>
                            <li><Link to="/decks/39?view=grouped">grouped - standard</Link></li>
                            <li><Link to="/decks/38?view=list">list - singleton</Link></li>
                            <li><Link to="/decks/39?view=list">list - standard</Link></li>
                            <li>Something with a companion (maybe a deck of all pending cards?)</li>
                            <li>Inventory</li>
                            <li>Anything for search?</li>
                            <li>Adding a deck?</li>
                        </ul>
                    </CardContent>
                </Card>
                <br/>
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Available Decks"} />

                    {/* <CardContent> */}
                        {/* <Paper> */}
                            <DeckList />
                        {/* </Paper> */}
                    {/* </CardContent> */}
                </Card>
                {/* <Typography variant="h5">
                    Available Decks
                </Typography>
                <DeckList /> */}
            </Box>
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(Home);