import React from 'react'
import { useState, useEffect } from 'react';

const TrainingHistory = () => {

const [pastTrainings, setPastTrainings] = useState([]);
const [dataIsLoaded, setDataIsLoaded] = useState(false);

const getpastTrainings = async () => {
    const userId = localStorage.getItem("userId");
    const res = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/GetPastTrainingsForUser/${userId}`);
    const data = await res.json();
    setPastTrainings(data.trainingEntities); 
    console.log("user id: ", userId);
    console.log("pastTrainings: ", pastTrainings);
    setDataIsLoaded(true);
}

useEffect(() => {
    getpastTrainings();
}, []);

useEffect(() => {
    console.log("Updated pastTrainings:", pastTrainings);
}, [pastTrainings]);


  return (
    <div>
      <h1>TRAINING HISTORY</h1>
      {dataIsLoaded ? 
      <ul>
        <li> {pastTrainings.map(training => training.date)} </li>
      </ul>
      : 
      "Loading"}
    </div>
  )
}

export default TrainingHistory;
