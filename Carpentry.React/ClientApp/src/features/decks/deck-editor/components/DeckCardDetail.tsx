import {  Table, TableHead, TableRow, TableCell, TableBody, Box, CardActions, Button, Card, CardHeader, CardMedia, CardContent, Typography, IconButton } from '@material-ui/core';
import React from 'react';
import { MoreVert } from '@material-ui/icons';
import styles from '../../../../app/App.module.css'
import {useAppSelector} from "../../../../app/hooks";
import {getSelectedDeckDetails} from "../deckEditorSlice";

interface ComponentProps{
    // selectedCard: DeckCardOverview | null;
    // inventoryCards: DeckCardDetail[];
    // onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    // onMenuClose: () => void;
    // onCardDetailClick: (cardId: number) => void;
    // onCardTagsClick: (cardId: number) => void;
}

export default function DeckCardDetail(props: ComponentProps): JSX.Element {
    // selectedCard: DeckCardOverview | null;
    
    //getSelectedCardOverview
    
    //TODO - make slice selector
    const selectedCard: DeckCardOverview | null = useAppSelector(state => {
        const selectedOverviewCardId = state.decks.deckEditor.selectedOverviewCardId;
        if(selectedOverviewCardId){
            return state.decks.data.detail.cardOverviews.byId[selectedOverviewCardId];
        }
        return null; 
    });
    
    const inventoryCards = useAppSelector(getSelectedDeckDetails);
    
    
    
    // inventoryCards: DeckCardDetail[];
    // onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    // onMenuClose: () => void;
    // onCardDetailClick: (cardId: number) => void;
    // onCardTagsClick: (cardId: number) => void;

    if(selectedCard === null){
        return (
            <Box className={styles.staticSection}>
                <Card>
                    <CardHeader titleTypographyProps={{variant:"body1"}} title={"no card selected"}/>
                    <CardContent>
                        <Typography>select a card</Typography>
                    </CardContent>
                </Card>
            </Box>
        );
    }
    else {
        return (
            <Box className={styles.staticSection}>
                <Card>
                    {
                        <CardMedia style={{height:"310px", width: "223px"}} image={selectedCard.img} />
                    }
                    <CardContent>
                        <Table size="small">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Card Detail</TableCell>
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {
                                props.inventoryCards.map(item => {

                                    const rowDeckCardId = item.id;
                                    return(
                                    <TableRow key={item.id}>
                                        {
                                            item.inventoryCardId && 
                                            <TableCell style={{textTransform:"uppercase"}}>
                                                {`${item.set} - ${item.collectorNumber}`}{item.isFoil &&" (foil)"}
                                            </TableCell>
                                        }
                                        {
                                            !item.inventoryCardId &&
                                            <TableCell>
                                                Empty
                                            </TableCell>
                                        }
                                        <TableCell size="small">
                                            <IconButton size="small" onClick={props.onMenuClick} name={item.name} value={rowDeckCardId}>
                                                <MoreVert />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                    )
                                })
                            }
                            </TableBody>
                        </Table>
                    </CardContent>
                    <CardActions>
                        <Button 
                            onClick={() => selectedCard && props.onCardDetailClick(selectedCard.cardId)} 
                            color="primary" 
                            variant="contained">Details</Button>
                        <Button 
                            onClick={() => selectedCard && props.onCardTagsClick(selectedCard.cardId)} 
                            color="primary" 
                            variant="contained">Tags</Button>
                    </CardActions>
                </Card>
            </Box>
        );
    }
}