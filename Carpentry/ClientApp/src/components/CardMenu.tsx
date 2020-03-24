import React from 'react'
import { Menu, MenuItem } from '@material-ui/core';

export interface ComponentProps {
    cardMenuAnchor: HTMLButtonElement | null;
    onCardMenuClose: () => void;
    onCardMenuSelect: (name: string) => void;
}

export default function CardMenu(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Menu open={Boolean(props.cardMenuAnchor)} onClose={props.onCardMenuClose} anchorEl={props.cardMenuAnchor} >
                
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value="">Make Commander</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value="">Move to Sideboard</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value="">Move to Mainboard</MenuItem>
                {/* <MenuItem onClick={() => {props.onCardMenuSelect("")}} value=""></MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value=""></MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("")}} value=""></MenuItem> */}
                <MenuItem onClick={() => {props.onCardMenuSelect("inventory")}} value="inventory">Move to Inventory</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("delete")}} value="delete">Delte card</MenuItem>
                <MenuItem onClick={() => {props.onCardMenuSelect("search")}} value="search">Search alternate versions</MenuItem>
            </Menu>
        </React.Fragment>
    )
}