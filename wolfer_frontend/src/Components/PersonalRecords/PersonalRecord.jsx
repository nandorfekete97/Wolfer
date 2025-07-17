import React, { useEffect, useState } from 'react'

const PersonalRecord = ({exerciseType, prList}) => {

const [maximumPr, setMaximumPr] = useState(0);

useEffect(() => {
  if (Array.isArray(prList) && prList.length > 0) {
    const maxWeight = Math.max(...prList.map(pr => pr.weight));
    setMaximumPr(maxWeight);
  }
  else {
    setMaximumPr(0);
  }
}, [prList]);

  return (
    <>
        <span className="record-exercise">{exerciseType}</span>
        <span className="record-weight">{maximumPr}</span>
    </>
  )
}

export default PersonalRecord
