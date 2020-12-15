import React from 'react';
import { Box, CardHeader, CardMedia, Table, TableHead, TableRow, TableCell, TableBody, Card, Typography } from '@material-ui/core';
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface InventoryDetailProps {
    selectedDetailItem: InventoryDetailDto;
}

interface InventoryDetailCardProps {
    card: MagicCard;
    inventoryCards: InventoryCard[];
}


export default function InventoryDetailLayout(props: InventoryDetailProps): JSX.Element {
    const { flexCol, flexSection, outlineSection, flexRow, staticSection, scrollSection } = appStyles();

    //TODO - Grouping REALLY should be done in a container...
    const displayCards: InventoryDetailCardProps[] = props.selectedDetailItem.cards.map(card => {
        return {
            card: card,
            inventoryCards: props.selectedDetailItem.inventoryCards
                .filter(inventoryCard => inventoryCard.set === card.set && inventoryCard.collectorNumber === card.collectionNumber),
        } as InventoryDetailCardProps;
    });

    return(<React.Fragment>
        <Box className={combineStyles(flexCol, flexSection)}>
            {
                displayCards.map(displayCard => {
                    let img = displayCard.card.imageUrl;

                    let cardTitle = `${displayCard.card.set} (${displayCard.card.collectionNumber}) - $${displayCard.card.price} | $${displayCard.card.priceFoil}`;
                    return (
                        <Card key={displayCard.card.cardId} className={combineStyles(outlineSection, flexCol)}>
                            <CardHeader titleTypographyProps={{variant:"body1"}} style={{textTransform:"uppercase"}} title={cardTitle} />
                            
                            <Box className={combineStyles(flexRow, flexSection)}> 
                                <Box className={staticSection}>
                                    <CardMedia 
                                        style={{height:"310px", width: "223px"}}
                                        // className={itemImage}
                                        image={img}
                                        />
                                </Box>
                                
                                <Box className={combineStyles(flexSection, flexCol)}>
                                    <Box className={scrollSection}style={{overflow:"auto"}}>
                                        {/* 
                                        className="flexSection flexCol"
                                         className="staticSection"
                                         className="flexSection" style={{overflow:"auto"}}
                                          style={{overflow:"auto"}}
                                        */}
                                    <Table size="small" >
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>Style</TableCell>
                                                <TableCell>Status</TableCell>
                                                {/* <TableCell>Action</TableCell> */}
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {
                                                displayCard.inventoryCards.map(item => {
                                                    return(
                                                    <TableRow key={item.id}>
                                                        <TableCell>
                                                            <Typography>{(item.isFoil && " foil") || "normal"}</Typography>
                                                        </TableCell>
                                                        <TableCell>
                                                            <Typography>
                                                                { item.statusId === 1 && "Inventory/Deck" }
                                                                { item.statusId === 2 && "Wish List" }
                                                                { item.statusId === 3 && "Sell List" }
                                                            </Typography>
                                                            {/* {item.deckCards.length > 0 && "In a Deck"}
                                                            {item.deckCards.length === 0 && item.statusId === 1 && "Inventory"}
                                                            { item.statusId === 2 && "Buy List"}
                                                            { item.statusId === 3 && "Sell List"} */}
                                                        </TableCell>

                                                        {/* <TableCell>
                                                            {item.deckCards.length === 0 && item.statusId === 1 && 
                                                                <Button size="small" variant="contained" onClick={() => props.handleUpdateCardClick && props.handleUpdateCardClick(item, 3)}>
                                                                    To Sell List
                                                                </Button>
                                                            }
                                                            { item.statusId === 2 && 
                                                                <Button size="small" variant="contained" onClick={() => props.handleUpdateCardClick && props.handleUpdateCardClick(item, 1)}>
                                                                    Mark as Arrived
                                                                </Button>
                                                            }
                                                            { item.statusId === 3 && 
                                                                <Button size="small" variant="contained" onClick={() => props.handleDeleteCardClick && props.handleDeleteCardClick(item.id)}>
                                                                    Delte
                                                                </Button>
                                                            }
                                                        </TableCell> */}
                                                    </TableRow>
                                                    )
                                                })
                                            }
                                        </TableBody>
                                    </Table>
                                    </Box>
                                    
                                </Box>
                            </Box>
                        </Card> 
                    )
                })
            }
        </Box>
    </React.Fragment>);
}