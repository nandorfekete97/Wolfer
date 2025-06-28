import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';

const EditTrainingModal = ({ editModalIsOpen, closeEditModal, training, handleUpdate, isSelectedDateToday }) => { 

  const today = new Date();
  const allHours = ["06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"];
  const availableMinutes = ["00", "15", "30", "45"];

  const [type, setType] = useState('');
  const [date, setDate] = useState('');
  const [hour, setHour] = useState('');
  const [minute, setMinute] = useState('');
  const [responseMessage, setResponseMessage] = useState("");

  const getFilteredHours = () => {
    if (!isSelectedDateToday) return allHours;

    const currentHour = today.getHours();
    return allHours.filter(h => Number(h) > currentHour);
  }

  useEffect(() => {
    if (training) {
      setType(training.trainingType || '');

      const dt = new Date(training.date);
      setDate(dt.toISOString().split('T')[0]);
      setHour(dt.getHours().toString().padStart(2, '0'));
      setMinute(dt.getMinutes().toString().padStart(2, '0'));
    }
  }, [training, editModalIsOpen]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!type || !date || !hour || !minute) {
      setResponseMessage("Please fill in all fields.");
      return;
    }

    const [year, month, day] = date.split("-").map(Number);
    const utcDate = new Date(Date.UTC(year, month - 1, day, Number(hour), Number(minute), 0, 0));
    const formattedDate = utcDate.toISOString();

    const updatedTraining = {
      id: training.id,
      trainingType: type,
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
            <h2>UPDATE TRAINING</h2>
            <form className="training-modification-form" onSubmit = {handleSubmit}>
                <div className="form-group">
                    <label>Training Type:</label>
                    <select
                      value={type}
                      onChange={(e) => setType(e.target.value)}
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
                      {getFilteredHours().map((h) => (
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