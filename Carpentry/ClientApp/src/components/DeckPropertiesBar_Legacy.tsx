//TODO Review and verify if actually used
//this is a collapsable component that can display / edit the properties of a deck
import React from 'react'
import { Paper, IconButton, Typography, ExpansionPanel, ExpansionPanelSummary, Chip, Avatar, Divider, ExpansionPanelDetails, TextField, InputAdornment, ExpansionPanelActions, Button } from '@material-ui/core';
import { KeyboardArrowUp, KeyboardArrowDown, Edit } from '@material-ui/icons';
import LandIcon from './LandIcon';
// import { KeyboardArrowDown, KeyboardArrowUp} from '@material-ui/icons'
// import { IconButton } from '@material-ui/core';

export interface DeckPropertiesBarProps{
    // value: number,
    // onValueChanged: (newValue: number) => void;
    totalPrice: number;
    deckProperties: DeckProperties;
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    onSaveClick: () => void;
    // onCancelClick: () => void;
    //onPropChange: (prop: string, value: string)
    //onManaCountChange: (color: string, value: number);
}

export default function DeckPropertiesBar(props: DeckPropertiesBarProps): JSX.Element {
    console.log('rendering properties');
    console.log(props);
    
    //TODO this needs a loading indicator
    return (
        <ExpansionPanel className="">
            <ExpansionPanelSummary
                expandIcon={<Edit />}
                aria-controls="panel1c-content"
                id="panel1c-header">
                <div className="flex-section flex-25">
                    <Typography>{props.deckProperties.name} - ${props.totalPrice}</Typography>
                </div>
                <div className="flex-section flex-25">
                    <Typography>{props.deckProperties.notes}</Typography>
                </div>
                <div className="flex-section flex-25">
                    <Typography>{props.deckProperties.format}</Typography>
                </div>
                <div className="flex-section flex-25">
                    <Chip size="small" avatar={<Avatar src="/img/W.svg" />} label={ props.deckProperties.basicW }/>
                    <Chip size="small" avatar={<Avatar src="/img/U.svg" />} label={ props.deckProperties.basicU }/>
                    <Chip size="small" avatar={<Avatar src="/img/B.svg" />} label={ props.deckProperties.basicB }/>
                    <Chip size="small" avatar={<Avatar src="/img/R.svg" />} label={ props.deckProperties.basicR }/>
                    <Chip size="small" avatar={<Avatar src="/img/G.svg" />} label={ props.deckProperties.basicG }/>
                </div>
            </ExpansionPanelSummary>
            <Divider />
            <ExpansionPanelDetails>
                <div className="flex-section flex-25">
                    <TextField 
                        label="Name"
                        color="primary"
                        name="name"
                        value={props.deckProperties.name}
                        onChange={props.onChange} />
                </div>
                <div className="flex-section flex-25">
                    <TextField 
                        label="Notes"
                        color="primary"
                        name="notes"
                        value={props.deckProperties.notes}
                        onChange={props.onChange} />
                </div>
                <div className="flex-section flex-25">
                    <TextField 
                        label="Format"
                        color="primary"
                        name="format"
                        value={props.deckProperties.format}
                        onChange={props.onChange} />
                </div>
                <div className="flex-section flex-25 flex-row">
                    {/* <Typography>Basics</Typography> */}
                    {/* I may want a series of InputFields in a bigger container rather than 5 TextFields */}

                    <TextField
                        name="basicW"
                        type="number"
                        value={props.deckProperties.basicW}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">W</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicU"
                        type="number"
                        value={props.deckProperties.basicU}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">U</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicB"
                        type="number"
                        value={props.deckProperties.basicB}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">B</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicR"
                        type="number"
                        value={props.deckProperties.basicR}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">R</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicG"
                        type="number"
                        value={props.deckProperties.basicG}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">G</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                </div>
            </ExpansionPanelDetails>
            <Divider />
            <ExpansionPanelActions>
                {/* <Button size="small" onClick={props.onCancelClick}>Cancel</Button> */}
                <Button size="small" onClick={props.onSaveClick} color="primary">Save</Button>
            </ExpansionPanelActions>
        </ExpansionPanel>
    
    );
}
