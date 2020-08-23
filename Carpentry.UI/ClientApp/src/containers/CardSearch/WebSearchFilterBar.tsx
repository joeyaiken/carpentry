//This component should represent the filter bar options present on CardSearch & Inventory
import React from 'react'
import { Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox } from '@material-ui/core';

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'
import { appStyles, combineStyles } from '../../styles/appStyles';

export interface WebSearchFilterBarProps{
    searchFilter: CardFilterProps,
    filterOptions: AppFiltersDto,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
}

export default function WebSearchFilterBar(props: WebSearchFilterBarProps): JSX.Element {
    const classes = appStyles();
    return(
        // <Box className={combineStyles([classes.flexSection, classes.flexRow])}>
        <Box className={combineStyles(classes.flexSection, classes.flexRow)}>
            {/* //NAME filter - Web only */}
            <Box className={`${classes.flexSection} ${classes.sidePadded}`}>
                <TextField
                    name="cardName"
                    className={classes.stretch}
                    label="Web"
                    value={props.searchFilter.cardName}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box>
            {/* //NAME IS EXCLUSIVE filter - Web only */}
            <Box className={`${classes.flexSection} ${classes.sidePadded}`}>
                <FormControl component="fieldset">
                    <FormControlLabel
                        name="exclusiveName"
                        onChange={(e, checked) => {props.handleBoolFilterChange("exclusiveName",checked)}}
                        checked={props.searchFilter.exclusiveName}
                        control={
                            <Checkbox 
                                color="primary"
                                icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                checkedIcon={<CheckBoxIcon fontSize="small" />}
                            />
                        }
                        label="Exclusive"
                    />
                </FormControl>
            </Box>
        </Box>
    );
}

