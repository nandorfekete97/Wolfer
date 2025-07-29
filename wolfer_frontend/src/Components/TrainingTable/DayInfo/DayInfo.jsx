import React, { useEffect, useImperativeHandle, useState } from 'react';
import Training from './Training'
import './DayInfo.css'
import DeleteTrainingsByDateModal from '../../Modals/DeleteTrainingsByDateModal';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const DayInfo = ({ date, signedUpTrainings, refreshSignedUpTrainings, showSignUp = true, refreshTrigger, triggerRefresh, isSelectedDateToday }) => {
  
  const [trainings, setTrainings] = useState([]);
  const [signedUpTrainingIdsForDay, setSignedUpTrainingIdsForDay] = useState([]);
  const [deleteTrainingsByDateModalIsOpen, setDeleteTrainingsByDateModalIsOpen] = useState(false);

  const getTrainings = async () => {
    const dateOnly = date.toLocaleDateString('sv-SE');
    const res = await fetch(`${import.meta.env.VITE_API_URL}/Training/GetTrainingsByDate/${dateOnly}`);
    const data = await res.json();

    // sorting trainings should (perhaps) be on backend 
    const sortedTrainings = data.trainingEntities.sort((a, b) => {
      const timeA = new Date(a.date).getTime();
      const timeB = new Date(b.date).getTime();
      return timeA - timeB;
    });

    setTrainings(sortedTrainings);
  };

  const getTodaysTrainingIds = () => {
    const todaysTrainings = signedUpTrainings.filter(training => isToday(training.date));
    const ids = todaysTrainings.map(t => t.id); 
    setSignedUpTrainingIdsForDay(ids);
  };

  useEffect(() => {
    getTodaysTrainingIds();
  }, [signedUpTrainings]);

  const isToday = (trainingDate) => {
    const trainingDay = new Date(trainingDate);
    return (
      trainingDay.getDate() === date.getDate() &&
      trainingDay.getMonth() === date.getMonth() &&
      trainingDay.getFullYear() === date.getFullYear()
    );
  }
  
  useEffect(() => {
    if (date) {
      getTrainings();
    }
  }, [date, refreshTrigger]);

  const handleDeleteAll = async (e) => {
  e.preventDefault();

  const dateOnly = date.toISOString().split("T")[0];

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/DeleteTrainingsByDate/${dateOnly}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        }
      });

      if (response.ok) {
        toast.success("Trainings were deleted successfully.");
        setDeleteTrainingsByDateModalIsOpen(false);
        getTrainings();
      } else {
        const data = await response.json();
        toast.error(data.message || 'Trainings could not be deleted.');
      }
    } catch (error) {
      toast.error('An error occurred during deleting trainings.');
    }
  }

  return (
    <>
        <h3 className="day-info">
          <div>
            {date ? `${date.toDateString()}` : ""} 
            <button
              className = "btn btn-sm btn-danger delete-button"
              // disabled = {isPast}
              onClick = {(e) => setDeleteTrainingsByDateModalIsOpen(true)}
            >
              Delete All
            </button>
          </div> 
        </h3>
        {trainings.map((training) => (
          <h5 key = {training.id}>
            <Training
              key={training.id}
              training={training} 
              signedUpTrainingIdsForDay={signedUpTrainingIdsForDay}
              refreshSignedUpTrainings={refreshSignedUpTrainings}
              refreshDayTrainings = {getTrainings}
              triggerRefresh = {triggerRefresh}
              showSignUp = {showSignUp}
              isSelectedDateToday = {isSelectedDateToday} 
            />
          </h5>
        ))}

        <DeleteTrainingsByDateModal
          deleteTrainingsByDateModalIsOpen = {deleteTrainingsByDateModalIsOpen}
          closeDeleteModal = {() => setDeleteTrainingsByDateModalIsOpen(false)}
          handleDeleteAll = {handleDeleteAll}
          date = {date}
        />
    </>
  )
}

export default DayInfo;