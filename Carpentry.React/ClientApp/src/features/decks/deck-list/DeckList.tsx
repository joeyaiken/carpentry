import React, {useEffect} from "react";
import {useDispatch, useSelector} from "react-redux";
import {loadDeckOverviews} from "../decksDataSlice";
import {Avatar, Table, TableBody, TableCell, TableHead, TableRow} from "@material-ui/core";
import {Link} from "react-router-dom";
import {RootState} from "../../../app/store";

const DeckListRow = (props: {deckId: number}) => {
  const deck: DeckOverviewDto = useSelector((state: RootState) =>
    state.decks.data.overviews.decksById[props.deckId]);

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
        {
          deck.colors.map(color => manaIcon(color))
        }
      </TableCell>
      <TableCell>{deck.validationIssues}</TableCell>
    </TableRow>
  )
}

export const DeckList = (): JSX.Element => {
  const dispatch = useDispatch();
  const shouldLoad = useSelector((state: RootState) => 
    !state.decks.data.overviews.isLoading && !state.decks.data.overviews.isInitialized);

  const deckIds: number[] = useSelector((state: RootState) => state.decks.data.overviews.deckIds);
  
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