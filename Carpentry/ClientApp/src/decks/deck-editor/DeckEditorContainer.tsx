import {connect, DispatchProp} from 'react-redux'
import React from 'react'
import {RootState} from '../../configureStore';
import {DeckEditorNewComponent} from "./DeckEditor";
import {ApiStatus} from "../../enums";

interface OwnProps {
  match: {
    params: {
      deckId: number
    }
  }
  location: {
    search: string;
  }
}

interface PropsFromState {
  deckId: number;
  viewMode: DeckEditorViewMode;//"list" | "grid";
  deckProperties: DeckPropertiesDto | null;

  deckStats: DeckStats | null;

  isPropsDialogOpen: boolean;

  //Overview
  groupedCardOverviews: CardOverviewGroup[];
  //Non-grouped views will just snag cards from item at position 0
  cardDetailsById: { [deckCardId: number]: DeckCardDetail };

  //Detail
  // cardMenuAnchor: HTMLButtonElement | null;
  // cardMenuAnchorId: number;

  selectedCard: DeckCardOverview | null;
  selectedInventoryCards: DeckCardDetail[];

  //card detail
  selectedCardId: number;
  isCardDetailDialogOpen: boolean;
  isCardTagsDialogOpen: boolean;
  
  detailApiStatus: ApiStatus;
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
  constructor(props: DeckEditorProps) {
    super(props);
  }

  render() {
    return(
      <DeckEditorNewComponent
        deckId={this.props.deckId}
        deckProperties={this.props.deckProperties} 
        deckStats={this.props.deckStats} 
        isPropsDialogOpen={this.props.isPropsDialogOpen} 
        groupedCardOverviews={this.props.groupedCardOverviews} 
        cardDetailsById={this.props.cardDetailsById}
        selectedCard={this.props.selectedCard} 
        selectedInventoryCards={this.props.selectedInventoryCards} 
        selectedCardId={this.props.selectedCardId} 
        isCardDetailDialogOpen={this.props.isCardDetailDialogOpen} 
        isCardTagsDialogOpen={this.props.isCardTagsDialogOpen}
        />
    )
  }
}

function selectDeckOverviews(state: RootState): CardOverviewGroup[] {
  const { cardOverviews, cardGroups } = state.decks.deckDetailData; //state.data.deckDetail;

  if(state.decks.deckEditor.viewMode === "grouped"){
    const result = cardGroups.map(group => {
      const groupResult: CardOverviewGroup = {
        name: group.name,
        cardOverviews: group.cardOverviewIds.map(id => cardOverviews.byId[id]),
      }
      return groupResult;
    });
    return result;

  } else {

    return [{
      name: "All",
      cardOverviews: cardOverviews.allIds.map(id => cardOverviews.byId[id]),
    }];

  }
}

function getSelectedCardOverview(state: RootState): DeckCardOverview | null {
  const selectedOverviewCardId = state.decks.deckEditor.selectedOverviewCardId;
  if(selectedOverviewCardId){
    return state.decks.deckDetailData.cardOverviews.byId[selectedOverviewCardId];
  }
  return null;
}

function getSelectedDeckDetails(state: RootState): DeckCardDetail[] {
  const { cardOverviews, cardDetails } = state.decks.deckDetailData;
  const { selectedOverviewCardId } = state.decks.deckEditor;
  if(selectedOverviewCardId){
    const match = cardOverviews.byId[selectedOverviewCardId];
    if(match){
      return match.detailIds.map(id => cardDetails.byId[id]);
    }
    // return cardOverviews.byId[selectedOverviewCardId].detailIds.map(id => cardDetails.byId[id]);
  }
  return [];
}

interface ParsedQueryString { [key: string]:string }

function parseQueryString(queryString: string): ParsedQueryString {
  const result: ParsedQueryString = {};
  //leading char will be '?'
  if(queryString.length === 0) return result;

  const substring = queryString.substring(1).split('&');

  if(substring && substring.length){
    substring.forEach(element => {
      var keyVal = element.split('=');
      const key = keyVal[0];
      const val = keyVal[1];
      result[key] = val;
    });
  }

  return result;
}

function mapStateToProps(state: RootState, ownProps: OwnProps): PropsFromState {

  const queryString = parseQueryString(ownProps.location.search);

  //const selectedCardName = decodeURI(queryString['card'] || '');
  const selectedCardId = +queryString['cardId'] || 0;

  const show = queryString['show'];

  let isDetailDialogOpen = false;
  let isTagsDialogOpen = false;

  if(show === "tags"){
    isTagsDialogOpen = true;
  }
  if(show === "detail"){
    isDetailDialogOpen = true;
  }


  //const x = state.decks.data.detail.cardDetails.byId

  const result: PropsFromState = {
    deckId: +ownProps.match.params.deckId,
    viewMode: state.decks.deckEditor.viewMode,  //state.app.deckEditor.viewMode,

    deckProperties: state.decks.deckDetailData.deckProps,
    isPropsDialogOpen: state.decks.deckEditor.isPropsModalOpen,
    deckStats: state.decks.deckDetailData.deckStats,

    //Overview
    groupedCardOverviews: selectDeckOverviews(state),
    cardDetailsById: state.decks.deckDetailData.cardDetails.byId,

    //Detail
    selectedCard: getSelectedCardOverview(state),
    selectedInventoryCards: getSelectedDeckDetails(state),

    //card detail
    selectedCardId: selectedCardId,
    isCardDetailDialogOpen: isDetailDialogOpen,
    isCardTagsDialogOpen: isTagsDialogOpen,

    detailApiStatus: state.decks.deckDetailData.status,
  }

  return result;
}

export default connect(mapStateToProps)(DeckEditor);