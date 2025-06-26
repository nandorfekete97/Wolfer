import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import './ResponseModal.css';

const ResponseMessageModal = ({ responseMessageModalIsOpen, closeResponseMessageModal, responseMessage }) => { 

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={responseMessageModalIsOpen}
            contentLabel="Response Message"
            ariaHideApp={false}
          >
            {responseMessage && (
                <div className = "response-message mt-2 text-info col-12">
                <h2>{responseMessage}</h2>
                </div>
            )}
            <button className="modal-btn btn btn-secondary" onClick={() => closeResponseMessageModal()}>OK</button>
          </Modal>
        </div>
      );
}

export default ResponseMessageModal;