import './TrainingTable.css'
import DayInfo from './DayInfo/DayInfo'
import React, { useState, useEffect } from 'react';

const TrainingsTable = () => {
  const [date, setDate] = useState(null);
  const [tomorrow, setTomorrow] = useState(null);
  const [dayAfterTomorrow, setDayAfterTomorrow] = useState(null);
  const [thirdDay, setThirdDay] = useState(null);

  useEffect(() => {
    setDate(new Date());
    let d = new Date();
    d.setDate(d.getDate() + 1);
    setTomorrow(d);
    d = new Date();
    d.setDate(d.getDate() + 2);
    setDayAfterTomorrow(d);
    d = new Date();
    d.setDate(d.getDate() + 3);
    setThirdDay(d);
  }, []);

  return (
    <div className="training-table">
      <div className="training-table-header">
          <h3 className="table-item col-sm-4">
              Time
          </h3>
          <h3 className="table-item col-sm-4">
              Type
          </h3>
          <h3 className="table-item col-sm-4">
              Register
          </h3>
      </div> 
      <DayInfo date={date}/>
      <DayInfo date={tomorrow}/>
      <DayInfo date={dayAfterTomorrow}/>
      <DayInfo date={thirdDay}/>
    </div>
    
  )
}

export default TrainingsTable
