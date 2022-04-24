import React from 'react'
import { Paper, Table, TableHead, TableRow, TableCell, TableBody, IconButton, Button } from '@material-ui/core';
// import ManaCostChip from '../../../common/components/ManaCostChip';
import { Star } from '@material-ui/icons';
import ManaCostChip from "../../../../common/components/ManaCostChip";
// import ManaCostChip from '../../../_components/ManaCostChip';

interface ComponentProps{

    // groupedCardOverviews: selectDeckOverviews(state),

    // function selectDeckOverviews(state: AppState): CardOverviewGroup[] {
    //     const { cardOverviews, cardGroups } = state.decks.data.detail; //state.data.deckDetail;
    //    
    //     if(state.decks.deckEditor.viewMode === "grouped"){
    //         const result = cardGroups.map(group => {
    //             const groupResult: CardOverviewGroup = {
    //                 name: group.name,
    //                 cardOverviews: group.cardOverviewIds.map(id => cardOverviews.byId[id]),
    //             }
    //             return groupResult;
    //         });
    //         return result;
    //    
    //     } else {
    //    
    //         return [{
    //             name: "All",
    //             cardOverviews: cardOverviews.allIds.map(id => cardOverviews.byId[id]),
    //         }];
    //    
    //     }
    // }
    groupedCardOverviews: CardOverviewGroup[];

    // cardDetailsById: state.decks.data.detail.cardDetails.byId,
    cardDetailsById: { [deckCardId: number]: DeckCardDetail };
    
    
    
    
    onCardSelected: (card: DeckCardOverview) => void;
    onCardDetailClick: (cardId: number) => void;
    onCardTagsClick: (cardId: number) => void;
}

export default function GroupedDeckCardList(props: ComponentProps): JSX.Element {
    return (
        <Paper>
            <Table size="small">
                {
                    props.groupedCardOverviews.map(group => (
                        <React.Fragment key={group.name}>
                            <TableHead key={`th-${group.name}`}>
                                <TableRow>
                                    <TableCell size="medium"></TableCell>
                                    <TableCell size="medium" colSpan={4}>{group.name} ({group.cardOverviews.length})</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody key={`tb-${group.name}`}>
                                {
                                    group.cardOverviews.map(cardItem => 
                                        <TableRow onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                                            key={cardItem.id+cardItem.name}>
                                            <TableCell>{cardItem.count}</TableCell>
                                            <TableCell>{cardItem.name}</TableCell>
                                            <TableCell>
                                                {
                                                    Boolean(cardItem.tags.length) ? 
                                                    <Button variant="outlined" style={{textTransform:"none"}} onClick={()=>{props.onCardTagsClick(cardItem.cardId)}} >
                                                        {cardItem.tags.toString()}
                                                    </Button>                                                    
                                                    :
                                                    <Button style={{textTransform:"none"}} onClick={()=>{props.onCardTagsClick(cardItem.cardId)}}>
                                                        untagged
                                                    </Button>
                                                }
                                            </TableCell>
                                            <TableCell><ManaCostChip costString={cardItem.cost} /></TableCell>
                                            <TableCell>
                                            <Button color="inherit" onClick={()=>{props.onCardDetailClick(cardItem.cardId)}} >
                                                {
                                                    cardItem.detailIds.map(id => {
                                                        const cardDetail = props.cardDetailsById[id];
                                                        let color = GetAvailabilityColor(cardDetail.availabilityId);
                                                        return(<Star style={{color: color}} key={id} />);
                                                    })
                                                }
                                            </Button>
                                            </TableCell>
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

function GetAvailabilityColor(availabilityId: number): string {
    switch(availabilityId){
        case 1: return "green";
        case 2: return "yellow";
        case 3: return "orange";
        case 4: return "red";
        default: return "blue";
    }
}