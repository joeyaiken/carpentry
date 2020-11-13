import React from 'react';
import { Box, Card, CardMedia, CardContent, Typography, Button } from '@material-ui/core';
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface SelectedCardDetailSectionProps {
    selectedCard: CardSearchResultDto;
    pendingCards?: PendingCardsDto;
    selectedCardDetail: InventoryDetailDto | null;
    handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    handleAddInventoryCard?: (inventoryCard: InventoryCard) => void;
    handleAddNewCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    // handleMoveCard?: (inventoryCard: InventoryCard) => void;
}

export default function SelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
    const { outlineSection, flexRow, flexCol } = appStyles();

    // console.log('rendering card search selected card section')
    return(
    // <Paper className={staticSection}>
    <Box className={flexCol}>
        {   
        
            // Object.keys(props.selectedCard.details).map((id) => {

            props.selectedCard.details.map((detail) => {
            //for(i = 0; i < props.selectedCard.details.length(); i++){
            // props.selectedCard.details.forEach(card => {

            // })

            

            // console.log(`SELECTED CARD ID ${id}`)
               
            let countNormal = 0;
            let countFoil = 0;
            if(props.pendingCards)
            {
                //countNormal = props.pendingCards.cards.filter(c => c.variantName === id && c.isFoil === false).length;
                countNormal = props.pendingCards.cards.filter(c => c.cardId === detail.cardId && c.isFoil === false).length;
                //countFoil = props.pendingCards.cards.filter(c => c.variantName === id && c.isFoil === true).length;
                countFoil = props.pendingCards.cards.filter(c => c.cardId === detail.cardId && c.isFoil === true).length;
            }

            //const thisPendingCard = (this.props.selectedCard) && this.props.pendingCards[this.props.selectedCard.multiverseId];
            return (
                <Card key={detail.cardId} className={combineStyles(outlineSection, flexRow)}>
                    {/* <Box>

                    </Box> */}
                    {/* <CardHeader
                        titleTypographyProps={{variant:"body1"}} 
                        title={props.card.name}
                    /> */}

                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        // className={itemImage}
                        // image={(props.selectedCard.details[id]) || undefined}
                        image={detail.imageUrl || undefined}
                         />
                    <CardContent>
                        <Box className={flexCol}>
                            <Box className={flexCol}>
                                <Box className={flexCol}>
                                    <Typography>{`${detail.price} | ${detail.priceFoil}`}</Typography>
                                </Box>
                                <Box className={flexCol}>
                                    <Typography>Normal ({countNormal})</Typography>
                                    <Box className={flexRow}>
                                        <Button variant="outlined" onClick={() => {props.handleRemovePendingCard(detail.name, detail.cardId, false)} } >-</Button>
                                        <Button variant="outlined" onClick={() => {props.handleAddPendingCard(detail.name, detail.cardId, false)} } >+</Button>
                                    </Box>
                                </Box>
                                <Box className={flexCol}>
                                    <Typography>Foil ({countFoil})</Typography>
                                    <Box className={flexRow}>
                                        <Button variant="outlined" onClick={() => {props.handleRemovePendingCard(detail.name, detail.cardId, true)} } >-</Button>
                                        <Button variant="outlined" onClick={() => {props.handleAddPendingCard(detail.name, detail.cardId, true)} } >+</Button>
                                    </Box>
                                </Box>
                            </Box>
                            {/* <Box className={classes.flexRow}>
                                <div className={classes.outlineSection}>
                                    
                                    <span>Count ({countNormal}</span>
                                    
                                </div>
                                <div className={classes.outlineSection}>
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

// </Paper>
);
}