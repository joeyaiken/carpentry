import React from 'react';
import { Box, CardContent, Typography } from '@material-ui/core';
// import VisualCard from './VisualCard';
import CardGridContainer from './CardGridContainer';
import GridCard from './GridCard';

interface ComponentProps{
    cardOverviews: InventoryOverviewDto[];
    onCardSelected: (cardName: string) => void;
}

export default function InventoryCardGrid(props: ComponentProps): JSX.Element {
    return (
        <React.Fragment>
            <CardGridContainer layout="grid">
                {
                    props.cardOverviews.map(cardItem => 
                        <GridCard 
                            key={cardItem.name}
                            onDetailClick={() => {props.onCardSelected(cardItem.name)}}
                            card={cardItem}
                            hideHeader={true}>
                            <CardContent>
                                <Box className="flex-col">
                                    <Box className="flex-row">
                                        {cardItem.count && (<Typography>{cardItem.count} Total</Typography>)}
                                    </Box>
                                </Box>
                            </CardContent>
                        </GridCard>
                    )
                }
            </CardGridContainer>
        </React.Fragment>
    );
}