import React, { useEffect, useState } from 'react';
import Training from './Training'
import './DayInfo.css'

const DayInfo = ({ date }) => {
  
  const [trainings, setTrainings] = useState([]);

  const getTrainings = async () => {
    const dateOnly = date.toISOString().split('T')[0];
    const res = await fetch(`http://localhost:5166/Training/GetTrainingsByDate/${dateOnly}`);
    const data = await res.json();
    setTrainings(data.trainingEntities); 
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
          <Training training={training} />
        ))}
    </>
  )
}

export default DayInfo
