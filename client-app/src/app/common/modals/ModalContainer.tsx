import { observer } from 'mobx-react-lite';
import React from 'react'
import { Modal } from 'semantic-ui-react';
import { useStore } from './../../stores/store';

const ModalContainer = () => {

    const {modalStore} = useStore();
    return (
        <Modal open={modalStore.modal.open} onClose={modalStore.closeModal} size='mini' >
            {modalStore.modal.body}
        </Modal>
    )
}

export default observer(ModalContainer)
