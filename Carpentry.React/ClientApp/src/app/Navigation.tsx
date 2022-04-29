import React, {ReactNode} from "react"
import {AppBar, Button, IconButton, Toolbar, Typography} from "@material-ui/core";
import {Menu} from "@material-ui/icons";
import styles from './App.module.css'
import {Link} from "react-router-dom";

interface WrappedButtonProps {
  children: ReactNode;
  href: string;
  navigate: any;
}

export const Navigation = (): JSX.Element => {
  const WrappedButton = (props: WrappedButtonProps): JSX.Element => {
    return (<Button className="nav-button" color="inherit">{props.children}</Button>)
  }
  
  const WrappedIconButton = (props: WrappedButtonProps): JSX.Element => {
    return (<IconButton color="inherit">{props.children}</IconButton>)
  }

  return (
    <AppBar id="app-nav-menu" position="static">
      <Toolbar>
        <Link component={WrappedIconButton} to={'/'}><Menu /></Link>
        <Typography variant="h5" className={styles.flexSection}>
          Carpentry
        </Typography>
        <Link component={WrappedButton} to={'/decks'}>Decks</Link>
        <Link component={WrappedButton} to={'/inventory'}>Inventory</Link>
        <Link component={WrappedButton} to={'/settings/sets'}>Settings</Link>
      </Toolbar>
    </AppBar>
  )
}

