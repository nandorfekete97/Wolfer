import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';
import { AllHours, AllMinutes } from '../../Utils/AllTimes';
import './EditTrainingModal.css';

const EditTrainingModal = ({ editModalIsOpen, closeEditModal, training, formattedDate, handleUpdate, isSelectedDateToday }) => { 

  const today = new Date();

  const [trainingType, setTrainingType] = useState('');
  const [trainingDate, setTrainingDate] = useState('');
  const [trainingHour, setTrainingHour] = useState('');
  const [trainingMinute, setTrainingMinute] = useState('');
  const [responseMessage, setResponseMessage] = useState("");

  const getFilteredHours = () => {
    if (!isSelectedDateToday) return AllHours;

    const currentHour = today.getHours();

    return AllHours.filter(h => Number(h) > currentHour);
  }
  
  useEffect(() => {
    if (training) {
      setTrainingType(training.trainingType || '');

      const dt = new Date(training.date);
      setTrainingDate(dt.toISOString().split('T')[0]);
      setTrainingHour(dt.getHours().toString().padStart(2, '0'));
      setTrainingMinute(dt.getMinutes().toString().padStart(2, '0'));
    }
  }, [training, editModalIsOpen]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!trainingType || !trainingDate || !trainingHour || !trainingMinute) {
      setResponseMessage("Please fill in all fields.");
      return;
    }

    const [year, month, day] = trainingDate.split("-").map(Number);
    const utcDate = new Date(Date.UTC(year, month - 1, day, Number(trainingHour), Number(trainingMinute), 0, 0));
    const formattedDate = utcDate.toISOString();

    const updatedTraining = {
      id: training.id,
      trainingType: trainingType,
      date: formattedDate,
    };

    const result = await handleUpdate(updatedTraining);

    setResponseMessage(result.message);
    if (result.success)
    {
      closeEditModal();
    }
  };

  useEffect(() => {
    if (editModalIsOpen) {
      setResponseMessage("");
    }
  }, [editModalIsOpen]);

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={editModalIsOpen}
            contentLabel="Training Modification"
            ariaHideApp={false}
          >
            <h2>UPDATE {getTrainingTypeLabel(training.trainingType)} for date: <br></br> {formattedDate}</h2>
            <form className="training-modification-form" onSubmit = {handleSubmit}>
                <div className="form-group">
                    <label>Training Type:</label>
                    <select
                      value={trainingType}
                      onChange={(e) => setTrainingType(e.target.value)}
                    >
                      <option value="">-- Select Training Type --</option>
                      {Object.entries(TrainingTypes).map(([value, label]) => (
                        <option key={value} value={value}>{label}</option>
                      ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Training Date:</label>
                    <input
                      type = "date"
                      min = {today.toISOString().split('T')[0]}
                      value = {trainingDate}
                      className = "training-date-input"
                      onChange = {(e) => setTrainingDate(e.target.value)}
                    />
                </div>

                <div className = "form-group">
                    <label>Training Hour:</label>
                    <select
                      value = {trainingHour}
                      onChange = {(e) => setTrainingHour(e.target.value)}
                    >
                      <option value = "">-- Select Hour --</option>
                      {getFilteredHours().map((h) => (
                        <option key = {h} value={h}>{h}</option>
                      ))}
                    </select>
                </div>

                <div className = "form-group">
                    <label>Training Minute:</label>
                    <select
                      value = {trainingMinute}
                      onChange = {(e) => setTrainingMinute(e.target.value)}
                    >
                      <option value="">-- Select Minute --</option>
                      {AllMinutes.map((m) => (
                          <option key={m} value={m}>{m}</option>
                      ))}
                    </select>
                </div>

                <button type = "submit" className = 'btn btn-primary'>UPDATE</button>
                {responseMessage && <p>{responseMessage}</p>}
                </form>

            <button className="modal-btn btn btn-secondary" onClick={() => closeEditModal()}>CANCEL</button>
          </Modal>
        </div>
      );
}

export default EditTrainingModal;