import React, { useEffect, useState } from 'react'
import PersonalRecord from './PersonalRecord'
import './PersonalRecords.css';
import AddPersonalRecordModal from '../Modals/AddPersonalRecordModal';
import { ExerciseTypes, getExerciseTypeLabel } from '../../Utils/ExerciseTypes';
import axios from 'axios';
import { toast } from 'react-toastify';

const PersonalRecords = () => {
    const [personalRecords, setPersonalRecords] = useState(new Map());
    const [addPersonalRecordModalIsOpen, setAddPersonalRecordModalIsOpen] = useState(false);
    const [refreshPersonalRecords, setRefreshPersonalRecords] = useState(false);

    const userId = localStorage.getItem("userId");
    const token = localStorage.getItem("token");

    const getPersonalRecords = async () => {
      try {
        const res = await axios.get(
          `${import.meta.env.VITE_API_URL}/PersonalRecord/GetByUserId/${userId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );

        const newMap = new Map();
        Object.keys(ExerciseTypes).forEach(exerciseTypeKey => {
          newMap.set(
            exerciseTypeKey,
            res.data.personalRecordEntities.filter(pr => pr.exerciseType === exerciseTypeKey)
          );
        })

        setPersonalRecords(newMap);
      } catch (err) {
        if (err.response)
        {
          toast.error(`Failed to get personal records. Status: ${err.response.status}`);
        } else {
          toast.error(`Network error getting personal records: ${err.message}`);
        }
      }  
    }

  useEffect(() => {
    getPersonalRecords();
  }, [refreshPersonalRecords]);

  return (
    <div className="personal-records-container">
        <h2 className="personal-records-header">Personal Records (KG)</h2>
        <button className="add-pr-button" onClick={() => setAddPersonalRecordModalIsOpen(true)}>
            ADD PR
        </button>
        <ol className="personal-record-list">
            {personalRecords ? Object.keys(ExerciseTypes).map(exerciseTypeKey => (
            <li key={exerciseTypeKey} className="personal-record-item">
                <PersonalRecord exerciseType={exerciseTypeKey} prList={personalRecords.get(exerciseTypeKey)}/>
            </li>
        )) : <></>}
        </ol>
        <AddPersonalRecordModal
            addPersonalRecordModalIsOpen = {addPersonalRecordModalIsOpen}
            closeAddPrModal = {() => setAddPersonalRecordModalIsOpen(false)}
            setRefreshPersonalRecords = {setRefreshPersonalRecords}
        />
    </div>
  );
}

export default PersonalRecords