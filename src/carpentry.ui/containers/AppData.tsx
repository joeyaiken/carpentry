import { connect, DispatchProp } from 'react-redux'
import React from 'react';
import {

} from '../actions'

interface PropsFromState {
    dataObject: string;
    cardIndex: string;
    cachedSetData: CachedSetData[];
}


interface CachedSetData {
    name: string;
    cardDictionary: ICardDictionary;
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
                {
                    this.props.cachedSetData.map((set) => {
                        return(<div className="outline-section flex-col">
                        <div className="outline-section">{set.name}</div>
                        <div className="outline-section"><textarea value={ JSON.stringify(set.cardDictionary) } /></div>
                        
                    </div>)
                    })
                }
            </div>
        );
    }
}

function mapStateToProps(state: State): PropsFromState {
    const result: PropsFromState = {
        dataObject: "",//JSON.stringify(state.data.deckList),
        cardIndex: "",//JSON.stringify(state.data.cardIndex)
        cachedSetData: Object.keys(state.addPack.apiCache).map((setKey: string) => {
            return {
                name: setKey,
                cardDictionary: state.addPack.apiCache[setKey]
            } as CachedSetData;
        })
    }
    return result;
}

export default connect(mapStateToProps)(AppData);



