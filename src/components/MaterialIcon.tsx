import React from 'react';

export interface MaterialIconProps {
        icon: string;
}

export default function(props: MaterialIconProps): JSX.Element {    
    return(
        <span className="material-icons">{props.icon}</span>
    )
}