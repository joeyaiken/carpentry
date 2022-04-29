import React, {useEffect} from "react";
import {loadDeckOverviews} from "../deckOverviewsSlice";
import {Avatar, Table, TableBody, TableCell, TableHead, TableRow} from "@material-ui/core";
import {Link} from "react-router-dom";
import {useAppDispatch, useAppSelector} from "../../../app/hooks";
import {RootState} from "../../../app/store";

const DeckListRow = (props: {deckId: number}): JSX.Element => {
  const deck: DeckOverviewDto = useAppSelector((state: RootState) =>
    state.decks.overviews.decksById[props.deckId]);

  const manaIcon = (type: string): JSX.Element => {
    return(<Avatar key={type} style={{height:"24px", width:"24px", display:"inline-flex", verticalAlign: "middle"}} src={`/img/${type}.svg`} />)
  }

  return (
    <TableRow key={deck.id}>
      <TableCell>
        <Link to={`/decks/${deck.id}`}>{deck.name}</Link>
      </TableCell>
      <TableCell>{deck.format}</TableCell>
      <TableCell>
        { deck.colors.map(color => manaIcon(color)) }
      </TableCell>
      <TableCell>{deck.validationIssues}</TableCell>
    </TableRow>
  )
}

export const DeckList = (): JSX.Element => {
  
  const dispatch = useAppDispatch();
  
  const shouldLoad = useAppSelector((state) => 
    !state.decks.overviews.isLoading && !state.decks.overviews.isInitialized);
  
  const deckIds: number[] = useAppSelector((state) => state.decks.overviews.deckIds);
  
  useEffect(() => {
    if(shouldLoad) dispatch(loadDeckOverviews());
  });

  return (
    <React.Fragment>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>Format</TableCell>
            <TableCell>Colors</TableCell>
            <TableCell>Validity</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          { deckIds.map(id => <DeckListRow key={id} deckId={id} />) }
        </TableBody>
      </Table>
    </React.Fragment>
  )
}