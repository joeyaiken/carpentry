import {Box} from "@material-ui/core";
import React from "react";
import {combineStyles} from "../../../styles/appStyles";
import {DeckCardDetail} from "./DeckCardDetail";
import {DeckCardGrid} from "./DeckCardGrid";
import {DeckCardList} from "./DeckCardList";
import {DeckPropsBar} from "./DeckPropsBar";
import {DeckStatsBar} from "./DeckStatsBar";
import {GroupedDeckCardList} from "./GroupedDeckCardList";
import {AppLayout} from "../../../features/common/components/AppLayout";
import styles from '../../../App.module.css';
import {useAppSelector} from "../../../hooks";

// This component could/should probably be merged into the DeckEditor component
export function DeckEditorLayout(): JSX.Element {
  const viewMode = useAppSelector(state => state.decks.deckEditor.viewMode);
  return(
    <AppLayout title="Decks">
      <DeckPropsBar />
      <Box className={combineStyles(styles.flexRow, styles.flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
        <div className={styles.flexSection} style={{ overflow:'auto', flex:'1 1 70%' }} >
          {viewMode === "list" && <DeckCardList />}
          {viewMode === "grid" && <DeckCardGrid />}
          {viewMode === "grouped" && <GroupedDeckCardList />}
        </div>
        <div className={styles.flexSection} style={{ overflow:'auto', flex:'1 1 30%' }} >
          <DeckCardDetail />
        </div>
      </Box>
      <DeckStatsBar />
    </AppLayout>
  )
}

