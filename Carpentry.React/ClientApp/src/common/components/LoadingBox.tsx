import React from 'react';
import { Box, CircularProgress } from '@material-ui/core';
import styles from '../../app/App.module.css';

export const LoadingBox = (): JSX.Element => {
  return (
    <Box className={styles.flexRow}>
      <CircularProgress/>
    </Box>
  );
}
