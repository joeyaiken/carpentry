import React from 'react';
import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'
import {Paper, Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox, Button} from '@material-ui/core';
// import { appStyles, combineStyles } from '../../../styles/appStyles';
import styles from '../../../../app/App.module.css';
import {NumericFilter} from "../../../../common/components/NumericFilter";
import {SelectFilter} from "../../../../common/components/SelectFilter";
import {TextFilter} from "../../../../common/components/TextFilter";
import {useAppSelector} from "../../../../hooks";

interface InventoryFilterBarProps{
  // searchFilter: InventoryFilterProps,
  // viewMethod: "grid" | "table";
  // filterOptions: AppFiltersDto,
  // handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  // handleBoolFilterChange: (filter: string, value: boolean) => void;
  // handleSearchButtonClick: () => void;
}

//This bar should just be a flex-grid of filter elements
//It probably shouldn't even have the "search" button
export const InventoryFilterBar = (props: InventoryFilterBarProps): JSX.Element => {
  // Need this to cache?
  // try this?
  // https://material-ui.com/components/autocomplete
  
  // TODO - Ensure these selectors aren't fucking up by trying to compare objects (maybe make them a fancy selector in the slice)
  const searchFilter = useAppSelector(state => state.inventory.overviews.filters);
  const filterOptions = useAppSelector(state => state.core.filterOptions);

  const handleFilterChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    
  }
  const handleBoolFilterChange = (filter: string, value: boolean): void => {
    
  }
  
  const handleSearchButtonClick = (): void => {
  
  }
  
  return(
    <Paper className={[styles.outlineSection, styles.flexCol].join(' ')}>
      <Box className={[styles.flexSection, styles.flexRow].join(' ')}>

        <SelectFilter id="group-by-filter" name="groupBy" value={searchFilter.groupBy} selectMultiple={false}
                      options={filterOptions.groupBy} handleFilterChange={handleFilterChange} />

        <SelectFilter id="sort-by-filter" name="sortBy" value={searchFilter.sortBy} selectMultiple={false}
                      options={filterOptions.sortBy} handleFilterChange={handleFilterChange} />

        <BooleanCheckboxGroup handleBoolFilterChange={handleBoolFilterChange}
                              options={[{name: "Desc", value: "sortDescending", checked: searchFilter.sortDescending}]} />

        <SelectFilter id="set-filter" name="set" value={searchFilter.set} selectMultiple={false}
                      options={filterOptions.sets} handleFilterChange={handleFilterChange} />

        <SelectFilter id="type-filter" name="type" value={searchFilter.type} selectMultiple={false}
                      options={filterOptions.types} handleFilterChange={handleFilterChange} />

        <SelectFilter id="color-identity-filter" name="colorIdentity" selectMultiple={true} value={searchFilter.colorIdentity}
                      options={filterOptions.colors} handleFilterChange={handleFilterChange} />

        <BooleanCheckboxGroup handleBoolFilterChange={handleBoolFilterChange}
                              options={[{name: "Exclusive", value: "exclusiveColorFilters", checked: searchFilter.exclusiveColorFilters},
                                {name: "Multi", value: "multiColorOnly", checked: searchFilter.multiColorOnly}]} />

      </Box>
      <Box className={[styles.flexSection, styles.flexRow].join(' ')}>
        <Box className={[styles.flexSection, styles.flexRow].join(' ')}>

          <TextFilter name="text" value={searchFilter.text}
                      handleFilterChange={handleFilterChange} />

          <SelectFilter id="rarity-filter" name="rarity" value={searchFilter.rarity} selectMultiple={true}
                        options={filterOptions.rarities} handleFilterChange={handleFilterChange} />

          <NumericFilter id="min-count-filter" name="minCount" value={searchFilter.minCount}
                         handleFilterChange={handleFilterChange} />

          <NumericFilter id="max-count-filter" name="maxCount" value={searchFilter.maxCount}
                         handleFilterChange={handleFilterChange} />

          <NumericFilter id="skip-filter" name="skip" value={searchFilter.skip}
                         handleFilterChange={handleFilterChange} />

          <NumericFilter id="take-filter" name="take" value={searchFilter.take}
                         handleFilterChange={handleFilterChange} />

        </Box>
        <Box className={[styles.staticSection, styles.center, styles.sidePadded].join(' ')}>
          <Button variant="contained" size="medium" color="primary" onClick={() => handleSearchButtonClick()}>
            Search
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}

interface BooleanCheckboxOption {
  name: string;
  value: string;
  checked: boolean;
}
interface BooleanCheckboxGroupProps {
  options: BooleanCheckboxOption[];
  handleBoolFilterChange: (filter: string, value: boolean) => void;
}
function BooleanCheckboxGroup(props: BooleanCheckboxGroupProps): JSX.Element {
  return(
    <FormControl component="fieldset">
      {
        props.options.map((item) => (<FormControlLabel
          key={item.value}
          name={item.value}
          onChange={(e, checked) => {props.handleBoolFilterChange(item.value,checked)}}
          checked={item.checked}
          control={<Checkbox  color="primary" icon={<CheckBoxOutlineBlankIcon fontSize="small" />} checkedIcon={<CheckBoxIcon fontSize="small" />} />}
          label={item.name} />))
      }
    </FormControl>
  );
}