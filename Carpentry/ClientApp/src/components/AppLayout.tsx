//TODO Review and verify if actually used
import React, { ReactNode } from 'react';

import {
  AppBar,
  Typography,
  Toolbar,
  IconButton,
  Container
} from '@material-ui/core';

import { Add, AddBox, Menu, FilterList } from '@material-ui/icons';

interface LayoutProps {
    children: ReactNode;
    isAddSelected: boolean;
    showAddButton: boolean;
    showFilterButton: boolean;

    // onAddToggle: any;
    // handleMenuClick: any;
    onButtonClick: (type: AppBarButtonType) => void;
}
// const appHeader: JSX.Element = (
//   <div className="app-header app-bar bar-dark">
//       <div className="header-section">
//           {/* <MaterialButton value="" isSelected={this.props.isNavOpen} onClick={this.handleNavClick} icon="menu"  />
//           <AppIcon /> */}
//       </div>
//       <div className="header-section pull-right">
//           {/* <MaterialButton value="data" icon="save" onClick={this.handleSheetToggle} />
//           <MaterialButton value="detail" icon="list" onClick={this.handleSheetToggle} />
//           <MaterialButton value="search" icon="search" onClick={this.handleSheetToggle} />
//           <MaterialButton value="rare" icon="grade" onClick={this.handleSheetToggle} /> */}
//       </div>
//   </div>
// );
export default function AppLayout(props: LayoutProps): JSX.Element {
  console.log('is add selected? ' + props.isAddSelected)
  //flex-col flex-section 
  return(
  <div className="stretch flex-col">
      <AppBar position="static">
          <Toolbar>
            <IconButton color="inherit" onClick={() => {props.onButtonClick("menu")}}>
                <Menu />
            </IconButton>
            <Typography variant="h5" className="flex-section">
                Carpentry
            </Typography>
            {
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
            }
          </Toolbar>
        </AppBar>
      <Container maxWidth="xl" className="flex-section flex-col" style={{overflow:'auto'}}>
        {props.children}
      </Container>
    </div>
  );
}
