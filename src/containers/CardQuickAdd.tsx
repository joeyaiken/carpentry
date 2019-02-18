import { connect, DispatchProp } from 'react-redux'
import React from 'react';
import {
    // selectDeck,
    // deckChanged
    searchApplied,
    searchValueChange,
    searchCardSelected,
    addCardToDeck,
    addCardToIndex

} from '../actions'
import InputField from '../components/InputField';
import { Card } from 'mtgsdk-ts';
import { stat } from 'fs';
//import Magic = require('mtgsdk-ts');
// import Magic from 'mtgsdk-ts';
import { mapCardToICard } from '../data/lumberyard';

interface PropsFromState {
    searchValue: string;
    searchResults: Card[];
    selectedSearchResultId?: string;
    selectedSearchResultName?: string;
}

type CardQuickAddProps = PropsFromState & DispatchProp<ReduxAction>;

class CardQuickAdd extends React.Component<CardQuickAddProps> {
    constructor(props: CardQuickAddProps) {
        super(props);
        //this.handleDeckSelected = this.handleDeckSelected.bind(this);
        this.handleSearchFilterChanged = this.handleSearchFilterChanged.bind(this);
        this.handleSearchClick = this.handleSearchClick.bind(this);
        //this.handleAddToRaresClick = this.handleAddToRaresClick(bind)
        this.handleCardClick = this.handleCardClick.bind(this);

        this.handleAddToDeckClick = this.handleAddToDeckClick.bind(this);
    }

    handleSearchFilterChanged(event: any){
        this.props.dispatch(searchValueChange(event.target.value))
    }

    handleSearchClick(event: any){
        event.preventDefault();
        //this.props.dispatch(searchApplied());
        this.props.dispatch(searchApplied(this.props.searchValue));
        //fetchCards
        //going to use 'cards-where' for most of this

        //this doesn't belong here but YOLO
        // console.log('searching for cards')
        // Magic.Cards.where({name: "Nicol"}).then(results => {
        //     console.log('grand result dump');
        //     console.log(results);
        //     for (const card of results) console.log(card.name);
        // });

    }

    handleCardClick(cardId: string, cardName: string){
        console.log('card clicked');
        console.log(cardId);
        console.log(cardName);
        this.props.dispatch(searchCardSelected(cardId, cardName));
    }

    // componentDidMount() {
    //     // const { dispatch, selectedSubreddit } = this.props
    //     // dispatch(fetchPostsIfNeeded(selectedSubreddit))
    // }

    // componentDidUpdate(prevProps: any) {
    //     // if (this.props.selectedSubreddit !== prevProps.selectedSubreddit) {
    //     //   const { dispatch, selectedSubreddit } = this.props
    //     //   dispatch(fetchPostsIfNeeded(selectedSubreddit))
    //     // }
    // }
    
    // handleChange(nextSubreddit: any) {
    //     // this.props.dispatch(selectSubreddit(nextSubreddit))
    //     // this.props.dispatch(fetchPostsIfNeeded(nextSubreddit))
    // }
    
    // handleRefreshClick(e: any) {
    //     // e.preventDefault()

    //     // const { dispatch, selectedSubreddit } = this.props
    //     // dispatch(invalidateSubreddit(selectedSubreddit))
    //     // dispatch(fetchPostsIfNeeded(selectedSubreddit))
    // }

    // let searchResults: JSX.Element;

    handleAddToDeckClick(){
        console.log('trying to add to deck');
        console.log(this.props.selectedSearchResultName);
        // this.props.dispatch(addCardToDeck(this.props.selectedSearchResultName || ""))
        // if(this.props.selectedSearchResult){
        //     this.props.dispatch(addCardToDeck(this.props.selectedSearchResult || ""))
        // }
        if(this.props.selectedSearchResultId){
            let thisCard = this.props.searchResults.find((card) => {
                return (card.id == this.props.selectedSearchResultId)
            })
            if(thisCard){
                this.props.dispatch(addCardToIndex(mapCardToICard(thisCard)))
                let card: IDeckCard = {
                    name: thisCard.name,
                    set: thisCard.set
                }
                this.props.dispatch(addCardToDeck(card))
            }
            
        }
        //thisCard
        // this.props.dispatch(add)
        
        // this.props.dispatch(addCardToIndex(mapCardToICard()))
    }
    handleAddToRaresClick(){

    }

    render(){
        //only want to show the "add" buttons if a card is selected

        // if(this.props.selectedSearchResult){

        // }

        const addCardButtons: JSX.Element = (
            <>
                <div className="outline-section flex-col">
                    <label>Add to deck</label>
                    <button onClick={this.handleAddToDeckClick}>Add</button>
                </div>
                <div className="outline-section flex-col">
                    <label>Add to rares</label>
                    <button onClick={this.handleAddToRaresClick}>Add</button>
                </div>
            </>
        );

        return(
            <div className="sd-card-quick-add card">
                <div className="card-header">
                    <label>Card Search</label>
                </div>
                <form onSubmit={this.handleSearchClick}>
                    <div className="outline-section flex-row">
                        
                            <InputField label="Card Search" value={this.props.searchValue} property="" onChange={this.handleSearchFilterChanged} />
                            <div className="outline-section flex-col">
                                <label>Search</label>
                                <button onClick={this.handleSearchClick}>GO</button>
                            </div>
                        {
                            (this.props.selectedSearchResultId) && addCardButtons
                        }
                        
                    </div>
                </form>
                <div className="outline-section flex-row card-container">
                    {
                        this.props.searchResults.map((card: Card, index: number) => {
                            // console.log('card match?')
                            // console.log(this.props.selectedSearchResult)
                            // console.log(card.id)
                            const cardIsSelected = (this.props.selectedSearchResultId == card.id);
                            return(
                                <div key={card.id} className={cardIsSelected ? "magic-card selected-card" : "magic-card"} onClick={() => this.handleCardClick(card.id, card.name) }>
                                    <img src={card.imageUrl} />
                                </div>
                            )
                        })
                    }
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: State): PropsFromState {
    const searchFilterName = state.cardSearch.searchFilter.name;
    const searchResults: Card[] = state.cardSearch.searchFilter.results.slice();
    const selectedSearchResultId = state.cardSearch.searchFilter.selectedCardId;
    const selectedSearchResultName = state.cardSearch.searchFilter.selectedCardName;
    // console.log('can we hit thisss?')
    const result: PropsFromState = {
        searchValue: searchFilterName,
        searchResults: searchResults,
        selectedSearchResultId: selectedSearchResultId,
        selectedSearchResultName: selectedSearchResultName
    }
    return result;
}

export default connect(mapStateToProps)(CardQuickAdd);



