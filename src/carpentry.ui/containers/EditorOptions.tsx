import { connect } from 'react-redux';
import { EditorOptionsBar } from '../components/EditorOptionsBar';
import { addDeck, logState } from '../actions';

// const mapStateToProps = (state) => 


interface StateProps {
    // deckList: CardDeck[]
}
interface DispatchProps {
    handleAddNewClick: () => void;
    handleLogClick: () => void;
}

const mapStateToProps = (state: any): StateProps => {
    // console.log('mapStateToProps');
    //'actions' are a property of the state?...

    return {
        deckList: state.actions.deckList
    }
}

const matchDispatchToProps = (dispatch: any): DispatchProps => ({
    handleAddNewClick: () => dispatch(addDeck()),
    handleLogClick: () => dispatch(logState())
    // onDeckItemClicked: (id: number) => dispatch(selectDeck(id))
});

export default connect<StateProps, DispatchProps>(
    mapStateToProps,
    matchDispatchToProps
)(EditorOptionsBar);

