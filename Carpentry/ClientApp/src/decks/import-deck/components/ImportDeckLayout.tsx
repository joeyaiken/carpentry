import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography, Box, TextareaAutosize, TextField, MenuItem } from '@material-ui/core';

interface ComponentProps {
    importProps: DeckImportUiProps;
    formatFilters: FilterOption[];
    
    onChange: (name: string, value: string) => void;
    
    onCloseClick?: () => void;
    onBackClick?: () => void;
    onValidateClick?: () => void;
    onSaveClick?: () => void;

    // importString: string;

    //stage // screen
    //maybe an enum?

    //isValid
    //'has been validated'
    isValidated?: boolean;
    validatedImport: ValidatedDeckImportDto;
    // isValid?: boolean; //(belongs to validatedImport)
}

export default function ImportDeckLayout(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Dialog open={true} onClose={props.onCloseClick} >
                <DialogTitle>Import Deck</DialogTitle>
                <DialogContent>
                    { !props.validatedImport.isValid && 
                        <ImportDetailLayout 
                            importProps={props.importProps}
                            formatFilters={props.formatFilters}
                            onChange={props.onChange} />
                    }
                    { props.isValidated && !props.validatedImport.isValid && <ImportValidationIssues untrackedSets={props.validatedImport.untrackedSets} />}
                    { props.isValidated && props.validatedImport.isValid && <ValidatedImportLayout />}
                    
                    {/* <DeckPropertiesLayout formatFilters={props.formatFilters} deck={props.deckProps} 
                        onChange={event => props.onChange(event.target.name, event.target.value)} /> */}
                </DialogContent>
                <DialogActions>
                    { !props.isValidated && <Button size="medium" onClick={props.onCloseClick}>Cancel</Button> }
                    { props.isValidated && <Button size="medium" variant="contained" color="primary" onClick={props.onBackClick}>Back</Button> }
                    { !props.validatedImport.isValid && <Button size="medium" variant="contained" color="primary" onClick={props.onValidateClick}>Validate</Button>}
                    { props.validatedImport.isValid && <Button size="medium" variant="contained" color="primary" onClick={props.onSaveClick}>Save</Button> }
                </DialogActions>
            </Dialog>
        </React.Fragment>
    );
}


//function - import props layout
interface ImportDetailLayoutProps {
    importProps: DeckImportUiProps;
    formatFilters: FilterOption[];
    
    onChange: (name: string, value: string) => void;
    
    // onCloseClick?: () => void;
    // onBackClick?: () => void;
    // onValidateClick?: () => void;
    // onSaveClick?: () => void;


    // importString: string;

    // //stage // screen
    // //maybe an enum?

    // //isValid
    // //'has been validated'
    // isValidated?: boolean;
    // isValid?: boolean;
}
function ImportDetailLayout(props: ImportDetailLayoutProps): JSX.Element {
    return(<React.Fragment>
        {/* Name */}
        <Box>
            <TextField 
                label="Name"
                color="primary"
                name="name"
                value={props.importProps.name} 
                onChange={event => props.onChange(event.target.name, event.target.value)} />
        </Box>
        {/* Format */}
        <Box>
            <TextField 
                select
                label="Format"
                color="primary"
                name="format"
                value={props.importProps.format} 
                onChange={event => props.onChange(event.target.name, event.target.value)}>
                    { props.formatFilters.map(option => 
                            <MenuItem key={option.value} value={option.name} style={{textTransform:"capitalize"}}>
                                {option.name}
                            </MenuItem>
                    )}
                </TextField>
        </Box>
        {/* Notes */}
        <Box>
            <TextField 
                label="Notes"
                color="primary"
                name="notes"
                value={props.importProps.notes} 
                onChange={event => props.onChange(event.target.name, event.target.value)} />
        </Box>
        {/* Import Method */}
        <Box>
            <TextField
                select
                label="Import Method"
                color="primary"
                name="importMethod"
                value={props.importProps.importMethod}
                onChange={event => props.onChange(event.target.name, event.target.value)}
                // margin="normal"
                >
                    <MenuItem key={'empty'} value={'empty'}>Empty Cards</MenuItem>
                    <MenuItem key={'new'} value={'new'}>New Cards</MenuItem>
            </TextField>
        </Box>
        {/* Import String */}
        <Box>
            <TextareaAutosize 
                placeholder="Cards" 
                rowsMin={3} rowsMax={10} 
                value={props.importProps.importString} 
                name={"importString"} 
                onChange={event => props.onChange(event.target.name, event.target.value)} />
        </Box>
    </React.Fragment>);
}

interface ImportValidationIssuesProps {
    untrackedSets: ValidatedDtoUntrackedSet[];
}
//import data fixer layout
function ImportValidationIssues(props: ImportValidationIssuesProps): JSX.Element {
    return(<Box>
        {/* this, or, DeckpropsLayout */}
        {/* <Typography>Name | Format | Notes</Typography> */}
        {
            props.untrackedSets.length && 
            <Box>
                <Typography>Untracked Sets:</Typography>
                {
                    props.untrackedSets.map(set => <Box>{set.setCode} [+]</Box>)
                }
            </Box>
        }
        <Typography>Basic Lands</Typography>
        {/* Followup details */}
        <Typography>Cards (table)</Typography>
    </Box>);
}

//function - validated import layout
function ValidatedImportLayout(): JSX.Element {
    return(<Box>
        {/* this, or, DeckpropsLayout */}
        <Typography>[ Name | Format | Notes ] (read only)</Typography>
        <Typography>Basic Lands</Typography>
        {/* Followup details */}
        {/* <Typography>?Untracked Sets?</Typography> */}
        <Typography>Cards (table)</Typography>
    </Box>);
}