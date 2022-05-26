import React from 'react'
import {Paper, Table, TableHead, TableRow, TableCell, TableBody, IconButton, Button} from '@material-ui/core';
import {Star} from '@material-ui/icons';
import ManaCostChip from "../../../../common/components/ManaCostChip";
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
import {deckEditorCardSelected,selectDeckOverviews} from "../deckEditorSlice";
import {useHistory} from "react-router";

interface ComponentProps {
  deckId: number;
}

export const GroupedDeckCardList = (props: ComponentProps): JSX.Element => {
  function GetAvailabilityColor(availabilityId: number): string {
    switch(availabilityId){
      case 1: return "green";
      case 2: return "yellow";
      case 3: return "orange";
      case 4: return "red";
      default: return "blue";
    }
  }
  
  const dispatch = useAppDispatch();
  const history = useHistory();
  
  const cardDetailsById = useAppSelector(state => state.decks.detail.cardDetails.byId);
  
  const groupedCardOverviews = useAppSelector(selectDeckOverviews);
  
  const onCardSelected = (card: DeckCardOverview): void => {
    dispatch(deckEditorCardSelected(card))
  }

  const onCardDetailClick = (cardId: number): void => {
    history.push(`/decks/${props.deckId}?cardId=${cardId}&show=detail`);
  }

  const onCardTagsClick = (cardId: number): void => {
    history.push(`/decks/${props.deckId}?cardId=${cardId}&show=tags`);
  }

  return (
    <Paper>
      <Table size="small">
        {
          groupedCardOverviews.map(group => (
            <React.Fragment key={group.name}>
              <TableHead key={`th-${group.name}`}>
                <TableRow>
                  <TableCell size="medium"/>
                  <TableCell size="medium" colSpan={4}>{group.name} ({group.cardOverviews.length})</TableCell>
                </TableRow>
              </TableHead>
              <TableBody key={`tb-${group.name}`}>
                {
                  group.cardOverviews.map(cardItem =>
                    <TableRow onClick={() => onCardSelected(cardItem)} onMouseEnter={() => onCardSelected(cardItem)}
                              key={cardItem.id+cardItem.name}>
                      <TableCell>{cardItem.count}</TableCell>
                      <TableCell>{cardItem.name}</TableCell>
                      <TableCell>
                        {
                          Boolean(cardItem.tags.length) ?
                            <Button variant="outlined" style={{textTransform:"none"}} onClick={()=>{onCardTagsClick(cardItem.cardId)}} >
                              {cardItem.tags.toString()}
                            </Button>
                            :
                            <Button style={{textTransform:"none"}} onClick={()=>{onCardTagsClick(cardItem.cardId)}}>
                              untagged
                            </Button>
                        }
                      </TableCell>
                      <TableCell><ManaCostChip costString={cardItem.cost} /></TableCell>
                      <TableCell>
                        <Button color="inherit" onClick={()=>{onCardDetailClick(cardItem.cardId)}} >
                          {
                            cardItem.detailIds.map(id => {
                              const cardDetail = cardDetailsById[id];
                              let color = GetAvailabilityColor(cardDetail.availabilityId);
                              return(<Star style={{color: color}} key={id} />);
                            })
                          }
                        </Button>
                      </TableCell>
                    </TableRow>
                  )
                }
              </TableBody>
            </React.Fragment>
          ))
        }
      </Table>
    </Paper>
  );
}

