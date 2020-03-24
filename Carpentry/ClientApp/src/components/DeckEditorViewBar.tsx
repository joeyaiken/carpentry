//TODO Review and verify if actually used

import React, { ReactNode } from 'react';

import {
  AppBar,
  Typography,
  Toolbar,
  IconButton,
  Box,
  TextField,
  MenuItem
} from '@material-ui/core';

import {
  Add, AddBox
} from '@material-ui/icons';

interface Props {
    options: DeckViewOptions;
    onViewChanged: (option: string, value: string) => void;

}

export default function DeckEditorViewBar(props: Props): JSX.Element {
    return(
        <React.Fragment>
            <AppBar color="default" position="relative">
                    <Toolbar>
                        <Typography variant="h5">Deck Editor</Typography>
                        <Box className="static-section side-padded">
                            <TextField
                                name="view"
                                select
                                label="View"
                                margin="normal"
                                value={props.options.view}
                                onChange={(event) => {props.onViewChanged('view',event.target.value)}}
                                >
                                    <MenuItem key="img" value="img" >Image</MenuItem>
                                    <MenuItem key="list" value="list" >List</MenuItem>
                                </TextField>
                        </Box>

                        <Box className="static-section side-padded">
                            <TextField
                                name="group"
                                select
                                label="Group"
                                margin="normal"
                                value={props.options.group}
                                onChange={(event) => {props.onViewChanged('group',event.target.value)}}
                                >
                                    <MenuItem key="None" value="none">None</MenuItem>
                                    <MenuItem key="Type" value="type">Type</MenuItem>
                                    <MenuItem key="Rarity" value="rarity">Rarity</MenuItem>
                                    <MenuItem key="Set" value="set">Set</MenuItem>
                                </TextField>
                        </Box>

                        <Box className="flex-section side-padded">
                            <TextField
                                name="sort"
                                select
                                label="Sort"
                                margin="normal"
                                value={props.options.sort}
                                onChange={(event) => {props.onViewChanged('sort',event.target.value)}}
                                >
                                    <MenuItem key="Name" value="name">Name</MenuItem>
                                    <MenuItem key="Rarity" value="rarity">Rarity</MenuItem>
                                    <MenuItem key="ManaCost" value="manaCost">ManaCost</MenuItem>
                                </TextField>
                        </Box>
                    </Toolbar>
                </AppBar>
        </React.Fragment>
    );
}


/*


                
                

*/