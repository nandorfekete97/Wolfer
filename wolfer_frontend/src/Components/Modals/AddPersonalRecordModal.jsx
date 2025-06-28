import Modal from 'react-modal';
import React, { useState } from 'react'
import { ExerciseTypes, getExerciseTypeLabel } from '../../Utils/ExerciseTypes';

const AddPersonalRecordModal = ({addPersonalRecordModalIsOpen, closeAddPrModal, setRefreshPersonalRecords}) => {

  const [exerciseType, setExerciseType] = useState("");
  const [weight, setWeight] = useState(0);
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
            weight: weight
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
        <form onSubmit={handleSubmit}>
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
              value={weight}
              onChange={(e) => setWeight(e.target.value)}
            >
            </input>
          </div>
          <button>SUBMIT</button>
          <button onClick={() => closeAddPrModal()}>CANCEL</button>
        </form>
      </Modal>
    </div>
  )
}

export default AddPersonalRecordModal
