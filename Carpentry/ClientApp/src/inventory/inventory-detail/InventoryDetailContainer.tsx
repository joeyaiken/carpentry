import React from 'react';
import { connect, DispatchProp } from 'react-redux';
// import { Box, CardHeader, CardMedia, Table, TableHead, TableRow, TableCell, TableBody, Card } from '@material-ui/core';
import InventoryDetailLayout from './components/InventoryDetailLayout';
import AppModal from '../../common/components/AppModal';
// import { requestInventoryDetail } from '../../_actions/inventoryActions';

import { ensureInventoryDetailLoaded } from './state/InventoryDetailActions';
import { AppState } from '../../configureStore';
// import { parseConfigFileTextToJson } from 'typescript';

interface OwnProps {
    match: {
        params: {
            cardId: number
        }
    }
}

interface PropsFromState { 
    selectedDetailItem: InventoryDetailDto;
    modalIsOpen: boolean;
    selectedCardId: number;
}

type InventoryDetailContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class InventoryDetailContainer extends React.Component<InventoryDetailContainerProps>{
    constructor(props: InventoryDetailContainerProps) {
        super(props);
        this.handleCloseModalClick = this.handleCloseModalClick.bind(this);
    }

    //on load/bind, ensure cardID is loaded
    componentDidMount() {
        // this.props.dispatch(ensureDeckDetailLoaded(this.props.deckId));
        this.props.dispatch(ensureInventoryDetailLoaded(this.props.selectedCardId))
    }

    handleCloseModalClick(cardId: number | null){
        // this.props.dispatch(requestInventoryDetail(cardId));
    }


    render() {
        // let title: string = (this.props.selectedDetailItem != null) ? 
        // `Inventory Detail - ${this.props.selectedDetailItem.cards[0].name}` : "Inventory Detail - Unknown";
        let cardName = "Unknown";
        if(this.props.selectedDetailItem.cards.length > 0){
            cardName = this.props.selectedDetailItem.cards[0].name;
        }
        return (
            <React.Fragment>
                <AppModal 
                    title={ `Inventory Detail - ${cardName}`}
                    isOpen={this.props.modalIsOpen} 
                    onCloseClick={() => {this.handleCloseModalClick(null)}} 
                    >
                        {
                            this.props.selectedDetailItem &&
                            <InventoryDetailLayout 
                                selectedDetailItem={this.props.selectedDetailItem}
                            />
                        }
                </AppModal>
            </React.Fragment>
        );
    }
}

//Eventually this should be replaced with something different...(like a different container)
function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { inventoryCardsById, inventoryCardAllIds, cardsById, allCardIds, selectedCardName } = state.inventory.data.detail;
    const result: InventoryDetailDto = {
        name: selectedCardName,
        inventoryCards: inventoryCardAllIds.map(invId => inventoryCardsById[invId]),
        cards: allCardIds.map(cardId => cardsById[cardId]),
    }
    return result;
}

//State
function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {
    const result: PropsFromState = {
        selectedDetailItem: selectInventoryDetail(state),
        // modalIsOpen: state.ui.isInventoryDetailModalOpen, //TODO - get this mapped from router state
        selectedCardId: ownProps.match.params.cardId,

        modalIsOpen: Boolean(ownProps.match.params.cardId != null),
    }
    return result;
}

export default connect(mapStateToProps)(InventoryDetailContainer);



