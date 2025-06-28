import React from 'react'
import { useState, useEffect, useRef } from 'react'
import TrainingsTable from '../TrainingTable/TrainingsTable';
import ResponseMessageModal from '../Modals/ResponseMessageModal';
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';
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
    
    const triggerRefresh = () => setRefreshTrigger(prev => !prev);

    const allHours = ["06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"];
    const availableMinutes = ["00", "15", "30", "45"];
    const today = new Date();
    const availableHours = allHours.filter((hour) => hour > today.getHours());

    const handleSubmit = async (e) => {
    e.preventDefault();

    if (type == null || date == null || hour == null || minute == null) {
      setResponseMessage("Training type, date and time cannot be empty.");
      return;
    }

    const time = hour + ":" + minute;
    const localDateTimeString = `${date}T${time}`;

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/AddTraining`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
          body: JSON.stringify({
            Date: localDateTimeString,
            TrainingType: type
        }),
      });

      if (response.ok) {
        setResponseMessage("Training was added successfully.");
        setRefreshTrigger(!refreshTrigger);
      } else {
        const data = await response.json();
        setResponseMessage(data.message || 'Training could not be added.');
      }
    } catch (error) {
      setResponseMessage('An error occurred during adding training.');
    }
    finally {
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
        <h3>ADD NEW TRAINING</h3>
        <form className="training-creation-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Training Type:</label>
            <select
              id="choices"
              onChange={(e) => setType(e.target.value)}
              value={type || ''}
            >
              <option value="">-- Select Training Type --</option>
              {Object.entries(trainingTypeOptions).map(([value, label]) => (
                <option key={value} value={value}>{label}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Training Date:</label>
            <input
              type="date"
              min={today.toISOString().split('T')[0]}
              className="training-date-input"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>Training Hour:</label>
            <select
              id="hours"
              onChange={(e) => setHour(e.target.value)}
              value={hour || ''}
            >
              <option value="">-- Select Hour --</option>
              {isSelectedDateToday ? 
                availableHours.map((h) => (
                  <option key={h} value={h}>{h}</option>
                )) :
                allHours.map((h) => (
                  <option key={h} value={h}>{h}</option>
                ))
              }
            </select>
          </div>

          <div className="form-group">
            <label>Training Minute:</label>
            <select
              id="minutes"
              onChange={(e) => setMinute(e.target.value)}
              value={minute || ''}
            >
              <option value="">-- Select Minute --</option>
              {}
              {availableMinutes.map((m) => (
                <option key={m} value={m}>{m}</option>
              ))}
            </select>
          </div>

          <button type="submit">Submit Training</button>
          {/* {responseMessage && <p>{responseMessage}</p>} */}
        </form>
      </div>

      <div className="training-box">
        <h3>EDIT TRAINING PLAN</h3>
        <TrainingsTable 
          showSignUp = {false} 
          refreshTrigger = {refreshTrigger} 
          triggerRefresh = {triggerRefresh} 
          isSelectedDateToday = {isSelectedDateToday}
        />
      </div>

      {responseMessage && (
        <ResponseMessageModal
        responseMessageModalIsOpen = {responseMessageModalIsOpen}
        closeResponseMessageModal = {() => setResponseMessageModalIsOpen(false)}
        responseMessage = {responseMessage}
        />
      )}
    </div>
);

}

export default Planning;