import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { RootState } from '../../configureStore';
import ImportDeckLayout from './components/ImportDeckLayout';
// import { push } from 'react-router-redux';
import { importDeckPropertyChanged, requestValidateImport } from './state/ImportDeckActions';
import {useAppDispatch} from "../../hooks";
import {useHistory} from "react-router";
// import { newDeckModalClosed, newDeckPropertyChanged, requestSaveNewDeck } from './state/NewDeckActions';

interface PropsFromState {
  // deckProps: DeckPropertiesDto;
  importProps: DeckImportUiProps;
  formatFilters: FilterOption[];
  // importString: string;
  validatedImport: ValidatedDeckImportDto;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class NewDeckContainer extends React.Component<ContainerProps> {
  constructor(props: ContainerProps) {
    super(props);
    // this.handleCloseClick = this.handleCloseClick.bind(this);
    // this.handleBackClick = this.handleBackClick.bind(this);
    // this.handleValidateClick = this.handleValidateClick.bind(this);
    // this.handleSaveClick = this.handleSaveClick.bind(this);
    // this.handleFieldChange = this.handleFieldChange.bind(this);
  }

  //on mount - clear state?

  // handleCloseClick() {
  //   // this.props.dispatch(newDeckModalClosed());
  //   this.props.dispatch(push('/'));
  // }

  // handleBackClick() {
  //
  // }

  // handleValidateClick() {
  //   const dto: CardImportDto = {
  //     importType: 1,
  //     importPayload: this.props.importProps.importString,
  //   };
  //   this.props.dispatch(requestValidateImport(dto));
  // }

  // handleSaveClick() {
  //   // this.props.dispatch(requestSaveNewDeck());
  // }

  // handleFieldChange(name: string, value: string){
  //   this.props.dispatch(importDeckPropertyChanged(name, value));
  // }

  render() {
    return(<ImportDeck 
      importProps={this.props.importProps}
      formatFilters={this.props.formatFilters}
      validatedImport={this.props.validatedImport}
    />)  
  }
  
  // render() {
  //   return (
  //     <ImportDeckLayout
  //       onCloseClick={this.handleCloseClick}
  //       onBackClick={this.handleBackClick}
  //       onValidateClick={this.handleValidateClick}
  //       onSaveClick={this.handleSaveClick}
  //
  //       // importString={this.props.importString}
  //
  //       onChange={this.handleFieldChange}
  //       formatFilters={this.props.formatFilters}
  //       importProps={this.props.importProps}
  //       isValidated={true}
  //       // isValid={false}
  //       validatedImport={this.props.validatedImport}
  //     />
  //   )
  // }
}
interface ImportDeckProps {
  // deckProps: DeckPropertiesDto;
  importProps: DeckImportUiProps;
  formatFilters: FilterOption[];
  // importString: string;
  validatedImport: ValidatedDeckImportDto;
}
export const ImportDeck = (props: ImportDeckProps): JSX.Element => {
  const dispatch = useAppDispatch();
  const history = useHistory();

  const handleCloseClick = (): void => {
    // this.props.dispatch(newDeckModalClosed());
    history.push('/');
  }

  const handleBackClick = (): void => {

  }

  const handleValidateClick = (): void => {
    const dto: CardImportDto = {
      importType: 1,
      importPayload: props.importProps.importString,
    };
    dispatch(requestValidateImport(dto));
  }

  const handleSaveClick = (): void => {
    // this.props.dispatch(requestSaveNewDeck());
  }

  const handleFieldChange = (name: string, value: string): void => {
    dispatch(importDeckPropertyChanged(name, value));
  }

  return (
    <ImportDeckLayout
      onCloseClick={handleCloseClick}
      onBackClick={handleBackClick}
      onValidateClick={handleValidateClick}
      onSaveClick={handleSaveClick}

      // importString={this.props.importString}

      onChange={handleFieldChange}
      formatFilters={props.formatFilters}
      importProps={props.importProps}
      isValidated={true}
      // isValid={false}
      validatedImport={props.validatedImport}
    />
  )
}

function mapStateToProps(state: RootState): PropsFromState {

  const result: PropsFromState = {
    importProps: state.decks.importDeck.ui,
    formatFilters: state.core.filterOptions.formats,
    validatedImport: state.decks.importDeck.validatedImport,
    // importString: state.decks.importDeck.importString,    
  }

  return result;
}

export default connect(mapStateToProps)(NewDeckContainer)
