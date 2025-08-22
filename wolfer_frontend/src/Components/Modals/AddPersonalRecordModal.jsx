import Modal from 'react-modal';
import React, { useEffect, useState } from 'react'
import { ExerciseTypes, getExerciseTypeLabel } from '../../Utils/ExerciseTypes';
import './AddPersonalRecordModal.css';
import { toast } from 'react-toastify';
import axios from 'axios';

const AddPersonalRecordModal = ({addPersonalRecordModalIsOpen, closeAddPrModal, setRefreshPersonalRecords}) => {

  let today = new Date()
  today = today.toISOString().split('T')[0];

  const [exerciseType, setExerciseType] = useState("");
  const [weight, setWeight] = useState(0);
  const [prDate, setPrDate] = useState(today);
  const userId = localStorage.getItem("userId");
  const token = localStorage.getItem("token");

  useEffect(() => {
    if (addPersonalRecordModalIsOpen) {
      setExerciseType("");
      setWeight(0);
      setPrDate(today);
    }
  }, [addPersonalRecordModalIsOpen]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await axios.post(
        `${import.meta.env.VITE_API_URL}/PersonalRecord/AddPersonalRecord/`, 
          {
            userId: userId,
            exerciseType: exerciseType,
            weight: weight,
            date: prDate,
          },
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );
     
      toast.success("Personal Record added successfully.");
      setRefreshPersonalRecords(prev => !prev);
      closeAddPrModal();
    }
      catch (err)
      {
        if (err.response)
        {
          toast.error(`Failed to add personal record. Status: ${err.response.status}`);
        } else {
          toast.error(`Network error while adding personal record: ${err.message}`);
        }
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