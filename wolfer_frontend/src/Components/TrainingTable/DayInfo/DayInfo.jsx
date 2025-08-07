import React, { useEffect, useImperativeHandle, useState } from 'react';
import Training from './Training'
import './DayInfo.css'
import DeleteTrainingsByDateModal from '../../Modals/DeleteTrainingsByDateModal';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const DayInfo = ({ date, signedUpTrainings, refreshSignedUpTrainings, showSignUp = true, refreshTrigger, triggerRefresh, isSelectedDateToday, isPlanning }) => {
  
  const [trainings, setTrainings] = useState([]);
  const [signedUpTrainingIdsForDay, setSignedUpTrainingIdsForDay] = useState([]);
  const [deleteTrainingsByDateModalIsOpen, setDeleteTrainingsByDateModalIsOpen] = useState(false);
  const [isTrainingDatePast, setIsTrainingDatePast] = useState(false);

  const trainingDateOnly = date.toISOString().split("T")[0];
  const todayDateOnly = new Date().toISOString().split("T")[0];

  const getTrainings = async () => {
    const dateOnly = date.toLocaleDateString('sv-SE');
    const token = localStorage.getItem("token");

    const res = await fetch(`${import.meta.env.VITE_API_URL}/Training/GetTrainingsByDate/${dateOnly}`,
      {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
              "Authorization": `Bearer ${token}`
            }
      }
    );
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
      setIsTrainingDatePast(trainingDateOnly <= todayDateOnly);
    }
  }, [date, refreshTrigger]);

  const handleDeleteAll = async (e) => {
  e.preventDefault();

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/DeleteTrainingsByDate/${trainingDateOnly}`, {
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

            {isPlanning ? 
              <button
                className = "btn btn-sm btn-danger delete-button"
                disabled = {isTrainingDatePast}
                onClick = {(e) => setDeleteTrainingsByDateModalIsOpen(true)}
              >
                Delete All
              </button>
              :
              ""
            }
            
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
              date = {date}
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