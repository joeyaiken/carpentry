import React, { ReactNode, Fragment } from 'react';

import {
  AppBar,
  Typography,
  Toolbar,
  IconButton,
  Container,
  Tabs,
  Tab
} from '@material-ui/core';

import { Add, AddBox, Menu, FilterList } from '@material-ui/icons';
import { Link, NavLink, Route, Switch } from 'react-router-dom';
import { ConnectedComponent } from 'react-redux';
import { appStyles, combineStyles } from '../styles/appStyles';

interface LayoutProps {
    children: ReactNode;

    routes: {
        path: string,
        component: ConnectedComponent<any, any>,
        name: string;
    }[];
    // isAddSelected: boolean;
    // showAddButton: boolean;
    // showFilterButton: boolean;

    // onAddToggle: any;
    // handleMenuClick: any;
    // onButtonClick: (type: AppBarButtonType) => void;
}
// const appHeader: JSX.Element = (
//   <div className= "app-header app-bar bar-dark">
//       <div className= "header-section">
//           {/* <MaterialButton value="" isSelected={this.props.isNavOpen} onClick={this.handleNavClick} icon="menu"  />
//           <AppIcon /> */}
//       </div>
//       <div className= "header-section pull-right">
//           {/* <MaterialButton value="data" icon="save" onClick={this.handleSheetToggle} />
//           <MaterialButton value="detail" icon="list" onClick={this.handleSheetToggle} />
//           <MaterialButton value="search" icon="search" onClick={this.handleSheetToggle} />
//           <MaterialButton value="rare" icon="grade" onClick={this.handleSheetToggle} /> */}
//       </div>
//   </div>
// );
export default function AppLayout(props: LayoutProps): JSX.Element {
    const { stretch, flexCol, flexSection } = appStyles();
//   console.log('is add selected? ' + props.isAddSelected)
  //flex-col flex-section 
  return(
  <div className={combineStyles(stretch, flexCol)}>
      <AppBar position="static">
          <Toolbar>
            <IconButton color="inherit" component={Link} to={'/'} >
                <Menu />
            </IconButton>
            
            <Typography variant="h5" className={flexSection}>
                {<Switch>{
                        props.routes.map(route => <Route path={route.path}>{route.name}</Route>)
                }</Switch>}
            </Typography>
            {/* <Container> */}
                <NavLink
                    to="/decks">
                    Decks
                </NavLink>
                <NavLink
                    to="/inventory">
                    Inventory
                </NavLink>

            {/* </Container> */}
            
        
            {/* <Route
                path='/'
                render={
                    ({location}) => (
                        <Fragment>
                            <Tabs value={location.pathname}>
                                <Tab value='/inventory' label='Inventory' component={Link} to={'/inventory'} />
                                <Tab value='/decks' label='Decks' component={Link} to={'/decks'} />
                                <Tab value='/decks/1' label='Active Deck' component={Link} to={'/decks/1'} />
                            </Tabs>
                        </Fragment>
                    )
                }
            /> */}

            {/* {
            <Typography variant="h5" className= "flex-section">
                Inventory
            </Typography>
            
            <Typography variant="h5" className= "flex-section">
                Decks
            </Typography>

            <Typography variant="h6" className= "flex-section">
                A Deck Name
            </Typography>
            
                props.showFilterButton &&
                <IconButton color="inherit" onClick={() => {props.onButtonClick("filter")}} >
                    {props.isAddSelected ? (<FilterList />) : (<FilterList />)}
                </IconButton>
            }
            {
                props.showAddButton &&
                <IconButton color="inherit" onClick={() => {props.onButtonClick("add")}} >
                    {props.isAddSelected ? (<AddBox />) : (<Add />)}
                </IconButton>
            } */}
          </Toolbar>
        </AppBar>
        <Container maxWidth="xl" className={combineStyles(flexSection, flexCol)} style={{overflow:'auto'}}>
            {props.children}
        </Container>
    </div>
  );
}
