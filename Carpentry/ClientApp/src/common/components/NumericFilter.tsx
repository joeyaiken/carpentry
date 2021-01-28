import { Box, TextField } from '@material-ui/core';
import React from 'react';
import { appStyles } from '../../styles/appStyles';

interface NumericFilterProps {
    name: string;
    value: number;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export default function NumericFilter(props: NumericFilterProps): JSX.Element {
    const { flexSection, sidePadded, stretch } = appStyles();
    return(
        <Box className={`${flexSection} ${sidePadded}`}>
            <TextField
                name={props.name}
                className={stretch}
                label={props.name}
                value={props.value}
                onChange={props.handleFilterChange}
                style={{textTransform: "capitalize"}}
                margin="normal"/>
        </Box>
    )
}