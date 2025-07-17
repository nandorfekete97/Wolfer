import React, { useEffect, useState } from 'react'
import PersonalRecord from './PersonalRecord'
import './PersonalRecords.css';
import AddPersonalRecordModal from '../Modals/AddPersonalRecordModal';
import { ExerciseTypes, getExerciseTypeLabel } from '../../Utils/ExerciseTypes';

const PersonalRecords = () => {
    const [personalRecords, setPersonalRecords] = useState(new Map());
    const [addPersonalRecordModalIsOpen, setAddPersonalRecordModalIsOpen] = useState(false);
    const [refreshPersonalRecords, setRefreshPersonalRecords] = useState(false);

    const userId = localStorage.getItem("userId");

    const getPersonalRecords = async () => {
        const res = await fetch(`${import.meta.env.VITE_API_URL}/PersonalRecord/GetByUserId/${userId}`);
        const data = await res.json();

        const newMap = new Map();
        Object.keys(ExerciseTypes).forEach(exerciseTypeKey => {
          newMap.set(
            exerciseTypeKey,
            data.personalRecordEntities.filter(pr => pr.exerciseType === exerciseTypeKey)
          );
        })

        setPersonalRecords(newMap);
    }

  useEffect(() => {
    getPersonalRecords();
  }, [refreshPersonalRecords]);

  useEffect(() =>{
    console.log("ExerciseTypes: ", ExerciseTypes);

  }, [ExerciseTypes]);

  return (
    <div className="personal-records-container">
        <h2 className="personal-records-header">Personal Records (KG)</h2>
        <button onClick={() => setAddPersonalRecordModalIsOpen(true)}>
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
