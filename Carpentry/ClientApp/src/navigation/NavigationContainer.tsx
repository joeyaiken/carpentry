import React from 'react'
import { connect, DispatchProp} from "react-redux";
import { push } from 'react-router-redux';
import {AppState} from "../configureStore";
import NavigationLayout from "./NavigationLayout";

interface PropsFromState {
  
}

type NavigationProps = PropsFromState & DispatchProp<ReduxAction>;

class NavigationContainer extends React.Component<NavigationProps> {
  constructor(props: NavigationProps) {
    super(props);
    this.handleNavClick = this.handleNavClick.bind(this);
  }

  handleNavClick(route: string){
    this.props.dispatch(push(route));
  }
  
  render() {
    return (<NavigationLayout onNavClick={this.handleNavClick} />);
  }
}

function mapStateToProps(state: AppState): PropsFromState {
  const result: PropsFromState = {
    
  }
  return result;
}

export default connect(mapStateToProps)(NavigationContainer);