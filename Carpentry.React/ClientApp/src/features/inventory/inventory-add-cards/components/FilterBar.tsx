import { Box, Button, Paper } from '@material-ui/core';
import React from 'react';
import styles from '../../../../app/App.module.css';
import {SetSearchFilterBar} from './SetSearchFilterBar';

interface FilterBarProps {
  filterOptions: AppFiltersDto;
  handleBoolFilterChange: (filter: string, value: boolean) => void;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  searchFilterProps: CardFilterProps;
  handleSearchButtonClick: () => void;
}

export const FilterBar = (props: FilterBarProps): JSX.Element => {
  return(<React.Fragment>
    <Paper className={[styles.outlineSection, styles.flexRow].join(' ')}>
      <SetSearchFilterBar
        filterOptions={props.filterOptions}
        handleBoolFilterChange={props.handleBoolFilterChange}
        handleFilterChange={props.handleFilterChange}
        searchFilter={props.searchFilterProps} />
      <Box className={[styles.staticSection, styles.center, styles.sidePadded].join(' ')}>
        <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
          Search
        </Button>
      </Box>
    </Paper>
  </React.Fragment>);
}