import React from 'react';

//Card List displays a collection of cards (in either list or card format)
//Doesn't need a title section
//Needs a handler for cards clicked
export interface CardListProps {
    cards: IMagicCard[]
    display: string | null; // card | list
}

export default function CardList(props: CardListProps): JSX.Element {

    return(
        <div className="sd-card-list">

            {/* <h3>Cards:<FindCard  /></h3>
             */}
            <div>
                {
                    props.cards.map((card) => {
                        return(<div>
                            card!
                        </div>);
                    })
                }
            </div>

        </div>
    )
}