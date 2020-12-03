import { Box } from "@material-ui/core";
import React from "react";
import { appStyles, combineStyles } from "../../../styles/appStyles";
import CardMenu from "./CardMenu";
import DeckCardDetail from "./DeckCardDetail";
import DeckCardGrid from "./DeckCardGrid";
import DeckCardList from "./DeckCardList";
import DeckPropsBar from "./DeckPropsBar";
import { DeckPropsDialog } from "./DeckPropsDialog";
import DeckStatsBar from "./DeckStatsBar";
import GroupedDeckCardList from "./GroupedDeckCardList";

declare interface ComponentProps{
    //props & modal

    //props
    deckProperties: DeckPropertiesDto | null;

    //modal
    dialogDeckProperties: DeckPropertiesDto | null;
    isPropsDialogOpen: boolean;
    onPropsModalOpen: () => void;
    onPropsModalClose: () => void;
    onModalPropsChange: (name: string, value: string | number) => void;
    onPropsModalSave: () => void;
    onPropsModalDisassembleClick: () => void;
    onPropsModalDeleteClick: () => void;
    formatFilterOptions: FilterOption[];

    //View
    handleToggleDeckView: () => void;
    viewMode: DeckEditorViewMode;

    onAddCardsClick: () => void;

    //overview
    groupedCardOverviews: CardOverviewGroup[];
    onCardSelected: (cardOverview: DeckCardOverview) => void;
    
    //detail
    cardMenuAnchor: HTMLButtonElement | null;
    selectedCard: DeckCardOverview | null;
    selectedInventoryCards: DeckCardDetail[];
    onCardMenuSelected: (name: DeckEditorCardMenuOption) => void;
    onCardMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onCardMenuClosed: () => void;

    //stats
    deckStats: DeckStats;
}

export function DeckEditorLayout(props: ComponentProps): JSX.Element {
    const { flexRow, flexSection } = appStyles();

    const firstGroup = props.groupedCardOverviews[0];

    return(
        <React.Fragment>
            {
                props.dialogDeckProperties && 
                <DeckPropsDialog 
                    isOpen={props.isPropsDialogOpen}
                    onCloseClick={props.onPropsModalClose}
                    onFieldChange={props.onModalPropsChange}
                    deckProperties={props.dialogDeckProperties}
                    formatFilterOptions={props.formatFilterOptions}
                    onSaveClick={props.onPropsModalSave} 
                    onDisassembleClick={props.onPropsModalDisassembleClick}
                    onDeleteClick={props.onPropsModalDeleteClick} />
            }
            
            { props.deckProperties && 
                <DeckPropsBar 
                    deckProperties={props.deckProperties} 
                    onEditClick={props.onPropsModalOpen} 
                    onAddCardsClick={props.onAddCardsClick}
                    onToggleViewClick={props.handleToggleDeckView} /> }
            

            <Box className={combineStyles(flexRow, flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
                <div className={flexSection} style={{ overflow:'auto', flex:'1 1 70%' }} >
                    {props.viewMode === "list" && <DeckCardList cardOverviews={firstGroup.cardOverviews} onCardSelected={props.onCardSelected} />}
                    {props.viewMode === "grid" && <DeckCardGrid cardOverviews={firstGroup.cardOverviews} onCardSelected={props.onCardSelected} />}
                    {props.viewMode === "grouped" && <GroupedDeckCardList groupedCardOverviews={props.groupedCardOverviews} onCardSelected={props.onCardSelected} />}
                </div>
                <div className={flexSection} style={{ overflow:'auto', flex:'1 1 30%' }} >
                    <CardMenu cardMenuAnchor={props.cardMenuAnchor} onCardMenuSelect={props.onCardMenuSelected} onCardMenuClose={props.onCardMenuClosed} />

                    <DeckCardDetail 
                        selectedCard={props.selectedCard} 
                        inventoryCards={props.selectedInventoryCards} 
                        onMenuClick={props.onCardMenuClick}
                        onMenuClose={props.onCardMenuClosed} />
                </div>
            </Box>

            <DeckStatsBar deckStats={props.deckStats} />
        </React.Fragment>
    )
}

