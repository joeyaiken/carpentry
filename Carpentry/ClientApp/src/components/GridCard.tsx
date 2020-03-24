//
// Card meant to be used by the CardGridContainer
//
import React, { ReactNode } from 'react';
//A component that displays a collection of cards
//Data can either be displayed in LIST or GRID form


//Current spots the grid should be implemented
//  CardSearch search results
//      Uses a list of MagicCard objects
//      Should have a button on each card (used to show details)
//  Inventory results
//      Uses a weird object
//      Should have a button on each card (used to show details)
//  Deck contents
//      Uses another weird object
//      Needs to have a menu button on a card object

//These will all need to eventually use more-standardised props

import { Typography, Box, Card, CardMedia, CardHeader, CardContent, IconButton, Menu, MenuItem, CardActions, Button } from '@material-ui/core';

import { MoreVert } from '@material-ui/icons';

interface RenderedCardProps{
    //card: MagicCard;
    card: InventoryOverviewDto;
    
    hideHeader?: boolean;
    // pendingCardObject: PendingCardsDto | null;
    // handleAddPendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    // handleRemovePendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    
    onDetailClick?: () => void;

    //(JSX attribute) onClick?: ((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void) | undefined
    onMenuClick?: ((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void) | undefined;

    //<React.Fragment>
    renderedCardContent?: React.ReactFragment;
    children?: ReactNode;
    // showPrices: boolean;
    showCounts?: boolean;
}
export default function GridCard(props: RenderedCardProps): JSX.Element {
    // const cardCount =  (props.pendingCardObject && props.pendingCardObject.cards.length) || 0;
    return (
        <Card key={props.card.name} className="outline-section">
            {
                (!props.hideHeader) && 
                <CardHeader
                    titleTypographyProps={{variant:"body1"}} 
                    // subheaderTypographyProps={{variant:"body2"}} 
                    title={props.card.name}
                    // subheader={`${props.card.type}`}
                    // action={
                    //     (props.onMenuClick) ?
                    //     <IconButton
                    //         name={props.card.name}
                    //         value={props.card.name}
                    //         onClick={props.onMenuClick}>
                    //         <MoreVert />
                    //     </IconButton>
                    //     : undefined
                    // }
                />
            }
            <CardMedia 
                style={{height:"310px", width: "223px"}}
                className="item-image"
                //image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.}&type=card`}
                image={props.card.img}
                title={props.card.name} />

            {/* DE */}
            {
                props.renderedCardContent
            }
            {/* <CardContent>
                    { (props.card.isFoil) && (<Typography>${props.data.priceFoil}</Typography>) }
                    { (!props.card.isFoil) && (<Typography>${props.data.price}</Typography>) }
            </CardContent> */}

            {/* INV */}
            {/* <CardContent>
                <Box className="flex-col">
                    <Box className="flex-row">
                        { (deckCount > 0) && (<Typography>{deckCount} in Decks</Typography>) }&nbsp;
                        { (invCount > 0) && (<Typography>{invCount} in Inventory</Typography>) }
                    </Box>
                </Box>
            </CardContent> */}

            {/* CS */}
            {
                (
                    props.showCounts
                ) && 
                <CardContent>
                    <Box className="flex-col">
                        <Box className="flex-row">
                            <Typography>PRICES ARE BROKEN FOR NOW</Typography>
                            {/* { (props.card.isFoil) && (<Typography>${props.data.priceFoil}</Typography>) }
                            { (!props.card.isFoil) && (<Typography>${props.data.price}</Typography>) } */}
                        </Box>
                        <Box className="flex-row">
                            <Typography>COUNTS ARE BROKEN FOR NOW</Typography>
                            {/* { (deckCount > 0) && (<Typography>{deckCount} in Decks</Typography>) }&nbsp;
                            { (invCount > 0) && (<Typography>{invCount} in Inventory</Typography>) } */}
                        </Box>
                    </Box>
                    
                </CardContent>
            }
            {
                props.children
            }
            {/*  variant="contained" */}
            <CardActions>
                <Button color="primary" size="small" onClick={props.onDetailClick} >
                    Details
                </Button>
            </CardActions>
            {/* <CardContent>
                <div className="flex-row">
                    <div className="outline-section">
                        <button onClick={() => {props.handleRemovePendingCard(props.card.multiverseId, false, "normal")} } >-</button>
                        <span>Count ({cardCount})</span>
                        <button onClick={() => {props.handleAddPendingCard(props.card.multiverseId, false, "normal")} } >+</button>    
                    </div>    
                </div>
            </CardContent> */}
        </Card>
    );
}