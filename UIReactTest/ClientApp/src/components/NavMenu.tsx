import * as React from 'react';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import './NavMenu.css';

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    public render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand>Test App - React</NavbarBrand>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
