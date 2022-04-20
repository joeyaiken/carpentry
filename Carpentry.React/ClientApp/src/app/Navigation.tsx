import React from "react"
import {AppBar, Button, IconButton, Toolbar, Typography} from "@material-ui/core";
import {Menu} from "@material-ui/icons";
import styles from './App.module.css'
import {Link} from "react-router-dom";

export const Navigation = (): JSX.Element => {
  return (
    <AppBar id="app-nav-menu" position="static">
      <Toolbar>
        <Link component={IconButton} color="inherit" to={'/'}>
          <Menu />
        </Link>
        <Typography variant="h5" className={styles.flexSection}>
          Carpentry
        </Typography>
        <Link component={Button} className="nav-button" to={'/decks'}>Decks</Link>
        <Link component={Button} className="nav-button" to={'/inventory'}>Inventory</Link>
        <Link component={Button} className="nav-button" to={'/settings/sets'}>Settings</Link>
      </Toolbar>
    </AppBar>
  )
}

