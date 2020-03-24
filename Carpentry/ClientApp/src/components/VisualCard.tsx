//table of deck cards
import React, { ReactNode } from 'react';

import { Box, CardHeader, CardMedia, Card } from '@material-ui/core';

interface ComponentProps{
    cardOverview: InventoryOverviewDto;
    //onCardSelected: ((card: InventoryOverviewDto) => void) | undefined;
    onCardSelected?: () => void;
    children?: ReactNode;
}

export default function VisualCard(props: ComponentProps): JSX.Element {
    
    return (
        <React.Fragment>
            <Card 
                key={props.cardOverview.name} 
                className="outline-section"
                onClick={props.onCardSelected}
                >
                <CardHeader titleTypographyProps={{variant:"body1"}} title={ `${props.cardOverview.name} - (${props.cardOverview.count})` } />
                {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={`${card.name} (${card.count})`}/> */}
                <CardMedia 
                    style={{height:"310px", width: "223px"}}
                    className="item-image"
                    image={props.cardOverview.img}
                    title={props.cardOverview.name} />
                {props.children}

            </Card>

        </React.Fragment>
    );
}