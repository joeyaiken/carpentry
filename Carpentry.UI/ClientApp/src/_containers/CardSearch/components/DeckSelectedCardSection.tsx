import React from 'react';
import { Box, Paper, Card, CardHeader, CardMedia, CardContent, Typography, Button } from '@material-ui/core';
import InventoryDetailTable from './InventoryDetailTable';
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface SelectedCardDetailSectionProps {
    selectedCard: CardSearchResultDto;
    // pendingCards?: PendingCardsDto;
    selectedCardDetail: InventoryDetailDto | null;
    handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    handleAddInventoryCard?: (inventoryCard: InventoryCard) => void;
    handleAddNewCard?: (cardId: number, isFoil: boolean) => void;
    // handleMoveCard?: (inventoryCard: InventoryCard) => void;
}

export default function DeckSelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
    const { outlineSection, flexCol, staticSection, flexRow, } = appStyles();
    
    return(
    // <Paper className={staticSection}>
    <Box className={flexCol}>
        <Card className={combineStyles(outlineSection, flexCol)}>
            <CardHeader titleTypographyProps={{variant:"body1"}} title="Inventory"/>
            {
                //What if this just itterated over the collection of inventory items, and displayed card data?
                //At the end of the day, I shouldn't have a ton of each card, including variants
                //Should this be Card components, or a table?
                //What if I don't allow cards that are already in a deck?

                // Inventory detail info will go here
                // How many exist of each variant?
                // How many foil / non foil?
                // How many in existing decks?
                props.selectedCardDetail && //props.selectedCardDetail.inventoryCards &&
                <InventoryDetailTable detail={props.selectedCardDetail} handleAddCardClick={props.handleAddInventoryCard} />
                
            }
            {/* Each inventory card should have a label for (in # decks) */}
        </Card>
        
        <Card className={combineStyles(outlineSection, flexCol)}>
            <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
            
            {   
                //Object.keys(props.selectedCard.variants).map((variant: string) => {
                props.selectedCard.details.map(detail => {
                return (
                    // <Card key={id} className= "outline-section flex-col">
                        // <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
                        <Box key={detail.cardId} className={combineStyles(outlineSection, flexRow)}>
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
                                        <Button variant="outlined" onClick={() => { props.handleAddNewCard && props.handleAddNewCard(detail.cardId, false) }}>
                                            Add Normal
                                        </Button>
                                        <Button variant="outlined" onClick={() => { props.handleAddNewCard && props.handleAddNewCard(detail.cardId, true) }}>
                                            Add Foil
                                        </Button>
                                    </Box>
                                </Box>
                            </CardContent>
                        </Box>
                    // </Card>
                )})
            }
        </Card>
    </Box>
// </Paper>
);
}