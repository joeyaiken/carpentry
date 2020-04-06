import React from 'react';
import AppLayout from '../components/AppLayout';
import { connect, DispatchProp } from 'react-redux';
import { AppState } from '../reducers';
import Inventory from './Inventory'
import DeckEditor from './DeckEditor'
import '../App.css';



// import {
//     // requestInventoryItems, 
//     // appBarSectionToggle,
// } from './actions/index.actions'

import {
    appBarAddClicked, navigate, requestDeckDetail, requestSaveNewDeck, newDeckFieldChange
} from '../actions/core.actions';
import { Typography, Modal } from '@material-ui/core';
// import { typography } from '@material-ui/system';
// import Inventory from './containers/Inventory';
import CardSearch from './CardSearch';
// import NewDeckModal from './components/NewDeckModal';
// import CoreData from './containers/CoreData';
import AppMenuContent from '../components/AppMenuContent';
// import DeckQuickAdd from './containers/DeckQuickAdd';
import AppModalLayout from '../components/AppModalLayout';
import DeckPropertiesLayout from '../components/DeckPropertiesLayout';

type AppContainerEnum = 'deckEditor' | 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null;

interface PropsFromState {
    isCardSearchShowing: boolean;
    // visibleContainer: string;
    visibleContainerEnum: AppContainerEnum;
    // availableDecks: DeckProperties[];

    newDeckDto: DeckProperties;
    isNewDeckModalOpen: boolean;

    filtersToDefault: FilterDescriptor[];
}

type AppProps = PropsFromState & DispatchProp<ReduxAction>;

class App extends React.Component<AppProps>{
    constructor(props: AppProps) {
        super(props);
        this.handleAppBarButtonClicked = this.handleAppBarButtonClicked.bind(this);
        this.handleSectionNavigation = this.handleSectionNavigation.bind(this);
        // this.handleMenuClicked = this.handleMenuClicked.bind(this);

        //This should be its own container
        this.handleDeckModalChange = this.handleDeckModalChange.bind(this);
        this.handleDeckModalClose = this.handleDeckModalClose.bind(this);
        this.handleDeckModalSave = this.handleDeckModalSave.bind(this);

        this.handleDeckSelected = this.handleDeckSelected.bind(this);
    }

    //Should THIS be the thing that initializes shit? Instead of a reducer?

    componentDidMount() {
        //IDK what exactly would be the right time to call off 
        
        // console.log('AUTOMATICALLY REDIRECTING TO DECK EDITOR, ID 27');
        // this.props.dispatch(requestDeckDetail(27));

        // console.log('AUTOMATICALLY REDIRECTING TO INVENTORY');
        //39 === snowball
        //36 === Dino EDH
        // console.log('AUTOMATICALLY REDIRECTING TO INVENTORY');
        // this.props.dispatch(navigate('inventory'));
    }

    handleAppBarButtonClicked(type: AppBarButtonType): void {
        // console.log('handleAppBarButtonClicked - CLICK');

        switch(type){
            case "menu":
                // console.log('menu click');
                this.props.dispatch(navigate(null));
                break;
            case "add":
                // let filtersToDefault: FilterDescriptor[] = [];
                
                // //is a deck selected?
                // const deck_is_selected = false;

                // //what's the format & color ID?
                // //CID can be from basic lands for now, eventually should come from Deck Stats

                // if(deck_is_selected){
                //     //filtersToDefault.push("Inventory tab","color filter","format filter")
                //     filtersToDefault.push(
                //         { name: "Colors", value: []},
                //         { name: "Format", value: ""},
                //         { name: "SearchMethod", value: "inventory"}
                //     );
                // }
                this.props.dispatch(appBarAddClicked(this.props.filtersToDefault))

                break;

            case "filter":

                break;
        }
        // onAddToggle={this.handleAppBarButtonClicked} 
        // handleMenuClick={this.handleMenuClicked} 
    }

    handleDeckSelected(deckId: number):void {
        this.props.dispatch(requestDeckDetail(deckId));
    }

    handleSectionNavigation(destination: 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null): void {
        this.props.dispatch(navigate(destination));
    }

    // handleMenuClicked(e: any){
    //     console.log('menu click');
    //     this.props.dispatch(navigate(null));
    // }

    // handleDeckModalOpen(): void {
    //     this.props.dispatch(openNewDeckModal());
    // }

    handleDeckModalClose(): void {
        // this.props.dispatch(cancleNewDeck());
        this.props.dispatch(navigate(null));
    }

    handleDeckModalChange(event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>): void {
        // console.log('yah');
        // console.log(event);
        this.props.dispatch(newDeckFieldChange(event.target.name, event.target.value));
        //this.props.dispatch(scratchModalItemChanged(event.target.name, event.target.value));
    }

    handleDeckModalSave(): void {
        this.props.dispatch(requestSaveNewDeck(this.props.newDeckDto));
    }

//declare type AppContainerEnum = 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null;
    getVisibleConatiner(containerEnum: AppContainerEnum): JSX.Element {
        // console.log(this.props.isCardSearchShowing);
        if(this.props.isCardSearchShowing)  return (<CardSearch />);
        // console.log(`visibleContainer - ${this.props.visibleContainerEnum}`)
        switch(containerEnum){
            
            //renderMenuNavigation
            //case AppContainerEnum.None: return (<Typography variant="h3">Enum: None</Typography>);
            //case AppContainerEnum.None: return this.renderMenuNavigation();
            case null: return (<AppMenuContent  
                // decks={this.props.availableDecks}
                onDeckClick={this.handleDeckSelected}
                onNavigationClick={this.handleSectionNavigation}
                />);
            case 'inventory': return (<Inventory />);

            //case AppContainerEnum.DeckEditor: return (<Typography variant="h3">Enum: Deck Editor</Typography>);
            case 'deckEditor': return (<DeckEditor />);
            // case 'inventory': return (<Inventory />);
            // case 'deck_editor': return (<DeckEditor />);
            default: return(<Typography variant="h3">Default - something went wrong</Typography>);
        }

    }


    render() {
        return (
            <React.Fragment>
                <Modal open={this.props.isNewDeckModalOpen} onClose={this.handleDeckModalClose} disableBackdropClick={true}>
                    <AppModalLayout
                        onCloseClick={this.handleDeckModalClose}
                        onSaveClick={this.handleDeckModalSave}
                        title="Add New Deck">
                        <DeckPropertiesLayout onChange={(a) => this.handleDeckModalChange(a)} deck={this.props.newDeckDto} />
                    </AppModalLayout>
                </Modal>
                <AppLayout 
                    showAddButton={true}
                    showFilterButton={true}
                    isAddSelected={this.props.isCardSearchShowing}
                    
                    onButtonClick={this.handleAppBarButtonClicked}
                    //onButtonClick: (type: AppBarButtonType) => void;


                    //handleMenuButtonClick(string)


                    //replace with "onButtonClick"?
                    //replace with "onButtonClick"?
                    
                    //also use onButtonClick for the filter button?
                    >
                        {/* <Typography variant="h2">Hello, App!</Typography> */}
                        {  
                            //this.getVisibleConatiner(this.props.visibleContainer)
                            this.getVisibleConatiner(this.props.visibleContainerEnum)
                        }
                </AppLayout>
          </React.Fragment>
        );
    }
}
//State
function mapStateToProps(state: AppState): PropsFromState {
    // console.log('what are the available decks?')
    // console.log(state.core.availableDecks);
    // var something = state.core.visibleContainer;
    

    let parsedFilters: FilterDescriptor[] = [];

    const { deckId, deckProps } = state.data.deckDetail;

    if(deckId > 0 && deckProps){
        let colorIDs: string[] = [];
        if(deckProps.basicW) colorIDs.push("W");
        if(deckProps.basicU) colorIDs.push("U");
        if(deckProps.basicB) colorIDs.push("B");
        if(deckProps.basicR) colorIDs.push("R");
        if(deckProps.basicG) colorIDs.push("G");
        
        parsedFilters.push(
            { name: "Colors", value: colorIDs},
            { name: "Format", value: deckProps.format},
            { name: "SearchMethod", value: "inventory"}
        );
    }

    //modal is open if visibleContainer === newDeck
    const result: PropsFromState = {
        isCardSearchShowing: state.app.core.isCardSearchShowing,
        // visibleContainer: 'inventory',
        visibleContainerEnum: state.app.core.visibleContainer,
        newDeckDto: state.ui.newDeckDto,
        isNewDeckModalOpen: (state.app.core.visibleContainer === 'newDeck'),
        filtersToDefault: parsedFilters,
    }
    return result;
}

export default connect(mapStateToProps)(App);
