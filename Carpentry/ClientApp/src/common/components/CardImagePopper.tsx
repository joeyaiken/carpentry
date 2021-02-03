import { Card, CardMedia, Popper } from '@material-ui/core';
import React from 'react'
import { appStyles } from '../../styles/appStyles';

interface CardImagePopperProps { 
    menuAnchor: HTMLElement | null;
    image: string;
    onClose: () => void;
}

export default function CardImagePopper(props: CardImagePopperProps): JSX.Element {
    const classes = appStyles();
    return (
        //works     1300
        //doesnt    1299
        <Popper style={{zIndex:1500}} open={Boolean(props.menuAnchor)} anchorEl={props.menuAnchor}>
            <Card>
                <CardMedia style={{height:"310px", width: "223px"}} image={props.image} />
                {/* <CardContent className={classes.flexSection}>
                    <Box className={classes.flexCol}>
                        <Box className={classes.flexRow}>
                            {cardItem.count && (<Typography>{cardItem.count} Total {cardItem.isFoil && " - (FOIL)"}</Typography>)}
                        </Box>
                        <Box className={classes.flexRow}>
                            {cardItem.price && (<Typography>${cardItem.price}</Typography>)}
                        </Box>
                    </Box>
                </CardContent> */}
            </Card>
        </Popper>
    );
}