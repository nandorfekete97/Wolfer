import './TrainingTable.css';
import DayInfo from './DayInfo/DayInfo';
import React, { useState, useEffect } from 'react';

const TrainingsTable = () => {
  const [weekDates, setWeekDates] = useState([]);

  useEffect(() => {
    const today = new Date();
    const days = Array.from({ length: 7 }, (_, i) => {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      return date;
    });
    setWeekDates(days);
  }, []);

  return (
    <div className="training-table">
      <div className="training-table-header">
        <h3 className="table-item col-sm-4">Time</h3>
        <h3 className="table-item col-sm-4">Type</h3>
        <h3 className="table-item col-sm-4">Register</h3>
      </div>
      {weekDates.map((day, idx) => (
        <DayInfo key={idx} date={day} />
      ))}
    </div>
  );
};

export default TrainingsTable;