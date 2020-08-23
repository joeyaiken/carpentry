import React from 'react';
import {
    Typography,
    Box,
    Card,
    CardHeader,
    CardContent,
    IconButton,
    Button,
} from '@material-ui/core';
import DeckList from '../DeckList/DeckListContainer';
import { Link } from 'react-router-dom';
import { ArrowForward } from '@material-ui/icons';
import { appStyles } from '../../styles/appStyles';

interface LayoutProps {

}

export default function HomeLayout(props: LayoutProps): JSX.Element {
    const { stretch, flexCol, flexSection, flexRow } = appStyles();
    return (
        <Box>
            <Typography variant="h4">
                Carpentry
            </Typography>
            {/* <i className="ms ms-g ms-cost"></i><i className="ms ms-gw ms-cost"></i><i className="ms ms-2g ms-cost"></i> */}
            <Typography variant="h6">
                A deck & inventory management tool for Magic the Gathering
            </Typography>
            <br/>
            <Box className={flexRow}>
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Inventory"}
                        action={
                            <Link to={'/inventory'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                    {/* <CardContent></CardContent> */}
                </Card>
                
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Tracked Sets"}
                        action={
                            <Link to={'/settings/sets'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                    {/* <CardContent></CardContent> */}
                </Card>

                {/* <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Settings"}
                        action={
                            <Link to={'/settings'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                </Card> */}
                <Card>
                    <CardHeader
                        titleTypographyProps={{variant:"h5"}}
                        title={"Backups"}
                        action={
                            <Link to={'/settings/backups/'}>
                                <IconButton size="medium"><ArrowForward /></IconButton>
                            </Link>
                        }
                    />
                </Card>
                {/* <CardHeader
                        
                        title={props.title}
                        action={
                            <IconButton size="medium" onClick={props.onCloseClick}><Close /></IconButton>
                        }
                    /> */}

            </Box>
            
            <br/>
            <Card>
                <CardHeader
                    titleTypographyProps={{variant:"h5"}}
                    title={"Available Decks"} 
                    action={
                        <Box>
                            <Button>Import</Button>
                            <Button>New</Button>
                        </Box>
                    }
                />
                <DeckList />
            </Card>
        </Box>
    );
}
