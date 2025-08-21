import React, { useEffect, useImperativeHandle, useState } from 'react';
import Training from './Training'
import './DayInfo.css'
import DeleteTrainingsByDateModal from '../../Modals/DeleteTrainingsByDateModal';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import axios from 'axios';

const DayInfo = ({ date, signedUpTrainings, refreshSignedUpTrainings, showSignUp = true, refreshTrigger, triggerRefresh, isSelectedDateToday, isPlanning }) => {
  
  const [trainings, setTrainings] = useState([]);
  const [signedUpTrainingIdsForDay, setSignedUpTrainingIdsForDay] = useState([]);
  const [deleteTrainingsByDateModalIsOpen, setDeleteTrainingsByDateModalIsOpen] = useState(false);
  const [isTrainingDatePast, setIsTrainingDatePast] = useState(false);

  const trainingDateOnly = date.toISOString().split("T")[0];
  const todayDateOnly = new Date().toISOString().split("T")[0];
  const formattedDate = formatDateString(date);

  const dateOnly = date.toLocaleDateString('sv-SE');
  const token = localStorage.getItem("token");

  const getTrainings = async () => {
    
    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/Training/GetTrainingsByDate/${dateOnly}`,
        {
          headers: {
              "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
              }
        }
      );

      // sorting trainings should (perhaps) be on backend 
      const sortedTrainings = res.data.trainingEntities.sort((a, b) => {
        const timeA = new Date(a.date).getTime();
        const timeB = new Date(b.date).getTime();
        return timeA - timeB;
      });

      setTrainings(sortedTrainings);
    } catch (err)
    {
      if (err.response)
      {
        toast.error(`Failed to fetch trainings for date. Status: ${err.response.status}`);
      } else {
        toast.error(`Network error while fetching trainings for date. Status: ${err.message}`);
      }
    }
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

    const token = localStorage.getItem("token");

    try {
      await axios.delete(`${import.meta.env.VITE_API_URL}/Training/DeleteTrainingsByDate/${trainingDateOnly}`, {
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`
        }
      });

      toast.success("Trainings were deleted successfully.");
      setDeleteTrainingsByDateModalIsOpen(false);
      getTrainings();

    } catch (err) {
        if (err.response) {
          toast.error(`Failed to delete trainings. Status: ${err.response.status}`);
        } else {
          toast.error(`Network error while deleting trainings. Status: ${err.message}`);
        }
    } 
  }

  function formatDateString(dateString) {
    const date = new Date(dateString);

    const weekDay = date.toLocaleString("en-GB", {weekday: "short"});
    const day = date.getDate();
    const month = date.toLocaleString("en-GB", {month: "short"});
    const year = date.getFullYear();

    return `${weekDay} ${day} ${month} ${year}`;
  }

  return (
    <>
        <h3 className="day-info">
          <div>
            {date ? `${formatDateString(date)}` : ""} 

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
              formattedDate = {formattedDate}
            />
          </h5>
        ))}

        <DeleteTrainingsByDateModal
          deleteTrainingsByDateModalIsOpen = {deleteTrainingsByDateModalIsOpen}
          closeDeleteModal = {() => setDeleteTrainingsByDateModalIsOpen(false)}
          handleDeleteAll = {handleDeleteAll}
          formattedDate = {formattedDate}
        />
    </>
  )
}

export default DayInfo;