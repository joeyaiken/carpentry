import React, { ReactNode } from 'react'
import { Modal } from '@material-ui/core';
import AppModalLayout from './AppModalLayout';

export interface ComponentProps {
    title: string;
    children: ReactNode;
    isOpen: boolean;
    onSaveClick?: () => void;
    onCloseClick: () => void;
    onDeleteClick?: () => void;
}

export default function AppModal(props: ComponentProps): JSX.Element {
    return(
        <React.Fragment>
            <Modal open={props.isOpen}>
                <AppModalLayout
                    onCloseClick={props.onCloseClick}
                    onSaveClick={props.onSaveClick}
                    onDeleteClick={props.onDeleteClick}
                    title={props.title}>
                        {props.children}
                </AppModalLayout>
            </Modal>
        </React.Fragment>
    )
}