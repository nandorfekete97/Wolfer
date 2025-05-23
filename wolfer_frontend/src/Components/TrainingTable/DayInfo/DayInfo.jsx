import React, { useEffect, useState } from 'react';
import Training from './Training'
import './DayInfo.css'

const DayInfo = ({ date, signedUpTrainings, refreshSignedUpTrainings }) => {
  
  const [trainings, setTrainings] = useState([]);
  const [signedUpTrainingIdsForDay, setSignedUpTrainingIdsForDay] = useState([]);

  const getTrainings = async () => {
  const dateOnly = date.toISOString().split('T')[0];
  const res = await fetch(`${import.meta.env.VITE_API_URL}/Training/GetTrainingsByDate/${dateOnly}`);
  const data = await res.json();

  // even if trainings were added in a non-chronological order, they should be displayed in a chronological order
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
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); 
    var yyyy = date.getFullYear();

    var today = yyyy + '-' + mm + '-' + dd;
    return today == trainingDate.split('T')[0];
  }
  
  console.log(trainings);

  useEffect(() => {
    if (date) {
      getTrainings();
    }
  }, [date]);

  return (
    <>
        <h3 className="day-info">
          {date ? `${date.toDateString()}` : ""}
        </h3>
        {trainings.map((training) => (
          <Training 
            key={training.id}
            training={training} 
            signedUpTrainingIdsForDay={signedUpTrainingIdsForDay}
            refreshSignedUpTrainings={refreshSignedUpTrainings} />
        ))}
    </>
  )
}

export default DayInfo
