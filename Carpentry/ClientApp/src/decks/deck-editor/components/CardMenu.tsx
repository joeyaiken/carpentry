import React from 'react'
import { Menu, MenuItem } from '@material-ui/core';
import {
  // cardMenuButtonClicked, 
  requestDeleteDeckCard, requestUpdateDeckCard} from "../state/DeckEditorActions";
import {useAppDispatch, useAppSelector} from "../../../hooks";

export interface ComponentProps {
  cardMenuAnchor: HTMLButtonElement | null;
  
  
  //card category
  cardCategoryId: string;
  // hasInventoryCard: boolean;
  //???
  onCardMenuClose: () => void;
  // onCardMenuSelect: (name: DeckEditorCardMenuOption) => void;
}

export const CardMenu = (props: ComponentProps): JSX.Element => {

  const dispatch = useAppDispatch();

  // const anchorId = parseInt(props.cardMenuAnchor?.value);

  // const test = props.cardMenuAnchor?.value ?? "0";
  
  const deckCardDetail = useAppSelector(state => 
    state.decks.deckDetailData.cardDetails.byId[parseInt(props.cardMenuAnchor?.value ?? "0")])
  
  const hasInventoryCard = Boolean(deckCardDetail?.inventoryCardId); 
  
  const onCardMenuSelect = (name: DeckEditorCardMenuOption) => {
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
        const confirmText = `Are you sure you want to delete ${props.cardMenuAnchor?.name}?`;
        if(window.confirm(confirmText)){
          deckCardDetail.inventoryCardId = null;
          dispatch(requestDeleteDeckCard(deckCardDetail.id));
        }
        break;
    }
    props.onCardMenuClose();
    //dispatch(cardMenuButtonClicked(null));
  }
  
  return(
    <React.Fragment>
      <Menu open={Boolean(props.cardMenuAnchor)} onClose={props.onCardMenuClose} anchorEl={props.cardMenuAnchor} >

        { props.cardCategoryId !== 'c'  &&
        <MenuItem onClick={() => {onCardMenuSelect("commander")}} value="">Make Commander</MenuItem> }

        { props.cardCategoryId !== 's' &&
        <MenuItem onClick={() => {onCardMenuSelect("sideboard")}} value="">Move to Sideboard</MenuItem> }

        { props.cardCategoryId !== '' &&
        <MenuItem onClick={() => {onCardMenuSelect("mainboard")}} value="">Move to Mainboard</MenuItem> }

        { hasInventoryCard &&
        <MenuItem onClick={() => {onCardMenuSelect("inventory")}} value="">Remove Inventory Card</MenuItem> }

        <MenuItem onClick={() => {onCardMenuSelect("delete")}} value="">Remove Deck Card</MenuItem>

      </Menu>
    </React.Fragment>
  )
}