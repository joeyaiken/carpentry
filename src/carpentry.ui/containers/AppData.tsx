import { connect, DispatchProp } from 'react-redux'
import React from 'react';
import {

} from '../actions'

interface PropsFromState {
    dataObject: string;
    cardIndex: string;
}
//This component is meant to show what local data changes exist
type AppDataProps = PropsFromState & DispatchProp<ReduxAction>;

class AppData extends React.Component<AppDataProps> {
    constructor(props: AppDataProps) {
        super(props);
    }
    render(){
        return(
            <div className="sd-card-quick-add card">
                <div className="card-header">
                    <label>App Data</label>
                </div>
                <div className="outline-section">
                    {/* <textarea  /> */}
                    <textarea value={ this.props.dataObject } />
                </div>
                <div className="outline-section">
                    {/* <textarea  /> */}
                    <textarea value={ this.props.cardIndex } />
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: State): PropsFromState {
    const result: PropsFromState = {
        dataObject: JSON.stringify(state.data.deckList),
        cardIndex: JSON.stringify(state.data.cardIndex)
    }
    return result;
}

export default connect(mapStateToProps)(AppData);



