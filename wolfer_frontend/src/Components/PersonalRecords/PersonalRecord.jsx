import React, { useState } from 'react'

const PersonalRecord = ({exerciseType, weight}) => {

  return (
    <>
        <span className="record-exercise">{exerciseType}</span>
        <span className="record-weight">{weight}</span>
    </>
  )
}

export default PersonalRecord
