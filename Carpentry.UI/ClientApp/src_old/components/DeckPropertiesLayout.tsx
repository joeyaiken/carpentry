//12~9 This was recently added to represent the edit screen of the deck properties modal. ATM it only creates new decks
import React,  { ChangeEvent } from 'react'
import { Box, TextField, MenuItem, InputAdornment } from '@material-ui/core';

export interface ComponentProps {
    deck: DeckProperties;
    formatFilters: FilterOption[];
    onChange: (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void;
}

export default function DeckPropertiesLayout(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Box>
                <TextField 
                    label="Name"
                    color="primary"
                    name="name"
                    value={props.deck.name} 
                    onChange={props.onChange} />
            </Box>

            <Box>
                <TextField 
                    select
                    label="Format"
                    color="primary"
                    name="format"
                    value={props.deck.format} 
                    onChange={props.onChange}>
                        {
                            props.formatFilters.map(option => 
                                <MenuItem key={option.value} value={option.value} style={{textTransform:"capitalize"}}>
                                    {option.name}
                                </MenuItem>
                            )
                        }
                        {/* <MenuItem key="Standard" value="standard">Standard</MenuItem>
                        <MenuItem key="Modern" value="modern">Modern</MenuItem>
                        <MenuItem key="Commander" value="commander">Commander</MenuItem>
                        <MenuItem key="Pioneer" value="pioneer">Pioneer</MenuItem>
                        <MenuItem key="Brawl" value="brawl">Brawl</MenuItem> */}
                    </TextField>
            </Box>

            <Box>
                <TextField 
                    label="Notes"
                    color="primary"
                    name="notes"
                    value={props.deck.notes} 
                    onChange={props.onChange} />
            </Box>
            {
                props.deck.id > 0 &&
                <Box className="flex-row">
                    <TextField
                            name="basicW"
                            type="number"
                            value={props.deck.basicW}
                            InputProps={{
                                startAdornment: <InputAdornment position="start">W</InputAdornment>,
                            }} 
                            onChange={props.onChange} 
                    />
                    <TextField
                        name="basicU"
                        type="number"
                        value={props.deck.basicU}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">U</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicB"
                        type="number"
                        value={props.deck.basicB}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">B</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicR"
                        type="number"
                        value={props.deck.basicR}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">R</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                    <TextField
                        name="basicG"
                        type="number"
                        value={props.deck.basicG}
                        InputProps={{
                            startAdornment: <InputAdornment position="start">G</InputAdornment>,
                        }} 
                        onChange={props.onChange} 
                    />
                </Box>
            }
            
        </React.Fragment>
    )
}
