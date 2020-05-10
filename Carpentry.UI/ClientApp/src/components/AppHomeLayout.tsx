import React from 'react';

import {
//   AppBar,
  Typography,
//   Toolbar,
//   IconButton,
//   Container,
//   Tabs,
//   Tab,
  Box
} from '@material-ui/core';
import DeckList from '../containers/DeckList';
// import { Add, AddBox, Menu, FilterList } from '@material-ui/icons';
// import { Link, Route } from 'react-router-dom';

interface LayoutProps {
    // children: ReactNode;
    // isAddSelected: boolean;
    // showAddButton: boolean;
    // showFilterButton: boolean;

    // onAddToggle: any;
    // handleMenuClick: any;
    // onButtonClick: (type: AppBarButtonType) => void;
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
export default function AppHomeLayout(props: LayoutProps): JSX.Element {
    return(
        <Box>
            {/* <Typography variant="h5">
                Carpentry - A deck & inventory management tool for Magic the Gathering
            </Typography> */}
            <Typography variant="h4">
                Carpentry
            </Typography>
            <Typography variant="h6">
                A deck & inventory management tool for Magic the Gathering
            </Typography>
            <br/>
            <Typography variant="h5">
                Available Decks
            </Typography>
            <DeckList />
        </Box>
    );
}
