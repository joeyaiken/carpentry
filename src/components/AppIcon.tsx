import React from 'react';
import MaterialIcon from './MaterialIcon'

interface AppIconProps {

}

export default function Appicon(props: AppIconProps): JSX.Element{
    const description: string = "a MTG deck management tooklit";
    return (
        <> 
            <MaterialIcon icon="build" />
            {/* <h1>carpentry</h1><h2>{description}</h2> */}
            <span className="app-icon-title">carpentry</span><span className="app-icon-description">{description}</span>
        </>
    );
}