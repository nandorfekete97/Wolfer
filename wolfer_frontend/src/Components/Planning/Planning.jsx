import React from 'react'
import { useState, useEffect, useRef } from 'react'
import TrainingsTable from '../TrainingTable/TrainingsTable';
import AddTraining from '../AddTraining/AddTraining';
import AddTrainings from '../AddTraining/AddTrainings';
import ResponseMessageModal from '../Modals/ResponseMessageModal';
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';
import { AllHours, AllMinutes } from '../../Utils/AllTimes';
import './Planning.css';

const Planning = () => {

  const [type, setType] = useState(null);
  const [date, setDate] = useState('');
  const [hour, setHour] = useState('');
  const [minute, setMinute] = useState('');
  const [responseMessage, setResponseMessage] = useState("");
  const [refreshTrigger, setRefreshTrigger] = useState(false);
  const [isSelectedDateToday, setIsSelectedDateToday] = useState(false);

  const triggerRefresh = () => setRefreshTrigger(prev => !prev);

  const today = new Date();
  const availableHours = AllHours.filter((hour) => hour > today.getHours());

  useEffect(() => {
    if (!date) {
      setIsSelectedDateToday(false);
      return;
    }

    const selectedDateString = new Date(date).toISOString().split('T')[0];
    const todayDateString = today.toISOString().split('T')[0];

    setIsSelectedDateToday(selectedDateString === todayDateString);
  }, [date]);

  return (
    <div className="planning-container">
      <div className="training-box">
        <AddTraining
          availableHours={availableHours}
          today={today}
          isSelectedDateToday={isSelectedDateToday}
          triggerRefresh={triggerRefresh}
        />
      </div>

      <div className="training-box">
        <AddTrainings
          availableHours={availableHours}
          today={today}
          isSelectedDateToday={isSelectedDateToday}
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