//table of deck cards
import React, { ReactNode } from 'react';

import { CardHeader, CardMedia, Card } from '@material-ui/core';
import { appStyles } from '../styles/appStyles';

interface ComponentProps{
    cardOverview: DeckCardOverview;
    //onCardSelected: ((card: InventoryOverviewDto) => void) | undefined;
    onCardSelected?: () => void;
    children?: ReactNode;
}

export default function VisualCard(props: ComponentProps): JSX.Element {
    const { outlineSection, } = appStyles();
    return (
        <React.Fragment>
            <Card 
                key={props.cardOverview.name} 
                className={outlineSection}
                onClick={props.onCardSelected}
                >
                <CardHeader titleTypographyProps={{variant:"body1"}} title={ `${props.cardOverview.name} - (${props.cardOverview.count})` } />
                {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={`${card.name} (${card.count})`}/> */}
                <CardMedia 
                    style={{height:"310px", width: "223px"}}
                    // className={itemImage}
                    image={props.cardOverview.img}
                    title={props.cardOverview.name} />
                {props.children}

            </Card>

        </React.Fragment>
    );
}