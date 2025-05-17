import { data } from 'react-router-dom';
import './Training.css'
import React, { useState, useEffect } from 'react';

const trainingTypeMap = [
  "Functional Body-Building",
  "Weight Lifting",
  "Crossfit",
  "Leg Day"
]

const Training = ({training, signedUpTrainingIdsForDay}) => {
  const [time, setTime] = useState(null);
  const [type, setType] = useState("");
  const [isDisabled, setIsDisabled] = useState(false);

  useEffect(() => {
    if (training) {
      const dateObj = new Date(training.date);
      setTime(dateObj.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit'}));
      setType(trainingTypeMap[training.trainingType]);
    }
  }, [training]);

  useEffect(() => {
    const disabled = signedUpTrainingIdsForDay.length > 0 && !signedUpTrainingIdsForDay.includes(training.id);
    setIsDisabled(disabled);
  }, [signedUpTrainingIdsForDay, training.id]);

  const signUpForTraining = async (e) =>{
    e.preventDefault();
    const userId = localStorage.getItem("userId");

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

        {/* if already signed up for training, make sign up button for given training change into sign off (Red) */}
        {/* at the moment other trainings' sign up button won't disable when being signed up for training that day */}

        <button
          type="button"
          disabled={isDisabled}
          id="sign-up-button"
          className={`btn btn-sm ${signedUpTrainingIdsForDay.includes(training.id) ? 'btn-danger' : 'btn-success'} col-sm-4`}
          onClick={(e) => signUpForTraining(e)}
        >
          {signedUpTrainingIdsForDay.includes(training.id) ? 'Sign Off' : 'Sign Up'}
        </button>
    </div>
  )
}

export default Training
