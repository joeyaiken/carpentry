import React from 'react'
import { Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox } from '@material-ui/core';
import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'
import { appStyles, combineStyles } from '../../../styles/appStyles';

export interface SetSearchFilterBarProps{
    searchFilter: CardFilterProps,
    filterOptions: AppFiltersDto,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
}

export default function SetSearchFilterBar(props: SetSearchFilterBarProps): JSX.Element {
  const classes = appStyles();
  return(
    <Box className={combineStyles(classes.flexSection, classes.flexRow)}>
      {/* //SET filter */}
      <Box className={`${classes.flexSection} ${classes.sidePadded}`}>
        <TextField
          id="set-select"
          name="set"
          className={classes.stretch}
          select
          label="Set filter"
          value={props.searchFilter.set}
          onChange={props.handleFilterChange}
          margin="normal" >
            <MenuItem key="null" value=""></MenuItem>
            { props.filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
        </TextField>
      </Box>
      {/* //Group filter */}
      <Box className={`${classes.flexSection} ${classes.sidePadded}`}>
        <TextField
          id="search-group-select"
          name="group"
          className={classes.stretch}
          select
          SelectProps={{ displayEmpty: true }}
          label="Group"
          value={props.searchFilter.group}
          onChange={props.handleFilterChange}
          margin="normal">
            <MenuItem key="null" value=""></MenuItem>
            { props.filterOptions.searchGroups.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
        </TextField>
      </Box>
      {/* //Type filter */}
      <Box className={`${classes.flexSection} ${classes.sidePadded}`}>
        <TextField
          name="type"
          className={classes.stretch}
          select
          SelectProps={{ displayEmpty: true }}
          label="Type filter"
          value={props.searchFilter.type}
          onChange={props.handleFilterChange}
          margin="normal">
            <MenuItem key="null" value=""></MenuItem>
            { props.filterOptions.types.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
        </TextField>
      </Box>
      {/* //Color Color Identity */}
      <Box className={`${classes.flexSection} ${classes.sidePadded}`}>
        <TextField
          name="colorIdentity"
          className={classes.stretch}
          label="Color filter"
          select
          SelectProps={{ multiple: true }}
          value={props.searchFilter.colorIdentity}
          onChange={props.handleFilterChange}
          margin="normal" >
          { props.filterOptions.colors.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
        </TextField>
      </Box>
      {/* //color booleans */}
      <Box className={combineStyles(classes.staticSection, classes.sidePadded)}>
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
      {/* //RARITY filter */}
      <Box className={combineStyles(classes.flexSection, classes.sidePadded)}>
        <TextField
          name="rarity"
          className={classes.stretch}
          select
          SelectProps={{ multiple: true }}
          label="Rarity filter"
          value={props.searchFilter.rarity}
          onChange={props.handleFilterChange}
          margin="normal" >
          { props.filterOptions.rarities.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
        </TextField>
      </Box>
    </Box>
  );
}

