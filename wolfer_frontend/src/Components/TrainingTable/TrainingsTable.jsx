import './TrainingTable.css';
import DayInfo from './DayInfo/DayInfo';
import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import axios from 'axios';

const TrainingsTable = ({ showSignUp=true, refreshTrigger, triggerRefresh, isSelectedDateToday, isPlanning=false }) => {
  const [weekDates, setWeekDates] = useState([]);
  const [signedUpTrainings, setSignedUpTrainings] = useState([]);
  const [dataIsLoaded, setDataIsLoaded] = useState(false);

  const userId = localStorage.getItem("userId");
  const token = localStorage.getItem("token");

  const getSignedUpTrainings = async () => {
    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/UserTraining/GetUpcomingTrainingsByUserId/${userId}`,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
          }
        }
      );

      setSignedUpTrainings(res.data.trainingEntities); 
      setDataIsLoaded(true);

    } catch (err) {
      if (err.response)
      {
        toast.error(`Failed to fetch upcoming trainings. Status: ${err.response.status}`);
      } else {
        toast.error(`Network error while fetching upcoming trainings: ${err.message}`);
      }
    } 
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
            triggerRefresh = {triggerRefresh}
            isSelectedDateToday = {isSelectedDateToday}
            isPlanning = {isPlanning}
          />
        )) :
      <h5>Loading</h5>
      }
    </div>
  );
};

export default TrainingsTable;