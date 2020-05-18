import React from 'react';
import {
    Typography,
    Box,
    Card,
    CardHeader,
    CardContent,
} from '@material-ui/core';
import DeckList from '../../containers/DeckList';
import { Link } from 'react-router-dom';

interface LayoutProps {

}

export default function HomeLayout(props: LayoutProps): JSX.Element {
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
                <DeckList />
            </Card>
        </Box>
    );
}
