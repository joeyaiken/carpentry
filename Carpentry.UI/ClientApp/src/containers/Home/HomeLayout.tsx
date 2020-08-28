import React from 'react';
import {
    Typography,
    Box,
    Card,
    CardHeader,
    CardContent,
    IconButton,
    Button,
    makeStyles,
} from '@material-ui/core';
import DeckList from '../DeckList/DeckListContainer';
import { Link } from 'react-router-dom';
import { ArrowForward } from '@material-ui/icons';
import { appStyles } from '../../styles/appStyles';

interface LayoutProps {
    
}




export default function HomeLayout(props: LayoutProps): JSX.Element {
    const { stretch, flexCol, flexSection, flexRow } = appStyles();

    const localStyles = makeStyles({

    
    
        // flexRow: {
        //     display: "flex",
        //     flexFlow: "row",
        // },
        // flexRowWrap: {
        //     display: "flex",
        //     flexFlow: "row",
        //     flexWrap: "wrap",
        // },
        // flexCol: {
        //     display: "flex",
        //     flexFlow: "column",
        // },
        // flexContainerVertical: {
        //     display: "flex",
        //     flexFlow: "column",
        // },
        // flexSection: {
        //     flex: '1 1 0%'
        // },
        // staticSection: {
        //     flex: "0 0 0%",
        // },
        // scrollSection: {
        //     flex: "1 1 0%",
        //     overflow: "auto",
        // },
        // outlineSection: {
        //     border: "solid #9E9E9E 1px",
        //     padding: "8px",
        //     margin: "8px",
        //     backgroundColor: "#FFFFFF",
        // },
        // stretch:{
        //     width: '100%',
        //     height: '100%',
        // },
        // sidePadded: {
        //     padding: "0px 8px 0px 8px"
        // },
        // center: {
        //     alignSelf: "center",
        // },
    
        containerLayout: {
            display: 'grid',
            gridTemplateColumns: "75% auto",
            gridTemplateRows: "50px auto"
            
        },
        titleContainer: {
            gridColumnStart: 1,
            gridColumnEnd: 2,
            gridRowStart: 1,
            gridRowEnd: 2,
        },
        sideTitleContainer: {
    
        },
    
        sideContainer: {
    
        },
        availableDecks: {
            gridColumnStart: 1,
            gridColumnEnd: 2,
            gridRowStart: 2,
            gridRowEnd: 0
        },
    })();


    // const styles = localStyles();
    return (
        <Box className={localStyles.containerLayout}>
            <Box className={localStyles.titleContainer}>
                <Typography variant="h4">
                    Carpentry
                </Typography>
                {/* <i className="ms ms-g ms-cost"></i><i className="ms ms-gw ms-cost"></i><i className="ms ms-2g ms-cost"></i> */}
                <Typography variant="h6">
                    A deck & inventory management tool for Magic the Gathering
                </Typography>
                <Box className={flexRow}>
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Inventory"}
                        action={
                            <Link to={'/inventory'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                    {/* <CardContent></CardContent> */}
                </Card>
                
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Tracked Sets"}
                        action={
                            <Link to={'/settings/sets'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                    {/* <CardContent></CardContent> */}
                </Card>

                {/* <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Settings"}
                        action={
                            <Link to={'/settings'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                </Card> */}
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Backups"}
                        action={
                            <Link to={'/settings/backups/'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                </Card>
                {/* <CardHeader
                        
                        title={props.title}
                        action={
                            <IconButton size="medium" onClick={props.onCloseClick}><Close /></IconButton>
                        }
                    /> */}

            </Box>
            
            </Box>
            
            
            <Box  className={localStyles.availableDecks}>
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Available Decks"} 
                        action={
                            <Box>
                                <Button>Import</Button>
                                <Button>New</Button>
                            </Box>
                        }
                    />
                    <DeckList />
                </Card>
            </Box>
            
        </Box>
    );
}
