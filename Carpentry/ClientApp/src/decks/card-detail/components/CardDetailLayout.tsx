import { Box, Button, Card, CardHeader, CardMedia, Paper, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@material-ui/core';
import React from 'react';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import InventoryDetailTable from './InventoryDetailTable';
// import SelectedCardSection from './SelectedCardSection';

interface ContainerLayoutProps {

    selectedCardId: number;
    allCardIds: number[];
    cardsById: { [cardId: number]: MagicCard }
    inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
    cardGroups: { [cardId: number]: number[] }

    // handleAddNewCardClick: (cardName: string, cardId: number, isFoil: boolean) => void;
    // handleAddExistingCardClick: (inventoryCard: InventoryCard) => void;
    // handleAddEmptyCard: (cardName: string) => void;

}

export default function CardDetailLayout(props: ContainerLayoutProps): JSX.Element {
    const { outlineSection, flexCol, flexRow, flexSection, staticSection, scrollSection,  } = appStyles();
    return(
    <React.Fragment>

        <Box className={outlineSection}>
            Deck Cards
            <Paper>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Card</TableCell>
                            <TableCell>Category</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    
                    {/* 
                        <IconButton size="small" onClick={props.onMenuClick} name={item.name} value={rowDeckCardId}>
                            <MoreVert />
                        </IconButton>
                    */}

                    <TableRow>
                        <TableCell>Set/#/Foil | empty</TableCell>
                        <TableCell>Mainboard|Sideboard|Commander</TableCell>
                        <TableCell>[actions]</TableCell>
                    </TableRow>

                    <TableRow>
                        <TableCell>Empty</TableCell>
                        <TableCell>Commander</TableCell>
                        <TableCell>[actions]</TableCell>
                    </TableRow>
                </Table>
            </Paper>
        </Box>



        {/* Inventory Card section */}
        <Box className={flexCol}>

        <Card className={combineStyles(outlineSection, flexCol)}>
            <CardHeader 
                titleTypographyProps={{variant:"body1"}} 
                title="Inventory"
                action={
                    <Button variant="outlined" 
                    // onClick={() => props.handleAddEmptyCard(props.selectedCard.name)}
                    >
                        Add Empty
                    </Button>
                }/>
            
            <Box className={combineStyles(flexCol, flexSection)}>
            {
                props.allCardIds.map(cardId => {
                    let card = props.cardsById[cardId];
                    let inventoryCardIds = props.cardGroups[cardId];
                    let img = card.imageUrl;

                    let cardTitle = `${card.set} (${card.collectionNumber}) - $${card.price} | $${card.priceFoil}`;

                    return (
                        <Card key={card.cardId} className={combineStyles(outlineSection, flexCol)}>
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
                                                inventoryCardIds.map(inventoryCardId => {
                                                    const item = props.inventoryCardsById[inventoryCardId];

                                                    return(
                                                    <TableRow key={item.id}>
                                                        <TableCell>
                                                            <Typography>{(item.isFoil && " foil") || "normal"}</Typography>
                                                        </TableCell>
                                                        <TableCell>
                                                            <Typography>

                                                                { item.deckId && item.deckName }
                                                                { !item.deckId && "Inventory" }
                                                                {/* { item.statusId === 1  && "Inventory/Deck" }
                                                                { item.statusId === 2 && "Wish List" }
                                                                { item.statusId === 3 && "Sell List" } */}
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
    

            {/* {   
                //Object.keys(props.selectedCard.variants).map((variant: string) => {
                props.selectedCard.details.map(detail => {
                return (
                    // <Card key={id} className= "outline-section flex-col">
                        // <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
                        <Card key={detail.cardId} className={combineStyles(outlineSection, flexRow)}>
                            <CardHeader titleTypographyProps={{variant:"body1"}} title={`${detail.setCode}-${detail.collectionNumber}`} />
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                // className={itemImage}
                                image={detail.imageUrl || undefined} />
                            <CardContent>
                                <Box className={flexCol}>
                                    <Box className={flexCol}>
                                        <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
                                    </Box>
                                    <Box className={flexCol}>
                                        <Button variant="outlined" onClick={() => props.handleAddNewCard(detail.name, detail.cardId, false)}>
                                            Add Normal
                                        </Button>
                                        <Button variant="outlined" onClick={() => props.handleAddNewCard(detail.name, detail.cardId, true)}>
                                            Add Foil
                                        </Button>
                                    </Box>
                                </Box>
                            </CardContent>
                        </Card>
                    // </Card>
                )})
            } */}
        </Card>
    </Box>




    </React.Fragment>);
}