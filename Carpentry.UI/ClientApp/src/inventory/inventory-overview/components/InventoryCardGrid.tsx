import React from 'react';
import { Box, CardContent, Typography, CardMedia, CardActions, Button, Card } from '@material-ui/core';
// import VisualCard from './VisualCard';
import CardGridContainer from './CardGridContainer';
// import GridCard from './GridCard';

import { appStyles } from '../../../styles/appStyles';
import { Link } from 'react-router-dom';

interface ComponentProps{
    cardOverviews: InventoryOverviewDto[];
    onCardSelected: (cardId: number) => void;
}

function DetailsButton(): JSX.Element {
    return (
    <Button color="primary" size="small">
        Details
    </Button>
    );
}

export default function InventoryCardGrid(props: ComponentProps): JSX.Element {
    const classes = appStyles();
    // const detailsButton: JSX.Element = () => {
    //     return (<></>)
    // }

    


    return (
        <React.Fragment>
            <CardGridContainer layout="grid">
                {
                    props.cardOverviews.map(cardItem => 
                        <Card key={ `${cardItem.id}${cardItem.isFoil}`} className={classes.outlineSection}>
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                //image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.}&type=card`}
                                image={cardItem.img}
                                title={cardItem.name} />
                            <Box className={classes.flexRow}>
                                <CardContent className={classes.flexSection}>
                                    <Box className={classes.flexCol}>
                                        <Box className={classes.flexRow}>
                                            {cardItem.count && (<Typography>{cardItem.count} Total {cardItem.isFoil && " - (FOIL)"}</Typography>)}
                                        </Box>
                                        <Box className={classes.flexRow}>
                                            {cardItem.price && (<Typography>${cardItem.price}</Typography>)}
                                        </Box>
                                        {/* <Box className={classes.flexRow}>
                                            <Typography>${cardItem.id}</Typography>
                                        </Box> */}
                                    </Box>


                                </CardContent>
                                <CardActions className={classes.flexSection}>

                                    {/* <Link to={'/settings/sets'}>
                                        <IconButton size="medium"><ArrowForward /></IconButton>
                                    </Link>
                                    
                                    // history.push(`/inventory/${cardId}`)
                                    */}


                                    {/* This needs to be swapped from an action to a link */}
                                    {/* <Link to={`/inventory/${cardItem.id}`}>
                                        <Button color="primary" size="small" onClick={() => {props.onCardSelected(cardItem.id)}} >
                                            Details
                                        </Button>
                                    </Link> */}
                                    {/* <Link 
                                        to={`/inventory/${cardItem.id}`}
                                        component={DetailsButton}
                                        // variant="contained"
                                    /> */}
                                        <Button color="primary" size="small" onClick={() => {props.onCardSelected(cardItem.id)}} >
                                            Details
                                        </Button>
                                    {/* </Link> */}
                                </CardActions>
                            </Box>
                        </Card>
                    )
                }
            </CardGridContainer>
        </React.Fragment>
    );
}