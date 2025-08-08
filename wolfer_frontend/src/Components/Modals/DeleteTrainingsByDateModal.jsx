import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import './DeleteModal.css';

const DeleteTrainingsByDateModal = ({deleteTrainingsByDateModalIsOpen, closeDeleteModal, handleDeleteAll, formattedDate}) => {

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={deleteTrainingsByDateModalIsOpen}
            contentLabel="Training Deletion"
            ariaHideApp={false}
          >
            <h2>DELETE all trainings for date: <br></br> {formattedDate}</h2>
            <button className="modal-btn btn btn-danger" onClick={(e) => handleDeleteAll(e)}>DELETE</button>
            <button className="modal-btn btn btn-secondary" onClick={() => closeDeleteModal()}>CANCEL</button>
          </Modal>
        </div>
      );
}

export default DeleteTrainingsByDateModal;