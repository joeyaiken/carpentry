import React from 'react'
import { Box } from '@material-ui/core'
import ManaIcon from './ManaIcon';
// declare type ManaIconType = "W" | "U" | "B" | "R" | "G" | number;
export interface ManaCostChipProps {
    costString: string;
}
export default function ManaCostChip(props: ManaCostChipProps): JSX.Element {
    const costParts = props.costString.split("").filter(part => part !== "{" && part !== "}" );
    return (<Box className={"flex-row"}>{
        costParts.map((part, index) => <ManaIcon key={`chip${index}`} manaType={part} />)
    }</Box>);
}