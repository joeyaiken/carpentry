import {Box, Button, Checkbox, FormControl, FormControlLabel, MenuItem, Paper, TextField} from '@material-ui/core';
import React, {useState} from 'react';
import styles from '../../../../App.module.css';
import CheckBoxOutlineBlankIcon from "@material-ui/icons/CheckBoxOutlineBlank";
import CheckBoxIcon from "@material-ui/icons/CheckBox";
import {useAppDispatch, useAppSelector} from "../../../../hooks";
import {combineStyles} from "../../../../styles/appStyles";
import {ApiStatus} from "../../../../enums";
import {searchCards} from "../inventoryAddCardsSlice";

export const FilterBar = (): JSX.Element => {
  const filterOptions = useAppSelector(state => state.core.filterOptions);
  const searchResultStatus = useAppSelector(state => state.inventory.inventoryAddCards.searchResults.status);

  // All of the filter values get to be local state
  // TODO - figure out if this is a bad approach.  It would be very tedious to manage everything individually
  const [searchFilter, setSearchFilter] = useState<CardFilterProps>({
    set: '',
    colorIdentity: [],
    rarity: [],
    type: '',
    exclusiveColorFilters: false,
    multiColorOnly: false,
    cardName: '',
    exclusiveName: false,
    maxCount: 0,
    minCount: 0,
    format: '',
    text: '',
    group: '',
  })

  const handleFilterChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    setSearchFilter({
      ...searchFilter,
      [event.target.name]: event.target.value
    })
  }

  const handleBoolFilterChange = (filter: string, value: boolean): void => {
    setSearchFilter({
      ...searchFilter,
      [filter]: value,
    })
  }
  
  const dispatch = useAppDispatch();
  
  const handleSearchButtonClick = (): void => {
    if(searchResultStatus !== ApiStatus.loading) dispatch(searchCards(searchFilter));
  }
  
  return(<React.Fragment>
    <Paper className={combineStyles(styles.outlineSection, styles.flexRow)}>
      <Box className={combineStyles(styles.flexSection, styles.flexRow)}>
        {/* SET filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            id="set-select"
            name="set"
            className={styles.stretch}
            select
            label="Set filter"
            value={searchFilter.set}
            onChange={handleFilterChange}
            margin="normal" >
            <MenuItem key="null" value="" />
            { filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>
        {/* Group filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            id="search-group-select"
            name="group"
            className={styles.stretch}
            select
            SelectProps={{ displayEmpty: true }}
            label="Group"
            value={searchFilter.group}
            onChange={handleFilterChange}
            margin="normal">
            <MenuItem key="null" value="" />
            { filterOptions.searchGroups.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
          </TextField>
        </Box>
        {/* Type filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="type"
            className={styles.stretch}
            select
            SelectProps={{ displayEmpty: true }}
            label="Type filter"
            value={searchFilter.type}
            onChange={handleFilterChange}
            margin="normal">
            <MenuItem key="null" value="" />
            { filterOptions.types.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
          </TextField>
        </Box>
        {/* Color Color Identity */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="colorIdentity"
            className={styles.stretch}
            label="Color filter"
            select
            SelectProps={{ multiple: true }}
            value={searchFilter.colorIdentity}
            onChange={handleFilterChange}
            margin="normal" >
            { filterOptions.colors.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>
        {/* color booleans */}
        <Box className={combineStyles(styles.staticSection, styles.sidePadded)}>
          <FormControl component="fieldset">
            <FormControlLabel
              name="exclusiveColorFilters"
              onChange={(e, checked) => {handleBoolFilterChange("exclusiveColorFilters",checked)}}
              checked={searchFilter.exclusiveColorFilters}
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
              onChange={(e, checked) => {handleBoolFilterChange("multiColorOnly",checked)}}
              checked={searchFilter.multiColorOnly}
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
        {/* RARITY filter */}
        <Box className={combineStyles(styles.flexSection, styles.sidePadded)}>
          <TextField
            name="rarity"
            className={styles.stretch}
            select
            SelectProps={{ multiple: true }}
            label="Rarity filter"
            value={searchFilter.rarity}
            onChange={handleFilterChange}
            margin="normal" >
            { filterOptions.rarities.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>
      </Box>
      <Box className={combineStyles(styles.staticSection, styles.center, styles.sidePadded)}>
        <Button variant="contained" size="medium" color="primary" onClick={() => handleSearchButtonClick()}>
          Search
        </Button>
      </Box>
    </Paper>
  </React.Fragment>);
}