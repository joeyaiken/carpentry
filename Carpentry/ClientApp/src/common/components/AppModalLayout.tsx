//This is the layout that represents the contents of a modal
import React, { ReactNode } from 'react'
import { Container, CardHeader, IconButton, Divider, CardContent, CardActions, Button, Card, Box } from '@material-ui/core';
import { Close } from '@material-ui/icons';

export interface ComponentProps {
    title: string;
    children: ReactNode;
    onSaveClick?: () => void;
    onCloseClick: () => void;
    onDeleteClick?: () => void;
}

export default function AppModalLayout(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Container maxWidth="sm" className="stretch">
                <Card className="flex-col" style={{maxHeight:"95%", margin:"10px"}} >
                    {/*  className="flex-col" */}
                    <CardHeader
                        
                        title={props.title}
                        action={
                            <IconButton size="medium" onClick={props.onCloseClick}><Close /></IconButton>
                        }
                    />
                    <Divider/>
                    <CardContent style={{overflow:"auto"}} className="flex-section flex-col">
                        {props.children}
                    </CardContent>
                    <Divider/>
                    <CardActions style={{justifyContent:"space-between"}}>
                        <Box>
                            {
                                props.onDeleteClick && 
                                <Button size="medium" variant="contained" color="secondary" onClick={props.onDeleteClick}>Delete</Button>
                            }
                        </Box>
                        <Box className="pull-right">
                            <Button size="medium" onClick={props.onCloseClick}>Cancel</Button>
                            <Button size="medium" variant="contained" color="primary" onClick={props.onSaveClick}>Save</Button>
                        </Box>
                    </CardActions>
                </Card>
            </Container>
        </React.Fragment>
    )
}
