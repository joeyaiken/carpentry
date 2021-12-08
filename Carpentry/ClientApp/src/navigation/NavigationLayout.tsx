import React from 'react';

import {
  AppBar,
  Typography,
  Toolbar,
  IconButton,
  Button,
} from '@material-ui/core';

import { Menu } from '@material-ui/icons';
import {appStyles} from "../styles/appStyles";

interface LayoutProps {
  onNavClick: (route: string) => void;
}
export default function NavigationLayout(props: LayoutProps): JSX.Element {
  const { flexSection } = appStyles();
  return(
    <AppBar id="app-nav-menu" position="static">
      <Toolbar>
        <IconButton color="inherit" onClick={() => props.onNavClick("/")}>
          <Menu />
        </IconButton>
        <Typography variant="h5" className={flexSection}>
          Carpentry
        </Typography>
        <Button className="nav-button" onClick={() => props.onNavClick("/decks")}>Decks</Button>
        <Button className="nav-button" onClick={() => props.onNavClick("/inventory")}>Inventory</Button>
        <Button className="nav-button" onClick={() => props.onNavClick("/settings/sets")}>Settings</Button>
      </Toolbar>
    </AppBar>
  );
}
