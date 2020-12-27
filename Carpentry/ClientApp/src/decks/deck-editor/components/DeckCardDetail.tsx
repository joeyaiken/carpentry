import {  Table, TableHead, TableRow, TableCell, TableBody, Box, CardActions, Button, Card, CardHeader, CardMedia, CardContent, Typography, IconButton } from '@material-ui/core';
import React from 'react';
import { MoreVert } from '@material-ui/icons';
import { appStyles } from '../../../styles/appStyles';

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    // cardOverviews: InventoryOverviewDto[];
    // onCardSelected: (card: InventoryOverviewDto) => void;
    selectedCard: DeckCardOverview | null;

    inventoryCards: DeckCardDetail[];

    onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onMenuClose: () => void;
    onCardDetailClick: (cardId: number) => void;
    onCardTagsClick: (cardId: number) => void;
    // onMenuSelect: (option: string) => void;
}

export default function DeckCardDetail(props: ComponentProps): JSX.Element {
    const { staticSection } = appStyles();
    if(props.selectedCard === null){
        return (
            <Box className={staticSection}>
                <Card>
                    <CardHeader titleTypographyProps={{variant:"body1"}} title={"no card selected"}/>
                    <CardContent>
                        <Typography>select a card</Typography>
                    </CardContent>
                </Card>
            </Box>
        );
    }
    else {
        return (
            <Box className={staticSection}>
                
                <Card>
                    {/* <CardHeader 
                        titleTypographyProps={{variant:"body1"}} 
                        title={props.selectedCard.name}
                        // action={
                        //     <Button 
                        //         onClick={() => { }} 
                        //         color="primary" 
                        //         variant="contained"
                        //         >Find More</Button>
                        // }
                    
                    /> */}
                    {
                        props.selectedCard && 
                        <CardMedia style={{height:"310px", width: "223px"}} image={props.selectedCard.img} />
                    }
                    <CardContent>
                        <Table size="small">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Card Detail</TableCell>
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {
                                props.inventoryCards.map(item => {

                                    const rowDeckCardId = item.id;
                                    return(
                                    <TableRow key={item.id}>
                                        {
                                            item.inventoryCardId && 
                                            <TableCell style={{textTransform:"uppercase"}}>
                                                {`${item.set} - ${item.collectorNumber}`}{item.isFoil &&" (foil)"}
                                            </TableCell>
                                        }
                                        {
                                            !item.inventoryCardId &&
                                            <TableCell>
                                                Empty
                                            </TableCell>
                                        }
                                        <TableCell size="small">
                                            {/* <IconButton size="small" onClick={props.onMenuClick} name={item.name} value={item.multiverseId}> */}
                                            {/* item.deckCards[0].id */}
                                            <IconButton size="small" onClick={props.onMenuClick} name={item.name} value={rowDeckCardId}>
                                                <MoreVert />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                    )
                                })
                            }
                            </TableBody>
                        </Table>
                    </CardContent>
                    <CardActions>
                        <Button 
                            onClick={() => props.selectedCard && props.onCardDetailClick(props.selectedCard.cardId)} 
                            color="primary" 
                            variant="contained">Details</Button>
                        <Button 
                            onClick={() => props.selectedCard && props.onCardTagsClick(props.selectedCard.cardId)} 
                            color="primary" 
                            variant="contained">Tags</Button>
                    </CardActions>
                </Card>
            </Box>
        );
    }
}