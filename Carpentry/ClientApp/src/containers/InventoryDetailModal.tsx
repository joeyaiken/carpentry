import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../reducers'
import {
    requestInventoryDetail,
} from '../actions/inventory.actions'
import AppModal from '../components/AppModal';
import InventoryDetailLayout from '../components/InventoryDetailLayout';

interface PropsFromState { 
    selectedDetailItem: InventoryDetailDto | null;
    modalIsOpen: boolean;
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class Inventory extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        this.handleCardDetailSelected = this.handleCardDetailSelected.bind(this);
    }

    handleCardDetailSelected(card: string | null){
        this.props.dispatch(requestInventoryDetail(card));
    }

    render() {
        return (
            <React.Fragment>
                <AppModal 
                    title="Card Details" 
                    isOpen={this.props.modalIsOpen} 
                    onCloseClick={() => {this.handleCardDetailSelected(null)}} 
                    onSaveClick={() => { }}
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
    const { inventoryCardsById, inventoryCardAllIds, cardsById, allCardIds } = state.data.inventoryDetail;
    const result: InventoryDetailDto = {
        inventoryCards: inventoryCardAllIds.map(invId => inventoryCardsById[invId]),
        cards: allCardIds.map(cardId => cardsById[cardId]),
    }
    return result;
}

//State
function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        selectedDetailItem: selectInventoryDetail(state),
        modalIsOpen: state.ui.isInventoryDetailModalOpen,
    }
    return result;
}

export default connect(mapStateToProps)(Inventory);



