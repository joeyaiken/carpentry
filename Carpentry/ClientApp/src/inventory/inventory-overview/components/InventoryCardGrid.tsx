import React from 'react';
import { Box, CardContent, Typography, CardMedia, CardActions, Button, Card, IconButton } from '@material-ui/core';
import CardGridContainer from './CardGridContainer';
import { appStyles } from '../../../styles/appStyles';
import { InfoOutlined } from '@material-ui/icons';

interface ComponentProps{
    cardOverviewsById: { [key: number]: InventoryOverviewDto }
    cardOverviewIds: number[];
    onCardSelected: (cardId: number) => void;
    onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onInfoButtonLeave: () => void;
}

export default function InventoryCardGrid(props: ComponentProps): JSX.Element {
    const classes = appStyles();
    return (
        <React.Fragment>
            <CardGridContainer layout="grid">
                { props.cardOverviewIds.map(overviewId => {
                    const cardItem = props.cardOverviewsById[overviewId];
                    return(
                        <Card 
                            title={cardItem.name}
                        key={cardItem.id} 
                        className={classes.outlineSection}>
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                //image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.}&type=card`}
                                image={cardItem.imageUrl}
                                title={cardItem.name} />
                            <Box className={classes.flexRow}>
                                <CardContent className={classes.flexSection}>
                                    <Box className={classes.flexCol}>
                                        { cardItem.totalCount === 0 &&
                                            <Typography>
                                                no cards
                                            </Typography>
                                        }
                                        { Boolean(cardItem.inventoryCount) &&
                                            <Typography>
                                                {`Inventory: ${cardItem.inventoryCount}`}
                                            </Typography>
                                        }
                                        { Boolean(cardItem.deckCount) &&
                                            <Typography>
                                                {`Decks: ${cardItem.deckCount}`}
                                            </Typography>
                                        }
                                        { Boolean(cardItem.sellCount) &&
                                            <Typography>
                                                {`Sell: ${cardItem.sellCount}`}
                                            </Typography>
                                        }
                                        { Boolean(cardItem.totalCount) &&
                                            <Typography>
                                                {`Total: ${cardItem.totalCount}`}
                                            </Typography>
                                        }
                                        { cardItem.isFoil && <Typography>(FOIL)</Typography> }
                                        {/* <Typography>
                                            ${ cardItem.isFoil ? cardItem.priceFoil : cardItem.price }
                                        </Typography> */}
                                        <Typography>${cardItem.price}</Typography>
                                    </Box>
                                </CardContent>
                                <CardActions className={classes.flexSection}>
                                        <Button color="primary" size="small" onClick={() => {props.onCardSelected(cardItem.cardId)}} >
                                            Details
                                        </Button>
                                        <IconButton 
                                            value={cardItem.id} 
                                            color="primary" 
                                            size="small" 
                                            onMouseEnter={props.onInfoButtonEnter}
                                            onMouseLeave={props.onInfoButtonLeave}
                                            >
                                            <InfoOutlined />
                                        </IconButton>
                                </CardActions>
                            </Box>
                        </Card>
                    )} 
                )}
            
            </CardGridContainer>
        </React.Fragment>
    );
}