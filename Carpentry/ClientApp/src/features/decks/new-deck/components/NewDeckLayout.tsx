import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@material-ui/core';
import {DeckPropertiesLayout} from "../../../common/components/DeckPropertiesLayout";

interface ComponentProps {
    deckProps: DeckPropertiesDto;
    formatFilters: FilterOption[];
    onChange: (name: string, value: string) => void;
    onSaveClick: () => void;
    onCloseClick: () => void;
}

export default function NewDeckLayout(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Dialog open={true} onClose={props.onCloseClick} >
                <DialogTitle>New Deck</DialogTitle>
                <DialogContent>
                    <DeckPropertiesLayout showLands={false} formatFilters={props.formatFilters} deck={props.deckProps} 
                        onChange={event => props.onChange(event.target.name, event.target.value)} />
                </DialogContent>
                <DialogActions>
                    <Button size="medium" onClick={props.onCloseClick}>Cancel</Button>
                    <Button id="save-new-deck-button" size="medium" variant="contained" color="primary" onClick={props.onSaveClick}>Save</Button>
                </DialogActions>
            </Dialog>
        </React.Fragment>
    );
}