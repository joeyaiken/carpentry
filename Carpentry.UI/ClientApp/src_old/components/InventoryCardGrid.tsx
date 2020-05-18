import React from 'react';
import { Box, CardContent, Typography, CardMedia, CardActions, Button, Card } from '@material-ui/core';
// import VisualCard from './VisualCard';
import CardGridContainer from './CardGridContainer';
// import GridCard from './GridCard';

interface ComponentProps{
    cardOverviews: InventoryOverviewDto[];
    onCardSelected: (cardName: string) => void;
}

export default function InventoryCardGrid(props: ComponentProps): JSX.Element {
    return (
        <React.Fragment>
            <CardGridContainer layout="grid">
                {
                    props.cardOverviews.map(cardItem => 
                        <Card key={cardItem.name} className="outline-section">
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                className="item-image"
                                //image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.}&type=card`}
                                image={cardItem.img}
                                title={cardItem.name} />
                            <Box className="flex-row">
                                <CardContent className="flex-section">
                                    <Box className="flex-col ">
                                        <Box className="flex-row">
                                            {cardItem.count && (<Typography>{cardItem.count} Total</Typography>)}
                                        </Box>
                                    </Box>
                                </CardContent>
                                <CardActions className="flex-section">
                                    <Button color="primary" size="small" onClick={() => {props.onCardSelected(cardItem.name)}} >
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