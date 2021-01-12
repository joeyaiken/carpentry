import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography, Box, TextareaAutosize } from '@material-ui/core';
import DeckPropertiesLayout from '../../../common/components/DeckPropertiesLayout';

interface ComponentProps {
    // deckProps: DeckPropertiesDto;
    // formatFilters: FilterOption[];
    // onChange: (name: string, value: string) => void;
    // onSaveClick: () => void;
    onCloseClick: () => void;
}

export default function ImportDeckLayout(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Dialog open={true} onClose={props.onCloseClick} >
                <DialogTitle>Import Deck</DialogTitle>
                <DialogContent>
                    <ImportPropsLayout />
                    {/* <ValidatedImportLayout /> */}

                    
                    {/* <DeckPropertiesLayout formatFilters={props.formatFilters} deck={props.deckProps} 
                        onChange={event => props.onChange(event.target.name, event.target.value)} /> */}
                </DialogContent>
                <DialogActions>
                    <Button size="medium" onClick={props.onCloseClick}>Cancel</Button>
                    <Button size="medium" variant="contained" color="primary" 
                    // onClick={props.onSaveClick}
                    >Validate</Button>
                    
                    <Button size="medium" variant="contained" color="primary" 
                    // onClick={props.onSaveClick}
                    >Back</Button>

                    <Button size="medium" variant="contained" color="primary" 
                    // onClick={props.onSaveClick}
                    >Save</Button>
                </DialogActions>
            </Dialog>
        </React.Fragment>
    );
}

//function - import props layout
function ImportPropsLayout(): JSX.Element {
    return(<Box>
        <Typography>Source? (empty, add, fill)</Typography>
        <TextareaAutosize placeholder="Cards" rowsMin={3} rowsMax={10} 
        // value={props.deckExportString} 
        />
    </Box>);
}

//import data fixer layout

//function - validated import layout
function ValidatedImportLayout(): JSX.Element {
    return(<Box>
        {/* this, or, DeckpropsLayout */}
        <Typography>Name | Format | Notes</Typography>
        <Typography>Basic Lands</Typography>
        {/* Followup details */}
        <Typography>?Untracked Sets?</Typography>
        <Typography>Cards (table)</Typography>
    </Box>);
}