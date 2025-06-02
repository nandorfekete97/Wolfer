import React, { useEffect, useState } from 'react';
import { use } from 'react';
import Modal from 'react-modal';
//import './EditTrainingModal.css';

const EditTrainingModal = ({ editModalIsOpen, closeEditModal, training, handleUpdate }) => {

  const today = new Date();

  const [responseMessage, setResponseMessage] = useState("");

  useEffect(() => {
    console.log("training in edit modal: ", training);
  }, )

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={editModalIsOpen}
            contentLabel="Training Modification"
            ariaHideApp={false}
          >
            <h2>UPDATE TRAINING</h2>
            {/* onSubmit={handleUpdate} should be called in form tag be */}
            <form className="training-modification-form" >
                <div className="form-group">
                    <label>Training Type:</label>
                    <select
                    id="choices"
                    // onChange={(e) => setType(e.target.value)}
                    // value={type}
                    >
                    <option value="">-- Select Training Type --</option>
                        {/* {availableTrainingTypes.map((t) => (
                        <option key={t} value={t}>{t}</option>
                    ))} */}
                    </select>
                </div>

                <div className="form-group">
                    <label>Training Date:</label>
                    <input
                    type="date"
                    min = {today.toISOString().split('T')[0]}
                    className="training-date-input"
                    // value should hold date (2025-07-18) of training under editing
                    
                    // onChange={(e) => setDate(e.target.value)}
                    />
                </div>

                <div className="form-group">
                    <label>Training Hour:</label>
                    <select
                    id="hours"
                    // onChange={(e) => setHour(e.target.value)}
                    // value={hour || ''}
                    >
                    <option value="">-- Select Hour --</option>
                    {/* {isSelectedDateToday ? 
                        availableHours.map((h) => (
                        <option key={h} value={h}>{h}</option>
                        )) :
                        allHours.map((h) => (
                        <option key={h} value={h}>{h}</option>
                        ))
                    } */}
                    </select>
                </div>

                <div className="form-group">
                    <label>Training Minute:</label>
                    <select
                    id="minutes"
                    // onChange={(e) => setMinute(e.target.value)}
                    // value={minute || ''}
                    >
                    <option value="">-- Select Minute --</option>
                    {/* {availableMinutes.map((m) => (
                        <option key={m} value={m}>{m}</option>
                    ))} */}
                    </select>
                </div>

                <button type="submit">Submit Training</button>
                {responseMessage && <p>{responseMessage}</p>}
                </form>

            <button className="modal-btn btn btn-secondary" onClick={() => closeEditModal()}>Cancel</button>
          </Modal>
        </div>
      );
}

export default EditTrainingModal;