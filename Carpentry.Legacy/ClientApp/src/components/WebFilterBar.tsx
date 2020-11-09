//This component should represent the filter bar options present on CardSearch & Inventory
import React from 'react'
import { Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox } from '@material-ui/core';

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'

export interface WebFilterBarProps{
    searchFilter: CardFilterProps,
    visibleFilters: CardFilterVisibilities;
    filterOptions: CoreFilterOptions,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
}

export default function WebFilterBar(props: WebFilterBarProps): JSX.Element {
    return(
        <Box className="flex-section flex-row">
            {   //NAME filter - Web only
                props.visibleFilters.name &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="cardName"
                        className="stretch"
                        label="Web"
                        value={props.searchFilter.cardName}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {   //NAME IS EXCLUSIVE filter - Web only
                props.visibleFilters.name &&
                <Box className="flex-section side-padded">
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
            }
        </Box>
    );
}

