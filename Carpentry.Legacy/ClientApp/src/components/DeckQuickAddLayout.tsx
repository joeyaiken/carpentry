//TODO Review and verify if actually used
import React from 'react'
import { Container, CardHeader, IconButton, Divider, CardContent, Box, TextField, MenuItem, CardActions, Button, Card } from '@material-ui/core';
import { Close } from '@material-ui/icons';
// import { KeyboardArrowDown, KeyboardArrowUp} from '@material-ui/icons'
// import { IconButton } from '@material-ui/core';

export interface ComponentProps{
    deck: DeckProperties;
    onSaveClick: () => void;
    onCloseClick: () => void;
    onChange: (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void
}

//AKA DeckQuickAdd_Modal_Layout
export default function DeckQuickAddLayout(props: ComponentProps): JSX.Element {
    return (
        <Container maxWidth="sm">
                <Card>
                    <CardHeader
                        title="Add New Deck"
                        action={
                            <IconButton size="medium" onClick={props.onCloseClick}>
                                <Close />
                            </IconButton>
                        }
                    />
                    <Divider/>
                    <CardContent>
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
                                label="Type"
                                color="primary"
                                name="type"
                                value={props.deck.format} 
                                onChange={props.onChange}>
                                    <MenuItem key="Standard" value="Standard">Standard</MenuItem>
                                    <MenuItem key="Modern" value="Modern">Modern</MenuItem>
                                    <MenuItem key="Commander" value="Commander">Commander</MenuItem>
                                    <MenuItem key="Oathbreaker" value="Oathbreaker">Oathbreaker</MenuItem>
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

                    </CardContent>
                    <Divider/>
                    <CardActions style={{justifyContent:"space-between"}}>
                        <Button size="medium" onClick={props.onCloseClick}>Cancel</Button>
                        <Button size="medium" variant="contained" color="primary" className="pull-right" onClick={props.onSaveClick}>Add</Button>
                    </CardActions>
                </Card>
            </Container>
    );
}