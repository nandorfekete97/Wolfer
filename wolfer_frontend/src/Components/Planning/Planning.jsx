import React from 'react'
import { useState, useEffect } from 'react'
import TrainingsTable from '../TrainingTable/TrainingsTable';
import Training from '../TrainingTable/DayInfo/Training';

const Planning = () => {

    const [type, setType] = useState(null);
    const [date, setDate] = useState('');
    const [hour, setHour] = useState('');
    const [minute, setMinute] = useState('');
    const [responseMessage, setResponseMessage] = useState("");

    const availableHours = ["06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"];
    const availableMinutes = ["00", "15", "30", "45"];
    const availableTrainingTypes = ["FunctionalBodyBuilding", "WeightLifting", "CrossFit", "LegDay"];

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
        setResponseMessage("Training was added successfully.")
      } else {
        const data = await response.json();
        setResponseMessage(data.message || 'Training could not be added.');
      }
    } catch (error) {
      setResponseMessage('An error occurred during adding training.');
    }
  };

  return (
    <div>
      <h2>Add New Training</h2>
      <form className="training-creation-form" onSubmit={handleSubmit}>
        <div className="form-group">
            <label>Training Type:</label>
            <select
            id="choices"
            onChange={(e) => setType(e.target.value)}
            value={type || ''}
            >
                <option value="">-- Select Training Type --</option>
                {availableTrainingTypes.map((t) => (
                  <option key={t} value={t}>{t}</option>
                ))}
            </select>
        </div>
        <div className="form-group">
            <label>Training Date:</label>
            <input
                type="date"
                className="training-date-input"
                value={date}
                onChange={(e) => setDate(e.target.value)}
            />
        </div>

        <div className="form-group">
          <label>Training Hour:</label>
          <select
            id='hours'
            onChange={(e) => setHour(e.target.value)}
            value={hour || ''}
          >
            <option value="">-- Select Hour --</option>
            {availableHours.map((h) => (
              <option key={h} value={h}>{h}</option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label>Training Minute:</label>
          <select
            id='minutes'
            onChange={(e) => setMinute(e.target.value)}
            value={minute || ''}
          >
            <option value="">-- Select Minute --</option>
            {availableMinutes.map((m) => (
              <option key={m} value={m}>{m}</option>
            ))}
          </select>
        </div>
 
        <button type='submit'>Submit Training</button>
        {responseMessage && <p>{responseMessage}</p>}
      </form>
      <div>
        <h2>Edit Training</h2>
        <TrainingsTable showSignUp={false}/>
      </div>
    </div>
  )
}

export default Planning;