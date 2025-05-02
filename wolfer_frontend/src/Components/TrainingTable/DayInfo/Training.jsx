import './Training.css'
import React, { useState, useEffect } from 'react';

const trainingTypeMap = [
  "Functional Body-Building",
  "Weight Lifting",
  "Crossfit",
  "Leg Day"
]

const Training = ({training}) => {
  const [time, setTime] = useState(null);
  const [type, setType] = useState("");

  useEffect(() => {
    if (training) {
      const dateObj = new Date(training.date);
      setTime(dateObj.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit'}));
      setType(trainingTypeMap[training.trainingType]);
    }
  }, [training]);

  return (
    <div className="training">
        <h5 className="training-info col-sm-4">
          {time} 
        </h5>
        <h5 className="training-info col-sm-4">
          {type}
        </h5>
        <button type="button" id="sign-up-button" className="btn btn-sm btn-success col-sm-4">
          Sign Up
        </button>
    </div>
  )
}

export default Training
