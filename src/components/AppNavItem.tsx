import React from 'react';
import LandIcon from './LandIcon'
import MaterialIcon from './MaterialIcon'

export interface AppNavItemProps {
    // label: string; label={item.details.name} 
    isSelected: boolean;
    deckData: ICardDeck;
    // icon?: string;
    onClick: () => void;
}

export default function AppNavItem(props: AppNavItemProps): JSX.Element {

    let itemBody: JSX.Element = (
        <div className="deck-mana-types">
            {props.deckData.details.basicLands.R > 0 && <LandIcon manaType={'R'} />}
            {props.deckData.details.basicLands.U > 0 && <LandIcon manaType={'U'} />}
            {props.deckData.details.basicLands.G > 0 && <LandIcon manaType={'G'} />}
            {props.deckData.details.basicLands.W > 0 && <LandIcon manaType={'W'} />}
            {props.deckData.details.basicLands.B > 0 && <LandIcon manaType={'B'} />}
        </div>
    );
            
    // if(props.deckData){
        // itemBody = 
        //     <div className="deck-mana-types">
        //         {props.deckData.details.basicLands.R > 0 && <LandIcon manaType={'R'} />}
        //         {props.deckData.details.basicLands.U > 0 && <LandIcon manaType={'U'} />}
        //         {props.deckData.details.basicLands.G > 0 && <LandIcon manaType={'G'} />}
        //         {props.deckData.details.basicLands.W > 0 && <LandIcon manaType={'W'} />}
        //         {props.deckData.details.basicLands.B > 0 && <LandIcon manaType={'B'} />}
        //     </div>
    // } else if (props.icon) {
    //     itemBody = <span className="material-icons">{props.icon}</span>
    // } else {
    //     itemBody = <div>No Data</div>
    // }

    //className={ props.isSelected ? "selected-card compact-card" : "compact-card"}
    return(
        <div className="cc-app-nav-item">
            <div className="outline-section" onClick={ props.onClick }>
                { (props.deckData.details.isUpToDate) ? (<MaterialIcon icon="done"/>) : (<MaterialIcon icon="clear"/>) }
                {/* <MaterialIcon icon="clear"/>
                <MaterialIcon icon="done"/> */}
                <label>{ props.deckData.details.name }</label>
                { itemBody }
            </div>
            
        </div>
    );
}



// export interface DeckListItemProps{
//     // deckData: CardDeck;
//     isSelected: boolean;
//     onClick: () => void;

//     //Eventually this should contain a binding for 'display-mode' or something
// }

// export function DeckListItem(props: DeckListItemProps): JSX.Element {
    
    
    
    
//     return(
//         <div className="sd-deck-card">
//             <div className={ props.isSelected ? "selected-card compact-card" : "compact-card"} onClick={ props.onClick }>
//                 {/* <label>{ props.deckData.description}</label> */}
//                 <div className="deck-mana-types">
//                     {props.deckData.basicLands.R > 0 && <LandIcon manaType={'R'} />}
//                     {props.deckData.basicLands.U > 0 && <LandIcon manaType={'U'} />}
//                     {props.deckData.basicLands.G > 0 && <LandIcon manaType={'G'} />}
//                     {props.deckData.basicLands.W > 0 && <LandIcon manaType={'W'} />}
//                     {props.deckData.basicLands.B > 0 && <LandIcon manaType={'B'} />}
//                 </div>
//             </div>
//             <label>{ props.deckData.name}</label>
//         </div>
        
//     );
// }
