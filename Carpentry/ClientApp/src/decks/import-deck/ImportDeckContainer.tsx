import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../../configureStore';
import ImportDeckLayout from './components/ImportDeckLayout';
import { push } from 'react-router-redux';
// import { newDeckModalClosed, newDeckPropertyChanged, requestSaveNewDeck } from './state/NewDeckActions';

interface PropsFromState {
    deckProps: DeckPropertiesDto;
    formatFilters: FilterOption[];
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class NewDeckContainer extends React.Component<ContainerProps> {
    constructor(props: ContainerProps) {
        super(props);
        // this.handleSaveClick = this.handleSaveClick.bind(this);
        this.handleCloseClick = this.handleCloseClick.bind(this);
        // this.handleFieldChange = this.handleFieldChange.bind(this);
    }

    //on mount - clear state?

    handleCloseClick() {
        // this.props.dispatch(newDeckModalClosed());
        this.props.dispatch(push('/'));
    }

    // handleSaveClick() {
    //     this.props.dispatch(requestSaveNewDeck());
    // }

    // handleFieldChange(name: string, value: string){
    //     this.props.dispatch(newDeckPropertyChanged(name, value));
    // }

    render() {
        return (
            <ImportDeckLayout 
                onCloseClick={this.handleCloseClick}
                // onSaveClick={this.handleSaveClick}
                // onChange={this.handleFieldChange}
                // formatFilters={this.props.formatFilters}
                // deckProps={this.props.deckProps}
            />
        )
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        deckProps: state.decks.newDeck.deckProps,
        formatFilters: state.core.data.filterOptions.formats,
    }
    return result;
}

export default connect(mapStateToProps)(NewDeckContainer)
