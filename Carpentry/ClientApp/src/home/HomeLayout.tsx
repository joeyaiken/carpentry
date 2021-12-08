import React from 'react';
import { Add, ArrowForward, SaveAlt } from '@material-ui/icons';
import DeckListContainer from '../decks/deck-list/DeckListContainer';
import AppLayout from '../common/components/AppLayout';
import {
    Typography,
    Box,
    Card,
    CardHeader,
    IconButton,
    makeStyles,
} from '@material-ui/core';

interface LayoutProps {
    onAddClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onImportClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onSettingsClick: () => void;
    onInventoryClick: () => void;
}

export default function HomeLayout(props: LayoutProps): JSX.Element {
    const localStyles = makeStyles({
        containerLayout: {
            display: 'grid',
            gridTemplateColumns: "auto 180px 180px",
            gridTemplateRows: "75px auto",
        },
        titleContainer: {
            gridColumnStart: 1,
            gridColumnEnd: 2,
            gridRowStart: 1,
            gridRowEnd: 2,
        },
        availableDecks: {
            gridColumnStart: 1,
            gridColumnEnd: 4,
            gridRowStart: 2,
            gridRowEnd: 0
        },
    })();

    return (
        <AppLayout title="Carpentry">
            <Box id="home-container" className={localStyles.containerLayout}>
                <Box className={localStyles.titleContainer}>
                    <Typography variant="h4" id="title">
                        Carpentry
                    </Typography>
                    <Typography variant="h6" id="subtitle">
                        A deck & inventory management tool for Magic the Gathering
                    </Typography>
                </Box>

                <Box className={localStyles.availableDecks}>
                    <Card>
                        <CardHeader
                            titleTypographyProps={{variant:"h5"}}
                            title={"Available Decks"} 
                            action={
                                <>
                                    <IconButton className="add-deck-button" size="medium" onClick={props.onAddClick}><Add /></IconButton>
                                    <IconButton className="import-deck-button" size="medium" onClick={props.onImportClick}><SaveAlt /></IconButton>
                                </>
                            } />
                        <DeckListContainer />
                    </Card>
                </Box>

                <Box> 
                    <Card>
                        <CardHeader
                            titleTypographyProps={{variant:"h5"}}
                            title={"Settings"}
                            action={
                                <IconButton size="medium" onClick={props.onSettingsClick}><ArrowForward /></IconButton>
                            } />
                    </Card>
                </Box>

                <Box>
                    <Card>
                        <CardHeader
                            titleTypographyProps={{variant:"h5"}}
                            title={"Inventory"}
                            action={
                                <IconButton size="medium" onClick={props.onInventoryClick}><ArrowForward /></IconButton>
                            } />
                    </Card>
                </Box>
            </Box>
        </AppLayout>
    );
}