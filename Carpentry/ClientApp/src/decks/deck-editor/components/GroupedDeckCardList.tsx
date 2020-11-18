import React from 'react'
import { Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import ManaCostChip from '../../../common/components/ManaCostChip';
// import ManaCostChip from '../../../_components/ManaCostChip';

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    
    //cardOverviews: InventoryOverviewDto[];
    groupedCardOverviews: CardOverviewGroup[];

    // cardGroups: GroupedInventoryOverview[];

    onCardSelected: (card: DeckCardOverview) => void;
}

export default function GroupedDeckCardList(props: ComponentProps): JSX.Element {
    // className= "flex-section"
    return (
        <Paper>
            <Table size="small">
                {/* <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>Count</TableCell>
                        <TableCell>Type</TableCell>
                        <TableCell>Cost</TableCell>
                        <TableCell>Category</TableCell>
                    </TableRow>
                </TableHead> */}
                {
                    props.groupedCardOverviews.map(group => (
                        <React.Fragment key={group.name}>
                            <TableHead key={`th-${group.name}`}>
                                <TableRow>
                                    <TableCell size="medium"></TableCell>
                                    <TableCell size="medium" colSpan={2}>{group.name} ({group.cardOverviews.length})</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody key={`tb-${group.name}`}>
                                {
                                    group.cardOverviews.map(cardItem => 
                                        <TableRow onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                                            key={cardItem.id+cardItem.name}>
                                            {/* <TableCell>{(cardItem.count > 1) && cardItem.count}</TableCell> */}
                                            <TableCell>{cardItem.count}</TableCell>
                                            <TableCell>{cardItem.name}</TableCell>
                                            {/* <TableCell>{cardItem.count}</TableCell> */}
                                            {/* <TableCell>{cardItem.type}</TableCell> */}
                                            {/* <TableCell>{cardItem.cost}</TableCell> */}
                                            <TableCell><ManaCostChip costString={cardItem.cost} /></TableCell>

                                            {/* <TableCell>{cardItem.description}</TableCell> */}
                                        </TableRow>
                                    )
                                }
                            </TableBody>
                        </React.Fragment>
                    ))
                }

         
            </Table>
        </Paper>
    );
}
