import {useAppDispatch, useAppSelector} from "../../hooks";
import {useHistory} from "react-router";
import {getSelectedDeckId} from "../deck-editor/deckDetailSlice";
import React, {useState} from "react";
import {requestAddDeckCard} from "../deck-add-cards/state/DeckAddCardsActions";
import {requestUpdateDeckCard} from "../deck-editor/state/DeckEditorActions";
import {forceLoadCardDetail} from "./state/CardDetailActions";
import {CardMenu} from "../deck-editor/components/CardMenu";
import {
  Box, Button,
  Card,
  CardHeader, CardMedia, IconButton,
  Menu,
  MenuItem,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow, Typography
} from "@material-ui/core";
import styles from "../../App.module.css";
import {combineStyles} from "../../styles/appStyles";
import {RootState} from "../../configureStore";
import {MoreVert} from "@material-ui/icons";

interface ContainerLayoutProps {
  selectedCardId: number;
}

export const CardDetail = (props: ContainerLayoutProps): JSX.Element => {

  const dispatch = useAppDispatch();
  const history = useHistory();

  // TODO - make this a proper multi-stage 'memoized' selector
  const activeDeckCardIds = useAppSelector(state =>
    state.decks.deckDetailData.cardDetails.allIds.filter(id => {
      const card = state.decks.deckDetailData.cardDetails.byId[id];
      return (card.name === state.decks.cardDetail.activeCardName);
    })
  )

  const allCardIds = useAppSelector(state => state.decks.cardDetail.cards.allIds);
  // const cardsById = useAppSelector(state => state.decks.cardDetail.cards.byId);
  const selectedDeckId = useAppSelector(getSelectedDeckId);

  const [deckCardMenuAnchor, setDeckCardMenuAnchor] = useState<HTMLButtonElement | null>(null);
  const [inventoryCardMenuAnchor, setInventoryCardMenuAnchor] = useState<HTMLButtonElement | null>(null);

  const selectedDeckCardCategoryId = useAppSelector(state => {
    if(!deckCardMenuAnchor) return '';
    const selectedDeckCard = state.decks.deckDetailData.cardDetails.byId[parseInt(deckCardMenuAnchor.value)]
    switch(selectedDeckCard.category){
      case "Sideboard": return 's';
      case "Commander": return 'c'
      default: return '';
    }
  })

  const selectedDeckCardName = useAppSelector(state => {
    if(!deckCardMenuAnchor) return '';
    const selectedDeckCard = state.decks.deckDetailData.cardDetails.byId[parseInt(deckCardMenuAnchor.value)]
    return selectedDeckCard.name;
  })

  // const invCardDeckId = useAppSelector(state => {
  //   if(!inventoryCardMenuAnchor) return 0;
  //   const selectedInventoryCard = state.decks.cardDetail.inventoryCards.byId[parseInt(inventoryCardMenuAnchor.value)]
  //   return selectedInventoryCard.deckId;
  // })

  const selectedInventoryCard = useAppSelector(state => {
    if(!inventoryCardMenuAnchor) return null;
    return state.decks.cardDetail.inventoryCards.byId[parseInt(inventoryCardMenuAnchor.value)];
  })
  const invCardDeckId = selectedInventoryCard?.deckId;

  const deckCardDetailsById = useAppSelector(state => state.decks.deckDetailData.cardDetails.byId);



  const onDeckCardMenuClick = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
    setDeckCardMenuAnchor(event.currentTarget);
  }

  const onInventoryCardMenuClick = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
    setInventoryCardMenuAnchor(event.currentTarget);
  }



  const onAddEmptyCardClick = (): void => {
    let deckCard: DeckCardDto = {
      categoryId: null,
      deckId: selectedDeckId,
      cardName: selectedDeckCardName,

      id: 0,
      inventoryCardId: 0,
      cardId: 0,
      isFoil: false,
      inventoryCardStatusId: 0,
    }
    dispatch(requestAddDeckCard(deckCard));
  }

  const handleInventoryCardMenuSelected = (menuName: InventoryCardMenuOption) => {
    if(!selectedInventoryCard) return;
    //inventoryCardMenuAnchorId
    //const inventoryCard = this.props.inventoryCardsById[this.props.inventoryCardMenuAnchorId];
    const inventoryCard = selectedInventoryCard;
    switch(menuName){
      case "add":
        //
        // //is there an empty card in the deck that can be filled?
        // const firstEmptyId = activeDeckCardIds.find(id => !Boolean(this.props.deckCardDetailsById[id].inventoryCardId));
        //
        // if(firstEmptyId){
        //   //If so, update it
        //   const thisDeckCard = this.props.deckCardDetailsById[firstEmptyId];
        //   thisDeckCard.inventoryCardId = inventoryCard.id;
        //   this.props.dispatch(requestUpdateDeckCard(thisDeckCard));
        //
        // } else {
        //   //if not, a new should be added
        //
        //   let newDeckCard: DeckCardDto = {
        //     id: 0,
        //     deckId: selectedDeckId,
        //     cardName: inventoryCard.name,
        //     categoryId: null,
        //     inventoryCardId: inventoryCard.id,
        //
        //     cardId: inventoryCard.cardId,
        //     isFoil: inventoryCard.isFoil,
        //     inventoryCardStatusId: 1,
        //   }
        //
        //   dispatch(requestAddDeckCard(newDeckCard));
        //   dispatch(forceLoadCardDetail(this.props.selectedCardId));
        // }
        break;
      case "remove":
        // const confirmText = `Are you sure you want to remove ${this.props.deckCardMenuAnchor?.name} from the deck?`;
        // if(window.confirm(confirmText)){
        //     deckCardDetail.inventoryCardId = null;
        //     this.props.dispatch(requestDeleteDeckCard(deckCardDetail.id));
        // }
        break;
      case "view":
        history.push(`/decks/${inventoryCard.deckId}?cardId=${inventoryCard.cardId}&show=detail`);
        break;
    }

    // // console.log('card anchor');
    // // console.log(this.props.cardMenuAnchor);
    // switch (name){
    //     // case "search":
    //     //     if(this.props.cardMenuAnchor != null){
    //     //         // this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
    //     //     }
    //     //     break;
    //     // case "delete":
    //     //         if(this.props.cardMenuAnchor != null){
    //     //             const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
    //     //             if(window.confirm(confirmText)){
    //     //                 // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
    //     //             }
    //     //         }
    //     //         break;
    //     case "sideboard":
    //         if(this.props.cardMenuAnchor != null){
    //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "sideboard"));
    //         }
    //         break;
    //     case "mainboard":
    //         if(this.props.cardMenuAnchor != null){
    //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "mainboard"));
    //         }
    //         break;
    //     case "commander":
    //         if(this.props.cardMenuAnchor != null){
    //             this.props.dispatch(requestUpdateDeckCardStatus(this.props.cardMenuAnchorId, "commander"));
    //         }
    //         break;
    // }

    // dispatch(inventoryCardMenuButtonClicked(null));
    setInventoryCardMenuAnchor(null);
  }

  const onInventoryCardAddClick = (): void => {
    if(!selectedInventoryCard) return;

    //is there an empty DeckCard in the deck that can be filled?
    const firstEmptyId = activeDeckCardIds.find(id => !Boolean(deckCardDetailsById[id].inventoryCardId));

    if(firstEmptyId){
      //If so, update it
      const thisDeckCard = deckCardDetailsById[firstEmptyId];
      thisDeckCard.inventoryCardId = selectedInventoryCard.id;
      dispatch(requestUpdateDeckCard(thisDeckCard));
    } else {
      //if not, a new deck card should be added
      let newDeckCard: DeckCardDto = {
        id: 0,
        deckId: selectedDeckId,
        cardName: selectedInventoryCard.name,
        categoryId: null,
        inventoryCardId: selectedInventoryCard.id,

        cardId: selectedInventoryCard.cardId,
        isFoil: selectedInventoryCard.isFoil,
        inventoryCardStatusId: 1,
      }

      dispatch(requestAddDeckCard(newDeckCard));
      dispatch(forceLoadCardDetail(props.selectedCardId));
    }
  }

  const onInventoryCardRemoveClick = (): void => {
    console.log('selected inventory card', selectedInventoryCard)

    // const confirmText = `Are you sure you want to remove ${this.props.deckCardMenuAnchor?.name} from the deck?`;
    // if(window.confirm(confirmText)){
    //     deckCardDetail.inventoryCardId = null;
    //     this.props.dispatch(requestDeleteDeckCard(deckCardDetail.id));
    // }
  }

  const onInventoryCardViewDeckClick = (): void => {
    if(!selectedInventoryCard) return;
    history.push(`/decks/${selectedInventoryCard.deckId}?cardId=${selectedInventoryCard.cardId}&show=detail`);
  }

  return(
    <React.Fragment>
      <CardMenu
        cardMenuAnchor={deckCardMenuAnchor}
        onCardMenuClose={() => setDeckCardMenuAnchor(null)}
        cardCategoryId={selectedDeckCardCategoryId}
      />
      <Menu
        open={Boolean(inventoryCardMenuAnchor)}
        onClose={() => setInventoryCardMenuAnchor(null)}
        anchorEl={inventoryCardMenuAnchor}
      >
        { //Only show if not in a deck
          !invCardDeckId &&
          <MenuItem onClick={onInventoryCardAddClick}>Add to Deck</MenuItem>
        }
        { //Only show if in this deck
          invCardDeckId &&  invCardDeckId === selectedDeckId &&
          <MenuItem onClick={onInventoryCardRemoveClick}>Remove from Deck</MenuItem>
        }
        { //Only show if in another deck
          invCardDeckId && invCardDeckId !== selectedDeckId &&
          <MenuItem onClick={onInventoryCardViewDeckClick}>View Deck</MenuItem>
        }
      </Menu>
      {/* <CardMenu 
            cardMenuAnchor={props.menuAnchor} 
            onCardMenuSelect={props.onCardMenuSelected} 
            onCardMenuClose={props.onCardMenuClosed} 
            cardCategoryId={''}
            hasInventoryCard={false}
            // cardCategoryId={props.selectedCard?.category || ''}
            // hasInventoryCard={Boolean(props.cardDetailsById[props.cardMenuAnchorId]?.inventoryCardId)}
            /> */}
      <Box className={styles.outlineSection}>
        Deck Cards
        <Paper>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>Card</TableCell>
                <TableCell>Category</TableCell>
                <TableCell />
              </TableRow>
            </TableHead>
            <TableBody>
              {
                activeDeckCardIds.map(id =>
                  <DeckCardTableRow
                    key={id}
                    deckCardId={id}
                    onDeckCardMenuClick={onDeckCardMenuClick}
                    onInventoryCardMenuClick={onInventoryCardMenuClick}
                  />
                )
              }
            </TableBody>
          </Table>
        </Paper>
      </Box>

      {/* Inventory Card section */}
      <Box className={styles.flexCol}>

        <Card className={combineStyles(styles.outlineSection, styles.flexCol)}>
          <CardHeader
            titleTypographyProps={{variant:"body1"}}
            title="Inventory"
            action={
              <Button variant="outlined"
                      onClick={() => onAddEmptyCardClick()}
              >
                Add Empty
              </Button>
            }/>

          <Box className={combineStyles(styles.flexCol, styles.flexSection)}>
            {
              allCardIds.map(cardId => {

                return <CardPrintSection
                  key={cardId}
                  cardId={cardId}
                  onInventoryCardMenuClick={onInventoryCardMenuClick}
                />
              })
            }
          </Box>
        </Card>
      </Box>
    </React.Fragment>);
}

interface DeckCardTableRowProps {
  deckCardId: number;
  onDeckCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onInventoryCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
}
const DeckCardTableRow = (props: DeckCardTableRowProps): JSX.Element => {

  // TODO - move this const to a slice
  const selectDeckCardDetail = (state: RootState, deckCardId: number): DeckCardDetail =>
    state.decks.deckDetailData.cardDetails.byId[deckCardId];

  const deckCard = useAppSelector(state => selectDeckCardDetail(state, props.deckCardId));

  return (
    <TableRow>
      <TableCell>{ deckCard.inventoryCardId ? `${deckCard.set} (${deckCard.collectorNumber}) ${deckCard.isFoil ? 'Foil' : 'Normal'}`  : "Empty"  }</TableCell>
      <TableCell>{deckCard.category}</TableCell>
      <TableCell>
        <IconButton size="small" value={props.deckCardId} onClick={props.onDeckCardMenuClick}>
          <MoreVert />
        </IconButton>
      </TableCell>
    </TableRow>
  );
}

const CardPrintSection = (props: {
  cardId: number,
  onInventoryCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void
}): JSX.Element => {
  const cardId = props.cardId;
  const card = useAppSelector(state => state.decks.cardDetail.cards.byId[cardId]);
  const inventoryCardIds = useAppSelector(state => state.decks.cardDetail.cardGroups[cardId]);
  const img = card.imageUrl;
  const cardTitle = `${card.set} (${card.collectionNumber}) - $${card.price} | $${card.priceFoil}`;
  return (
    <Card key={card.cardId} className={combineStyles(styles.outlineSection, styles.flexCol)}>
      <CardHeader titleTypographyProps={{variant:"body1"}} style={{textTransform:"uppercase"}} title={cardTitle} />

      <Box className={combineStyles(styles.flexRow, styles.flexSection)}>
        <Box className={styles.staticSection}>
          <CardMedia style={{height:"310px", width: "223px"}} image={img} />
        </Box>
        <Box className={combineStyles(styles.flexSection, styles.flexCol)}>
          <Box className={styles.scrollSection} style={{overflow:"auto"}}>
            <Table size="small" >
              <TableHead>
                <TableRow>
                  <TableCell>Style</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell>Action</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {
                  inventoryCardIds.map(inventoryCardId => {
                    return <CardPrintInventoryCardRow
                      key={inventoryCardId}
                      inventoryCardId={inventoryCardId}
                      onInventoryCardMenuClick={props.onInventoryCardMenuClick}
                    />
                  })
                }
              </TableBody>
            </Table>
          </Box>
        </Box>
      </Box>
    </Card>
  )
}

const CardPrintInventoryCardRow = (props: {
  inventoryCardId: number,
  onInventoryCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void
}): JSX.Element => {
  const item = useAppSelector(state => state.decks.cardDetail.inventoryCards.byId[props.inventoryCardId]);
  return(
    <TableRow key={item.id}>
      <TableCell>
        <Typography>{(item.isFoil && " foil") || "normal"}</Typography>
      </TableCell>
      <TableCell>
        <Typography>
          { item.deckId && item.deckName }
          { !item.deckId && "Inventory" }
        </Typography>
      </TableCell>

      <TableCell>
        <IconButton size="small" value={item.id} onClick={props.onInventoryCardMenuClick}>
          <MoreVert />
        </IconButton>
      </TableCell>
    </TableRow>
  )
}
