import React from 'react'
import { Box, Typography } from '@material-ui/core';


export default function CardSetSettingsContainerLayout(): JSX.Element {

    return(
        <Box>
            
            <Box>
                <Typography variant="h4">
                    Tracked Sets
                </Typography>
                [Add] [Update All]
            </Box>
            <Box>
                [Table of set results]
            </Box>
        </Box>
);
}