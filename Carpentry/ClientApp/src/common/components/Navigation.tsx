import React from "react"
import {AppBar, Button, IconButton, Toolbar, Typography} from "@material-ui/core";
import {Menu} from "@material-ui/icons";
import styles from '../../App.module.css'
import {useHistory} from "react-router";

export const Navigation = (): JSX.Element => {
  const history =  useHistory();
  const Navigate = (route: string): void => {
    history.push(route);
  }
  return (
    <AppBar id="app-nav-menu" position="static">
      <Toolbar>
        <IconButton onClick={() => Navigate('/')} color="inherit"><Menu /></IconButton>
        <Typography variant="h5" className={styles.flexSection}>
          Carpentry
        </Typography>
        <Button onClick={() => Navigate('/decks')} className="nav-button" color="inherit">Decks</Button>
        <Button onClick={() => Navigate('/inventory')} className="nav-button" color="inherit">Inventory</Button>
        <Button onClick={() => Navigate('/settings')} className="nav-button" color="inherit">Settings</Button>
      </Toolbar>
    </AppBar>
  )
}