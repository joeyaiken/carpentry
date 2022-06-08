import React, {useEffect, useState} from "react";
import {useAppDispatch, useAppSelector} from "../../../../hooks";
import {useHistory} from "react-router";
import {ApiStatus} from "../../../../enums";
import {getInventoryOverviews} from "../inventoryOverviewSlice";
import {
  AppBar,
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  Paper,
  Toolbar,
  Typography
} from "@material-ui/core";
import {combineStyles} from "../../../../styles/appStyles";
import styles from "../../../../App.module.css";
import SelectFilter from "../../../../common/components/SelectFilter";
import TextFilter from "../../../../common/components/TextFilter";
import CheckBoxOutlineBlankIcon from "@material-ui/icons/CheckBoxOutlineBlank";
import CheckBoxIcon from "@material-ui/icons/CheckBox";
import NumericFilter from "../../../common/components/NumericFilter";

// Holds the AppBar, filter bar, and related form value logic
export const InventoryFilterAppBar = (): JSX.Element => {
  // Need this to remember previous searches?
  // try this?
  // https://material-ui.com/components/autocomplete

  // All of the filter values get to be local state
  // TODO - figure out if this is a bad approach.  It would be very tedious to manage everything individually
  const [searchFilter, setSearchFilter] = useState<InventoryFilterProps>({
    groupBy: "unique",
    sortBy: "price",
    set: "",
    text: "",
    exclusiveColorFilters: false,
    multiColorOnly: false,
    skip: 0,
    take: 25,
    type: "",
    colorIdentity: [],
    rarity: [],
    minCount: 0,
    maxCount: 0,
    sortDescending: true,
  });

  const filterOptions = useAppSelector(state => state.core.filterOptions);

  const history = useHistory();

  const handleAddCardsClick = () => history.push('/inventory/add-cards/');
  const handleTrimmingToolClick = () => history.push('/inventory/trimming-tool');

  const handleQuickFilterClick = (filter: string): void => {
    let newFilter =  {...searchFilter};
    switch(filter){
      case "Most Expensive": //by unique, price descending
        newFilter.groupBy = 'unique';
        newFilter.sortBy = 'price';
        newFilter.sortDescending = true;
        break;
      case "Highest Count": //by name, owned count descending
        newFilter.groupBy = 'name';
        newFilter.sortBy = 'count';
        newFilter.sortDescending = true;
        break;
      case "Owned Cards": //by name, by name, where MinCount == 1
        newFilter.groupBy = 'name';
        newFilter.sortBy = 'name';
        newFilter.sortDescending = false;
        newFilter.minCount = 1;
        break;
      case "Clear Secondary": //
        newFilter.set = '';
        newFilter.type = '';
        newFilter.colorIdentity = [];
        newFilter.exclusiveColorFilters = false;
        newFilter.text = '';
        newFilter.rarity = [];
        break;
    }
    setSearchFilter(newFilter);
  }

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
  const overviewDataStatus = useAppSelector(state => state.inventory.overviews.data.status);
  const handleSearchButtonClick = (): void => {
    if(overviewDataStatus === ApiStatus.loading)
      return;
    dispatch(getInventoryOverviews(searchFilter));
  };

  useEffect(() => {
    if(overviewDataStatus === ApiStatus.uninitialized)
      dispatch(getInventoryOverviews(searchFilter));
  })

  return (
    <React.Fragment>
      <AppBar color="default" position="relative">
        <Toolbar>
          <Typography variant="h6">
            Inventory
          </Typography>
          <QuickFilter name="Most Expensive" onClick={() => handleQuickFilterClick("Most Expensive")} /> {/* by unique, price descending */}
          <QuickFilter name="Highest Count" onClick={() => handleQuickFilterClick("Highest Count")} />{/*  */}
          <QuickFilter name="Owned Cards" onClick={() => handleQuickFilterClick("Owned Cards")} />{/*  */}
          <QuickFilter name="Clear Secondary" secondary={true} onClick={() => handleQuickFilterClick("Clear Secondary")} />

          <Button onClick={handleAddCardsClick}>Add Cards</Button>
          <Button onClick={handleTrimmingToolClick}>Trimming Tool</Button>
        </Toolbar>
      </AppBar>
      <Paper className={combineStyles(styles.outlineSection, styles.flexCol)}>
        <Box className={combineStyles(styles.flexSection, styles.flexRow)}>
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
        <Box className={combineStyles(styles.flexSection, styles.flexRow)}>
          <Box className={combineStyles(styles.flexSection, styles.flexRow)}>

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
          <Box className={combineStyles(styles.staticSection, styles.center, styles.sidePadded)}>
            <Button variant="contained" size="medium" color="primary" onClick={() => handleSearchButtonClick()}>
              Search
            </Button>
          </Box>
        </Box>

      </Paper>
    </React.Fragment>
  )
}

// QuickFilter is a component only used by the FilterAppBar
interface QuickFilterProps {
  name: string;
  secondary?: boolean;
  onClick?: () => void;
}
const QuickFilter = (props: QuickFilterProps): JSX.Element => {
  return(
    <Button color={Boolean(props.secondary) ? "secondary" : "primary"} variant="outlined" size="small" style={{textTransform:"none"}} onClick={props.onClick}>
      {props.name}
    </Button>
  );
}

// BooleanCheckboxGroup is a component only used by the FilterAppBar
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
