import { Box, Button, Paper } from '@material-ui/core';
import React from 'react';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import SetSearchFilterBar from './SetSearchFilterBar';
// import WebSearchFilterBar from './WebSearchFilterBar';

interface FilterBarProps {
    filterOptions: AppFiltersDto;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    searchFilterProps: CardFilterProps;
    // cardSearchMethod: "set" | "web" | "inventory";
    handleSearchButtonClick: () => void;
}

export default function FilterBar(props: FilterBarProps): JSX.Element{
    const {  flexRow, outlineSection, staticSection, center, sidePadded } = appStyles();
    return(<React.Fragment>
        <Paper className={combineStyles(outlineSection, flexRow)}>
            {/* {   (props.cardSearchMethod === "set") &&  */}
                    <SetSearchFilterBar 
                        filterOptions={props.filterOptions}
                        handleBoolFilterChange={props.handleBoolFilterChange}
                        handleFilterChange={props.handleFilterChange}
                        searchFilter={props.searchFilterProps} />
            {/* } */}
            {/* {   (props.cardSearchMethod === "web") && 
                    <WebSearchFilterBar 
                        filterOptions={props.filterOptions}
                        handleBoolFilterChange={props.handleBoolFilterChange}
                        handleFilterChange={props.handleFilterChange}
                        searchFilter={props.searchFilterProps} />
            } */}
            <Box className={combineStyles(staticSection, center, sidePadded)}>
                <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
                    Search
                </Button>
            </Box>

        </Paper>
    </React.Fragment>);
}