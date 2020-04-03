//This component should represent the filter bar options present on CardSearch & Inventory
import React from 'react'
import { Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox } from '@material-ui/core';

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'

export interface CardFilterBarProps{
    searchFilter: CardFilterProps,
    visibleFilters: CardFilterVisibilities;
    filterOptions: CoreFilterOptions,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
}

export default function CardFilterBar(props: CardFilterBarProps): JSX.Element {
    return(
        <Box className="flex-section flex-row">
            {   //Text filter
                props.visibleFilters.text &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="text"
                        className="stretch"
                        label="Text"
                        value={props.searchFilter.text}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {   //SET filter
                props.visibleFilters.set &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="set"
                        className="stretch"
                        select
                        label="Set filter"
                        value={props.searchFilter.set}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                            <MenuItem key="null" value=""></MenuItem>
                            { props.filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                        </TextField>
                </Box>
            }
            {   //Type filter
                props.visibleFilters.type &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="type"
                        className="stretch"
                        select
                        SelectProps={{
                            displayEmpty: true
                        }}
                        label="Type filter"
                        value={props.searchFilter.type}
                        onChange={props.handleFilterChange}
                        margin="normal">
                            { props.filterOptions.types.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
                    </TextField>
                </Box>
            }
            {   //Color Color Identity
                props.visibleFilters.color &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="colorIdentity"
                        className="stretch"
                        label="Color filter"
                        select
                        SelectProps={{
                            multiple: true
                        }}
                        value={props.searchFilter.colorIdentity}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                        { props.filterOptions.colors.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                    </TextField>
                </Box>
            }
            {   //color booleans
                props.visibleFilters.color &&
                <Box className="static-section side-padded">
                    <FormControl component="fieldset">
                        <FormControlLabel
                            name="exclusiveColorFilters"
                            onChange={(e, checked) => {props.handleBoolFilterChange("exclusiveColorFilters",checked)}}
                            checked={props.searchFilter.exclusiveColorFilters}
                            control={
                                <Checkbox 
                                    color="primary"
                                    icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                    checkedIcon={<CheckBoxIcon fontSize="small" />}
                                />
                            }
                            label="Exclusive"
                        />
                        <FormControlLabel
                            name="multiColorOnly"
                            onChange={(e, checked) => {props.handleBoolFilterChange("multiColorOnly",checked)}}
                            checked={props.searchFilter.multiColorOnly}
                            control={
                                <Checkbox
                                    color="primary"
                                    icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                    checkedIcon={<CheckBoxIcon fontSize="small" />}
                                />
                            }
                            label="Multi"
                        />
                    </FormControl>
                </Box>
            }
            
            {   //RARITY filter
                props.visibleFilters.rarity &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="rarity"
                        className="stretch"
                        select
                        SelectProps={{ multiple: true }}
                        label="Rarity filter"
                        value={props.searchFilter.rarity}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                        { props.filterOptions.rarities.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                    </TextField>
                </Box>
            }
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
{/* 
            {   //Min Count
                props.visibleFilters.count &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="minCount"
                        className="stretch"
                        label="Min"
                        value={props.searchFilter.minCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {   //Max Count
                props.visibleFilters.count &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="maxCount"
                        className="stretch"
                        label="Max"
                        value={props.searchFilter.maxCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            } */}
            {   //Format
                props.visibleFilters.format &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="format"
                        className="stretch"
                        select
                        label="Format"
                        value={props.searchFilter.format}
                        onChange={props.handleFilterChange}
                        SelectProps={{
                            displayEmpty: true
                        }}
                        margin="normal" >
                        <MenuItem key="none" value=""></MenuItem>
                        <MenuItem key="Standard" value="standard">Standard</MenuItem>
                        <MenuItem key="Modern" value="modern">Modern</MenuItem>
                        <MenuItem key="Commander" value="commander">Commander</MenuItem>
                        <MenuItem key="Pioneer" value="pioneer">Pioneer</MenuItem>
                        <MenuItem key="Brawl" value="brawl">Brawl</MenuItem>
                    </TextField>
                </Box>
            }
    </Box>
  );
}

