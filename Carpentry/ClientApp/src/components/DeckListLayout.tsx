//TODO Review and verify if actually used
import React from 'react';
import { CardHeader, CardActions, Typography, Button, Card, Menu, MenuItem, Paper, Table, TableHead, TableRow, TableCell, TableBody, Chip, Avatar, IconButton, Badge } from '@material-ui/core';
import { MoreVert } from '@material-ui/icons';

export interface ComponentProps{
    onDeckClick: (deckId: number) => void;
    
    decks: DeckProperties[];
    ///
    deckMenuAnchor: HTMLButtonElement | null;

    onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onMenuClose: () => void;
    onMenuSelect: (option: string) => void;
}

function manaIcon(type: string): JSX.Element {
    return(<Avatar style={{height:"24px", width:"24px", display:"inline-flex", verticalAlign: "middle"}} src={`/img/${type}.svg`} />)
}

export default function DeckListLayout(props: ComponentProps): JSX.Element {
    if(true) {
     return (
        <React.Fragment>
            <Menu open={Boolean(props.deckMenuAnchor)} onClose={props.onMenuClose} anchorEl={props.deckMenuAnchor} >
                <MenuItem onClick={() => {props.onMenuSelect("edit")}} value="inventory">Edit</MenuItem>
                <MenuItem onClick={() => {props.onMenuSelect("delete")}} value="delete">Delte</MenuItem>
                {/* Export? */}
                {/* Copy? */}
            </Menu>
        
            <Paper className="flex-section">
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>Format</TableCell>
                            <TableCell>Colors</TableCell>
                            <TableCell>Validity</TableCell>
                            <TableCell>?</TableCell> {/* (actions) */}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            props.decks.map(deck => 
                                <TableRow key={deck.id}>
                                    <TableCell onClick={() => {props.onDeckClick(deck.id)}}>{deck.name}</TableCell>
                                    <TableCell onClick={() => {props.onDeckClick(deck.id)}}>{deck.format}</TableCell>
                                    <TableCell onClick={() => {props.onDeckClick(deck.id)}}>
                                        {
                                            //deckList.
                                        }
                                        {deck.basicW > 0 && manaIcon("W")}
                                        {deck.basicU > 0 && manaIcon("U")}
                                        {deck.basicB > 0 && manaIcon("B")}
                                        {deck.basicR > 0 && manaIcon("R")}
                                        {deck.basicG > 0 && manaIcon("G")}
                                    </TableCell>
                                    <TableCell onClick={() => {props.onDeckClick(deck.id)}}>{deck.notes}</TableCell>
                                    <TableCell>
                                        <IconButton size="small" onClick={props.onMenuClick} name={deck.name} value={deck.id}>
                                            <MoreVert />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            )
                        }
                    </TableBody>
                </Table>
            </Paper>
        </React.Fragment>
    );   
    }
    
    // letsThrowAnError
    
    return (
        <Card>
            <CardHeader 
                titleTypographyProps={{variant:"h6"}} 
                title={"Decks"} 
            />         
            <CardActions>
                <div className="flex-col">
                {
                    (!props.decks || props.decks.length < 1) ? (
                        <Typography variant="h5">...decks aren't loading right now...</Typography>
                    ) : (
                        props.decks.map((deck) => {
                            return (<Typography key={deck.id}><Button onClick={() => { props.onDeckClick(deck.id) }} color="primary">{deck.name}</Button></Typography>)
                        })
                    )
                }
                </div>
            </CardActions>
        </Card>
    );
}
