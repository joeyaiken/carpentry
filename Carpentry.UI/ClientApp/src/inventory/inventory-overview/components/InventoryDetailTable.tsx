import React from 'react';
import { Table, TableHead, TableRow, TableCell, TableBody, Button } from '@material-ui/core';

interface InventoryDetailTableProps {
    detail: InventoryDetailDto
    //handleAddCardClick?: (inventoryCard: InventoryCard) => void;
    handleUpdateCardClick?: (inventoryCard: InventoryCard, statusId: number) => void;
    handleDeleteCardClick?: (inventoryCardId: number) => void;
}

export default function InventoryDetailTable(props: InventoryDetailTableProps): JSX.Element {
    return(
        <Table size="small">
            <TableHead>
                <TableRow>
                    <TableCell>Set</TableCell>
                    <TableCell>Style</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Action</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {
                    // props.detail.inventoryCards.map(item => {
                    //     const thisCard = props.detail.cards.find(x => x.multiverseId === item.multiverseId);
                    //     // console.log('why is this showing deck');
                    //     // console.log(item.deckCards);
                    //     return(
                    //     <TableRow key={item.id}>
                    //         <TableCell>{thisCard && thisCard.set}</TableCell>
                    //         <TableCell>{item.variantName}{item.isFoil &&" foil"}</TableCell>
                    //         <TableCell>
                    //             {item.deckCards.length > 0 && "In a Deck"}
                    //             {item.deckCards.length === 0 && item.statusId === 1 && "Inventory"}
                    //             { item.statusId === 2 && "Buy List"}
                    //             { item.statusId === 3 && "Sell List"}
                    //         </TableCell>
                    //         <TableCell>
                    //             {/* {item.deckCards.length > 0 && "In a Deck"} */}
                    //             {item.deckCards.length === 0 && item.statusId === 1 && 
                    //                 <Button size="small" variant="contained" onClick={() => props.handleUpdateCardClick && props.handleUpdateCardClick(item, 3)}>
                    //                     To Sell List
                    //                 </Button>
                    //             }
                    //             { item.statusId === 2 && 
                    //                 <Button size="small" variant="contained" onClick={() => props.handleUpdateCardClick && props.handleUpdateCardClick(item, 1)}>
                    //                     Mark as Arrived
                    //                 </Button>
                    //             }
                    //             { item.statusId === 3 && 
                    //                 <Button size="small" variant="contained" onClick={() => props.handleDeleteCardClick && props.handleDeleteCardClick(item.id)}>
                    //                     Delte
                    //                 </Button>
                    //             }
                    //         </TableCell>
                    //         {item.deckCards.length > 0 && <TableCell>In a Deck</TableCell>}
                    //     </TableRow>
                    //     )
                    // })
                }
            </TableBody>
        </Table>
    );
}
