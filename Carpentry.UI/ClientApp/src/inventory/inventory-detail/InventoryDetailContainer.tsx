import React from 'react';
import { connect, DispatchProp } from 'react-redux';
import { Box, CardHeader, CardMedia, Table, TableHead, TableRow, TableCell, TableBody, Card } from '@material-ui/core';
import InventoryDetailLayout from './components/InventoryDetailLayout';
import AppModal from '../../common/components/AppModal';
import { requestInventoryDetail } from '../../_actions/inventoryActions';
import { AppState } from '../../_reducers';

import { ensureInventoryDetailLoaded } from './state/InventoryDetailActions';
import { parseConfigFileTextToJson } from 'typescript';

interface OwnProps {
    match: {
        params: {
            cardId: number
        }
    }
}

interface PropsFromState { 
    selectedDetailItem: InventoryDetailDto;
    modalIsOpen: boolean;
    selectedCardId: number;
}

type InventoryDetailContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class InventoryDetailContainer extends React.Component<InventoryDetailContainerProps>{
    constructor(props: InventoryDetailContainerProps) {
        super(props);
        this.handleCloseModalClick = this.handleCloseModalClick.bind(this);
    }

    //on load/bind, ensure cardID is loaded
    componentDidMount() {
        // this.props.dispatch(ensureDeckDetailLoaded(this.props.deckId));
        this.props.dispatch(ensureInventoryDetailLoaded(this.props.selectedCardId))
    }

    handleCloseModalClick(cardId: number | null){
        // this.props.dispatch(requestInventoryDetail(cardId));
    }


    render() {
        // let title: string = (this.props.selectedDetailItem != null) ? 
        // `Inventory Detail - ${this.props.selectedDetailItem.cards[0].name}` : "Inventory Detail - Unknown";
        let cardName = "Unknown";
        if(this.props.selectedDetailItem.cards.length > 0){
            cardName = this.props.selectedDetailItem.cards[0].name;
        }
        return (
            <React.Fragment>
                <AppModal 
                    title={ `Inventory Detail - ${cardName}`}
                    isOpen={this.props.modalIsOpen} 
                    onCloseClick={() => {this.handleCloseModalClick(null)}} 
                    >
                        {
                            this.props.selectedDetailItem &&
                            <InventoryDetailLayout 
                                selectedDetailItem={this.props.selectedDetailItem}
                            />
                        }
                </AppModal>
            </React.Fragment>
        );
    }
}

// interface InventoryDetailProps {
//     selectedDetailItem: InventoryDetailDto;
// }

// interface InventoryDetailCardProps {
//     // set: string;
//     card: MagicCard;
//     // variant: string;
//     inventoryCards: InventoryCard[];
//     //
//     //
//     //
//     //
//     //
// }

// function InventoryDetailLayout(props: InventoryDetailProps): JSX.Element {

//     //need to know all set / variant combos that should be listed

//     //For each card, need to list all relevant variants

//     //  only variants that actually exist in the cards should be included

//     //--How about for a V1, I only do a card per set, not per variant/style


//     // console.log(`inventoryCards ${props.selectedDetailItem.inventoryCards.toString()}}`)
//     // console.log('all sets')
//     // props.selectedDetailItem.inventoryCards.forEach(card =>{
//     //     console.log(card.set);
//     // })
//     const displayCards: InventoryDetailCardProps[] = props.selectedDetailItem.cards.map(card => {
//         // console.log(`inventoryCard ${props.selectedDetailItem.inventoryCard.set}`)
//         // console.log(`inventoryCards ${props.selectedDetailItem.inventoryCards.toString}}`)
//         // console.log(`card ${card.set}`)
//         return {
//             card: card,
//             inventoryCards: props.selectedDetailItem.inventoryCards.filter(inventoryCard => inventoryCard.set === card.set),
//         } as InventoryDetailCardProps;
//     });

//     return(<React.Fragment>
//         {/* I don't remember what was incomplete and what may be dups or something */}

//         <Box className="flex-col flex-section">
//             {
//                 displayCards.map(displayCard => {
//                     let img = displayCard.card.variants['normal'] || '';;
//                     // if(displayCard.card.variants['normal']){
//                     //     img = displayCard.card.variants['normal'] || '';
//                     // }
//                     return (
//                         <Card 
//                             key={displayCard.card.name} 
//                             className="outline-section flex-col"
//                             //style={{overflow:"auto"}}
//                             // onClick={props.onCardSelected}
//                             >
//                             {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={ `${displayCard.card.name} - (${displayCard.card.set})` } /> */}
//                             <CardHeader titleTypographyProps={{variant:"body1"}} style={{textTransform:"uppercase"}} title={ `${displayCard.card.set} (${displayCard.inventoryCards.length})` } />
                            
//                             <Box className="flex-row flex-section"> 
//                                 <Box className="static-section">
//                                     <CardMedia 
//                                         style={{height:"310px", width: "223px"}}
//                                         className="item-image"
//                                         image={img}
//                                         />
//                                 </Box>
                                
//                                 <Box className="flex-section flex-col">
//                                     <Box className="scroll-section" style={{overflow:"auto"}}>
//                                         {/* 
//                                         className="flex-section flex-col"
//                                          className="static-section"
//                                          className="flex-section" style={{overflow:"auto"}}
//                                           style={{overflow:"auto"}}
//                                         */}
//                                     <Table size="small" >
//                                         <TableHead>
//                                             <TableRow>
//                                                 <TableCell>Style</TableCell>
//                                                 <TableCell>Status</TableCell>
//                                                 {/* <TableCell>Action</TableCell> */}
//                                             </TableRow>
//                                         </TableHead>
//                                         <TableBody>
//                                             {
//                                                 displayCard.inventoryCards.map(item => {

//                                                     // const thisCard = displayCard.card;

//                                                     return(
//                                                     <TableRow key={item.id}>
//                                                         <TableCell>{item.variantName}{item.isFoil &&" foil"}</TableCell>
//                                                         <TableCell>
//                                                             {item.deckCards.length > 0 && "In a Deck"}
//                                                             {item.deckCards.length === 0 && item.statusId === 1 && "Inventory"}
//                                                             { item.statusId === 2 && "Buy List"}
//                                                             { item.statusId === 3 && "Sell List"}
//                                                         </TableCell>
//                                                         {/* <TableCell>
//                                                             {item.deckCards.length === 0 && item.statusId === 1 && 
//                                                                 <Button size="small" variant="contained" onClick={() => props.handleUpdateCardClick && props.handleUpdateCardClick(item, 3)}>
//                                                                     To Sell List
//                                                                 </Button>
//                                                             }
//                                                             { item.statusId === 2 && 
//                                                                 <Button size="small" variant="contained" onClick={() => props.handleUpdateCardClick && props.handleUpdateCardClick(item, 1)}>
//                                                                     Mark as Arrived
//                                                                 </Button>
//                                                             }
//                                                             { item.statusId === 3 && 
//                                                                 <Button size="small" variant="contained" onClick={() => props.handleDeleteCardClick && props.handleDeleteCardClick(item.id)}>
//                                                                     Delte
//                                                                 </Button>
//                                                             }
//                                                         </TableCell> */}
//                                                     </TableRow>
//                                                     )
//                                                 })
//                                             }
//                                         </TableBody>
//                                     </Table>
//                                     </Box>
                                    
//                                 </Box>
//                             </Box>
//                         </Card> 
//                     )


//                 })
//             }

//             {/* <InventoryDetailTable 
//                 detail={props.selectedDetailItem}
//                 // handleUpdateCardClick={this.handleUpdateInventoryCard}
//                 // handleDeleteCardClick={this.handleDeleteInventoryCard} 
//                 />
//              */}
//         </Box>
//         {/* I need to just itterate over all cards as a table, and include a DELETE button */}
//         {/* <Box>
//             {
                
//                 props.selectedDetailItem.cards.map(cardInstance => 
//                     <Box>
//                         <Typography>
//                             {cardInstance.name}&nbsp;{cardInstance.set}
//                         </Typography>
//                         <Typography>
//                             {
//                                 props.selectedDetailItem.inventoryCards.filter(item => item.multiverseId === cardInstance.multiverseId).length
//                                 //this.props.selectedDetailItem.items.filter(item => item.multiverseId === cardInstance.multiverseId).length
//                             } Total
//                         </Typography>
//                     </Box>
//                 )
//             }
//         </Box> */}
//     </React.Fragment>);
// }

//Eventually this should be replaced with something different...(like a different container)
function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { inventoryCardsById, inventoryCardAllIds, cardsById, allCardIds } = state.data.inventory.detail;
    const result: InventoryDetailDto = {
        inventoryCards: inventoryCardAllIds.map(invId => inventoryCardsById[invId]),
        cards: allCardIds.map(cardId => cardsById[cardId]),
    }
    return result;
}

//State
function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    const result: PropsFromState = {
        selectedDetailItem: selectInventoryDetail(state),
        // modalIsOpen: state.ui.isInventoryDetailModalOpen, //TODO - get this mapped from router state
        selectedCardId: ownProps.match.params.cardId,

        modalIsOpen: Boolean(ownProps.match.params.cardId != null),
    }
    return result;
}

export default connect(mapStateToProps)(InventoryDetailContainer);



