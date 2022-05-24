import { Box, TextField } from '@material-ui/core';
import React from 'react';
import styles from '../../app/App.module.css';

interface TextFilterProps {
  name: string;
  value: string;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const TextFilter = (props: TextFilterProps): JSX.Element => {
  return (
    <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
      <TextField
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