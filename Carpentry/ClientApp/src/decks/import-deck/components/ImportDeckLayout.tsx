import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography, Box, TextareaAutosize, TextField, MenuItem } from '@material-ui/core';
import DeckPropertiesLayout from '../../../common/components/DeckPropertiesLayout';

interface ComponentProps {
    // deckProps: DeckPropertiesDto;
    // formatFilters: FilterOption[];
    
    // onChange: (name: string, value: string) => void;
    
    onCloseClick?: () => void;
    onBackClick?: () => void;
    onValidateClick?: () => void;
    onSaveClick?: () => void;


    importString: string;

    //stage // screen
    //maybe an enum?

    //isValid
    //'has been validated'
    isValidated?: boolean;
    isValid?: boolean;
}

export default function ImportDeckLayout(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Dialog open={true} onClose={props.onCloseClick} >
                <DialogTitle>Import Deck</DialogTitle>
                <DialogContent>
                    { !props.isValidated && <ImportPropsLayout />}
                    { props.isValidated && !props.isValid && <ImportDataFixerLayout />}
                    { props.isValidated && props.isValid && <ValidatedImportLayout />}
                    
                    {/* <DeckPropertiesLayout formatFilters={props.formatFilters} deck={props.deckProps} 
                        onChange={event => props.onChange(event.target.name, event.target.value)} /> */}
                </DialogContent>
                <DialogActions>
                    { !props.isValidated && <Button size="medium" onClick={props.onCloseClick}>Cancel</Button> }
                    { props.isValidated && <Button size="medium" variant="contained" color="primary" onClick={props.onBackClick}>Back</Button> }
                    { !props.isValid && <Button size="medium" variant="contained" color="primary" onClick={props.onValidateClick}>Validate</Button>}
                    { props.isValid && <Button size="medium" variant="contained" color="primary" onClick={props.onSaveClick}>Save</Button> }
                </DialogActions>
            </Dialog>
        </React.Fragment>
    );
}

//function - import props layout
function ImportPropsLayout(): JSX.Element {
    return(<React.Fragment>
        <Box>
            <TextField
                // name="Import Type"
                // className={stretch}
                select
                label="Import Method"
                // value={props.selectedExportType}
                // onChange={(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => props.onExportTypeChange(event.target.value)}
                margin="normal">
                    <MenuItem key={'empty'} value={'empty'}>Empty Cards</MenuItem>
                    <MenuItem key={'new'} value={'new'}>New Cards</MenuItem>
                    {/* <MenuItem key={'fill'} value={'fill'}>Deck List</MenuItem> */}
            </TextField>
        </Box>
        <TextareaAutosize placeholder="Cards" rowsMin={3} rowsMax={10} 
        // value={props.deckExportString} 
        />
    </React.Fragment>);
}

//import data fixer layout
function ImportDataFixerLayout(): JSX.Element {
    return(<Box>
        {/* this, or, DeckpropsLayout */}
        <Typography>Name | Format | Notes</Typography>
        <Typography>Basic Lands</Typography>
        {/* Followup details */}
        <Typography>?Untracked Sets?</Typography>
        <Typography>Cards (table)</Typography>
    </Box>);
}

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