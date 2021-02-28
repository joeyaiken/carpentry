import { Component, Input, OnInit } from "@angular/core";

@Component({
    selector: 'app-mana-chip',
    templateUrl: 'mana-chip.component.html',
    styleUrls: ['mana-chip.component.less']
})
export class ManaChipComponent implements OnInit {

    //mana parts
    @Input() manaParts: string[];

    constructor() { }
    ngOnInit(): void { }
}




// <div fxLayout="row" className="flex-row">{
    
//     splitParts.map((str, index) => {
//         var pipStr = str.split("/").join(''); //want to remove the '/' from hybrid mana
//         var pipClass = `ms ms-${pipStr} ms-cost`;
//         return(<i key={index} className={pipClass}></i>);
//     })
//     // costParts.map((part, index) => <ManaIcon key={`chip${index}`} manaType={part} />)
// }
// </div>


// import React from 'react'
// import { Box } from '@material-ui/core'
// // import ManaIcon from './ManaIcon';
// // declare type ManaIconType = "W" | "U" | "B" | "R" | "G" | number;
// export interface ManaCostChipProps {
//     costString: string;
// }

// //This component represents the "Mana Cost" of a card
// //An example imput string is "{2}{R}{R}"
// //Edge cases: Hybrid mana: "{R/G}{R/G}"
// export default function ManaCostChip(props: ManaCostChipProps): JSX.Element {
//     // const costParts = props.costString.toLowerCase().split("").filter(part => part !== "{" && part !== "}" );

//     const trimmedString = props.costString.toLowerCase().substring(1, props.costString.length - 1);
//     const splitParts = trimmedString.split("}{");

//     //const stringParts = props.costString.split("")
//     return (<Box className={"flex-row"}>{
//         //splitParts

//         splitParts.map((str, index) => {
//             var pipStr = str.split("/").join(''); //want to remove the '/' from hybrid mana
//             var pipClass = `ms ms-${pipStr} ms-cost`;
//             return(<i key={index} className={pipClass}></i>);
//         })
//         // costParts.map((part, index) => <ManaIcon key={`chip${index}`} manaType={part} />)
//     }</Box>);
// }