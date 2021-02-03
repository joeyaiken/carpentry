import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from '@material-ui/core';
import { push } from 'react-router-redux';
import { AppState } from '../../configureStore';
import TrimmingToolLayout from './TrimmingToolLayout';
import { addPendingCard, cardImageAnchorSet, clearPendingCards, removePendingCard, requestTrimmingToolCards, trimmingToolFilterChanged } from './state/TrimmingToolActions'
import CardImagePopper from '../../common/components/CardImagePopper';

interface PropsFromState { 
    modalIsOpen: boolean;
    filterOptions: AppFiltersDto;
    searchFilters: TrimmingToolRequest;
    searchResultsById: { [key: number]: InventoryOverviewDto }
    searchReusltIds: number[];
    cardImageMenuAnchor: HTMLButtonElement | null;
    pendingCardsById: { [id: number]: TrimmedCard }
    pendingCardsIds: number[];
}

type TrimmingTipsProps = PropsFromState & DispatchProp<ReduxAction>;

class TrimmingTipsContainer extends React.Component<TrimmingTipsProps>{
    constructor(props: TrimmingTipsProps) {
        super(props);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleCloseModalClick = this.handleCloseModalClick.bind(this);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
        this.handleInfoButtonEnter = this.handleInfoButtonEnter.bind(this);
        this.handleInfoButtonLeave = this.handleInfoButtonLeave.bind(this);
        this.handleTrimClick = this.handleTrimClick.bind(this);
    }

    componentDidMount() {
        this.props.dispatch(requestTrimmingToolCards());
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(trimmingToolFilterChanged(event.target.name, event.target.value));
    }

    handleSearchButtonClick() {
        this.props.dispatch(requestTrimmingToolCards())
    }

    handleCloseModalClick(){
        //clear pending cards
        this.props.dispatch(clearPendingCards());
        this.props.dispatch(push('/inventory'));
    }

    handleAddPendingCard(name: string, cardId: number, isFoil: boolean){
        this.props.dispatch(addPendingCard(name, cardId, isFoil));
    }

    handleRemovePendingCard(name: string, cardId: number, isFoil: boolean){
        this.props.dispatch(removePendingCard(name, cardId, isFoil));
    }

    handleInfoButtonEnter(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        this.props.dispatch(cardImageAnchorSet(event.currentTarget));
    }

    handleInfoButtonLeave() {
        this.props.dispatch(cardImageAnchorSet(null));
    }

    handleTrimClick() {
        //
    }

    render() {
        const cardImageAnchorId = this.props.cardImageMenuAnchor?.value;
        const selectedOverview: InventoryOverviewDto = this.props.searchResultsById[cardImageAnchorId ?? 0];
        return (
            <React.Fragment>
                <CardImagePopper 
                    menuAnchor={this.props.cardImageMenuAnchor}
                    onClose={this.handleInfoButtonLeave}
                    image={selectedOverview?.imageUrl}
                    />
                <Dialog maxWidth="xl" open={this.props.modalIsOpen} onClose={() => {this.handleCloseModalClick()}} >
                    <DialogTitle>{`Trimming Tool`}</DialogTitle>
                    <DialogContent>
                        <TrimmingToolLayout
                            searchReusltIds={this.props.searchReusltIds}
                            searchResultsById={this.props.searchResultsById}
                            searchFilters={this.props.searchFilters}
                            pendingCardsById={this.props.pendingCardsById}
                            pendingCardsIds={this.props.pendingCardsIds}
                            onSearchClick={this.handleSearchButtonClick}
                            filterOptions={this.props.filterOptions}
                            onAddPendingCard={this.handleAddPendingCard}
                            onRemovePendingCard={this.handleRemovePendingCard}
                            onFilterChange={this.handleFilterChange}
                            onInfoButtonEnter={this.handleInfoButtonEnter}
                            onInfoButtonLeave={this.handleInfoButtonLeave}
                         />
                    </DialogContent>
                    <DialogActions>
                        <Button size="medium" onClick={() => this.handleCloseModalClick()}>Cancel</Button>
                        <Button size="medium" color="primary" variant="contained" onClick={() => this.handleTrimClick()}>Trim</Button>
                    </DialogActions>
                </Dialog>
            </React.Fragment>
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const containterState = state.inventory.trimmingTool;
    const result: PropsFromState = {
        modalIsOpen: true,
        filterOptions: state.core.data.filterOptions,
        searchFilters: containterState.searchProps,
        searchResultsById: containterState.searchResults.byId,
        searchReusltIds: containterState.searchResults.allIds,
        cardImageMenuAnchor: containterState.cardImageMenuAnchor,
        pendingCardsById: containterState.pendingCards.byId,
        pendingCardsIds: containterState.pendingCards.allIds,
    }
    return result;
}

export default connect(mapStateToProps)(TrimmingTipsContainer);