import { connect, DispatchProp } from 'react-redux'
import React from 'react'
// import {
//     // requestSaveDeck,
//     // deckEditorCardSelected,
//     // cardMenuButtonClick,
//     // deckCardRequestAlternateVersions,
//     // requestDeleteDeckCard,
//     // deckPropertyChanged,
//     // openDeckPropsModal,
//     // requestCancelDeckModalChanges,
//     // toggleDeckViewMode,
// } from '../actions/deckEditor.actions'

import { AppState } from '../reducers'

import { Typography, Box } from '@material-ui/core';

import { useLocation } from 'react-router-dom';
import { ensureDeckDetailLoaded } from '../actions/deckEditor.actions';
// import DeckPropertiesLayout from '../components/DeckPropertiesLayout';
// import DeckPropsBar from '../components/DeckPropsBar';
// import DeckCardList from '../components/DeckCardList';
// import DeckCardDetail from '../components/DeckCardDetail';
// import AppModal from '../components/AppModal';
// import CardMenu from '../components/CardMenu';
// import DeckCardGrid from '../components/DeckCardGrid';
// import DeckStatsBar from '../components/DeckStatsBar';
// import DeckEditorPropsBar from './DeckEditorPropsBar';
// import DeckEditorCardOverviews from './DeckEditorCardOverviews';
// import DeckEditorCardDetail from './DeckEditorCardDetail';
// import DeckEditorStatsBar from './DeckEditorStatsBar';
// import DeckEditorPropsModal from './DeckEditorPropsModal';

/**
 * The Deck Editor is basically a fancy data table
 */
// function useQuery() {
//     return new URLSearchParams(useLocation().search);
// }
interface PropsFromState {
    // viewMode: DeckEditorViewMode;//"list" | "grid";
    deckProperties: DeckDetailDto | null;
    // cardOverviews: InventoryOverviewDto[];
    // cardMenuAnchor: HTMLButtonElement | null;
    // deckPropsModalOpen: boolean;
    // selectedCard: InventoryOverviewDto | null;
    // selectedInventoryCards: InventoryCard[];
    // deckStats: DeckStats | null;
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props);
    }

    componentDidMount() {
        // this.props.dispatch(ensureDeckOverviewsLoaded())
        console.log('deck editor mount');
        //load deck detail: id, view
        //const location = useLocation();

        // const query = new URLSearchParams(location.search);
        
        // console.log(location);
        // this.props.dispatch(ensureDeckDetailLoaded());
        

    }


    //React.CssProperties

    render() {
        if(this.props.deckProperties === null){
            return(<Box><Typography>ERROR - Deck properties === null, cannot render</Typography></Box>)
        } else {
            return(
                <React.Fragment>
                    {/* IDK if this should be an AppModal with a container inside of it or not
                        AKA "DeckEditorPropsModal" that contains an AppModal
                            or 
                        "DeckEditorPropsForm" inside of an AppModal, that belongs to this Deck Editor
                    */}
                    {/* <DeckEditorPropsModal />

                    <DeckEditorPropsBar /> */}

                    <Box className="flex-row flex-section" style={{
                        overflow:'auto',
                        alignItems:'stretch'
                        }}>
                        {/* Does this need to be flex 1 0 ? */}
    
                        {/* <Box className="flex-row" > */}
                        <div className="flex-section" 
                            style={{
                                overflow:'auto',
                                flex:'1 1 70%'
                            }}
                        >
                            {/* <DeckEditorCardOverviews /> */}
                        </div>
                        <div className="flex-section" 
                            style={{
                                overflow:'auto',
                                flex:'1 1 30%'
                            }}
                        >
                            {/* <DeckEditorCardDetail /> */}
                        </div>
                    </Box>
                    {/* <DeckEditorStatsBar /> */}
                </React.Fragment>
            )
        }
    }
}

// function selectDeckOverviews(state: AppState): InventoryOverviewDto[] {
//     const { allCardOverviewNames, cardOverviewsByName } = state.data.deckDetail;
//     return allCardOverviewNames.map(name => cardOverviewsByName[name]);
// }

// function getSelectedCardOverview(state: AppState): InventoryOverviewDto | null {
//     if(state.app.deckEditor.selectedOverviewCardName){
//         return state.data.deckDetail.cardOverviewsByName[state.app.deckEditor.selectedOverviewCardName];
//     }
//     return null;
// }

// function getSelectedDeckDetails(state: AppState): InventoryCard[] {
//     const { selectedInventoryCardIds, cardDetailsById } = state.data.deckDetail;
//     return selectedInventoryCardIds.map(id => cardDetailsById[id]);
//     //selectedInventoryCards: state.deckEditor.selectedInventoryCards,
// }

function mapStateToProps(state: AppState): PropsFromState {

    console.log('DE state to props');
    console.log(state);
    const result: PropsFromState = {
        // viewMode: state.app.deckEditor.viewMode,
        // cardMenuAnchor: state.ui.deckEditorMenuAnchor,
        // selectedCard: getSelectedCardOverview(state),
        // selectedInventoryCards: getSelectedDeckDetails(state),
        deckProperties: null //state.data.deckDetail.deckProps,
        // cardOverviews: selectDeckOverviews(state),
        // deckStats: state.data.deckDetail.deckStats,
        // deckPropsModalOpen: state.ui.deckPropsModalOpen,
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor)