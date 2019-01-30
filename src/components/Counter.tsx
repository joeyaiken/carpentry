import React from 'react'

export interface CounterProps{
    value: number,
    onValueChanged: (newValue: number) => void;
}

export default function Counter(props: CounterProps): JSX.Element {
    return (
        <div className="sd-counter">
            <button onClick={() => props.onValueChanged(props.value + 1)}>
                <span className="material-icons">keyboard_arrow_up</span>
            </button>
            <input type="number" value={props.value} onChange={(event) => props.onValueChanged(Number(event.target.value))} />
            <button onClick={() => props.onValueChanged(props.value - 1)}>
                <span className="material-icons">keyboard_arrow_down</span>
            </button>
        </div>
    );
}