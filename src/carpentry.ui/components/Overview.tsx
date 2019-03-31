import React from 'react';

export interface OverviewProps {
        icon: string;
}

export default function(props: OverviewProps): JSX.Element {    
    return(
        <span className="material-icons">{props.icon}</span>
    )
}