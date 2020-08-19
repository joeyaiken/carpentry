import React from 'react';
import { Box, CardContent, Typography, CardMedia, CardActions, Button, Card } from '@material-ui/core';
// import VisualCard from './VisualCard';
import CardGridContainer from './CardGridContainer';
// import GridCard from './GridCard';

import { appStyles } from '../../styles/appStyles';

interface ComponentProps{
    cardOverviews: InventoryOverviewDto[];
    onCardSelected: (cardId: number) => void;
}

export default function InventoryCardGrid(props: ComponentProps): JSX.Element {
    const classes = appStyles();
    return (
        <React.Fragment>
            <CardGridContainer layout="grid">
                {
                    props.cardOverviews.map(cardItem => 
                        <Card key={cardItem.name} className={classes.outlineSection}>
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                //image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.}&type=card`}
                                image={cardItem.img}
                                title={cardItem.name} />
                            <Box className={classes.flexRow}>
                                <CardContent className={classes.flexSection}>
                                    <Box className={classes.flexCol}>
                                        <Box className={classes.flexRow}>
                                            {cardItem.count && (<Typography>{cardItem.count} Total</Typography>)}
                                        </Box>
                                    </Box>
                                </CardContent>
                                <CardActions className={classes.flexSection}>
                                    <Button color="primary" size="small" onClick={() => {props.onCardSelected(cardItem.id)}} >
                                        Details
                                    </Button>
                                </CardActions>
                            </Box>
                        </Card>
                    )
                }
            </CardGridContainer>
        </React.Fragment>
    );
}