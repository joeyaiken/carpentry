import React from 'react'
import { Avatar } from '@material-ui/core';
export interface ManaIconProps {
    manaType: string;
}
export default function ManaIcon(props: ManaIconProps): JSX.Element {
    //<Chip size="small" avatar={<Avatar src="/img/W.svg" />} label={ props.deckProperties.basicW }/>
    const size = 20;
    let color = "#00160b"


    let background = "#e0e0e0"
    switch(props.manaType){
        case "W":
            background = "#fffbd5"
            break;
        case "U":
            background = "#aae0fa"
            break;
        case "B":
            background = "#cbc2bf"
            break;
        case "R":
            background = "#f9aa8f"
            break;
        case "G":
            background = "#9bd3ae"
            break;
    }


    return(
            <Avatar
                style={{
                        height: size,
                        width: size,
                        color: color,
                        background: background,
                    }}
            >
            <i style={{fontSize:14
            }}
            
            // style={{
            //     //color: "red"
            //     background:"red"
            // }}
            
            className={`ms ms-${props.manaType.toLocaleLowerCase()}`}></i>
        
        </Avatar>
        );
}