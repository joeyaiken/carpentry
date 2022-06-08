import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { RootState } from '../../configureStore';
import NewDeckLayout from './components/NewDeckLayout';
import { newDeckModalClosed, newDeckPropertyChanged, requestSaveNewDeck } from './state/NewDeckActions';
import {useAppDispatch} from "../../hooks";
import {useHistory} from "react-router";

interface PropsFromState {
  deckProps: DeckPropertiesDto;
  formatFilters: FilterOption[];
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class NewDeckContainer extends React.Component<ContainerProps> {
  constructor(props: ContainerProps) {
    super(props);
    // this.handleSaveClick = this.handleSaveClick.bind(this);
    // this.handleCloseClick = this.handleCloseClick.bind(this);
    // this.handleFieldChange = this.handleFieldChange.bind(this);
  }

  //on mount - clear state?

  // handleCloseClick() {
  //   this.props.dispatch(newDeckModalClosed());
  //   this.props.dispatch(push('/'));
  // }
  //
  // handleSaveClick() {
  //   this.props.dispatch(requestSaveNewDeck());
  // }
  //
  // handleFieldChange(name: string, value: string){
  //   this.props.dispatch(newDeckPropertyChanged(name, value));
  // }

  render(){
    return(
      <NewDeck
        deckProps={this.props.deckProps} 
        formatFilters={this.props.formatFilters}  
      />
    )
  }

}

interface NewDeckProps {
  deckProps: DeckPropertiesDto;
  formatFilters: FilterOption[];
}
const NewDeck = (props: NewDeckProps): JSX.Element => {
  
  const dispatch = useAppDispatch();
  const history = useHistory();
  
  const handleCloseClick = (): void => {
    dispatch(newDeckModalClosed());
    history.push('/');
  }

  const handleSaveClick = (): void => {
    dispatch(requestSaveNewDeck());
  }

  const handleFieldChange = (name: string, value: string): void => {
    dispatch(newDeckPropertyChanged(name, value));
  }
  
  return (
    <NewDeckLayout
      onCloseClick={handleCloseClick}
      onSaveClick={handleSaveClick}
      onChange={handleFieldChange}
      formatFilters={props.formatFilters}
      deckProps={props.deckProps}
    />
  )
}

function mapStateToProps(state: RootState): PropsFromState {
  const result: PropsFromState = {
    deckProps: state.decks.newDeck.deckProps,
    formatFilters: state.core.filterOptions.formats,
  }
  return result;
}

export default connect(mapStateToProps)(NewDeckContainer)
