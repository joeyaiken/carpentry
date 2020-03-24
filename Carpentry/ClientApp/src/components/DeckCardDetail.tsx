import {  Paper, Table, TableHead, TableRow, TableCell, TableBody, Box, CardActions, Button, Card, CardHeader, CardMedia, CardContent, Typography, IconButton, Menu, MenuItem } from '@material-ui/core';
import React from 'react';
import { MoreVert } from '@material-ui/icons';

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    // cardOverviews: InventoryOverviewDto[];
    // onCardSelected: (card: InventoryOverviewDto) => void;
    selectedCard: InventoryOverviewDto | null;

    inventoryCards: InventoryCard[];
    cardMenuAnchor: HTMLButtonElement | null;

    onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onMenuClose: () => void;
    onMenuSelect: (option: string) => void;
}

export default function DeckCardDetail(props: ComponentProps): JSX.Element {
    if(props.selectedCard == null){
        return (
            <Box className="static-section">
                <Card>
                    <CardHeader titleTypographyProps={{variant:"body1"}} title={"no card selected"}/>
                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        className="item-image" />
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
            <Box className="static-section">
                <Menu open={Boolean(props.cardMenuAnchor)} onClose={props.onMenuClose} anchorEl={props.cardMenuAnchor} >
                    <MenuItem onClick={() => {props.onMenuSelect("")}}>Set as Commander</MenuItem>
                    <MenuItem onClick={() => {props.onMenuSelect("")}}>Move to Sideboard</MenuItem>
                    <MenuItem onClick={() => {props.onMenuSelect("")}}>Move to Main Deck</MenuItem>
                    <MenuItem onClick={() => {props.onMenuSelect("")}}>Remove from Deck</MenuItem>
                    {/* <MenuItem onClick={() => {props.onMenuSelect("")}}></MenuItem> */}
                    {/* <MenuItem onClick={() => {props.onMenuSelect("")}}></MenuItem> */}
                    {/* <MenuItem onClick={() => {props.onMenuSelect("")}}></MenuItem> */}
                    {/* <MenuItem onClick={() => {props.onMenuSelect("edit")}} value="inventory">Edit</MenuItem>
                    <MenuItem onClick={() => {props.onMenuSelect("delete")}} value="delete">Delte</MenuItem> */}
                    {/* Export? */}
                    {/* Copy? */}
                </Menu>
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
                        className="item-image"
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

                                    return(
                                    <TableRow key={item.id}>
                                        <TableCell>{item.set}</TableCell>
                                        <TableCell>{item.variantName}{item.isFoil &&" foil"}</TableCell>
                                        <TableCell size="small">
                                            <IconButton size="small" onClick={props.onMenuClick} name={item.name} value={item.multiverseId}>
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