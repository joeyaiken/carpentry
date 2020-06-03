import React, { ReactNode } from 'react';
import { AppBar, Typography, Toolbar, IconButton, Container } from '@material-ui/core';
import { Cached } from '@material-ui/icons';
import { appStyles } from '../styles/appStyles';

interface LayoutProps {
    children: ReactNode;
    onButtonClick: () => void;
}

export default function AppLayout(props: LayoutProps): JSX.Element {
    const classes = appStyles();
    return(
        <div className={`${classes.stretch} ${classes.flexCol}`}>
            <AppBar position="static" onClick={props.onButtonClick}>
                <Toolbar>
                    <IconButton color="inherit">
                        <Cached />
                    </IconButton>
                    <Typography variant="h5" className={classes.flexSection}>
                        App Query Results
                    </Typography>
                </Toolbar>
            </AppBar>
            <Container maxWidth="xl" className={`${classes.flexSection} ${classes.flexCol}`} style={{overflow:'auto'}}>
                {props.children}
            </Container>
        </div>
    );
}
