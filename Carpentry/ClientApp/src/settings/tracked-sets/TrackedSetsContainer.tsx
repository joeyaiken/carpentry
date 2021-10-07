import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import TrackedSetsContainerLayout from './components/TrackedSetsContainerLayout';
import { AppState } from '../../configureStore';
import { requestAddTrackedSet, requestRemoveTrackedSet, requestTrackedSets, requestUpdateTrackedSet } from './state/TrackedSetsActions';

interface PropsFromState {
  trackedSetDetails: SetDetailDto[];

  showUntracked: boolean;
  isLoading: boolean;
}

type TrackedSetsContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class TrackedSetsContainer extends React.Component<TrackedSetsContainerProps> {
  constructor(props: TrackedSetsContainerProps) {
    super(props);
    this.handleRefreshClick = this.handleRefreshClick.bind(this);
    this.handleShowUntrackedClick = this.handleShowUntrackedClick.bind(this);
    this.handleAddTrackedSetClick = this.handleAddTrackedSetClick.bind(this);
    this.handleUpdateTrackedSetClick = this.handleUpdateTrackedSetClick.bind(this);
    this.handleRemoveTrackedSetClick = this.handleRemoveTrackedSetClick.bind(this);
  }

  componentDidMount() {
    //showUntracked, update
    this.props.dispatch(requestTrackedSets(this.props.showUntracked, false));
  }

  handleRefreshClick(): void {
    this.props.dispatch(requestTrackedSets(this.props.showUntracked, true));
  }

  handleShowUntrackedClick(): void {
    this.props.dispatch(requestTrackedSets(!this.props.showUntracked, false));
  }

  handleAddTrackedSetClick(setId: number): void {
    this.props.dispatch(requestAddTrackedSet(setId))
  }

  handleUpdateTrackedSetClick(setId: number): void {
    this.props.dispatch(requestUpdateTrackedSet(setId))
  }

  handleRemoveTrackedSetClick(setId: number): void {
    this.props.dispatch(requestRemoveTrackedSet(setId))
  }

  render() {
    return (
      <TrackedSetsContainerLayout
        onRefreshClick={this.handleRefreshClick}
        onShowUntrackedClick={this.handleShowUntrackedClick}
        trackedSetDetails={this.props.trackedSetDetails}
        showUntrackedValue={this.props.showUntracked}
        onAddSetClick={this.handleAddTrackedSetClick}
        onRemoveSetClick={this.handleRemoveTrackedSetClick}
        onUpdateSetClick={this.handleUpdateTrackedSetClick}
        isLoading={this.props.isLoading}
      />
    );
  }
}

function selectTrackedSets(state: AppState): SetDetailDto[] {
  const { setsById, setIds } = state.settings.trackedSets;
  const result: SetDetailDto[] = setIds.map((setId) => setsById[setId]);
  return result;
}

function mapStateToProps(state: AppState, ownProps): PropsFromState {
  const result: PropsFromState = {
    trackedSetDetails: selectTrackedSets(state),
    showUntracked: state.settings.trackedSets.showUntracked,
    isLoading: state.settings.trackedSets.isLoading,
    // isLoading: true,
  }
  return result;
}

export default connect(mapStateToProps)(TrackedSetsContainer);