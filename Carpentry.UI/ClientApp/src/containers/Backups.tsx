import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../reducers';
import { Typography, Box, CardContent, Card, CardHeader, Paper, Button } from '@material-ui/core';
import DeckList from '../containers/DeckList';
import { Link } from 'react-router-dom';

interface PropsFromState {

}

type BackupsProps = PropsFromState & DispatchProp<ReduxAction>;

class Backups extends React.Component<BackupsProps> {
    constructor(props: BackupsProps) {
        super(props);
    }

    // componentDidMount() {
    //     this.props.dispatch(ensureDeckOverviewsLoaded())
    // }

    render() {
        return (
            <Box>
                <Typography variant="h4">
                    Carpentry Backups
                </Typography>
                <Typography variant="h6">
                    This will be where I backup and restore app data
                </Typography>

                <Typography>
                    Backup Directory
                </Typography>
                <Typography>
                    [dir]
                </Typography>
                <Typography>
                    Last Backup
                </Typography>
                <Typography>
                    [date time]
                </Typography>
                {/* <Typography>
                    [backup] [restore]
                </Typography> */}
                <Box>
                    <Button variant="contained" color="primary">Request Backup</Button><Button color="primary">Request Restore</Button>
                </Box>
                <Paper>
                    [ ---- progress info ---- ]
                </Paper>
                {/* <br/>
                <Card>
                    <CardHeader
                        //titleTypographyProps={{variant:"body1"}}
                        title={"???"}
                    />
                    <CardContent>

                    </CardContent>
                </Card> */}

            </Box>
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(Backups);