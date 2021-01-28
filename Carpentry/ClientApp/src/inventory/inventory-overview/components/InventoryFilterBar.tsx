import React from 'react';
import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'
import { Paper, Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox, Button } from '@material-ui/core';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import NumericFilter from '../../../common/components/NumericFilter';
import TextFilter from '../../../common/components/TextFilter';
import SelectFilter from '../../../common/components/SelectFilter';

interface InventoryFilterBarProps{
    searchFilter: InventoryFilterProps,
    viewMethod: "grid" | "table";
    filterOptions: AppFiltersDto,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
    handleSearchButtonClick: () => void;
}

//This bar should just be a flex-grid of filter elements
//It probably shouldn't even have the "search" button
export default function InventoryFilterBar(props: InventoryFilterBarProps): JSX.Element {
    const { outlineSection, flexCol, flexRow, flexSection, sidePadded, staticSection, center } = appStyles();
    // Need this to cache?
    // try this?
    // https://material-ui.com/components/autocomplete

    return(
        <Paper className={combineStyles(outlineSection, flexCol)}>
            <Box className={combineStyles(flexSection, flexRow)}>

                {/* <SelectFilter name="view" value={props.viewMethod} selectMultiple={false} 
                    options={[{name: "grid", value: "grid"},{name: "table", value: "table"}]}
                    handleFilterChange={props.handleFilterChange} /> */}

                <SelectFilter name="groupBy" value={props.searchFilter.groupBy} selectMultiple={false} 
                    options={props.filterOptions.groupBy} handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="sortBy" value={props.searchFilter.sortBy} selectMultiple={false}
                    options={props.filterOptions.sortBy} handleFilterChange={props.handleFilterChange} />

                <BooleanCheckboxGroup handleBoolFilterChange={props.handleBoolFilterChange}
                    options={[{name: "Desc", value: "sortDescending", checked: props.searchFilter.sortDescending}]} />

                <SelectFilter name="set" value={props.searchFilter.set} selectMultiple={false}
                    options={props.filterOptions.sets} handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="type" value={props.searchFilter.type} selectMultiple={false}
                    options={props.filterOptions.types} handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="colorIdentity" selectMultiple={true} value={props.searchFilter.colorIdentity}
                    options={props.filterOptions.colors} handleFilterChange={props.handleFilterChange} />
    
                <BooleanCheckboxGroup handleBoolFilterChange={props.handleBoolFilterChange}
                    options={[{name: "Exclusive", value: "exclusiveColorFilters", checked: props.searchFilter.exclusiveColorFilters},
                    {name: "Multi", value: "multiColorOnly", checked: props.searchFilter.multiColorOnly}]} />

            </Box>
            <Box className={combineStyles(flexSection, flexRow)}>
                <Box className={combineStyles(flexSection, flexRow)}>

                    <TextFilter name="text" value={props.searchFilter.text}
                        handleFilterChange={props.handleFilterChange} />

                    {/* <SelectFilter name="format" value={props.searchFilter.format} selectMultiple={false} options={props.filterOptions.formats}
                        handleFilterChange={props.handleFilterChange} /> */}

                    <SelectFilter name="rarity" value={props.searchFilter.rarity} selectMultiple={true}
                        options={props.filterOptions.rarities} handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="minCount" value={props.searchFilter.minCount}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="maxCount" value={props.searchFilter.maxCount}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="skip" value={props.searchFilter.skip}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="take" value={props.searchFilter.take}
                        handleFilterChange={props.handleFilterChange} />

                </Box>
                <Box className={combineStyles(staticSection, center, sidePadded)}>
                    <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
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