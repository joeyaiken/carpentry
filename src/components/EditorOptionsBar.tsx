import React from 'react';
import FindCard from '../containers/FindCard';

export interface EditorOptionsBarProps {
    handleAddNewClick: () => void;
    handleLogClick: () => void;
}

export function EditorOptionsBar(props: EditorOptionsBarProps ): JSX.Element {
    // return (
    //     <div id="options">
    //         <button onClick={ this.handleAddNewClick }>Add New</button>
    //         <button onClick={ this.handleLogClick }>Log</button>
    //     </div>
    // );

    return (
        <div id="options" className="padded-section">
            <button onClick={ props.handleAddNewClick }>Add New</button>
            <button onClick={ props.handleLogClick }>Log</button>
            <FindCard />
        </div>
    );
}