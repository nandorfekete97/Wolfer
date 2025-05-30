import React, { useState } from 'react';
import Modal from 'react-modal';
import './DeleteModal.css';

const DeleteModal = ({deleteModalIsOpen, closeDeleteModal, handleDelete}) => {

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={deleteModalIsOpen}
            contentLabel="Training Deletion"
            ariaHideApp={false}
          >
            <h2>Are you sure you want to delete this training?</h2>
            <button className="modal-btn btn btn-danger" onClick={(e) => handleDelete(e)}>Delete</button>
            <button className="modal-btn btn btn-secondary" onClick={() => closeDeleteModal()}>Cancel</button>
          </Modal>
        </div>
      );
}

export default DeleteModal;