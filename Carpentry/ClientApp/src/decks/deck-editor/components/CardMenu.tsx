import React from 'react'
import { Menu, MenuItem } from '@material-ui/core';

export interface ComponentProps {
    cardMenuAnchor: HTMLButtonElement | null;
    //card category
    cardCategoryId: string;
    hasInventoryCard: boolean;
    //???
    onCardMenuClose: () => void;
    onCardMenuSelect: (name: DeckEditorCardMenuOption) => void;
}

export default function CardMenu(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Menu open={Boolean(props.cardMenuAnchor)} onClose={props.onCardMenuClose} anchorEl={props.cardMenuAnchor} >
                
                { props.cardCategoryId !== 'c'  && 
                    <MenuItem onClick={() => {props.onCardMenuSelect("commander")}} value="">Make Commander</MenuItem> }
                
                { props.cardCategoryId !== 's' && 
                    <MenuItem onClick={() => {props.onCardMenuSelect("sideboard")}} value="">Move to Sideboard</MenuItem> }
                
                { props.cardCategoryId !== '' && 
                    <MenuItem onClick={() => {props.onCardMenuSelect("mainboard")}} value="">Move to Mainboard</MenuItem> }
                
                { props.hasInventoryCard && 
                    <MenuItem onClick={() => {props.onCardMenuSelect("inventory")}} value="">Remove Inventory Card</MenuItem> }

                <MenuItem onClick={() => {props.onCardMenuSelect("delete")}} value="">Remove Deck Card</MenuItem>
                
            </Menu>
        </React.Fragment>
    )
}