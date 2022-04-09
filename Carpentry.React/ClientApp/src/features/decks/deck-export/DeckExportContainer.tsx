import { connect, DispatchProp } from 'react-redux'
import React from 'react';
import { AppState } from '../../configureStore';
import { DeckExportLayout } from './DeckExportLayout';
import { closeExportDialog, exportTypeChanged, requestDeckExport } from './state/DeckExportActions';

interface PropsFromState {
    deckId: number;
    deckExportString: string;
    isExportDialogOpen: boolean;
    selectedExportType: DeckExportType;
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckExport extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props);
        // this.handleExportOpenClick = this.handleExportOpenClick.bind(this);
        this.handleExportTypechange = this.handleExportTypechange.bind(this);
        this.handleExportDialogButtonClick = this.handleExportDialogButtonClick.bind(this);
        this.handleExportCloseClick = this.handleExportCloseClick.bind(this);
    }

    handleExportTypechange(exportType: string): void {
        this.props.dispatch(exportTypeChanged(exportType));
    }

    handleExportDialogButtonClick(): void {
        this.props.dispatch(requestDeckExport(this.props.deckId, this.props.selectedExportType));
    }

    handleExportCloseClick(): void {
        this.props.dispatch(closeExportDialog());
    }

    render() {
        return(
            <DeckExportLayout
                isDialogOpen={this.props.isExportDialogOpen}
                selectedExportType={this.props.selectedExportType} 
                deckExportString={this.props.deckExportString}
                onExportTypeChange={this.handleExportTypechange}
                onExportButtonClick={this.handleExportDialogButtonClick}
                onExportCloseClick={this.handleExportCloseClick} />
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const containerProps = state.decks.deckExport;
    const result: PropsFromState = {
        deckId: state.decks.data.detail.deckId,
        deckExportString: containerProps.deckExportPayload,
        isExportDialogOpen: containerProps.isDialogOpen,
        selectedExportType: containerProps.selectedExportType,
    }
    return result;
}

export default connect(mapStateToProps)(DeckExport);