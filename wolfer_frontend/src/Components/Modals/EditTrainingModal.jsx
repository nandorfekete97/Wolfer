import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';

const EditTrainingModal = ({ editModalIsOpen, closeEditModal, training, handleUpdate }) => { 

  const today = new Date();
  const availableTrainingTypes = ["FunctionalBodyBuilding", "WeightLifting", "CrossFit", "LegDay"];
  const allHours = [6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
  const availableMinutes = ["00", "15", "30", "45"];

  const [type, setType] = useState('');
  const [date, setDate] = useState('');
  const [hour, setHour] = useState('');
  const [minute, setMinute] = useState('');
  const [responseMessage, setResponseMessage] = useState("");

  useEffect(() => {
    if (training) {
      setType(training.trainingType || '');

      const dt = new Date(training.date);
      setDate(dt.toISOString().split('T')[0]);
      setHour(dt.getHours().toString().padStart(2, '0'));
      setMinute(dt.getMinutes().toString().padStart(2, '0'));
    }
  }, [training, editModalIsOpen]);

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
                      value={type}
                      onChange={(e) => setType(e.target.value)}
                    >
                      <option value="">-- Select Training Type --</option>
                      {availableTrainingTypes.map((t) => (
                        <option key={t} value={t}>{t}</option>
                      ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Training Date:</label>
                    <input
                      type = "date"
                      min = {today.toISOString().split('T')[0]}
                      value = {date}
                      className = "training-date-input"
                      onChange = {(e) => setDate(e.target.value)}
                    />
                </div>

                <div className = "form-group">
                    <label>Training Hour:</label>
                    <select
                      value = {hour}
                      onChange = {(e) => setHour(e.target.value)}
                    >
                      <option value = "">-- Select Hour --</option>
                      {allHours.map((h) => (
                        <option key = {h} value={h}>{h}</option>
                      ))}
                    </select>
                </div>

                <div className = "form-group">
                    <label>Training Minute:</label>
                    <select
                      value = {minute}
                      onChange = {(e) => setMinute(e.target.value)}
                    >
                      <option value="">-- Select Minute --</option>
                      {availableMinutes.map((m) => (
                          <option key={m} value={m}>{m}</option>
                      ))}
                    </select>
                </div>

                <button type = "submit" className = 'btn btn-primary'>Submit Training</button>
                {responseMessage && <p>{responseMessage}</p>}
                </form>

            <button className="modal-btn btn btn-secondary" onClick={() => closeEditModal()}>Cancel</button>
          </Modal>
        </div>
      );
}

export default EditTrainingModal;