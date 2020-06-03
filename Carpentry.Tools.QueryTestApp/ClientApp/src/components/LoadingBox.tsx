import React from 'react';
import { Box, CircularProgress } from '@material-ui/core';
import { appStyles } from '../styles/appStyles';

export default function LoadingBox(): JSX.Element {
    const classes = appStyles();
    return(
        <Box className={classes.flexRow}>
            <CircularProgress />
        </Box>
    );
}

