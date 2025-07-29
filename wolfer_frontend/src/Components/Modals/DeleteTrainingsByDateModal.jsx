import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import './DeleteModal.css';

const DeleteTrainingsByDateModal = ({deleteTrainingsByDateModalIsOpen, closeDeleteModal, handleDeleteAll, date}) => {

  const dateOnly = date.toISOString().split("T")[0];

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={deleteTrainingsByDateModalIsOpen}
            contentLabel="Training Deletion"
            ariaHideApp={false}
          >
            <h2>DELETE ALL TRAININGS FOR DATE: {dateOnly}</h2>
            <button className="modal-btn btn btn-danger" onClick={(e) => handleDeleteAll(e)}>Delete</button>
            <button className="modal-btn btn btn-secondary" onClick={() => closeDeleteModal()}>Cancel</button>
          </Modal>
        </div>
      );
}

export default DeleteTrainingsByDateModal;