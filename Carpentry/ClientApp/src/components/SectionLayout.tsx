import React, { ReactNode } from 'react';

import {
  AppBar,
  Typography,
  Toolbar,
  Tabs,
  Tab,
  Box
} from '@material-ui/core';

interface LayoutProps {
    children: ReactNode;
    title: string;
    activeTab?: string;
    tabNames?: string[];
    onTabClick?: (tabName: string) => void;
}

export default function SectionLayout(props: LayoutProps): JSX.Element {
    return(
        <Box className="flex-col">
            <AppBar color="default" position="relative">
                <Toolbar>
                    <Typography variant="h6">
                        {props.title}
                    </Typography>
                    {
                        props.tabNames &&
                        <Tabs value={props.activeTab} onChange={(e, value) => {props.onTabClick && props.onTabClick(value)}} >
                            {
                                props.tabNames.map(tabName =><Tab value={tabName} label={tabName} /> )
                            }
                        </Tabs>
                    }
                </Toolbar>
            </AppBar>
            <Box>
                {props.children}
            </Box>
        </Box>
    );
}