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
    card: MagicCard;
    // pendingCardObject: PendingCardsDto | null;
    handleAddPendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    handleRemovePendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    
    //(JSX attribute) onClick?: ((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void) | undefined
    onMenuClick?: ((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void) | undefined;

    //<React.Fragment>
    renderedCardContent?: React.ReactFragment;

    // showPrices: boolean;
    showCounts?: boolean;
}
function RenderedCard(props: RenderedCardProps): JSX.Element {
    // const cardCount =  (props.pendingCardObject && props.pendingCardObject.cards.length) || 0;
    return (
        <Card key={props.card.multiverseId} className="outline-section">
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

            <CardMedia 
                style={{height:"310px", width: "223px"}}
                className="item-image"
                image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.multiverseId}&type=card`}
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
                        {
                            props.handleAddPendingCard && props.handleRemovePendingCard &&
                            <Box className="flex-row">
                                <div className="outline-section">
                                    {/* <button onClick={() => {props.handleRemovePendingCard(props.card.multiverseId, false, "normal")} } >-</button> */}
                                    <button onClick={() => { props.handleRemovePendingCard && props.handleRemovePendingCard(props.card.multiverseId, false, "normal")} } >-</button>
                                    <Typography>COUNTS ARE BROKEN FOR NOW</Typography>
                                    {/* <span>Count ({cardCount})</span> */}
                                    <button onClick={() => {props.handleAddPendingCard && props.handleAddPendingCard(props.card.multiverseId, false, "normal")} } >+</button>    
                                </div>    
                            </Box>

                        }
                        
                    </Box>
                    
                </CardContent>
            }
            <CardContent>
                <Box className="">

                </Box>
                
                <div className="flex-row">
                    <div className="outline-section">
                        {/* <button onClick={() => {props.handleRemovePendingCard(props.card.multiverseId, false, "normal")} } >-</button> */}
                        {/* <span>Count ({cardCount})</span> */}
                        {/* <button onClick={() => {props.handleAddPendingCard(props.card.multiverseId, false, "normal")} } >+</button>     */}
                    </div>    
                </div>
            </CardContent>
            {/*  variant="contained" */}
            <CardActions>
                <Button color="primary" size="small" onClick={() => {  } } >
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

interface MoreRenderedCardProps {
    title: string | null;
    subtitle: string | null;
    data: MagicCard;
    //how do I handle onClick? or just the buttons in general?

    

}




interface MagicCardProps {
    card: Card;
    data: MagicCardDto;
    onMenuClick: any;
}

function DeckEditorCard(props: MagicCardProps): JSX.Element {
    return(
        <Card>
            <CardHeader 
                titleTypographyProps={{variant:"body1"}} 
                subheaderTypographyProps={{variant:"body2"}} 
                title={props.data.name} 
                subheader={`${props.data.type}${ props.data.cmc && (' - '+props.data.cmc) }`}
                action={
                        <IconButton
                            name={props.data.name}
                            value={props.card.id}
                            onClick={props.onMenuClick}>
                            <MoreVert />
                        </IconButton>
                    }
                />
                <CardMedia
                    style={{height:"310px",width:"223px"}}
                    className="item-image"
                    image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.multiverseId}&type=card`}
                    title={props.data.name}
                />
                <CardContent>
                    { (props.card.isFoil) && (<Typography>${props.data.priceFoil}</Typography>) }
                    { (!props.card.isFoil) && (<Typography>${props.data.price}</Typography>) }
                </CardContent>
        </Card>);
}

interface CardProps {
    item: InventoryDisplayItem;
    onMenuClick: any;
}

function InventoryCard(props: CardProps): JSX.Element {
    let deckCount = props.item.deckCount;
    let invCount = props.item.inventoryCount;
    return(
        <Card>
            <CardHeader 
                titleTypographyProps={{variant:"body1"}} 
                subheaderTypographyProps={{variant:"body2"}} 
                title={props.item.name}
                subheader={`${props.item.data.type}`}
                action={
                      <IconButton
                          name={props.item.name}
                          value={props.item.name}
                          onClick={props.onMenuClick}>
                          <MoreVert />
                      </IconButton>
                }
            />
            <CardMedia
                style={{height:"310px",width:"223px"}}
                className="item-image"
                image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.item.data.multiverseId}&type=card`}
                title={props.item.data.name}
            />
            <CardContent>
                <Box className="flex-col">
                    <Box className="flex-row">
                        { (deckCount > 0) && (<Typography>{deckCount} in Decks</Typography>) }&nbsp;
                        { (invCount > 0) && (<Typography>{invCount} in Inventory</Typography>) }
                    </Box>
                </Box>
            </CardContent>
        </Card>);
}

interface CardListProps {
    //   children: ReactNode;
    //   onAddClick: any;
    layout: "grid" | "list"; //eventual other versions include LIST or TABLE

    //expect some array of items, each item CONTAINS a magic card but isn't just a magic card
    cards: CardListItem[];

    handleAddPendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    handleRemovePendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
}
export default function CardList(props: CardListProps): JSX.Element {
    //How do I handle the variety of card options?
    //Maybe when I need to show a count, I just pass in a string



    //This is for card search:
    if(props.layout === "grid"){
        return(
            <React.Fragment>
                <Box className="flex-row-wrap">
                    {
                        props.cards.map((card: CardListItem) => 
                            <RenderedCard 
                                card={card.data}  
                                handleAddPendingCard={props.handleAddPendingCard}
                                handleRemovePendingCard={props.handleRemovePendingCard}
                                // pendingCardObject={this.props.pendingCards[card.multiverseId]}
                                />)
                    }
                </Box>
            </React.Fragment>
        );
    }

    //If this was inventory:
    //The inventory uses a list of InventoryDisplayItems
    /*
    InventoryDisplayItem{
        name
        data
        deckCount
        inventoryCount
        // cards[] (does this not exist anymore?)
        //  I guess I decided it wasn't necissary for displaying a card

    }
    */

    //If this was deck editor:
    //The deck editor uses a list of IMagicCard objects
    //an IMagicCard is a combo of a Card (?!) and a MagicCardDto
    /*
    Card {
        id
        MultiverseId
        isFoil
        deckId
    }
    */
    return(
        <React.Fragment>
            {/* <Typography>No layout selected</Typography> */}
        </React.Fragment>
    );
}

