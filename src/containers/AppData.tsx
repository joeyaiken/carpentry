import { connect, DispatchProp } from 'react-redux'
import React from 'react';
import {
    appDataStoreStringChanged,
    // deckChanged
    // searchApplied,
    // searchValueChange,
    // searchCardSelected,
    // addCardToDeck

} from '../actions'
// import InputField from '../components/InputField';
// import { Card } from 'mtgsdk-ts';
// import { stat } from 'fs';
//import Magic = require('mtgsdk-ts');
// import Magic from 'mtgsdk-ts';


interface PropsFromState {
    dataObject: any;
    // searchValue: string;
    // searchResults: Card[];
    // selectedSearchResultId?: string;
    // selectedSearchResultName?: string;
}

type AppDataProps = PropsFromState & DispatchProp<ReduxAction>;

class AppData extends React.Component<AppDataProps> {
    constructor(props: AppDataProps) {
        super(props);
        // //this.handleDeckSelected = this.handleDeckSelected.bind(this);
        // this.handleSearchFilterChanged = this.handleSearchFilterChanged.bind(this);
        // this.handleSearchClick = this.handleSearchClick.bind(this);
        // //this.handleAddToRaresClick = this.handleAddToRaresClick(bind)
        // this.handleCardClick = this.handleCardClick.bind(this);

        this.handleDataObjectUpdated = this.handleDataObjectUpdated.bind(this);
    }

    handleDataObjectUpdated(event: any){
        this.props.dispatch(appDataStoreStringChanged(event.target.value))
    }

    // handleSearchClick(){
    //     //this.props.dispatch(searchApplied());
    //     this.props.dispatch(searchApplied(this.props.searchValue));
    //     //fetchCards
    //     //going to use 'cards-where' for most of this

    //     //this doesn't belong here but YOLO
    //     // console.log('searching for cards')
    //     // Magic.Cards.where({name: "Nicol"}).then(results => {
    //     //     console.log('grand result dump');
    //     //     console.log(results);
    //     //     for (const card of results) console.log(card.name);
    //     // });

    // }


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

    // handleAddToDeckClick(){
    //     console.log('trying to add to deck');
    //     console.log(this.props.selectedSearchResultName)
    //     this.props.dispatch(addCardToDeck(this.props.selectedSearchResultName || ""))
    //     // if(this.props.selectedSearchResult){
    //     //     this.props.dispatch(addCardToDeck(this.props.selectedSearchResult || ""))
    //     // }
        
    // }
    // handleAddToRaresClick(){

    // }

    render(){


        //only want to show the "add" buttons if a card is selected

        // if(this.props.selectedSearchResult){

        // }

        // const addCardButtons: JSX.Element = (
        //     <>
        //         <div className="outline-section flex-col">
        //             <label>Add to deck</label>
        //             <button onClick={this.handleAddToDeckClick}>Add</button>
        //         </div>
        //         <div className="outline-section flex-col">
        //             <label>Add to rares</label>
        //             <button onClick={this.handleAddToRaresClick}>Add</button>
        //         </div>
        //     </>
        // );
        /* 
        
        <div className="outline-section flex-row">
                    <InputField label="Card Search" value={this.props.searchValue} property="" onChange={this.handleSearchFilterChanged} />
                    <div className="outline-section flex-col">
                        <label>Search</label>
                        <button onClick={this.handleSearchClick}>GO</button>
                    </div>
                    {
                        // (this.props.selectedSearchResultId) && addCardButtons
                    }
                    
                </div>
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
        
        */
        return(
            <div className="sd-card-quick-add card">
                <div className="card-header">
                    <label>App Data</label>
                </div>
                <div className="outline-section">
                    {/* <textarea  /> */}
                    <textarea value={ this.props.dataObject } onChange={ this.handleDataObjectUpdated } />
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: State): PropsFromState {
    // const searchFilterName = state.cardSearch.searchFilter.name;
    // const searchResults: Card[] = state.cardSearch.searchFilter.results.slice();
    // const selectedSearchResultId = state.cardSearch.searchFilter.selectedCardId;
    // const selectedSearchResultName = state.cardSearch.searchFilter.selectedCardName;
    // console.log('can we hit thisss?')

    // detailList: IDeckDetail[]; //should this be a dictionary instead?
    // cardLists: ICardList[]; //should this be a dictionary instead?
    // rareStore: ICardList;
    const appStateObj = {
        detailList: state.data.detailList,
        cardLists: state.data.cardLists
        //rareStore: state.data.rareStore
    }

    const result: PropsFromState = {
        dataObject: JSON.stringify(appStateObj)
        // searchValue: searchFilterName,
        // searchResults: searchResults,
        // selectedSearchResultId: selectedSearchResultId,
        // selectedSearchResultName: selectedSearchResultName
    }
    return result;
}

export default connect(mapStateToProps)(AppData);



