import React, { useEffect, useState } from 'react'

const PersonalRecord = ({exerciseType, prList}) => {

const [maximumPr, setMaximumPr] = useState(0);

useEffect(() => {
    setMaximumPr(prList.map(pr => pr.weight).max());
}, [prList]);

  return (
    <>
        <span className="record-exercise">{exerciseType}</span>
        <span className="record-weight">{maximumPr}</span>
    </>
  )
}

export default PersonalRecord
