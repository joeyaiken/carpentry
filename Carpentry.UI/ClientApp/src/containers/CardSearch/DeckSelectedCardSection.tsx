import React from 'react';
import { Box, Paper, Card, CardHeader, CardMedia, CardContent, Typography, Button } from '@material-ui/core';
import InventoryDetailTable from './InventoryDetailTable';

interface SelectedCardDetailSectionProps {
    selectedCard: MagicCard;
    pendingCards?: PendingCardsDto;
    selectedCardDetail: InventoryDetailDto | null;
    handleAddPendingCard: (data: MagicCard, isFoil: boolean, variant: string) => void;
    handleRemovePendingCard: (multiverseId: number, isFoil: boolean, variant: string) => void;
    handleAddInventoryCard?: (inventoryCard: InventoryCard) => void;
    handleAddNewCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    // handleMoveCard?: (inventoryCard: InventoryCard) => void;
}

export default function DeckSelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
    
    return(<Paper className="static-section">
    <Box className="flex-column">
        <Card className="outline-section flex-col">
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
        
        <Card className="outline-section flex-col">
            <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
            
            {   Object.keys(props.selectedCard.variants).map((variant: string) => {
                return (
                    // <Card key={id} className="outline-section flex-col">
                        // <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
                        <Box key={variant} className="flex-row outline-section">
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                className="item-image"
                                image={(props.selectedCard.variants[variant]) || undefined} />
                            <CardContent>
                                <Box className="flex-col">
                                    <Box className="flex-col">
                                        <Typography>{`${props.selectedCard.prices[variant]} | ${props.selectedCard.prices[`${variant}_foil`]}`}</Typography>
                                    </Box>
                                    <Box className="flex-col">
                                        <Button 
                                            variant="outlined" 
                                            onClick={() => { props.handleAddNewCard && props.handleAddNewCard(props.selectedCard.multiverseId,false,variant) }} 
                                        >
                                            Add Normal
                                        </Button>
                                        <Button variant="outlined" onClick={() => { props.handleAddNewCard && props.handleAddNewCard(props.selectedCard.multiverseId,true,variant) }} >Add Foil</Button>
                                    </Box>
                                </Box>
                            </CardContent>
                        </Box>
                    // </Card>
                )})
            }
        </Card>
    </Box>
</Paper>);
}