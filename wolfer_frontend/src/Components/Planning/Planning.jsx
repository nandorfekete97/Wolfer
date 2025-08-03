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
  const [responseMessageModalIsOpen, setResponseMessageModalIsOpen] = useState(false);
  const [selectedTrainings, setSelectedTrainings] = useState([]);
  const [multipleDateInput, setMultipleDateInput] = useState('');

  const triggerRefresh = () => setRefreshTrigger(prev => !prev);

  const today = new Date();
  const availableHours = AllHours.filter((hour) => hour > today.getHours());

  const handleMultipleSubmit = async (e) => {
    e.preventDefault();

    if (type == null || hour == null || minute == null || selectedTrainings.length === 0) {
      setResponseMessage("Training type, time and at least one date must be selected.");
      setResponseMessageModalIsOpen(true);
      return;
    }

    //const time = `${hour}:${minute}`;
    const trainingList = selectedTrainings.map((d) => ({
      Date: `${d.date}T${d.hour.padStart(2, '0')}:${d.minute.padStart(2, '0')}:00`,
      TrainingType: type
    }));

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/AddTrainings`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(trainingList),
      });

      if (response.ok) {
        setResponseMessage("Trainings were added successfully.");
        setSelectedTrainings([]);
        setRefreshTrigger(!refreshTrigger);
      } else {
        const data = await response.json();
        setResponseMessage(data.message || "Trainings could not be added.");
      }
    }
    catch {
      setResponseMessage("An error occured during adding trainings.");
    } finally {
      setResponseMessageModalIsOpen(true);
    }
  };

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

      {responseMessage && (
        <ResponseMessageModal
          responseMessageModalIsOpen={responseMessageModalIsOpen}
          closeResponseMessageModal={() => setResponseMessageModalIsOpen(false)}
          responseMessage={responseMessage}
        />
      )}
    </div>
  );

}

export default Planning;