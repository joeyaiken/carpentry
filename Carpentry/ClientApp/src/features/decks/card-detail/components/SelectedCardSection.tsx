import React from 'react';
import { Box, Card, CardHeader, CardMedia, CardContent, Typography, Button } from '@material-ui/core';
import InventoryDetailTable from './InventoryDetailTable';
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface ComponentProps {
    // selectedCard: CardSearchResultDto;
    selectedCardDetail: InventoryDetailDto | null;
    handleAddInventoryCard: (inventoryCard: InventoryCard) => void;
    handleAddNewCard: (cardName: string, cardId: number, isFoil: boolean) => void;
    handleAddEmptyCard: (cardName: string) => void;
    // handleMoveCard?: (inventoryCard: InventoryCard) => void;
}

export default function SelectedCardSection(props: ComponentProps): JSX.Element {
    // const { outlineSection, flexCol, flexRow, flexSection,  } = appStyles();
    return(<></>);
//     return(
//     // <Paper className={staticSection}>
//     <Box className={flexCol}>
//         <Card className={combineStyles(outlineSection, flexCol)}>
//             <CardHeader titleTypographyProps={{variant:"body1"}} title="Inventory"/>
//             {
//                 //What if this just itterated over the collection of inventory items, and displayed card data?
//                 //At the end of the day, I shouldn't have a ton of each card, including variants
//                 //Should this be Card components, or a table?
//                 //What if I don't allow cards that are already in a deck?

//                 // Inventory detail info will go here
//                 // How many exist of each variant?
//                 // How many foil / non foil?
//                 // How many in existing decks?
//                 props.selectedCardDetail && //props.selectedCardDetail.inventoryCards &&
//                 <InventoryDetailTable detail={props.selectedCardDetail} handleAddCardClick={props.handleAddInventoryCard} />
                
//             }
//             {/* Each inventory card should have a label for (in # decks) */}
//         </Card>
// {/* 
//         <Card className={outlineSection}>
//             <CardHeader titleTypographyProps={{variant:"body1"}} title="Add Empty"/>
//         </Card> */}

//         <Card className={combineStyles(outlineSection, flexCol)}>
//             {/* <CardHeader 
//                 titleTypographyProps={{variant:"body1"}} 
//                 title="Add New"
//                 action={
//                     <Button variant="outlined" onClick={() => props.handleAddEmptyCard(props.selectedCard.name)}>
//                         Add Empty
//                     </Button>
//                 }/> */}
            
//             <Box className={combineStyles(flexCol, flexSection)}>
//             {
//                 displayCards.map(displayCard => {
//                     let img = displayCard.card.imageUrl;

//                     let cardTitle = `${displayCard.card.set} (${displayCard.card.collectionNumber}) - $${displayCard.card.price} | $${displayCard.card.priceFoil}`;
//                     return (
//                         <Card key={displayCard.card.cardId} className={combineStyles(outlineSection, flexCol)}>
//                             <CardHeader titleTypographyProps={{variant:"body1"}} style={{textTransform:"uppercase"}} title={cardTitle} />
                            
//                             <Box className={combineStyles(flexRow, flexSection)}> 
//                                 <Box className={staticSection}>
//                                     <CardMedia 
//                                         style={{height:"310px", width: "223px"}}
//                                         // className={itemImage}
//                                         image={img}
//                                         />
//                                 </Box>
                                
//                                 <Box className={combineStyles(flexSection, flexCol)}>
//                                     <Box className={scrollSection}style={{overflow:"auto"}}>
//                                         {/* 
//                                         className="flexSection flexCol"
//                                          className="staticSection"
//                                          className="flexSection" style={{overflow:"auto"}}
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
//                                                     return(
//                                                     <TableRow key={item.id}>
//                                                         <TableCell>
//                                                             <Typography>{(item.isFoil && " foil") || "normal"}</Typography>
//                                                         </TableCell>
//                                                         <TableCell>
//                                                             <Typography>
//                                                                 { item.statusId === 1 && "Inventory/Deck" }
//                                                                 { item.statusId === 2 && "Wish List" }
//                                                                 { item.statusId === 3 && "Sell List" }
//                                                             </Typography>
//                                                             {/* {item.deckCards.length > 0 && "In a Deck"}
//                                                             {item.deckCards.length === 0 && item.statusId === 1 && "Inventory"}
//                                                             { item.statusId === 2 && "Buy List"}
//                                                             { item.statusId === 3 && "Sell List"} */}
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
//         </Box>
    

//             {/* {   
//                 //Object.keys(props.selectedCard.variants).map((variant: string) => {
//                 props.selectedCard.details.map(detail => {
//                 return (
//                     // <Card key={id} className= "outline-section flex-col">
//                         // <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
//                         <Card key={detail.cardId} className={combineStyles(outlineSection, flexRow)}>
//                             <CardHeader titleTypographyProps={{variant:"body1"}} title={`${detail.setCode}-${detail.collectionNumber}`} />
//                             <CardMedia 
//                                 style={{height:"310px", width: "223px"}}
//                                 // className={itemImage}
//                                 image={detail.imageUrl || undefined} />
//                             <CardContent>
//                                 <Box className={flexCol}>
//                                     <Box className={flexCol}>
//                                         <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
//                                     </Box>
//                                     <Box className={flexCol}>
//                                         <Button variant="outlined" onClick={() => props.handleAddNewCard(detail.name, detail.cardId, false)}>
//                                             Add Normal
//                                         </Button>
//                                         <Button variant="outlined" onClick={() => props.handleAddNewCard(detail.name, detail.cardId, true)}>
//                                             Add Foil
//                                         </Button>
//                                     </Box>
//                                 </Box>
//                             </CardContent>
//                         </Card>
//                     // </Card>
//                 )})
//             } */}
//         </Card>
//     </Box>
// // </Paper>
// );
}

