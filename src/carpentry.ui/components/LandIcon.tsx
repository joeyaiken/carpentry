import React from 'react'
import manaR from '../img/R.svg'
import manaU from '../img/U.svg'
import manaG from '../img/G.svg'
import manaW from '../img/W.svg'
import manaB from '../img/B.svg'

export interface LandIconProps {
    manaType: string;
    isTransparent?: boolean;
    onClick?: any;
}

export default function LandIcon(props: LandIconProps): JSX.Element {
    let iconStr: string = ''; //JSX.Element
    switch(props.manaType){
        case 'R':
            iconStr = manaR;
            break;
            case 'U':
            iconStr = manaU;
            break;
            case 'G':
            iconStr = manaG;
            break;
            case 'W':
            iconStr = manaW;
            break;
            case 'B':
            iconStr = manaB;
            break;
    }

    return(<img src={iconStr} className={ "mana-icon" + (props.isTransparent ? ' transparent' : '') } />);
}