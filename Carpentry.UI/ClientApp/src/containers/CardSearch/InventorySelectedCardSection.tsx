import React from 'react';
import { Paper, Box, Card, CardMedia, CardContent, Typography, Button } from '@material-ui/core';

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

export default function SelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
    return(<Paper className="static-section">
    <Box className="flex-column">
        {   Object.keys(props.selectedCard.variants).map((id: string) => {
               
            let countNormal = 0;
            let countFoil = 0;
            if(props.pendingCards)
            {
                countNormal = props.pendingCards.cards.filter(c => c.variantName === id && c.isFoil === false).length;
                countFoil = props.pendingCards.cards.filter(c => c.variantName === id && c.isFoil === true).length;
            }

            //const thisPendingCard = (this.props.selectedCard) && this.props.pendingCards[this.props.selectedCard.multiverseId];
            return (
                <Card key={id} className="outline-section flex-row">
                    {/* <Box>

                    </Box> */}
                    {/* <CardHeader
                        titleTypographyProps={{variant:"body1"}} 
                        title={props.card.name}
                    /> */}

                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        className="item-image"
                        image={(props.selectedCard.variants[id]) || undefined} />
                    <CardContent>
                        <Box className="flex-col">
                            <Box className="flex-col">
                                <Box className="flex-col">
                                    <Typography>{`${props.selectedCard.prices[id]} | ${props.selectedCard.prices[`${id}_foil`]}`}</Typography>
                                </Box>
                                <Box className="flex-col">
                                    <Typography>Normal ({countNormal})</Typography>
                                    <Box className="flex-row">
                                        <Button variant="outlined" onClick={() => {props.handleRemovePendingCard(props.selectedCard.multiverseId, false, id)} } >-</Button>
                                        <Button variant="outlined" onClick={() => {props.handleAddPendingCard(props.selectedCard, false, id)} } >+</Button>
                                    </Box>
                                </Box>
                                <Box className="flex-col">
                                    <Typography>Foil ({countFoil})</Typography>
                                    <Box className="flex-row">
                                        <Button variant="outlined" onClick={() => {props.handleRemovePendingCard(props.selectedCard.multiverseId, true, id)} } >-</Button>
                                        <Button variant="outlined" onClick={() => {props.handleAddPendingCard(props.selectedCard, true, id)} } >+</Button>
                                    </Box>
                                </Box>
                            </Box>
                            {/* <Box className="flex-row">
                                <div className="outline-section">
                                    
                                    <span>Count ({countNormal}</span>
                                    
                                </div>
                                <div className="outline-section">
                                    <span>Foil ({countFoil})</span>
                                </div>
                            </Box> */}
                        </Box>
                        
                    </CardContent>
                    
                    {/*  variant="contained" */}
                    {/* <CardActions>
                        
                    </CardActions> */}
                </Card>
            )})
        


        }
    </Box>

</Paper>);
}
