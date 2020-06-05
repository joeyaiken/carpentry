import {  Table, TableHead, TableRow, TableCell, TableBody, Box, CardActions, Button, Card, CardHeader, CardMedia, CardContent, Typography, IconButton } from '@material-ui/core';
import React from 'react';
import { MoreVert } from '@material-ui/icons';
import { appStyles } from '../../styles/appStyles';

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    // cardOverviews: InventoryOverviewDto[];
    // onCardSelected: (card: InventoryOverviewDto) => void;
    selectedCard: DeckCardOverview | null;

    inventoryCards: DeckCard[];

    onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onMenuClose: () => void;
    // onMenuSelect: (option: string) => void;
}

export default function DeckCardDetail(props: ComponentProps): JSX.Element {
    const { staticSection } = appStyles();
    if(props.selectedCard === null){
        return (
            <Box className={staticSection}>
                <Card>
                    <CardHeader titleTypographyProps={{variant:"body1"}} title={"no card selected"}/>
                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        // className={itemImage} 
                        />
                    <CardContent>
                        <Typography>select a card</Typography>
                    </CardContent>
                    <CardActions>

                    </CardActions>
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
                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        // className={itemImage}
                        image={props.selectedCard.img} />
                    <CardContent>
                        <Table size="small">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Set</TableCell>
                                    <TableCell>Style</TableCell>
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {
                                props.inventoryCards.map(item => {
                                    // const thisCard = props.detail.cards.find(x => x.multiverseId === item.multiverseId);

                                    const rowDeckCardId = item.id;
                                    // console.log(item);
                                    return(
                                    <TableRow key={item.id}>
                                        <TableCell>{item.set}</TableCell>
                                        <TableCell>{item.variantName}{item.isFoil &&" foil"}</TableCell>
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
                            onClick={() => { }} 
                            color="primary" 
                            variant="contained"
                            >Find More</Button>
                    </CardActions>
                </Card>
            </Box>
        );
    }
}