import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import HomeLayout from './HomeLayout';
import { AppState } from '../configureStore';
import { push } from 'react-router-redux';
import { addMenuButtonClicked } from './state/HomeActions';
import { Menu, MenuItem } from '@material-ui/core';

interface PropsFromState {
    addMenuAnchor: HTMLButtonElement | null;
}

type HomeProps = PropsFromState & DispatchProp<ReduxAction>;

class HomeContainer extends React.Component<HomeProps> {
    constructor(props: HomeProps) {
        super(props);
        this.handleAddClick = this.handleAddClick.bind(this);
        this.handleSettingsClick = this.handleSettingsClick.bind(this);
        this.handleInventoryClick = this.handleInventoryClick.bind(this);
        this.handleAddMenuSelected = this.handleAddMenuSelected.bind(this);
    }

    handleAddClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void{
        this.props.dispatch(addMenuButtonClicked(event.currentTarget));
    }

    handleAddMenuClose(): void {
        this.props.dispatch(addMenuButtonClicked(null));
    }

    handleAddMenuSelected(option: "new" | "import"): void {
        switch(option){
            case "new":
                this.props.dispatch(push('add-deck'));
                break;
            case "import":
                this.props.dispatch(push('import-deck'));
                break;
        }
        this.props.dispatch(addMenuButtonClicked(null));
    }

    handleSettingsClick(): void {
        this.props.dispatch(push('/settings/sets'));
    }

    handleInventoryClick(): void {
        this.props.dispatch(push('/inventory'));
    }

    render() {
        return (
            <React.Fragment>
                <Menu open={Boolean(this.props.addMenuAnchor)} onClose={this.handleAddMenuClose} anchorEl={this.props.addMenuAnchor}>
                    <MenuItem onClick={() => this.handleAddMenuSelected("new")}>New Deck</MenuItem>
                    <MenuItem onClick={() => this.handleAddMenuSelected("import")}>Import Deck</MenuItem>
                </Menu>
                <HomeLayout 
                    onAddClick={this.handleAddClick} 
                    onSettingsClick={this.handleSettingsClick} 
                    onInventoryClick={this.handleInventoryClick} />
            </React.Fragment>
            
        );
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        addMenuAnchor: state.home.cardMenuAnchor,
    }
    return result;
}

export default connect(mapStateToProps)(HomeContainer);