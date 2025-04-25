import React, { useEffect } from 'react';
import Training from './Training'
import './DayInfo.css'

const DayInfo = ({ date }) => {
  
    useEffect(() => {
      console.log(date);
    }, []);

  return (
    <>
        <h3 className="day-info">
          {date ? `${date.toDateString()}` : ""}
        </h3>
        <Training/>
        <Training/>
    </>
  )
}

export default DayInfo
