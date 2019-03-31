import React from 'react'
import Counter from './Counter'
import LandIcon from './LandIcon'

export interface LandCounterProps {
    manaType: string;
    count: number
    onCountChange: (count: number) => void;
}

export default function LandCounter(props: LandCounterProps): JSX.Element {
    return(
        <div className="sd-land-counter">
            <LandIcon manaType={props.manaType} />
            <Counter value={props.count} onValueChanged={props.onCountChange} />
        </div>
    );
}