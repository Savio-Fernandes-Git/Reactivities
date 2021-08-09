import React from 'react';
import { NavLink } from 'react-router-dom';
import { Button, Container, Menu } from 'semantic-ui-react';

const NavBar = () => {
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/' exact header>
                    <img style={{marginRight:'10px'}} src="/assets/logo.png" alt="logo"/>
                    Reactivities
                </Menu.Item>
                <Menu.Item as={NavLink} to='/activities' name="Activities">
                </Menu.Item>
                <Menu.Item as={NavLink} to='/errors' name="Errors">
                </Menu.Item>
                <Menu.Item header>
                    <Button as={NavLink} to='/createActivity' positive content='Create Activity'/>
                </Menu.Item>
            </Container>
        </Menu>
    )
}

export default NavBar
