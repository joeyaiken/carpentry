import {connect, DispatchProp} from 'react-redux';
import React from 'react';
import {RootState} from '../../configureStore';
import {ensureCardDetailLoaded} from './state/CardDetailActions';
import {CardDetail} from "./CardDetail";

interface PropsFromState {
  selectedCardId: number;
}

interface OwnProps {
  selectedCardId: number;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardDetailContainer extends React.Component<ContainerProps>{
  constructor(props: ContainerProps) {
    super(props);
  }
  componentDidMount() {
    this.props.dispatch(ensureCardDetailLoaded(this.props.selectedCardId));
  }
  render(){
    return (<CardDetail selectedCardId={this.props.selectedCardId} />);
  }
}

function mapStateToProps(state: RootState, ownProps: OwnProps): PropsFromState {
  return {
    selectedCardId: ownProps.selectedCardId,
  };
}

export default connect(mapStateToProps)(CardDetailContainer);