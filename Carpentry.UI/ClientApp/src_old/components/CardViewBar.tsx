//TODO Review and verify if actually used
//This component is the blue View/Group/Sort bar on the Deck Editor
import React from 'react'
import { Paper, IconButton } from '@material-ui/core';
import { KeyboardArrowUp, KeyboardArrowDown, ViewModule, ViewHeadline, TextFormat, SignalCellularAlt, Grade } from '@material-ui/icons';
import MaterialButton from './MaterialButton';
// import { KeyboardArrowDown, KeyboardArrowUp} from '@material-ui/icons'
// import { IconButton } from '@material-ui/core';

export interface CardViewBarProps{
    // value: number,
    onViewChange: (newValue: string) => void;
    onGroupChange: (newValue: string) => void;
    onSortChange: (newValue: string) => void;
    // selectedCard: string;
}

export default function CardViewBar(props: CardViewBarProps): JSX.Element {
    return (
        <div className="card-header app-bar">
          {/* <div className="header-section">
                  <label>Card Binder</label>
              </div> */}
          <div className="header-section">
              <label>View:</label>

              <IconButton color="inherit" onClick={() => props.onViewChange("card")}>
                  <ViewModule />
              </IconButton>
              <IconButton color="inherit" onClick={() => props.onViewChange("list")}>
                  <ViewHeadline />
              </IconButton>
              {/* <MaterialButton value="card" isSelected={(this.props.display === "card")} icon="view_module" onClick={this.handleViewChange} />
              <MaterialButton value="list" isSelected={(this.props.display === "list")} icon="view_headline" onClick={this.handleViewChange} /> */}
          </div>
          <div className="header-section">
              <label>Group:</label>
              <IconButton color="inherit" onClick={() => props.onGroupChange("none")}>
                  <ViewModule />
              </IconButton>
              <IconButton color="inherit" onClick={() => props.onGroupChange("spellType")}>
                  <ViewModule />
              </IconButton>
              <IconButton color="inherit" onClick={() => props.onGroupChange("rarity")}>
                  <ViewModule />
              </IconButton>
              {/* <MaterialButton value="none" isSelected={(this.props.groupBy === "none")} icon="view_headline" onClick={this.handleGroupChange} />
              <MaterialButton value="spellType" isSelected={(this.props.groupBy === "spellType")} icon="flash_on" onClick={this.handleGroupChange} />
              <MaterialButton value="rarity" isSelected={(this.props.groupBy === "rarity")} icon="grade" onClick={this.handleGroupChange} /> */}
          </div>
          <div className="header-section">
              <label>Sort:</label>
              <IconButton color="inherit" onClick={() => props.onSortChange("name")}>
                  <TextFormat />
              </IconButton>
              <IconButton color="inherit" onClick={() => props.onSortChange("rarity")}>
                  <Grade />
              </IconButton>
              <IconButton color="inherit" onClick={() => props.onSortChange("manaCost")}>
                  <SignalCellularAlt />
              </IconButton>
              {/* <MaterialButton value="name" isSelected={(this.props.sortBy === "name")} icon="text_format" onClick={this.handleSortChange} />
              <MaterialButton value="rarity" isSelected={(this.props.sortBy === "rarity")} icon="grade" onClick={this.handleSortChange} />
              <MaterialButton value="manaCost" isSelected={(this.props.sortBy === "manaCost")} icon="signal_cellular_alt" onClick={this.handleSortChange} /> */}
          </div>
          {/* <div className="header-section">
              <label>Status:</label>
              <MaterialButton value="clear" isSelected={!this.props.isDeckUpToDate} icon="clear" onClick={this.handleStatusChange} />
              <MaterialButton value="done" isSelected={this.props.isDeckUpToDate} icon="done" onClick={this.handleStatusChange} />
          </div> */}
          {/* {
              (props.selectedCard != null) && selectedCardHeaderSection
          } */}

          <div className="pull-right">
              <MaterialButton value="" isSelected={false} icon="expand_less" onClick={() => { }} />
          </div>
          {/* <label className="pull-right">(icon)</label> */}
          {/* <div className="section-header pull-right">
                  <label>(icon)</label>
              </div> */}
      </div>
    );
}