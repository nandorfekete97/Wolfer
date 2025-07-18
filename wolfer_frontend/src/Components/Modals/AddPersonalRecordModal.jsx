import Modal from 'react-modal';
import React, { useState } from 'react'
import { ExerciseTypes, getExerciseTypeLabel } from '../../Utils/ExerciseTypes';
import './AddPersonalRecordModal.css';


const AddPersonalRecordModal = ({addPersonalRecordModalIsOpen, closeAddPrModal, setRefreshPersonalRecords}) => {

  let today = new Date()
  today = today.toISOString().split('T')[0];

  const [exerciseType, setExerciseType] = useState("");
  const [weight, setWeight] = useState(0);
  const [prDate, setPrDate] = useState(today);
  const [responseMessage, setResponseMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    const userId = localStorage.getItem("userId");

    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/PersonalRecord/AddPersonalRecord/`, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify({
            userId: userId,
            exerciseType: exerciseType,
            weight: weight,
            date: prDate
            }),
        });

        if (response.ok) {
            setResponseMessage("Successfully added Personal Record.");
            setRefreshPersonalRecords(prev => !prev);
            closeAddPrModal();
        } else {
            const data = await response.json();
            setResponseMessage(data.message || 'Failed to add Personal Record.');
        }
    } catch (error) {
        setResponseMessage('An error occurred during adding Personal Record.');
    }
  }

  return (
    <div>
      <Modal isOpen={addPersonalRecordModalIsOpen}>
        <form onSubmit={handleSubmit} className="modal-content add-pr-form">
          <div>
            <select
              value={exerciseType}
              onChange={(e) => setExerciseType(e.target.value)}
            >
              <option value="">-- Select Exercise Type --</option>
              {Object.entries(ExerciseTypes).map(([value, label]) => (
                <option key={value} value={value}>{label}</option>
              ))}
            </select>
          </div>
        
          <div>
            <input
              type="number"
              value={weight}
              onChange={(e) => setWeight(e.target.value)}
              placeholder="Weight (kg)"
            >
            </input>
          </div>

          <div>
            <input 
              type='date'
              value={prDate}
              onChange={(e) => setPrDate(e.target.value)}
            >
            </input>
          </div>
          <button>SUBMIT</button>
          <button onClick={() => closeAddPrModal()} type="button" className="cancel-button">CANCEL</button>
        </form>
      </Modal>
    </div>
  )
}

export default AddPersonalRecordModal
