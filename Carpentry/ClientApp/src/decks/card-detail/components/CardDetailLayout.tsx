import { Box, Button, Card, CardHeader, CardMedia, IconButton, Paper, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@material-ui/core';
import { MoreVert } from '@material-ui/icons';
import React from 'react';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import CardMenu from '../../deck-editor/components/CardMenu';
import InventoryDetailTable from './InventoryDetailTable';
// import SelectedCardSection from './SelectedCardSection';

interface ContainerLayoutProps {

    selectedCardId: number;
    allCardIds: number[];
    cardsById: { [cardId: number]: MagicCard }
    inventoryCardsById: { [inventoryCardId: number]: InventoryCard }
    cardGroups: { [cardId: number]: number[] }

    //deck cards
    deckCardDetailsById: { [deckCardId: number]: DeckCardDetail };
    activeDeckCardIds: number[];

    // handleAddNewCardClick: (cardName: string, cardId: number, isFoil: boolean) => void;
    // handleAddExistingCardClick: (inventoryCard: InventoryCard) => void;
    onAddEmptyCardClick: () => void;

    //menu anchor
    // menuAnchor: HTMLButtonElement | null;
    // menuAnchorId: number;
    // // selectedCard: DeckCardOverview | null;
    // // selectedInventoryCards: DeckCardDetail[];
    // onCardMenuSelected: (name: DeckEditorCardMenuOption) => void;
    onInventoryCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onDeckCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    // onCardMenuClosed: () => void;
}

export default function CardDetailLayout(props: ContainerLayoutProps): JSX.Element {
    const { outlineSection, flexCol, flexRow, flexSection, staticSection, scrollSection,  } = appStyles();
    return(
    <React.Fragment>
        {/* <CardMenu 
            cardMenuAnchor={props.menuAnchor} 
            onCardMenuSelect={props.onCardMenuSelected} 
            onCardMenuClose={props.onCardMenuClosed} 
            cardCategoryId={''}
            hasInventoryCard={false}
            // cardCategoryId={props.selectedCard?.category || ''}
            // hasInventoryCard={Boolean(props.cardDetailsById[props.cardMenuAnchorId]?.inventoryCardId)}
            /> */}
        <Box className={outlineSection}>
            Deck Cards
            <Paper>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell>Card</TableCell>
                            <TableCell>Category</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    {
                        props.activeDeckCardIds.map(id => {
                            const deckCard = props.deckCardDetailsById[id];
                            
                            return(
                            <TableRow>
                                <TableCell>{ deckCard.inventoryCardId ? `${deckCard.set} (${deckCard.collectorNumber}) ${deckCard.isFoil ? 'Foil' : 'Normal'}`  : "Empty"  }</TableCell>
                                <TableCell>{deckCard.category}</TableCell>
                                <TableCell>
                                    <IconButton size="small" value={id} onClick={props.onDeckCardMenuClick}>
                                        <MoreVert />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                            )
                        })
                    }
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
                    onClick={() => props.onAddEmptyCardClick()}
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
                                    <CardMedia style={{height:"310px", width: "223px"}} image={img} />
                                </Box>
                                <Box className={combineStyles(flexSection, flexCol)}>
                                    <Box className={scrollSection}style={{overflow:"auto"}}>
                                    <Table size="small" >
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>Style</TableCell>
                                                <TableCell>Status</TableCell>
                                                <TableCell>Action</TableCell>
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
                                                            </Typography>
                                                        </TableCell>
                                                        
                                                        <TableCell>
                                                            <IconButton size="small" value={item.id} onClick={props.onInventoryCardMenuClick}>
                                                                <MoreVert />
                                                            </IconButton>
                                                        </TableCell>
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
        <Box>Ensuring this is visible...</Box>
    

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