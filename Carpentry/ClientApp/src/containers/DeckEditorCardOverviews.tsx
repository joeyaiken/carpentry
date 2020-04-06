import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { deckEditorCardSelected } from '../actions/deckEditor.actions';
import { AppState } from '../reducers';
import { Typography, Box, Paper, Table, TableHead, TableRow, TableCell, TableBody, Tab } from '@material-ui/core';
import DeckCardList from '../components/DeckCardList';
import DeckCardGrid from '../components/DeckCardGrid';
import ManaCostChip from '../components/ManaCostChip';

interface PropsFromState {
    viewMode: DeckEditorViewMode;
    // cardOverviews: InventoryOverviewDto[];

    groupedCardOverviews: CardOverviewGroup[]; //what if this is all there was, instead of just overviews?
    //Non-grouped views will just snag cards from item at position 0
}

type DeckEditorCardOverviewProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditorCardOverviews extends React.Component<DeckEditorCardOverviewProps> {
    constructor(props: DeckEditorCardOverviewProps) {
        super(props);
        this.handleCardSelected = this.handleCardSelected.bind(this);
    }

    handleCardSelected(cardOverview: InventoryOverviewDto) {
        this.props.dispatch(deckEditorCardSelected(cardOverview))
    }

    render() {
        return(
            <React.Fragment>
                {this.props.viewMode === "list" && 
                    <DeckCardList 
                        //cardOverviews={this.props.cardOverviews} 
                        cardOverviews={this.props.groupedCardOverviews[0].cardOverviews} 
                        onCardSelected={this.handleCardSelected} 
                    />}
                {/* {this.props.viewMode === "grid" && <DeckCardGrid cardOverviews={this.props.cardOverviews} onCardSelected={this.handleCardSelected} />} */}
                {this.props.viewMode === "grid" && <DeckCardGrid cardOverviews={this.props.groupedCardOverviews[0].cardOverviews} onCardSelected={this.handleCardSelected} />}
                {this.props.viewMode === "grouped" && <GroupedDeckCardList groupedCardOverviews={this.props.groupedCardOverviews} onCardSelected={this.handleCardSelected} />}
            </React.Fragment>
        )
    }
}

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    
    //cardOverviews: InventoryOverviewDto[];
    groupedCardOverviews: CardOverviewGroup[];

    // cardGroups: GroupedInventoryOverview[];

    onCardSelected: (card: InventoryOverviewDto) => void;
}

function GroupedDeckCardList(props: ComponentProps): JSX.Element {
    // className="flex-section"
    return (
        <Paper>
            <Table size="small">
                {/* <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>Count</TableCell>
                        <TableCell>Type</TableCell>
                        <TableCell>Cost</TableCell>
                        <TableCell>Category</TableCell>
                    </TableRow>
                </TableHead> */}
                {
                    props.groupedCardOverviews.map(group => (
                        <React.Fragment key={group.name}>
                            <TableHead key={`th-${group.name}`}>
                                <TableRow>
                                    <TableCell size="medium"></TableCell>
                                    <TableCell size="medium" colSpan={2}>{group.name} ({group.cardOverviews.length})</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody key={`tb-${group.name}`}>
                                {
                                    group.cardOverviews.map(cardItem => 
                                        <TableRow onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                                            key={cardItem.multiverseId+cardItem.name}>
                                            <TableCell>{(cardItem.count > 1) && cardItem.count}</TableCell>
                                            <TableCell>{cardItem.name}</TableCell>
                                            {/* <TableCell>{cardItem.count}</TableCell> */}
                                            {/* <TableCell>{cardItem.type}</TableCell> */}
                                            {/* <TableCell>{cardItem.cost}</TableCell> */}
                                            <TableCell><ManaCostChip costString={cardItem.cost} /></TableCell>

                                            {/* <TableCell>{cardItem.description}</TableCell> */}
                                        </TableRow>
                                    )
                                }
                            </TableBody>
                        </React.Fragment>
                    ))
                }

         
            </Table>
        </Paper>
    );
}

function selectDeckOverviews(state: AppState): CardOverviewGroup[] {
    const { allCardOverviewIds, cardOverviewsById, cardGroups } = state.data.deckDetail;

    if(state.app.deckEditor.viewMode === "grouped"){

        const result = cardGroups.map(group => {
            const groupResult: CardOverviewGroup = {
                name: group.name,
                cardOverviews: group.cardOverviewIds.map(id => cardOverviewsById[id]),
            }
            return groupResult;
        });
        return result;

    } else {

        return [{
            name: "All",
            cardOverviews: allCardOverviewIds.map(id => cardOverviewsById[id]),
        }];

    }
}

// function selectGroupedOverviews(state: AppState) : CardOverviewGroup[] {
//     const { cardOverviewsByName, cardGroups } = state.data.deckDetail;

//     const result = cardGroups.map(group => {
//         const groupResult: CardOverviewGroup = {
//             name: group.name,
//             cardOverviews: group.cardNames.map(cardName => cardOverviewsByName[cardName]),
//         }
//         return groupResult;
//     });

//     return result;
// }

// declare interface GroupedInventoryOverview {
//     name: string;
//     cards: InventoryOverviewDto[];
// }

function mapStateToProps(state: AppState): PropsFromState {

    //cardGroups: CardOverviewGroup[];
    //Need to map: NamedCardGroup => CardOverviewGroup
    const result: PropsFromState = {
        viewMode: state.app.deckEditor.viewMode,
        groupedCardOverviews: selectDeckOverviews(state),
        //cardOverviews: selectDeckOverviews(state),
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditorCardOverviews)
