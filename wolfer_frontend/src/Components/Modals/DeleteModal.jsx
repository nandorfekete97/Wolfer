import React, { useState } from 'react';
import Modal from 'react-modal';

const DeleteModal = ({deleteModalIsOpen, closeDeleteModal, handleDelete}) => {

  return (
        <div>
          <Modal
            isOpen={deleteModalIsOpen}
            contentLabel="Training Deletion"
            ariaHideApp={false}
          >
            <h2>Are you sure you want to delete this training?</h2>
            <button onClick={(e) => handleDelete(e)}>Delete</button>
            <button onClick={() => closeDeleteModal()}>Cancel</button>
          </Modal>
        </div>
      );
}

export default DeleteModal;