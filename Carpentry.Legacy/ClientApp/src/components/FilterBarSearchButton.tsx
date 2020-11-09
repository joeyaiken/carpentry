//TODO Review and verify if actually used
import React from 'react';
import { Box, Button } from '@material-ui/core';

export interface FilterBarSearchButtonProps {
    handleSearchButtonClick: () => void;
}

export default function FilterBarSearchButton(props: FilterBarSearchButtonProps): JSX.Element {
    return(
        <Box className="static-section center side-padded">
            <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
                Search
            </Button>
        </Box>
    );
}