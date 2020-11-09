//TODO Review and verify if actually used
import React, { ReactNode } from 'react';

import {
    AppBar,
    Typography,
    Toolbar,
    IconButton,
    Paper,
    Box,
    TextField,
    MenuItem,
    ExpansionPanel,
    ExpansionPanelSummary,
    ExpansionPanelDetails,
    Divider,
    ExpansionPanelActions,
    Button,
    FormControl,
    InputLabel,
    Input,
    InputAdornment,
    Chip,
    Avatar,
    Card,
    CardHeader,
    CardContent,
    Menu,
    CardMedia
} from '@material-ui/core';

import { ExpandMore, Edit, MoreVert } from '@material-ui/icons';

// import {
//   Add, AddBox, Menu
// } from '@material-ui/icons';

interface LayoutProps {
//   children: ReactNode;
//   onAddToggle: any;
//   isAddSelected: boolean;
    options: DeckViewOptions;
    dto: DeckDto;
    collections: CardGroup[];

    // isCardMenuOpen: boolean;
    // cardMenuAnchorElement: HTMLElement | null;
    // handleCardMenuClick: (event: React.MouseEvent<HTMLButtonElement>) => void;
    
}



export default function GroupEditorLayout(props: LayoutProps): JSX.Element {

    //Eventually all of these should be individual components that are loaded from App.tsx

    const renderViewBar = (): JSX.Element => {
        return (<Paper className="flex-row">
            <Box className="static-section side-padded">
                <TextField
                    name="view"
                    select
                    label="View"
                    margin="normal"
                    value={props.options.view}
                    // onChange
                    >
                        <MenuItem key="img" value="img" >Image</MenuItem>
                        <MenuItem key="list" value="list" >List</MenuItem>
                    </TextField>
            </Box>

            <Box className="static-section side-padded">
                <TextField
                    name="group"
                    select
                    label="Group"
                    margin="normal"
                    value={props.options.group}
                    // onChange
                    >
                        <MenuItem key="None" value="None">None</MenuItem>
                        <MenuItem key="Type" value="Type">Type</MenuItem>
                        <MenuItem key="Rarity" value="Rarity">Rarity</MenuItem>
                    </TextField>
            </Box>

            <Box className="flex-section side-padded">
                <TextField
                    name="sort"
                    select
                    label="Sort"
                    margin="normal"
                    value={props.options.sort}
                    // onChange
                    >
                        <MenuItem key="Name" value="Name">Name</MenuItem>
                        <MenuItem key="Rarity" value="Rarity">Rarity</MenuItem>
                        <MenuItem key="ManaCost" value="ManaCost">ManaCost</MenuItem>
                    </TextField>
            </Box>
        </Paper>);
    }

    const renderPropertiesBar = (): JSX.Element => {
        return (
            <ExpansionPanel className="">
                <ExpansionPanelSummary
                    expandIcon={<Edit />}
                    aria-controls="panel1c-content"
                    id="panel1c-header">
                    <div className="flex-section flex-25">
                        <Typography>{props.dto.props.name}</Typography>
                    </div>
                    <div className="flex-section flex-25">
                        <Typography>{props.dto.props.notes}</Typography>
                    </div>
                    <div className="flex-section flex-25">
                        <Typography>{props.dto.props.format}</Typography>
                    </div>
                    <div className="flex-section flex-25">
                        {/* <Chip avatar={<Avatar src="/static/images/avatar/1.jpg" />} /> */}
                        <Chip size="small" avatar={<Avatar>W</Avatar>} label={ props.dto.props.basicW }/>
                        <Chip size="small" avatar={<Avatar>U</Avatar>} label={ props.dto.props.basicU }/>
                        <Chip size="small" avatar={<Avatar>B</Avatar>} label={ props.dto.props.basicB }/>
                        <Chip size="small" avatar={<Avatar>R</Avatar>} label={ props.dto.props.basicR }/>
                        <Chip size="small" avatar={<Avatar>G</Avatar>} label={ props.dto.props.basicG }/>
                    </div>
                </ExpansionPanelSummary>
                <Divider />
                <ExpansionPanelDetails>
                    <div className="flex-section flex-25">
                        <TextField 
                            label="Name"
                            color="primary"
                            name="name"
                            value={props.dto.props.name} />
                    </div>
                    <div className="flex-section flex-25">
                        <TextField 
                            label="Notes"
                            color="primary"
                            name="notes"
                            value={props.dto.props.notes} />
                    </div>
                    <div className="flex-section flex-25">
                        <TextField 
                            label="Format"
                            color="primary"
                            name="format"
                            value={props.dto.props.format} />
                    </div>
                    <div className="flex-section flex-25 flex-row">
                        {/* <Typography>Basics</Typography> */}
                        {/* I may want a series of InputFields in a bigger container rather than 5 TextFields */}

                        <TextField
                            name="basicW"
                            type="number"
                            value={props.dto.props.basicW}
                            InputProps={{
                                startAdornment: <InputAdornment position="start">W</InputAdornment>,
                            }} 
                        />
                        <TextField
                            name="basicU"
                            type="number"
                            value={props.dto.props.basicU}
                            InputProps={{
                                startAdornment: <InputAdornment position="start">U</InputAdornment>,
                            }} 
                        />
                        <TextField
                            name="basicB"
                            type="number"
                            value={props.dto.props.basicB}
                            InputProps={{
                                startAdornment: <InputAdornment position="start">B</InputAdornment>,
                            }} 
                        />
                        <TextField
                            name="basicR"
                            type="number"
                            value={props.dto.props.basicR}
                            InputProps={{
                                startAdornment: <InputAdornment position="start">R</InputAdornment>,
                            }} 
                        />
                        <TextField
                            name="basicG"
                            type="number"
                            value={props.dto.props.basicG}
                            InputProps={{
                                startAdornment: <InputAdornment position="start">G</InputAdornment>,
                            }} 
                        />
                    </div>
                </ExpansionPanelDetails>
                <Divider />
                <ExpansionPanelActions>
                    <Button size="small">Cancel</Button>
                    <Button size="small" color="primary">
                        Save
                    </Button>
                </ExpansionPanelActions>
            </ExpansionPanel>
        );
        
    }

    const renderGroupSection = (collection: any): JSX.Element => {
        return (
            <ExpansionPanel defaultExpanded>
                <ExpansionPanelSummary
                    expandIcon={<ExpandMore />}
                    aria-controls="panel1c-content"
                    id="panel1c-header">
                    <div className="flex-section">
                        <Typography>{collection.name} <Chip size="small" label={ collection.items.length }/></Typography> 
                    </div>
                </ExpansionPanelSummary>
                {/* <Divider />
                <ExpansionPanelActions>
                    <Button size="small">Cancel</Button>
                    <Button size="small" color="primary">Save</Button>
                </ExpansionPanelActions> */}
                <Divider />
                <ExpansionPanelDetails>
                    { 
                        collection.cards.map((item) => {
                            return renderGroupItem(item)
                        })
                    }
                </ExpansionPanelDetails>
                
                
            </ExpansionPanel>
        );
    }

    //this is the card btn
     //onClick={props.handleCardMenuClick} >
    const renderGroupItem = (item: any): JSX.Element => {
        return(
            <Card>
                <CardHeader 
                    titleTypographyProps={{variant:"body1"}} 
                    subheaderTypographyProps={{variant:"body2"}} 
                    title={item.name} 
                    subheader={`${item.type}${ item.cost > 0 && (' - '+item.cost) }`}
                    action={
                        <>
                            <IconButton>
                                <MoreVert />
                            </IconButton>
                            <Menu
                                open={false}
                                // open={props.isCardMenuOpen}
                            >
                                <MenuItem>Move to Sideboard</MenuItem>
                                <MenuItem>Remove from Deck</MenuItem>
                                <MenuItem>Delete from Inventory</MenuItem>
                            </Menu>
                        </>
                    }
                />
                { item.img != null && (<CardMedia
                    className="item-image"
                    image={item.img}
                    title={item.name}
                />) }
                {/* <CardMedia
                    className={classes.media}
                    image="/static/images/cards/paella.jpg"
                    title="Paella dish"
                /> */}

                {/* <CardContent>
                    <Typography>{item.type}</Typography>
                    <Typography>{item.cost}</Typography>
                </CardContent> */}
            </Card>
        )
    }

    const renderQuickStatsBar = (): JSX.Element => {
        return(<Paper className="flex-row">
            <Paper className="flex-section outline-section">
                <Typography>Count / Limit</Typography>
            </Paper>
            <Paper className="flex-section outline-section">
                <Typography>Type breakdown</Typography>
            </Paper>
            <Paper className="flex-section outline-section">
                <Typography>CMC breakdown</Typography>
            </Paper>
            <Paper className="flex-section outline-section">
                <Typography>Color Breakdown</Typography>
                {/* Goal: Determine if the current mana base is appropriate for the current color breakdown */}
                {/* Like: What % of the cards need blue / what % of lands can produce blue */}
            </Paper>
            <Paper className="flex-section outline-section">
                <Typography>Is Valid?</Typography>
            </Paper>
            {/* Could do a rarity breakdown but that wouldn't  */}
        </Paper>);
    }

    return(
        <div className="flex-col">
            <AppBar color="default" position="relative">
                <Toolbar>
                    <Typography variant="h5">
                        Group Editor
                    </Typography>
                </Toolbar>
            </AppBar>
            <div>
                {renderViewBar()}
                {renderPropertiesBar()}
                {props.collections.map((collection) => renderGroupSection(collection))}
                {renderQuickStatsBar()}
            </div>
        </div>
    );
}
