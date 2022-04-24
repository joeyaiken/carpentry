import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, TextareaAutosize, TextField } from "@material-ui/core";
import React from "react";
// import { appStyles, combineStyles } from "../../../styles/appStyles";
import CardMenu from "./CardMenu";
import DeckCardDetail from "./DeckCardDetail";
import {DeckCardGrid} from "./DeckCardGrid";
import {DeckCardList} from "./DeckCardList";
import {DeckPropsBar} from "./DeckPropsBar";
import {DeckStatsBar} from "./DeckStatsBar";
import GroupedDeckCardList from "./GroupedDeckCardList";
import { AppLayout } from "../../../../common/components/AppLayout";
import styles from "../../../../app/App.module.css";
import {useAppSelector} from "../../../../app/hooks";
// import AppLayout from "../../../common/components/AppLayout";

declare interface ComponentProps{
  // //props & modal
  //
  // //props
  // deckProperties: DeckPropertiesDto | null;
  //
  // //modal
  // onPropsModalOpen: () => void;
  //
  // //View
  // handleToggleDeckView: () => void;
  // viewMode: DeckEditorViewMode;
  //
  // onAddCardsClick: () => void;
  //
  // //overview
  // groupedCardOverviews: CardOverviewGroup[];
  // cardDetailsById: { [deckCardId: number]: DeckCardDetail };
  //
  // onCardSelected: (cardOverview: DeckCardOverview) => void;
  //
  // //detail
  // cardMenuAnchor: HTMLButtonElement | null;
  // cardMenuAnchorId: number;
  // selectedCard: DeckCardOverview | null;
  // selectedInventoryCards: DeckCardDetail[];
  // onCardMenuSelected: (name: DeckEditorCardMenuOption) => void;
  // onCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  // onCardMenuClosed: () => void;
  //
  // //cardDetail/tags
  // onCardDetailClick: (cardId: number) => void;
  // onCardTagsClick: (cardId: number) => void;
  //
  // //stats
  // deckStats: DeckStats;
  //
  // //export
  // onExportClick: () => void;
}

export function DeckEditorLayout(props: ComponentProps): JSX.Element {
  // const { flexRow, flexSection } = appStyles();

  // const firstGroup = props.groupedCardOverviews[0];

  const viewMode = useAppSelector(state => state.decks.deckEditor.viewMode);
  
  
  
  return(
    <AppLayout title="Decks">
      { 
        // props.deckProperties &&
      <DeckPropsBar /> }

      <Box className={[styles.flexRow, styles.flexSection].join(' ')} style={{ overflow:'auto', alignItems:'stretch' }}>
        <div className={styles.flexSection} style={{ overflow:'auto', flex:'1 1 70%' }} >
          {viewMode === "list" && <DeckCardList />}
          {viewMode === "grid" && <DeckCardGrid />}
          {viewMode === "grouped" &&
          <GroupedDeckCardList
              groupedCardOverviews={props.groupedCardOverviews}
              cardDetailsById={props.cardDetailsById}
              onCardSelected={props.onCardSelected}
              onCardDetailClick={props.onCardDetailClick}
              onCardTagsClick={props.onCardTagsClick}  /> }
        </div>
        <div className={styles.flexSection} style={{ overflow:'auto', flex:'1 1 30%' }} >
          <CardMenu
            // cardMenuAnchor={props.cardMenuAnchor}
            onCardMenuSelect={props.onCardMenuSelected}
            onCardMenuClose={props.onCardMenuClosed}
            cardCategoryId={props.selectedCard?.category || ''}
            hasInventoryCard={Boolean(props.cardDetailsById[props.cardMenuAnchorId]?.inventoryCardId)}
          />

          <DeckCardDetail
            selectedCard={props.selectedCard}
            inventoryCards={props.selectedInventoryCards}
            onMenuClick={props.onCardMenuClick}
            onMenuClose={props.onCardMenuClosed}
            onCardDetailClick={props.onCardDetailClick}
            onCardTagsClick={props.onCardTagsClick} />
        </div>
      </Box>

      <DeckStatsBar />
    </AppLayout>
  )
}

