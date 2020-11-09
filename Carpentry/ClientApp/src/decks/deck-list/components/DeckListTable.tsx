import React from 'react';
import { Table, TableHead, TableRow, TableCell, TableBody, Avatar, IconButton } from '@material-ui/core';
import { MoreVert } from '@material-ui/icons';
import { Link } from 'react-router-dom';

export interface ComponentProps{
    onDeckClick: (deckId: number) => void;
    
    decks: DeckOverviewDto[];
    ///
    // deckMenuAnchor: HTMLButtonElement | null;

    //onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onMenuClick?: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    // onMenuClose: () => void;
    // onMenuSelect: (option: string) => void;
}

function manaIcon(type: string): JSX.Element {
    return(<Avatar key={type} style={{height:"24px", width:"24px", display:"inline-flex", verticalAlign: "middle"}} src={`/img/${type}.svg`} />)
}

export default function DeckListTable(props: ComponentProps): JSX.Element {
    // if(true) {
     return (
        <React.Fragment>
            {/* <Menu open={Boolean(props.deckMenuAnchor)} onClose={props.onMenuClose} anchorEl={props.deckMenuAnchor} >
                <MenuItem onClick={() => {props.onMenuSelect("edit")}} value="inventory">Edit</MenuItem>
                <MenuItem onClick={() => {props.onMenuSelect("delete")}} value="delete">Delte</MenuItem>
            </Menu> */}
        
            {/* <Paper className= "flex-section"> */}
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>Format</TableCell>
                            <TableCell>Colors</TableCell>
                            <TableCell>Validity</TableCell>
                            {
                                Boolean(props.onMenuClick) && 
                                <TableCell></TableCell>
                            }
                        </TableRow>
                    </TableHead>
                    <TableBody>
                    {
                        props.decks.map(deck => 
                            <TableRow key={deck.id}>
                                <TableCell onClick={() => {props.onDeckClick(deck.id)}}>
                                    <Link to={`/decks/${deck.id}`}>{deck.name}</Link> 
                                </TableCell>
                                <TableCell onClick={() => {props.onDeckClick(deck.id)}}>{deck.format}</TableCell>
                                <TableCell onClick={() => {props.onDeckClick(deck.id)}}>
                                {
                                    deck.colors.map(color => manaIcon(color))
                                }
                                </TableCell>
                                <TableCell onClick={() => {props.onDeckClick(deck.id)}}>{deck.validationIssues}</TableCell>
                                {
                                    Boolean(props.onMenuClick) && 
                                    <TableCell>
                                        <IconButton size="small" onClick={props.onMenuClick} name={deck.name} value={deck.id}>
                                            <MoreVert />
                                        </IconButton>
                                    </TableCell>
                                }
                            </TableRow>
                        )
                    }
                    </TableBody>
                </Table>
            {/* </Paper> */}
        </React.Fragment>
    );   
}
