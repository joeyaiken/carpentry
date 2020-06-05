//TODO Review and verify if actually used
import React from 'react';
import { Box, Button } from '@material-ui/core';
import { combineStyles, appStyles } from '../styles/appStyles';

export interface FilterBarSearchButtonProps {
    handleSearchButtonClick: () => void;
}

export default function FilterBarSearchButton(props: FilterBarSearchButtonProps): JSX.Element {
    const { staticSection, center, sidePadded } = appStyles();
    return(
        <Box className={combineStyles(staticSection, center, sidePadded)}>
            <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
                Search
            </Button>
        </Box>
    );
}