import { Card, CardMedia, Popper } from '@material-ui/core';
import React from 'react'

interface CardImagePopperProps { 
  menuAnchor: HTMLElement | null;
  image: string;
  onClose: () => void;
}

export const CardImagePopper = (props: CardImagePopperProps): JSX.Element => {
  return (
    <Popper style={{zIndex:1500}} open={Boolean(props.menuAnchor)} anchorEl={props.menuAnchor}>
      <Card>
        <CardMedia style={{height:"310px", width: "223px"}} image={props.image} />
      </Card>
    </Popper>
  );
}