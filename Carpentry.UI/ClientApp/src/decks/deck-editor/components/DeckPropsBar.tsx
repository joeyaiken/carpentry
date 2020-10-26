import React from 'react';

import { Typography, Box, AppBar, Chip, Toolbar, Avatar, Button } from '@material-ui/core';
import { Link } from 'react-router-dom';

interface ComponentProps{
    //totalPrice: number;
    deckProperties: DeckPropertiesDto;
    onEditClick: () => void;
    onToggleViewClick: () => void;
    // onAddCardsClick: () => void;
}

const ManaChip = (type: String, value: number): JSX.Element => 
    (<Chip size="small" avatar={<Avatar src={`/img/${type}.svg`}/>} label={ value }/>);

export default function DeckPropsBar(props: ComponentProps): JSX.Element {

    const { basicW, basicU, basicB, basicR, basicG } = props.deckProperties;

    return (
        <AppBar color="default" position="relative">
            <Toolbar>
                {/* <Typography variant="h5">Deck Editor</Typography> */}
                <Typography variant="h5">{props.deckProperties.name}</Typography>
                {/* &nbsp; */}
                {/* <Typography variant="h6">--{props.deckProperties && props.deckProperties.name+ ' - '+ props.deckProperties.notes}</Typography> */}
                <Box>
                {/* <div className= "flex-section flex-25"> */}
                    { basicW > 0 && ManaChip('W',basicW)}
                    { basicU > 0 && ManaChip('U',basicU)}
                    { basicB > 0 && ManaChip('B',basicB)}
                    { basicR > 0 && ManaChip('R',basicR)}
                    { basicG > 0 && ManaChip('G',basicG)}
                    {/* <Chip size="small" avatar={<Avatar src="/img/W.svg" />} label={ basicW }/> */}
                    
                    
                    {/* <Chip size="small" avatar={<Avatar src="/img/W.svg" />} label={ basicW }/> */}
                    {/* <Chip size="small" avatar={<Avatar src="/img/U.svg" />} label={ basicU }/>
                    <Chip size="small" avatar={<Avatar src="/img/B.svg" />} label={ basicB }/>
                    <Chip size="small" avatar={<Avatar src="/img/R.svg" />} label={ basicR }/>
                    <Chip size="small" avatar={<Avatar src="/img/G.svg" />} label={ basicG }/> */}
                </Box>
                <Box>
                    <Button onClick={props.onToggleViewClick} color="primary" variant="contained">
                        Toggle View
                    </Button>
                    <Button onClick={props.onEditClick} color="primary" variant="contained">
                        Edit
                    </Button>
                    {/* <Button onClick={props.onAddCardsClick} color="primary" variant="contained">
                        Add Cards
                    </Button> */}
                    <Link to={`/decks/${props.deckProperties.id}/addCards/`}>
                        <Button color="primary" variant="contained">Add Cards</Button>
                    </Link>
                </Box>
            </Toolbar>
        </AppBar>
    );
}