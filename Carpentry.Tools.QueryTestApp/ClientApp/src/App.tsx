import React from 'react';
import { connect, DispatchProp } from 'react-redux';
import './styles/App.css';
import { AppState } from './reducers'
import InventoryCardGrid from './components/CardGrid';
import LoadingBox from './components/LoadingBox';
import AppLayout from './components/AppLayout';
import { requestInventoryOverviews } from './actions'

interface PropsFromState { 
    isLoading: boolean;
    searchResults: InventoryOverviewDto[];
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class AppResults extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
    }

    componentDidMount() {
        this.props.dispatch(requestInventoryOverviews());
    }

    handleSearchButtonClick() {
        this.props.dispatch(requestInventoryOverviews());
    }

    render() {
        return (
            <AppLayout onButtonClick={this.handleSearchButtonClick}>
                { 
                    (this.props.isLoading) ? <LoadingBox /> : 
                    <InventoryCardGrid cardOverviews={this.props.searchResults} /> 
                }
            </AppLayout>
        );
    }
}

function selectInventoryOverviews(state: AppState): InventoryOverviewDto[] {
    const { byId, allIds } = state.data;
    const result: InventoryOverviewDto[] = allIds.map(id => byId[id]);
    return result;
}

//State
function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        searchResults: selectInventoryOverviews(state),
        isLoading: state.data.isLoading,
    }
    return result;
}

export default connect(mapStateToProps)(AppResults);