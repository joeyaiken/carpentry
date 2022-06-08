import { Box, TextField } from '@material-ui/core';
import React from 'react';
import styles from '../../../App.module.css';

interface NumericFilterProps {
  id: string;
  name: string;
  value: number;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export default function NumericFilter(props: NumericFilterProps): JSX.Element {
  return(
    <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
      <TextField
        id={props.id}
        type="number"
        name={props.name}
        className={styles.stretch}
        label={props.name}
        value={props.value}
        onChange={props.handleFilterChange}
        style={{textTransform: "capitalize"}}
        margin="normal"/>
    </Box>
  )
}