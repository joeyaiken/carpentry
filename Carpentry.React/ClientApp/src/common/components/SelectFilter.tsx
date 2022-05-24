import { Box, MenuItem, TextField } from '@material-ui/core';
import React from 'react';
import styles from '../../app/App.module.css';

interface SelectFilterProps {
  id: string;
  name: string;
  options: FilterOption[];
  value: string | string[];
  selectMultiple: boolean;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const SelectFilter = (props: SelectFilterProps): JSX.Element => {
  return (
    <Box id={props.id} className={`${styles.flexSection} ${styles.sidePadded}`}>
      <TextField
        name={props.name}
        className={styles.stretch}
        select
        SelectProps={{multiple: props.selectMultiple}}
        label={props.name}
        value={props.value}
        onChange={props.handleFilterChange}
        style={{textTransform: "capitalize"}}
        margin="normal">
        {props.options.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>))}
      </TextField>
    </Box>
  );
}