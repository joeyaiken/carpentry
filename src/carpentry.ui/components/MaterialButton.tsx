import React from 'react';

export interface MaterialButtonProps {
    //What properties would this button class need?
        //onClick
        //material icon
        //isSelected
        //value
        value: string;
        icon: string;
        isSelected?: boolean;
        onClick: (value: string) => void;
}

export default function(props: MaterialButtonProps): JSX.Element {

    let classStr = "material-icons";
    if(props.isSelected){
        classStr += " selected-icon";
    }

    return(
        <button className="sd-material-button" onClick={() => props.onClick(props.value)}>
            <span className={classStr}>{props.icon}</span>
        </button>
    )
}