import './TrainingTable.css';
import DayInfo from './DayInfo/DayInfo';
import React, { useState, useEffect } from 'react';

const TrainingsTable = ({ showSignUp=true, refreshTrigger }) => {
  const [weekDates, setWeekDates] = useState([]);
  const [signedUpTrainings, setSignedUpTrainings] = useState([]);
  const [dataIsLoaded, setDataIsLoaded] = useState(false);

  const getSignedUpTrainings = async () => {
    const userId = localStorage.getItem("userId");
    const res = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/GetUpcomingTrainingsByUserId/${userId}`);
    const data = await res.json();
    setSignedUpTrainings(data.trainingEntities); 
    setDataIsLoaded(true);
  }

  useEffect(() => {
    getSignedUpTrainings();
  }, []);

  useEffect(() => {
    const today = new Date();
    const days = Array.from({ length: 7 }, (_, i) => {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      return date;
    });
    setWeekDates(days);
  }, []);

  return (
    <div className="training-table">
      <div className="training-table-header">
        <h3 className="table-item col-sm-4">Time</h3>
        <h3 className="table-item col-sm-4">Type</h3>
        {showSignUp ? 
        <h3 className="table-item col-sm-4">Register</h3>
        :
        <h3 className="table-item col-sm-4">Modification</h3>
        }
      </div>
      {dataIsLoaded ?
        weekDates.map((day, idx) => (
          <DayInfo 
            key= {idx} 
            date= {day} 
            signedUpTrainings= {signedUpTrainings}
            refreshSignedUpTrainings= {getSignedUpTrainings} 
            showSignUp = {showSignUp}
            refreshTrigger = {refreshTrigger}
          />
        )) :
      <h5>Loading</h5>
      }
    </div>
  );
};

export default TrainingsTable;