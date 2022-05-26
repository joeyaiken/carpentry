import React from 'react';
import { Typography, Box, AppBar, Chip, Toolbar, Avatar, Button } from '@material-ui/core';
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
// import {openDeckPropsModal, toggleDeckViewMode} from "../state/DeckEditorActions";
// import {openExportDialog} from "../../deck-export/state/DeckExportActions";
import {Link} from "react-router-dom";
import {selectDeckProperties} from "../../deckDetailSlice";
import {toggleDeckViewMode} from "../deckEditorSlice";
import {useHistory} from "react-router";

interface ComponentProps {
    deckId: number
}
export const DeckPropsBar = (props: ComponentProps): JSX.Element => {
    const deckProperties = useAppSelector(selectDeckProperties)
    
    const dispatch = useAppDispatch();
    const history = useHistory();

    const onToggleViewClick = (): void => {
        dispatch(toggleDeckViewMode());
    }

    const onEditClick = (): void => {
        // dispatch(openDeckPropsModal(deckProperties));
    }

    const onExportClick = (): void => {
        // dispatch(openExportDialog());
    }

    const onAddCardsClick = (): void => {
        history.push(`/decks/${props.deckId}/addCards`);
    }

    const ManaChip = (type: String, value: number): JSX.Element =>
      (<Chip size="small" avatar={<Avatar src={`/img/${type}.svg`}/>} label={ value }/>);
    
    // const addCardsLink = `/decks/${props.id}/addCards`;
    
    return (
        <AppBar color="default" position="relative">
            <Toolbar>
                <Typography variant="h5">{deckProperties.name}</Typography>
                <Box>
                    { deckProperties.basicW > 0 && ManaChip('W',deckProperties.basicW)}
                    { deckProperties.basicU > 0 && ManaChip('U',deckProperties.basicU)}
                    { deckProperties.basicB > 0 && ManaChip('B',deckProperties.basicB)}
                    { deckProperties.basicR > 0 && ManaChip('R',deckProperties.basicR)}
                    { deckProperties.basicG > 0 && ManaChip('G',deckProperties.basicG)}
                </Box>
                <Box>
                    <Button onClick={onToggleViewClick} color="primary" variant="contained">
                        Toggle View
                    </Button>
                    <Button onClick={onEditClick} color="primary" variant="contained">
                        Edit
                    </Button>
                    <Button onClick={onExportClick} color="primary" variant="contained">
                        Export
                    </Button>
                    {/*<Link to={addCardsLink} component={Button} color="primary" variant="contained" className="add-cards-button">*/}
                    
                    {/*<Link to={`/decks/${deckProperties.id}/addCards`} */}
                    {/*      component={Button} */}
                    {/*      color="primary" */}
                    {/*      // variant="contained"*/}
                    {/*      className="add-cards-button"*/}
                    {/*>*/}
                    {/*    Add Cards*/}
                    {/*</Link>*/}
                    
                    <Button onClick={onAddCardsClick} color="primary" variant="contained" className="add-cards-button">
                        Add Cards
                    </Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
}