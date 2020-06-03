import React from 'react';
import { Box, CardContent, Typography, CardMedia, CardActions, Button, Card } from '@material-ui/core';
import { appStyles } from '../styles/appStyles';

interface ComponentProps{
    cardOverviews: InventoryOverviewDto[];
}

export default function CardGrid(props: ComponentProps): JSX.Element {
    const classes = appStyles();
    return (
        <React.Fragment>
            <Box className={classes.flexRowWrap}>
                {
                    props.cardOverviews.map(cardItem => 
                        <Card key={cardItem.name} className={classes.outlineSection}>
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                image={cardItem.img}
                                title={cardItem.name} />
                            <Box className={classes.flexRow}>
                                <CardContent className={classes.flexSection}>
                                    <Box className={classes.flexCol}>
                                        <Box className={classes.flexRow}>
                                            {/* {cardItem.count && (<Typography>{cardItem.count} Total</Typography>)} */}
                                            <Typography>
                                                {cardItem.count}
                                                {cardItem.price && ` - $${cardItem.price}`}
                                                {cardItem.isFoil && " (FOIL)"}
                                            </Typography>

                                            
                                        </Box>
                                    </Box>
                                </CardContent>
                            </Box>
                        </Card>
                    )
                }
            </Box>
        </React.Fragment>
    );
}