import React from 'react'
import { Box, Typography, Button, Switch, FormControlLabel, IconButton, TableHead, Paper, Table, TableRow, TableCell, TableBody } from '@material-ui/core';
import { appStyles } from '../../styles/appStyles';
import { MoreHoriz, MoreVert, Refresh } from '@material-ui/icons';

declare interface ComponentProps {
    onRefreshClick: () => void;
    onShowUntrackedClick: () => void;

    trackedSetDetails: SetDetailDto[];
    showUntrackedValue: boolean;
}

export default function CardSetSettingsContainerLayout(props: ComponentProps): JSX.Element {
    const { outlineSection, flexRow, flexSection, staticSection } = appStyles();
    return(
        <Box>
            
            <Box className={flexRow}>
                <Box className={flexSection}>
                    <Typography variant="h4">
                        Tracked Sets
                    </Typography>
                </Box>
                {/* <Box className={staticSection}> */}
                    <FormControlLabel
                        onClick={props.onShowUntrackedClick}
                        control={
                        <Switch
                            checked={props.showUntrackedValue}
                            //onChange={handleChange}
                            name="checkedB"
                            color="primary"
                        />  
                        }
                        label="Show Untracked"
                    />
                    <Button disabled={true} color="primary" variant="contained" >Update All</Button>
                    <IconButton color="inherit" onClick={props.onRefreshClick}>
                        <Refresh />
                    </IconButton>
                {/* </Box> */}
                
                {/* <FormGroup row>
                    <FormControlLabel
                        control={
                        <Switch
                            checked={false}
                            //onChange={handleChange}
                            name="checkedB"
                            color="primary"
                        />
                        }
                        label="Primary"
                    />
                    <FormControlLabel control={<Switch />} label="Uncontrolled" />
                    <FormControlLabel disabled control={<Switch />} label="Disabled" />
                    <FormControlLabel disabled control={<Switch checked />} label="Disabled" />
                    </FormGroup> */}
                
                
                {/* <IconButton  color="primary">
                    <MoreHoriz />
                </IconButton>
                <Button color="primary" variant="contained">
                    <MoreHoriz />
                </Button>
                <IconButton color="inherit">
                    <MoreVert />
                </IconButton>
                <Button color="primary" variant="contained">
                    <MoreVert />
                </Button> */}
            </Box>
            <Paper>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell>Code</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Owned</TableCell>
                            <TableCell>Collected</TableCell>
                            <TableCell>Last Updated</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>                        
                        {
                            props.trackedSetDetails.map(setDetail => 
                                <TableRow key={setDetail.code}>
                                    <TableCell>{setDetail.code}</TableCell>
                                    <TableCell>{setDetail.name}</TableCell>
                                    <TableCell>[owned count]</TableCell>
                                    <TableCell>[collected count]</TableCell>
                                    <TableCell>[last updated]</TableCell>
                                    <TableCell>[btn]</TableCell>
                                </TableRow>
                                
                                )
                            // props.cardOverviews.map(cardItem => 
                            //     <TableRow onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                            //         key={cardItem.id+cardItem.name}>
                            //         <TableCell>{cardItem.name}</TableCell>
                            //         <TableCell>{cardItem.count}</TableCell>
                            //         <TableCell>{cardItem.type}</TableCell>
                            //         <TableCell>{cardItem.cost}</TableCell>
                            //         <TableCell>{cardItem.category}</TableCell>
                            //     </TableRow>
                            // )
                        }
                    </TableBody>
                </Table>
            </Paper>
        </Box>
);
}