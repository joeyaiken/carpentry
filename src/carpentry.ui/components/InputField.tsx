import React from 'react';

export interface InputFieldProps{
    label: string;
    value: string;
    property: string;
    onChange: any;
}

export default function InputField(props: InputFieldProps): JSX.Element {
    return(
        <div className="sd-input-field outline-section flex-col">
            <label>{props.label}</label>
            <input name={props.property} value={props.value} onChange={props.onChange} />
        </div>
    )
}