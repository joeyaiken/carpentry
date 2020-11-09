import React from 'react';
import {
    Typography,
    Box,
    Card,
    CardHeader,
    CardContent,
    IconButton,
    makeStyles,
} from '@material-ui/core';
// import DeckList from '../containers/DeckList/DeckListContainer';
//import {} from '../decks/deck-list/'
// import DeckList from '../_containers/DeckList/DeckListContainer';
import { Link } from 'react-router-dom';
import { ArrowForward } from '@material-ui/icons';
import { futimes } from 'fs';
import DeckListContainer from '../decks/deck-list/DeckListContainer';
// import { appStyles } from '../styles/appStyles';

interface LayoutProps {
    
}

export default function HomeLayout(props: LayoutProps): JSX.Element {
    // const { stretch, flexCol, flexSection, flexRow } = appStyles();

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
            gridTemplateRows: "75px auto"
            
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
                <Typography variant="h6">
                    A deck & inventory management tool for Magic the Gathering
                </Typography>
            </Box>

            <Box  className={localStyles.availableDecks}>
                <DeckListCard />
            </Box>

            <Box className={localStyles.sideTitleContainer}> 
                <TrackedSetsCard />
            </Box>

            <Box className={localStyles.sideContainer}>
                <InventoryCard />
                <TrimmingTipsCard />
                <WishlistHelperCard />
                <BuylistHelperCard />
                
                
                {/* <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Backups"}
                        action={
                            <Link to={'/settings/backups/'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                </Card> */}
            </Box>

            
        </Box>
    );
}


function DeckListCard(): JSX.Element {
    return(
        <Card>
            <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Available Decks"} 
                action={
                    <Link to={'/inventory/sets'}>
                        <IconButton size="medium"><ArrowForward /></IconButton>
                    </Link>
                }
            />
            <DeckListContainer />
        </Card>
    );
}

function TrackedSetsCard(): JSX.Element {
    return (
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
        </Card>
    );
}

function InventoryCard(): JSX.Element {
    return (
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
        </Card>
    );
}

function TrimmingTipsCard(): JSX.Element {
    return(
        <Card>
            <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Trimming Tips"} 
                action={
                    <Link to={'/inventory/trimmingTips'}>
                        <IconButton size="medium"><ArrowForward /></IconButton>
                    </Link>
                }
            />
            <CardContent>
                <Typography>
                    Top 5/10/25/?? trimming tips
                </Typography>
                <Typography>
                    (examples of cards I could / should trim from my collection)
                </Typography>
            </CardContent>
        </Card>
    );
}

function WishlistHelperCard(): JSX.Element {
    return(
        <Card>
            <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Wishlist Helper"} 
                action={
                    <Link to={'/inventory/wishlistHelper'}>
                        <IconButton size="medium"><ArrowForward /></IconButton>
                    </Link>
                }
            />
            <CardContent>
                <Typography>
                    Links to TCG Player / Scryfall / ?? for cards on my wishlist
                </Typography>
                <Typography>
                    Quick links to changing status / deleting
                </Typography>
            </CardContent>
        </Card>
    );
}

function BuylistHelperCard(): JSX.Element {
    return (
        <Card>
            <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Buylist Helper"} 
                action={
                    <Link to={'/inventory/buylistHelper'}>
                        <IconButton size="medium"><ArrowForward /></IconButton>
                    </Link>
                }
            />
            <CardContent>
                <Typography>
                    AKA "Sell List Helper"
                </Typography>
                <Typography>
                    List of cards on sell list, maybe sorted by price
                </Typography>
                <Typography>
                    Ways to delete multiple sell list cards at once
                </Typography>
            </CardContent>
        </Card>
    );
}