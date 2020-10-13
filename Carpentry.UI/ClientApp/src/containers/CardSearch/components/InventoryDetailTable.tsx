import React from 'react';
import { Table, TableHead, TableRow, TableCell, TableBody, Button } from '@material-ui/core';

interface InventoryDetailTableProps {
    detail: InventoryDetailDto
    handleAddCardClick?: (inventoryCard: InventoryCard) => void;
}

export default function InventoryDetailTable(props: InventoryDetailTableProps): JSX.Element {
    return(
        <Table size="small">
            <TableHead>
                <TableRow>
                    <TableCell>Set</TableCell>
                    <TableCell>Style</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell></TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {/* {
                    props.detail.inventoryCards.map(item => {
                        const thisCard = props.detail.cards.find(x => x.multiverseId === item.multiverseId);

                        return(
                        <TableRow key={item.id}>
                            <TableCell>{thisCard && thisCard.set}</TableCell>
                            <TableCell>{item.variantName}{item.isFoil &&" foil"}</TableCell>
                            <TableCell>
                                {
                                    item.deckCards.length > 0 &&
                                    item.deckCards[0].deckName
                                }
                                {
                                    item.deckCards.length === 0 &&
                                    item.statusId === 1 &&
                                    "Inventory"
                                }
                                {
                                    item.statusId === 2 &&
                                    "Buy List"
                                }
                                {
                                    item.statusId === 2 &&
                                    "Sell List"
                                }
                            </TableCell>
                            {
                                item.deckCards.length === 0 && 
                                <TableCell>
                                    <Button size="small" variant="contained"
                                        onClick={() => props.handleAddCardClick && props.handleAddCardClick(item)}
                                    >Add</Button>
                                </TableCell>
                            }
                            {
                                item.deckCards.length === 1 && 
                                <TableCell>
                                    <Button size="small" variant="contained"
                                        onClick={() => props.handleAddCardClick && props.handleAddCardClick(item)}
                                    >Move</Button>
                                </TableCell>
                            }
                            {item.deckCards.length > 1 && <TableCell>In a Deck</TableCell>}
                        </TableRow>
                        )
                    })
                } */}
            </TableBody>
        </Table>
    );
}
