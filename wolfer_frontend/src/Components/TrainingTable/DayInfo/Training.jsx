import { data } from 'react-router-dom';
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
  const [isSignedUp, setIsSignedUp] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (training) {
      const dateObj = new Date(training.date);
      setTime(dateObj.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit'}));
      setType(trainingTypeMap[training.trainingType]);
    }
  }, [training]);

  const signUpForTraining = async (e) =>{
    e.preventDefault();
  
    console.log("training: ", training);
    console.log("training id: ", training.id);
    const userId = localStorage.getItem("userId");
    console.log("user id: ", userId);

    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/SignUserUpForTraining/users/${userId}/trainings/${training.id}`, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify({
            userId: userId,
            trainingId: training.id,
            }),
        });

        if (response.ok) {
            setIsSignedUp(true);
        } else {
            const data = await response.json();
            setError(data.message || 'Failed to sign up for training.');
        }
    } catch (error) {
        setError('An error occurred during signing up for training.');
    }
  }

  return (
    <div className="training">
        <h5 className="training-info col-sm-4">
          {time} 
        </h5>
        <h5 className="training-info col-sm-4">
          {type}
        </h5>
        <button type="button" id="sign-up-button" className="btn btn-sm btn-success col-sm-4" onClick={(e) => signUpForTraining(e)}>
          Sign Up
        </button>
    </div>
  )
}

export default Training
