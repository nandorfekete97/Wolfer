import React from 'react'
import { useState, useEffect } from 'react'

const Planning = () => {

    const [type, setType] = useState(null);
    const [date, setDate] = useState('');
    const [time, setTime] = useState('');
    const [responseMessage, setResponseMessage] = useState("");
    
    useEffect(() => {
        console.log("type: ", type);
    }, [type]);

    useEffect(() => {
        console.log("date: ", date);
    }, [date]);

    useEffect(() => {
        console.log("time: ", time);
    }, [time]);

    const handleSubmit = async (e) => {
    e.preventDefault();

    if (type == null || date == null || time == null) {
      setResponseMessage("Training type, date and time cannot be empty.");
      return;
    }

    const isoString = `${date}T${time}`;
    const fulldate = new Date(isoString);

    console.log("fulldate: ", fulldate);

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/AddTraining`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
          body: JSON.stringify({
          Date: fulldate.toISOString(),
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
      <form className="training-creation-form" onSubmit={handleSubmit}>
        <div className="form-group">
            <label>Training Type:</label>
            <select
            id="choices"
            onChange={(e) => setType(e.target.value)}
            value={type || ''}
            >
                <option value="">-- Training Type --</option>
                <option value="FunctionalBodyBuilding">Functional Body-Building</option>
                <option value="WeightLifting">Weight Lifting</option>
                <option value="CrossFit">Crossfit</option>
                <option value="LegDay">Leg Day</option>
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
            <label>Training Time:</label>
            <input
                type="time"
                className="training-time-input"
                value={time}
                onChange={(e) => setTime(e.target.value)}
            />
        </div>
        <button type='submit'>Submit Training</button>
        {responseMessage && <p>{responseMessage}</p>}
      </form>
    </div>
  )
}

export default Planning;