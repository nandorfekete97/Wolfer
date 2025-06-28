import React, { useEffect, useState } from 'react'
import PersonalRecord from './PersonalRecord'
import './PersonalRecords.css';
import AddPersonalRecordModal from '../Modals/AddPersonalRecordModal';

const PersonalRecords = () => {
    const [personalRecords, setPersonalRecords] = useState([]);
    const [addPersonalRecordModalIsOpen, setAddPersonalRecordModalIsOpen] = useState(false);
    const [refreshPersonalRecords, setRefreshPersonalRecords] = useState(false);

    const userId = localStorage.getItem("userId");

    const getPersonalRecords = async () => {
    const res = await fetch(`${import.meta.env.VITE_API_URL}/PersonalRecord/GetByUserId/${userId}`);
    const data = await res.json();

    data.personalRecordEntities.sort((a,b) => (a.exerciseType > b.exerciseType) ? 1 : ((b.exerciseType > a.exerciseType) ? -1 : 0));

    setPersonalRecords(data.personalRecordEntities); 
  }

  useEffect(() => {
    getPersonalRecords();
  }, [refreshPersonalRecords]);

  useEffect(() =>{
    console.log("addPersonalRecordModalIsOpen: ", addPersonalRecordModalIsOpen);
  }, [addPersonalRecordModalIsOpen]);

  return (
    <div className="personal-records-container">
        <h2 className="personal-records-header">Personal Records</h2>
        <button onClick={() => setAddPersonalRecordModalIsOpen(true)}>
            ADD PR
        </button>
        <ol className="personal-record-list">
            {personalRecords.map((record) => (
            <li key={record.id} className="personal-record-item">
                <PersonalRecord exerciseType={record.exerciseType} weight={record.weight}/>
            </li>
        ))}
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
