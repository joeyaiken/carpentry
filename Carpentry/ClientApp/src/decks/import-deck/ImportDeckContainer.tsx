import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { AppState } from '../../configureStore';
import ImportDeckLayout from './components/ImportDeckLayout';
import { push } from 'react-router-redux';
import { requestValidateImport } from './state/ImportDeckActions';
// import { newDeckModalClosed, newDeckPropertyChanged, requestSaveNewDeck } from './state/NewDeckActions';

interface PropsFromState {
    // deckProps: DeckPropertiesDto;
    // formatFilters: FilterOption[];
    importString: string;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class NewDeckContainer extends React.Component<ContainerProps> {
    constructor(props: ContainerProps) {
        super(props);
        this.handleCloseClick = this.handleCloseClick.bind(this);
        this.handleBackClick = this.handleBackClick.bind(this);
        this.handleValidateClick = this.handleValidateClick.bind(this);
        this.handleSaveClick = this.handleSaveClick.bind(this);
        // this.handleFieldChange = this.handleFieldChange.bind(this);
    }

    //on mount - clear state?

    handleCloseClick() {
        // this.props.dispatch(newDeckModalClosed());
        this.props.dispatch(push('/'));
    }

    handleBackClick() {

    }

    handleValidateClick() {
        const dto: CardImportDto = {
            importType: 1,
            importPayload: this.props.importString
        };
        this.props.dispatch(requestValidateImport(dto));
    }

    handleSaveClick() {
        // this.props.dispatch(requestSaveNewDeck());
    }

    // handleFieldChange(name: string, value: string){
    //     this.props.dispatch(newDeckPropertyChanged(name, value));
    // }

    render() {
        return (
            <ImportDeckLayout 
                onCloseClick={this.handleCloseClick}
                onBackClick={this.handleBackClick}
                onValidateClick={this.handleValidateClick}
                onSaveClick={this.handleSaveClick}

                importString={this.props.importString}
                
                // onChange={this.handleFieldChange}
                // formatFilters={this.props.formatFilters}
                // deckProps={this.props.deckProps}
                isValidated={false}
                isValid={false}
            />
        )
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    
    const result: PropsFromState = {
        // deckProps: state.decks.newDeck.deckProps,
        // formatFilters: state.core.data.filterOptions.formats,
        importString: state.decks.importDeck.importString,    
    }
    
    return result;
}

export default connect(mapStateToProps)(NewDeckContainer)
