//TODO Review and verify if actually used
import React from 'react';
import { CardHeader, IconButton, Menu, MenuItem, CardMedia, Card, Typography, CardContent } from '@material-ui/core';
import { MoreVert } from '@material-ui/icons';

export interface MagicCardProps {
    card: Card;
    data: MagicCardDto;

    //card: MagicCardDto;
    // cardIsSelected: boolean;
    // display: string | null; // card | list
    onMenuClick: any;
}

export default function DeckEditorCard(props: MagicCardProps): JSX.Element {
    
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

    // if (!props.card) {
    //     return (
    //         <div className="outline-section">
    //             <Typography>No card data</Typography>
    //         </div>
    //     );
    // }

    if(props.data.name == "Abundance"){
        console.log(props.data)
    }
    return(
        <Card>
            <CardHeader 
                titleTypographyProps={{variant:"body1"}} 
                subheaderTypographyProps={{variant:"body2"}} 
                title={props.data.name} 
                subheader={`${props.data.type}${ props.data.cmc && (' - '+props.data.cmc) }`}
                action={
                        <>
                            <IconButton
                                name={props.data.name}
                                value={props.card.id}
                                onClick={props.onMenuClick}>
                                <MoreVert />
                            </IconButton>
                            {/* <Menu
                                open={false}
                                // open={props.isCardMenuOpen}
                            >
                                <MenuItem>Move to Sideboard</MenuItem>
                                <MenuItem>Remove from Deck</MenuItem>
                                <MenuItem>Delete from Inventory</MenuItem>
                            </Menu> */}
                        </>
                    }
                />
                {/* { props.data.imageUrl != null && (<CardMedia
                    className="item-image"
                    // image={props.data.imageUrl}
                    image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.multiverseId}&type=card`}
                    title={props.data.name}
                />) } */}
                <CardMedia
                    style={{height:"310px",width:"223px"}}
                    className="item-image"
                    // image={props.data.imageUrl}
                    image={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.multiverseId}&type=card`}
                    title={props.data.name}
                />
                <CardContent>
                    { (props.card.isFoil) && (<Typography>${props.data.priceFoil}</Typography>) }
                    { (!props.card.isFoil) && (<Typography>${props.data.price}</Typography>) }
                </CardContent>
            {/* <div className={props.cardIsSelected ? "magic-card selected-card" : "magic-card"} onClick={props.onClick}>
                <img src={`https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=${props.card.multiverseId}&type=card`} />
                <div>
                    <span>[ {props.card.set} ]</span>
                    <span>[ {props.card.colorIdentity} ]</span>
                </div>
            </div> */}
        </Card>);
}
