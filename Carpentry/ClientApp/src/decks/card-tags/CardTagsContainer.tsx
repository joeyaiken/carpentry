import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { RootState } from '../../configureStore';
import CardTagsLayout from './components/CardTagsLayout';
import { ensureTagDetailLoaded, newTagChange, requestAddCardTag, requestRemoveCardTag } from './state/CardTagsActions';

interface PropsFromState { 
    selectedDeckId: number;
    selectedCardId: number;
    selectedCardName: string;
    newTagName: string;
    existingTags: CardTagDetailTag[];
    tagSuggestions: string[];
}

interface OwnProps {
    selectedCardId: number;
}

type ContainerProps = PropsFromState & DispatchProp<ReduxAction>;

class CardDetailContainer extends React.Component<ContainerProps>{
    constructor(props: ContainerProps) {
        super(props);
        this.handleAddTagButtonClick = this.handleAddTagButtonClick.bind(this);
        this.handleNewTagChange = this.handleNewTagChange.bind(this);
        this.handleAddSuggestionClick = this.handleAddSuggestionClick.bind(this);
        this.handleRemoveTagClick = this.handleRemoveTagClick.bind(this);
    }

    componentDidMount() {
        this.props.dispatch(ensureTagDetailLoaded(this.props.selectedCardId))
    }
  
    handleAddTagButtonClick() {
        const dto: CardTagDto = {
            cardName: this.props.selectedCardName,
            deckId: this.props.selectedDeckId,
            tag: this.props.newTagName,
        }
        this.props.dispatch(requestAddCardTag(dto));
    }

    handleNewTagChange(newValue: string) {
        this.props.dispatch(newTagChange(newValue));
    }

    handleAddSuggestionClick(tag: string) {
        const dto: CardTagDto = {
            cardName: this.props.selectedCardName,
            deckId: this.props.selectedDeckId,
            tag: tag,
        };
        this.props.dispatch(requestAddCardTag(dto));
    }

    handleRemoveTagClick(cardTagId: number) {
        this.props.dispatch(requestRemoveCardTag(cardTagId));
    }

    render(){
        return(
            <CardTagsLayout
                newTagName={this.props.newTagName}
                selectedCardName={this.props.selectedCardName}
                existingTags={this.props.existingTags}
                tagSuggestions={this.props.tagSuggestions}
                onNewTagChange={this.handleNewTagChange} 
                onAddTagButtonClick={this.handleAddTagButtonClick}
                onAddSuggestionClick={this.handleAddSuggestionClick}
                onRemoveTagClick={this.handleRemoveTagClick} />
        );
    }
}

function mapStateToProps(state: RootState, ownProps: OwnProps): PropsFromState {
    const containerState = state.decks.cardTags;
    const result: PropsFromState = {
        selectedDeckId: state.decks.data.detail.deckId,
        selectedCardId: ownProps.selectedCardId,
        selectedCardName: containerState.cardName,
        newTagName: containerState.newTagName,
        existingTags: containerState.existingTags,
        tagSuggestions: containerState.tagSuggestions,
    }
    return result;
}

export default connect(mapStateToProps)(CardDetailContainer);