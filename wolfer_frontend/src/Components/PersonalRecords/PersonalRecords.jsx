import React, { useEffect, useState } from 'react'
import PersonalRecord from './PersonalRecord'
import './PersonalRecords.css';

const PersonalRecords = () => {
    const [personalRecords, setPersonalRecords] = useState([]);
    const userId = localStorage.getItem("userId");

    const getPersonalRecords = async () => {
    const res = await fetch(`${import.meta.env.VITE_API_URL}/PersonalRecord/GetByUserId/${userId}`);
    const data = await res.json();
    setPersonalRecords(data.personalRecordEntities); 
  }

  useEffect(() => {
    getPersonalRecords();
  }, []);

  return (
    <div className="personal-records-container">
        <h2 className="personal-records-header">Personal Records</h2>
        <ul className="personal-record-list">
            {personalRecords.map((record) => (
            <li key={record.id} className="personal-record-item">
                <PersonalRecord exerciseType={record.exerciseType} weight={record.weight}/>
            </li>
        ))}
        </ul>
    </div>
  );
}

export default PersonalRecords
