import React, { useEffect, useImperativeHandle, useState } from 'react';
import Training from './Training'
import './DayInfo.css'

const DayInfo = ({ date, signedUpTrainings, refreshSignedUpTrainings, showSignUp = true, refreshTrigger, triggerRefresh, isSelectedDateToday }) => {
  
  const [trainings, setTrainings] = useState([]);
  const [signedUpTrainingIdsForDay, setSignedUpTrainingIdsForDay] = useState([]);

  const getTrainings = async () => {
    const dateOnly = date.toLocaleDateString('sv-SE');
    const res = await fetch(`${import.meta.env.VITE_API_URL}/Training/GetTrainingsByDate/${dateOnly}`);
    const data = await res.json();

    // sorting trainings should (perhaps) be on backend 
    const sortedTrainings = data.trainingEntities.sort((a, b) => {
      const timeA = new Date(a.date).getTime();
      const timeB = new Date(b.date).getTime();
      return timeA - timeB;
    });

    setTrainings(sortedTrainings);
  };

  const getTodaysTrainingIds = () => {
    const todaysTrainings = signedUpTrainings.filter(training => isToday(training.date));
    const ids = todaysTrainings.map(t => t.id); 
    setSignedUpTrainingIdsForDay(ids);
  };

  useEffect(() => {
    getTodaysTrainingIds();
  }, [signedUpTrainings]);

  const isToday = (trainingDate) => {
    const trainingDay = new Date(trainingDate);
    return (
      trainingDay.getDate() === date.getDate() &&
      trainingDay.getMonth() === date.getMonth() &&
      trainingDay.getFullYear() === date.getFullYear()
    );
  }
  
  useEffect(() => {
    if (date) {
      getTrainings();
    }
  }, [date, refreshTrigger]);

  return (
    <>
        <h3 className="day-info"> {date ? `${date.toDateString()}` : ""} </h3>
        {trainings.map((training) => (
          <h5 key = {training.id}>
            <Training
              key={training.id}
              training={training} 
              signedUpTrainingIdsForDay={signedUpTrainingIdsForDay}
              refreshSignedUpTrainings={refreshSignedUpTrainings}
              refreshDayTrainings = {getTrainings}
              triggerRefresh = {triggerRefresh}
              showSignUp = {showSignUp}
              isSelectedDateToday = {isSelectedDateToday} 
            />
          </h5>
        ))}
    </>
  )
}

export default DayInfo;