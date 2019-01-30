import React from 'react';

export interface MagicCardProps{
    card: IMagicCard;
    cardIsSelected: boolean;
    display: string | null; // card | list
    onClick: any;
}

export default function MagicCard(props: MagicCardProps): JSX.Element{

    const noDataCard: JSX.Element = 
        <div className="outline-section">
            <label>No card data</label>
        </div>;

    // console.log(props.card)

    if(!props.card.data){
        return noDataCard;
    }

    const imgCard: JSX.Element = 
        <div className={props.cardIsSelected ? "magic-card selected-card" : "magic-card"} onClick={props.onClick}>
            <img src={props.card.data.imageUrl} />
        </div>;
    const detailCard: JSX.Element = 
        <div>
            haHA
        </div>;

    // const result: JSX.Element = (props.card.data) ? (
    //     <div className={props.cardIsSelected ? "magic-card selected-card" : "magic-card"} onClick={props.onClick}>
    //         <img src={props.card.data.imageUrl} />
    //     </div>
    // ) : (
    //     <div className="outline-section">
    //         <label>No card data</label>
    //     </div>
    // );
    let result: JSX.Element;

    switch (props.display) {
        case 'card':
            result = imgCard;
            break;
        case 'list':
            result = detailCard;
            break;
        case null:
            result = noDataCard;
            break;
        default:
            result = noDataCard;
            break;

    }
        




    
    return result;
    
}