import React from 'react';

export interface MagicCardProps{
    card: ICard;
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

    if(!props.card){
        return noDataCard;
    }

    const imgCard: JSX.Element = 
        <div className={props.cardIsSelected ? "magic-card selected-card" : "magic-card"} onClick={props.onClick}>
            <img src={props.card.imageUrl} />

            <div>
                <span>[ { props.card.set } ]</span>
                <span>[ { props.card.setName } ]</span>
                {/* <span>[ { props.card.colors } ]</span> */}
                <span>[ { props.card.colorIdentity } ]</span>
            </div>
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