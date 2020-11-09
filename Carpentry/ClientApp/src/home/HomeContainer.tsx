import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import HomeLayout from './HomeLayout';
import { AppState } from '../configureStore';

interface PropsFromState {
}

type HomeProps = PropsFromState & DispatchProp<ReduxAction>;

class HomeContainer extends React.Component<HomeProps> {
    render() {
        return (
            <HomeLayout />
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(HomeContainer);