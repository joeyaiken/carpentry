import React from 'react';
import InputField from '../components/InputField'
import CardList from '../components/CardList'

export interface DeckDetailProps{
    data: CardDeck
    handleChange: any;
    handleSaveClick: any;
    isExpanded: boolean;
}

export default class DeckDetail extends React.Component<DeckDetailProps> {
    constructor(props:DeckDetailProps){
        super(props);
    }
    render(){
        const deck = this.props.data;


        


        const result = !deck ? <div>nothing</div> : <>
            <div className="sd-deck-detail card">
                <div className="card-header">
                    <label>Deck Summary</label>
                </div>                
                <div className="flex-row outline-section card-body">
                    <InputField label="Active Deck" value={deck.name} onChange={this.props.handleChange} property="name" />
                    <InputField label="Desciption" value={deck.description} onChange={this.props.handleChange} property="description" />
                    <InputField label="Deck Type" value={deck.type} onChange={this.props.handleChange} property="type" />
                    <InputField label="Colors" value={deck.colors} onChange={this.props.handleChange} property="colors" />
                    <div className="outline-section">
                        <label>Save</label>
                        <div className="outliune-section">
                            <button onClick={this.props.handleSaveClick}>save</button>
                        </div>
                    </div>
                </div>
            </div>
        </>
        

        return(result);
    }
}