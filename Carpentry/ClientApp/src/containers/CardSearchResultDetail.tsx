import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../reducers';

import { 
    requestAddDeckCard,
    cardSearchAddPendingCard,
    cardSearchRemovePendingCard,

} from '../actions/cardSearch.actions';

import {
    Button,
    Typography,
    Paper,
    Box,
    Card,
    CardHeader,
    CardContent,
    CardMedia,
    TableHead,
    TableRow,
    TableBody,
    Table,
    TableCell,
} from '@material-ui/core';

interface PropsFromState { 
    searchContext: "deck" | "inventory";
    pendingCards: { [key:number]: PendingCardsDto }
    selectedCard: MagicCard | null;
    selectedCardDetail: InventoryDetailDto | null;
    
}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

interface SelectedCardDetailSectionProps {
    selectedCard: MagicCard;
    pendingCards?: PendingCardsDto;
    selectedCardDetail: InventoryDetailDto | null;
    handleAddPendingCard: (multiverseId: number, isFoil: boolean, variant: string) => void;
    handleRemovePendingCard: (multiverseId: number, isFoil: boolean, variant: string) => void;
    handleAddInventoryCard?: (inventoryCard: InventoryCard) => void;
    handleAddNewCard?: (multiverseId: number, isFoil: boolean, variant: string) => void;
    // handleMoveCard?: (inventoryCard: InventoryCard) => void;
}

function SelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
    return(<Paper className="static-section">
    <Box className="flex-column">
        {   Object.keys(props.selectedCard.variants).map((id: string) => {
               
            let countNormal = 0;
            let countFoil = 0;
            if(props.pendingCards)
            {
                countNormal = props.pendingCards.cards.filter(c => c.variantName == id && c.isFoil == false).length;
                countFoil = props.pendingCards.cards.filter(c => c.variantName == id && c.isFoil == true).length;
            }

            //const thisPendingCard = (this.props.selectedCard) && this.props.pendingCards[this.props.selectedCard.multiverseId];
            return (
                <Card key={id} className="outline-section flex-row">
                    {/* <Box>

                    </Box> */}
                    {/* <CardHeader
                        titleTypographyProps={{variant:"body1"}} 
                        title={props.card.name}
                    /> */}

                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        className="item-image"
                        image={(props.selectedCard.variants[id]) || undefined} />
                    <CardContent>
                        <Box className="flex-col">
                            <Box className="flex-col">
                                <Box className="flex-col">
                                    <Typography>{`${props.selectedCard.prices[id]} | ${props.selectedCard.prices[`${id}_foil`]}`}</Typography>
                                </Box>
                                <Box className="flex-col">
                                    <Typography>Normal ({countNormal})</Typography>
                                    <Box className="flex-row">
                                        <Button variant="outlined" onClick={() => {props.handleRemovePendingCard(props.selectedCard.multiverseId, false, id)} } >-</Button>
                                        <Button variant="outlined" onClick={() => {props.handleAddPendingCard(props.selectedCard.multiverseId, false, id)} } >+</Button>
                                    </Box>
                                </Box>
                                <Box className="flex-col">
                                    <Typography>Foil ({countFoil})</Typography>
                                    <Box className="flex-row">
                                        <Button variant="outlined" onClick={() => {props.handleRemovePendingCard(props.selectedCard.multiverseId, true, id)} } >-</Button>
                                        <Button variant="outlined" onClick={() => {props.handleAddPendingCard(props.selectedCard.multiverseId, true, id)} } >+</Button>
                                    </Box>
                                </Box>
                            </Box>
                            {/* <Box className="flex-row">
                                <div className="outline-section">
                                    
                                    <span>Count ({countNormal}</span>
                                    
                                </div>
                                <div className="outline-section">
                                    <span>Foil ({countFoil})</span>
                                </div>
                            </Box> */}
                        </Box>
                        
                    </CardContent>
                    
                    {/*  variant="contained" */}
                    {/* <CardActions>
                        
                    </CardActions> */}
                </Card>
            )})
        


        }
    </Box>

</Paper>);
}

interface InventoryDetailTableProps {
    detail: InventoryDetailDto
    handleAddCardClick?: (inventoryCard: InventoryCard) => void;
}

function InventoryDetailTable(props: InventoryDetailTableProps): JSX.Element {
    return(
        <Table size="small">
            <TableHead>
                <TableRow>
                    <TableCell>Set</TableCell>
                    <TableCell>Style</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell></TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {
                    props.detail.inventoryCards.map(item => {
                        const thisCard = props.detail.cards.find(x => x.multiverseId === item.multiverseId);

                        return(
                        <TableRow key={item.id}>
                            <TableCell>{thisCard && thisCard.set}</TableCell>
                            <TableCell>{item.variantName}{item.isFoil &&" foil"}</TableCell>
                            <TableCell>
                                {
                                    item.deckCards.length > 0 &&
                                    item.deckCards[0].deckName
                                }
                                {
                                    item.deckCards.length == 0 &&
                                    item.statusId == 1 &&
                                    "Inventory"
                                }
                                {
                                    item.statusId == 2 &&
                                    "Buy List"
                                }
                                {
                                    item.statusId == 2 &&
                                    "Sell List"
                                }
                            </TableCell>
                            {
                                item.deckCards.length === 0 && 
                                <TableCell>
                                    <Button size="small" variant="contained"
                                        onClick={() => props.handleAddCardClick && props.handleAddCardClick(item)}
                                    >Add</Button>
                                </TableCell>
                            }
                            {
                                item.deckCards.length === 1 && 
                                <TableCell>
                                    <Button size="small" variant="contained"
                                        onClick={() => props.handleAddCardClick && props.handleAddCardClick(item)}
                                    >Move</Button>
                                </TableCell>
                            }
                            {item.deckCards.length > 1 && <TableCell>In a Deck</TableCell>}
                        </TableRow>
                        )
                    })
                }
            </TableBody>
        </Table>
    );
}

function DeckSelectedCardSection(props: SelectedCardDetailSectionProps): JSX.Element {
    
    return(<Paper className="static-section">
    <Box className="flex-column">
        <Card className="outline-section flex-col">
            <CardHeader titleTypographyProps={{variant:"body1"}} title="Inventory"/>
            {
                //What if this just itterated over the collection of inventory items, and displayed card data?
                //At the end of the day, I shouldn't have a ton of each card, including variants
                //Should this be Card components, or a table?
                //What if I don't allow cards that are already in a deck?

                // Inventory detail info will go here
                // How many exist of each variant?
                // How many foil / non foil?
                // How many in existing decks?
                props.selectedCardDetail && //props.selectedCardDetail.inventoryCards &&
                <InventoryDetailTable detail={props.selectedCardDetail} handleAddCardClick={props.handleAddInventoryCard} />
                
            }
            {/* Each inventory card should have a label for (in # decks) */}
        </Card>
        
        <Card className="outline-section flex-col">
            <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
            
            {   Object.keys(props.selectedCard.variants).map((variant: string) => {
                return (
                    // <Card key={id} className="outline-section flex-col">
                        // <CardHeader titleTypographyProps={{variant:"body1"}} title="Add New"/>
                        <Box key={variant} className="flex-row outline-section">
                            <CardMedia 
                                style={{height:"310px", width: "223px"}}
                                className="item-image"
                                image={(props.selectedCard.variants[variant]) || undefined} />
                            <CardContent>
                                <Box className="flex-col">
                                    <Box className="flex-col">
                                        <Typography>{`${props.selectedCard.prices[variant]} | ${props.selectedCard.prices[`${variant}_foil`]}`}</Typography>
                                    </Box>
                                    <Box className="flex-col">
                                        <Button 
                                            variant="outlined" 
                                            onClick={() => { props.handleAddNewCard && props.handleAddNewCard(props.selectedCard.multiverseId,false,variant) }} 
                                        >
                                            Add Normal
                                        </Button>
                                        <Button variant="outlined" onClick={() => { props.handleAddNewCard && props.handleAddNewCard(props.selectedCard.multiverseId,true,variant) }} >Add Foil</Button>
                                    </Box>
                                </Box>
                            </CardContent>
                        </Box>
                    // </Card>
                )})
            }
        </Card>
    </Box>
</Paper>);
}

class CardSearch extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
        this.handleAddExistingCardClick = this.handleAddExistingCardClick.bind(this);
        this.handleAddNewCardClick = this.handleAddNewCardClick.bind(this);
    }

    handleAddPendingCard(multiverseId: number, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchAddPendingCard(multiverseId, isFoil, variant));
    }

    handleRemovePendingCard(multiverseId: number, isFoil: boolean, variant: string){
        this.props.dispatch(cardSearchRemovePendingCard(multiverseId, isFoil, variant));
    }

    handleAddExistingCardClick(inventoryCard: InventoryCard): void{
        this.props.dispatch(requestAddDeckCard(inventoryCard));
    }
    
    handleAddNewCardClick(multiverseId: number, isFoil: boolean, variant: string): void{
        let inventoryCard: InventoryCard = {
            id: 0,
            deckCards: [],
            isFoil: isFoil,
            variantName: variant,
            multiverseId: multiverseId,
            statusId: 1,
            name: '',
            set: '',
        }
        this.props.dispatch(requestAddDeckCard(inventoryCard));
    }

    render() {
        return (
        <React.Fragment>
            {
                this.props.selectedCard && this.props.searchContext == "inventory" &&
                <SelectedCardSection 
                    selectedCard={this.props.selectedCard}
                    pendingCards={this.props.pendingCards[this.props.selectedCard.multiverseId]}
                    handleAddPendingCard={this.handleAddPendingCard}
                    handleRemovePendingCard={this.handleRemovePendingCard}
                    selectedCardDetail={null} />
            }
            {
                this.props.selectedCard && this.props.searchContext == "deck" &&
                <DeckSelectedCardSection 
                    selectedCard={this.props.selectedCard}
                    pendingCards={this.props.pendingCards[this.props.selectedCard.multiverseId]}
                    handleAddPendingCard={this.handleAddPendingCard}
                    handleRemovePendingCard={this.handleRemovePendingCard} 
                    selectedCardDetail={this.props.selectedCardDetail}
                    handleAddInventoryCard={this.handleAddExistingCardClick}
                    handleAddNewCard={this.handleAddNewCardClick}
                    // handleMoveCard={this.handleMoveCardClick}
                    
                    />
            }
        </React.Fragment>);
    }
}

function selectInventoryDetail(state: AppState): InventoryDetailDto {
    const { allCardIds, cardsById, inventoryCardAllIds, inventoryCardsById } = state.data.cardSearchInventoryDetail;
    const result: InventoryDetailDto = {
        cards: allCardIds.map(id => cardsById[id]),
        inventoryCards: inventoryCardAllIds.map(id => inventoryCardsById[id]),
    }
    return result;
}
function mapStateToProps(state: AppState): PropsFromState {
   
    const result: PropsFromState = {
        searchContext: (state.app.core.visibleContainer == "deckEditor") ? "deck":"inventory",
        pendingCards: state.data.cardSearchPendingCards.pendingCards,
        selectedCard: state.app.cardSearch.selectedCard,
        selectedCardDetail: selectInventoryDetail(state),
    }

    return result;
}

export default connect(mapStateToProps)(CardSearch);
