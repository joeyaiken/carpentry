import React from 'react'
import { Menu, MenuItem } from '@material-ui/core';
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
import {cardMenuButtonClicked, requestDeleteDeckCard, requestUpdateDeckCard} from "../state/DeckEditorActions";

export interface ComponentProps {
    // cardMenuAnchor: HTMLButtonElement | null;
    //card category
    cardCategoryId: string;
    hasInventoryCard: boolean;
    //???
    // onCardMenuClose: () => void;
    // onCardMenuSelect: (name: DeckEditorCardMenuOption) => void;
}

export default function CardMenu(props: ComponentProps): JSX.Element {


    const dispatch = useAppDispatch();

    const onCardMenuClose = (): void => {
        dispatch(cardMenuButtonClicked(null));
    }

    const onCardMenuSelect = (name: DeckEditorCardMenuOption): void => {

        //hasInventoryCard={Boolean(props.cardDetailsById[props.cardMenuAnchorId]?.inventoryCardId)}
        const deckCardDetail = this.props.cardDetailsById[this.props.cardMenuAnchorId];
        switch(name){
            case "sideboard":
                deckCardDetail.category = name;
                dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "mainboard":
                deckCardDetail.category = "";
                dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "commander":
                deckCardDetail.category = name;
                dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "inventory":
                deckCardDetail.inventoryCardId = null;
                dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "delete":
                const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor?.name}?`;
                if(window.confirm(confirmText)){
                    deckCardDetail.inventoryCardId = null;
                    dispatch(requestDeleteDeckCard(deckCardDetail.id));
                }
                break;
        }
        dispatch(cardMenuButtonClicked(null));
    }

    const cardMenuAnchor: HTMLButtonElement | null = useAppSelector(state => state.decks.deckEditor.cardMenuAnchor);
    // const cardCategoryId = useAppSelector(state => state.)
    return(
        <React.Fragment>
            <Menu open={Boolean(cardMenuAnchor)} onClose={onCardMenuClose} anchorEl={cardMenuAnchor} >
                
                { props.cardCategoryId !== 'c'  && 
                    <MenuItem onClick={() => {onCardMenuSelect("commander")}} value="">Make Commander</MenuItem> }
                
                { props.cardCategoryId !== 's' && 
                    <MenuItem onClick={() => {onCardMenuSelect("sideboard")}} value="">Move to Sideboard</MenuItem> }
                
                { props.cardCategoryId !== '' && 
                    <MenuItem onClick={() => {onCardMenuSelect("mainboard")}} value="">Move to Mainboard</MenuItem> }
                
                { props.hasInventoryCard && 
                    <MenuItem onClick={() => {onCardMenuSelect("inventory")}} value="">Remove Inventory Card</MenuItem> }

                <MenuItem onClick={() => {onCardMenuSelect("delete")}} value="">Remove Deck Card</MenuItem>
                
            </Menu>
        </React.Fragment>
    )
}