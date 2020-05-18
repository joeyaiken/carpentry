//TODO Review and verify if actually used
import React from 'react'
import { KeyboardArrowDown, KeyboardArrowUp} from '@material-ui/icons'
import { IconButton } from '@material-ui/core';

export interface CounterProps{
    value: number,
    onValueChanged: (newValue: number) => void;
}

export default function Counter(props: CounterProps): JSX.Element {
    return (
        <div className="sd-counter">
            <IconButton color="inherit" onClick={() => props.onValueChanged(props.value + 1)}>
              <KeyboardArrowUp />
            </IconButton>
            <input type="number" value={props.value} onChange={(event) => props.onValueChanged(Number(event.target.value))} />
            <IconButton color="inherit" onClick={() => props.onValueChanged(props.value - 1)}>
              <KeyboardArrowDown />
            </IconButton>
        </div>
    );
}