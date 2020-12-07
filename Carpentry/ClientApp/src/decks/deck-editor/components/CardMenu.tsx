import React from 'react'
import { Menu, MenuItem } from '@material-ui/core';

export interface ComponentProps {
    cardMenuAnchor: HTMLButtonElement | null;
    onCardMenuClose: () => void;
    onCardMenuSelect: (name: DeckEditorCardMenuOption) => void;
}

export default function CardMenu(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Menu open={Boolean(props.cardMenuAnchor)} onClose={props.onCardMenuClose} anchorEl={props.cardMenuAnchor} >
                
                <MenuItem onClick={() => {props.onCardMenuSelect("commander")}} value="">Make Commander</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("sideboard")}} value="">Move to Sideboard</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("mainboard")}} value="">Move to Mainboard</MenuItem>
                {/* <MenuItem onClick={() => {props.onCardMenuSelect("")}} value=""></MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value=""></MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value=""></MenuItem> */}
                {/* <MenuItem onClick={() => {props.onCardMenuSelect("inventory")}} value="inventory">Move to Inventory</MenuItem> */}
                {/* <MenuItem onClick={() => {props.onCardMenuSelect("delete")}} value="delete">Remove from deck</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("search")}} value="search">Search alternate versions</MenuItem> */}
            </Menu>
        </React.Fragment>
    )
}