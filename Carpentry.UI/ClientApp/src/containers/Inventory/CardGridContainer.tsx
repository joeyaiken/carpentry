//
// Grid component that holds display cards
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

import { Typography, Box } from '@material-ui/core';
import { appStyles } from '../../styles/appStyles';

interface CardGridContainerProps {
    //   children: ReactNode;
    //   onAddClick: any;
    layout: "grid"; //eventual other versions include LIST or TABLE

    //expect some array of items, each item CONTAINS a magic card but isn't just a magic card
    // cards: CardListItem[];

    // handleAddPendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    // handleRemovePendingCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;

    children: ReactNode;
}
export default function CardGridContainer(props: CardGridContainerProps): JSX.Element {
    //How do I handle the variety of card options?
    //Maybe when I need to show a count, I just pass in a string

    const classes = appStyles();

    //This is for card search:
    if(props.layout === "grid"){
        return(
            <React.Fragment>
                <Box className={classes.flexRowWrap}>
                    {props.children}
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
            <Typography>No layout selected</Typography>
        </React.Fragment>
    );
}

