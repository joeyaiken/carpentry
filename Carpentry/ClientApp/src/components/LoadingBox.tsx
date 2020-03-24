import React from 'react';
import { Box, CircularProgress } from '@material-ui/core';

export default function LoadingBox(): JSX.Element {
    return(
        <Box className="flex-row">
            <CircularProgress />
        </Box>
    );
}

