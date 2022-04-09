import { Box, Button, IconButton, TextField, Typography } from '@material-ui/core';
import { Add, Close } from '@material-ui/icons';
import React, { ChangeEvent } from 'react';
import { appStyles } from '../../../styles/appStyles';

interface ContainerLayoutProps {
    selectedCardName: string;
    newTagName: string;
    existingTags: CardTagDetailTag[];
    tagSuggestions: string[];
    
    onNewTagChange: (value: string) => void;
    onAddTagButtonClick: () => void;
    onRemoveTagClick: (tagId: number) => void;
    onAddSuggestionClick: (cardName: string) => void;
}

export default function CardTagsLayout(props: ContainerLayoutProps): JSX.Element {
    const { outlineSection, flexCol, flexRow, flexSection, staticSection, scrollSection,  } = appStyles();
    return(
    <React.Fragment>
        
        <Box className={outlineSection}>
            <Typography>{props.selectedCardName}</Typography>
        </Box>

        <Box className={outlineSection}>
            <Typography>Existing Tags</Typography>
            { !Boolean(props.existingTags) && <Box className={outlineSection}>no existing tags</Box> }
            {
                Boolean(props.existingTags) && 
                props.existingTags.map(tag => {
                    return(<Box className={outlineSection}>{tag.tag}
                    <IconButton color="inherit" size="small" onClick={()=> props.onRemoveTagClick(tag.cardTagId) }>
                        <Close />
                    </IconButton>
                    </Box>)
                })
            }
        </Box>
        
        <Box className={outlineSection}>
            <Typography>Tag Suggestions</Typography>
            { !Boolean(props.tagSuggestions) && <Box className={outlineSection}>no tag suggestions</Box> }
            {
                Boolean(props.tagSuggestions) && 
                props.tagSuggestions.map(tag => {
                    return(<Box className={outlineSection}>{tag}
                    
                    <IconButton color="inherit" size="small" onClick={()=> props.onAddSuggestionClick(tag) }>
                        <Add />
                    </IconButton>
                    </Box>)
                })
            }
        </Box>

        <Box className={outlineSection}>
            <TextField
                name="New Tag"
                label="New Tag"
                value={props.newTagName}
                onChange={(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => props.onNewTagChange(event.target.value)} />
            <Button color={"primary"} variant={"contained"} onClick={props.onAddTagButtonClick}>
                Add
            </Button>
     
        </Box>

    </React.Fragment>);
}