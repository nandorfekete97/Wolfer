import './Training.css'
import React, { useState, useEffect } from 'react';

const Training = () => {
  const [time, setTime] = useState("09:00");
  const [type, setType] = useState("Crossfit");

  return (
    <div className="training">
        <h5 className="training-info col-sm-4">
          {time} 
        </h5>
        <h5 className="training-info col-sm-4">
          {type}
        </h5>
        <button type="button" id="sign-up-button" className="btn btn-sm btn-success col-sm-4">
          Sign Up
        </button>
    </div>
  )
}

export default Training
