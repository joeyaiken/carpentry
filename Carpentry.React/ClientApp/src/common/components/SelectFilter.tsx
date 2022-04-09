import { Box, MenuItem, TextField } from '@material-ui/core';
import React from 'react';
import { appStyles } from '../../styles/appStyles';

interface SelectFilterProps {
    id: string;
    name: string;
    options: FilterOption[];
    value: string | string[];
    selectMultiple: boolean;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export default function SelectFilter(props: SelectFilterProps): JSX.Element {
    const { flexSection, sidePadded, stretch } = appStyles();
    return(
        <Box id={props.id} className={`${flexSection} ${sidePadded}`}>
                <TextField
                    name={props.name}
                    className={stretch}
                    select
                    SelectProps={{ multiple: props.selectMultiple }}
                    label={props.name}
                    value={props.value}
                    onChange={props.handleFilterChange}
                    style={{textTransform: "capitalize"}}
                    margin="normal" >
                    { props.options.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                </TextField>
            </Box>
    );
}