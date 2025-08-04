import React from 'react'
import { useState, useEffect } from 'react'
import TrainingsTable from '../TrainingTable/TrainingsTable';
import AddTraining from '../AddTraining/AddTraining';
import AddTrainings from '../AddTraining/AddTrainings';
import { AllHours } from '../../Utils/AllTimes';
import './Planning.css';

const Planning = () => {

  const [refreshTrigger, setRefreshTrigger] = useState(false);
  const [isSelectedDateToday, setIsSelectedDateToday] = useState(false);
  const [trainingDate, setTrainingDate] = useState('');

  const triggerRefresh = () => setRefreshTrigger(prev => !prev);

  const today = new Date();
  const availableHours = AllHours.filter((hour) => hour > today.getHours());

  useEffect(() => {
    if (!trainingDate) {
      setIsSelectedDateToday(false);
      return;
    }

    const selectedDateString = new Date(trainingDate).toISOString().split('T')[0];
    const todayDateString = today.toISOString().split('T')[0];

    setIsSelectedDateToday(selectedDateString === todayDateString);
  }, [trainingDate]);

  return (
    <div className="planning-container">
      <div className="training-box">
        <AddTraining
          availableHours={availableHours}
          today={today}
          isSelectedDateToday={isSelectedDateToday}
          trainingDate={trainingDate}
          setTrainingDate={setTrainingDate}
          triggerRefresh={triggerRefresh}
        />
      </div>

      <div className="training-box">
        <AddTrainings
          availableHours={availableHours}
          today={today}
          isSelectedDateToday={isSelectedDateToday}
          trainingDate={trainingDate}
          setTrainingDate={setTrainingDate}
          triggerRefresh={triggerRefresh}
        />
      </div>

      <div className="training-box">
        <h3>EDIT TRAINING PLAN</h3>
        <TrainingsTable
          showSignUp={false}
          refreshTrigger={refreshTrigger}
          triggerRefresh={triggerRefresh}
          isSelectedDateToday={isSelectedDateToday}
          isPlanning={true}
        />
      </div>
    </div>
  );

}

export default Planning;