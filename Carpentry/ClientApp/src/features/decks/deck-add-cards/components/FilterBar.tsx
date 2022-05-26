import {Box, Button, Checkbox, FormControl, FormControlLabel, MenuItem, Paper, TextField} from '@material-ui/core';
import React from 'react';
import styles from "../../../../app/App.module.css";
import CheckBoxOutlineBlankIcon from "@material-ui/icons/CheckBoxOutlineBlank";
import CheckBoxIcon from "@material-ui/icons/CheckBox";
import {useAppSelector} from "../../../../app/hooks";
import {selectSearchFilterProps} from "../deckAddCardsSlice";

interface FilterBarProps {
  filterOptions: AppFiltersDto;
}

export default function FilterBar(props: FilterBarProps): JSX.Element{

  // const filterOptions = useAppSelector(state => state.decks.deckAddCards.searchFilterProps);
  
  const searchFilterProps = useAppSelector(selectSearchFilterProps);
  
  const handleFilterChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    //this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
  };

  const handleBoolFilterChange = (filter: string, value: boolean): void => {
    //this.props.dispatch(cardSearchFilterValueChanged("cardSearchFilterProps", filter, value));
  };
  
  const handleSearchButtonClick = (): void => {
    //this.props.dispatch(requestCardSearch())
  };
  
  return(<React.Fragment>
    <Paper className={[styles.outlineSection, styles.flexRow].join(' ')}>
      <Box className={[styles.flexSection, styles.flexRow].join(' ')}>
        {/* //Text filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="text"
            className={styles.stretch}
            label="Text"
            value={searchFilterProps.text}
            onChange={handleFilterChange}
            margin="normal"/>
        </Box>

        {/* //SET filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="set"
            className={styles.stretch}
            select
            label="Set filter"
            value={searchFilterProps.set}
            onChange={handleFilterChange}
            margin="normal" >
            <MenuItem key="null" value="">
            </MenuItem>
            { props.filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>

        {/* //Group filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="group"
            className={styles.stretch}
            select
            SelectProps={{
              displayEmpty: true
            }}
            label="Group"
            value={searchFilterProps.group}
            onChange={handleFilterChange}
            margin="normal">
            <MenuItem key="null" value="">
            </MenuItem>
            { props.filterOptions.searchGroups.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
          </TextField>
        </Box>

        {/* //Type filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="type"
            className={styles.stretch}
            select
            SelectProps={{
              displayEmpty: true
            }}
            label="Type filter"
            value={searchFilterProps.type}
            onChange={handleFilterChange}
            margin="normal">
            <MenuItem key="null" value="">
            </MenuItem>
            { props.filterOptions.types.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
          </TextField>
        </Box>
        {/* //Color Color Identity */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="colorIdentity"
            className={styles.stretch}
            label="Color filter"
            select
            SelectProps={{
              multiple: true
            }}
            value={searchFilterProps.colorIdentity}
            onChange={handleFilterChange}
            margin="normal" >
            { props.filterOptions.colors.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>
        {/* //color booleans */}
        <Box className={[styles.staticSection, styles.sidePadded].join(' ')}>
          <FormControl component="fieldset">
            <FormControlLabel
              name="exclusiveColorFilters"
              onChange={(e, checked) => {handleBoolFilterChange("exclusiveColorFilters",checked)}}
              checked={searchFilterProps.exclusiveColorFilters}
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
              checked={searchFilterProps.multiColorOnly}
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
        <Box className={[styles.flexSection, styles.sidePadded].join(' ')}>
          <TextField
            name="rarity"
            className={styles.stretch}
            select
            SelectProps={{ multiple: true }}
            label="Rarity filter"
            value={searchFilterProps.rarity}
            onChange={handleFilterChange}
            margin="normal" >
            { props.filterOptions.rarities.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>

        <NumericFilter id="min-count-filter" name="minCount" value={searchFilterProps.minCount}
                       handleFilterChange={handleFilterChange} />
      </Box>
      <Box className={[styles.staticSection, styles.center, styles.sidePadded].join(' ')}>
        <Button id="search-button" variant="contained" size="medium" color="primary" onClick={handleSearchButtonClick}>
          Search
        </Button>
      </Box>
    </Paper>
  </React.Fragment>);
}

interface NumericFilterProps {
  id: string;
  name: string;
  value: number|null;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}
function NumericFilter(props: NumericFilterProps): JSX.Element {
  return(
    <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
      <TextField
        id={props.id}
        name={props.name}
        className={styles.stretch}
        label={props.name}
        value={props.value}
        onChange={props.handleFilterChange}
        style={{textTransform: "capitalize"}}
        margin="normal"/>
    </Box>
  )
}