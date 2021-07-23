import React from 'react';
import { Button, Container, Menu } from 'semantic-ui-react';
import { useStore } from '../stores/store';

const NavBar = () => {

    const {activityStore} = useStore()
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header>
                    <img style={{marginRight:'10px'}} src="/assets/logo.png" alt="logo"/>
                    Reactivities
                </Menu.Item>
                <Menu.Item name="Activities">
                </Menu.Item>
                <Menu.Item header>
                    <Button onClick={() => activityStore.openForm()} positive content='Create Activity'/>
                </Menu.Item>
            </Container>
        </Menu>
    )
}

export default NavBar
