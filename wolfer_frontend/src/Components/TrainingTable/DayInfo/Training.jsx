import './Training.css'
import React, { useState, useEffect } from 'react';
import DeleteModal from '../../Modals/DeleteTrainingModal';
import EditTrainingModal from '../../Modals/EditTrainingModal';
import ResponseMessageModal from '../../Modals/ResponseMessageModal';
import UsersByTrainingModal from '../../Modals/UsersByTrainingModal';
import { TrainingTypes, getTrainingTypeLabel } from '../../../Utils/TrainingTypes';

const Training = ({ training, signedUpTrainingIdsForDay, refreshSignedUpTrainings, refreshDayTrainings, triggerRefresh, showSignUp = true, isSelectedDateToday }) => {
  const [time, setTime] = useState(null);
  const [type, setType] = useState("");
  const [isDisabled, setIsDisabled] = useState(false);
  const [responseMessage, setResponseMessage] = useState("");
  const [deleteModalIsOpen, setDeleteModalIsOpen] = useState(false);
  const [editModalIsOpen, setEditModalIsOpen] = useState(false);
  const [responseMessageModalIsOpen, setResponseMessageModalIsOpen] = useState(false);
  const [usersByTrainingModalIsOpen, setUsersByTrainingModalIsOpen] = useState(false);
  const [usersByTraining, setUsersByTraining] = useState([]);

  const isPast = new Date(training.date).getTime() < Date.now();

  useEffect(() => {
    if (training) {
      const dateObj = new Date(training.date);
      setTime(dateObj.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit'}));
      setType(training.trainingType);
    }
  }, [training]);

  useEffect(() => {
    const disabled = signedUpTrainingIdsForDay.length > 0 && !signedUpTrainingIdsForDay.includes(training.id);
    setIsDisabled(disabled);
  }, [signedUpTrainingIdsForDay, training.id]);

  const signUpForTraining = async (e) =>{
    e.preventDefault();
    const userId = localStorage.getItem("userId");

    // part where we find out how many are signed up to training
    try{
      const response = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/GetUsersByTrainingId/${training.id}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        }
      });

      if (response.ok)
      {
        const data = await response.json();
        const currentUsers = data.userEntities || [];

        if (currentUsers.length >= 15)
        {
          toast.error("This training is full.");
          return;
        }
      } else {
        toast.error("Failed to fetch current training signups.");
        return;
      }
    } catch (error) {
      toast.error('An error occurred fetching data for training.');
    }

    // part that existed before, functional, responsible for signing up to training
    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/SignUserUpForTraining/users/${userId}/trainings/${training.id}`, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify({
            userId: userId,
            trainingId: training.id,
            }),
        });

        if (response.ok) {
          console.log("Signup response:", response.status, await response.text());

            toast.success("Successfully signed up for training.");
        } else {
            const data = await response.json();
            toast.error(data.message || 'Failed to sign up for training.');
        }
    } catch (error) {
        toast.error('An error occurred during signing up for training.');
    }
    finally {
      refreshSignedUpTrainings();
    }
  }

  const signOffFromTraining = async (e) =>{
    e.preventDefault();
    const userId = localStorage.getItem("userId");

    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/SignUserOffFromTraining/users/${userId}/trainings/${training.id}`, {
            method: 'DELETE',
            headers: {
            'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            setResponseMessage("Successfully signed off from training.");
            refreshSignedUpTrainings();
        } else {
            const data = await response.json();
            setResponseMessage(data.message || 'Failed to sign off from training.');
        }
    } catch (error) {
        setError('An error occurred during signing off from training.');
    }
    finally {
      setResponseMessageModalIsOpen(true);
      refreshSignedUpTrainings();
    }
  }
  
  const showUsersByTraining = async (training) => {

    try{
      const response = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/GetUsersByTrainingId/${training.id}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        }
      });

      if (response.ok)
      {
        const data = await response.json();
        setUsersByTraining(data.userEntities);
        setUsersByTrainingModalIsOpen(true);
      } else {
        setResponseMessage(data.message || 'Failed to show users for training.');
      }
    } catch (error) {
      setError('An error occurred during showing users for training.');
    }
  }

  const handleDelete = async (e) => {
    e.preventDefault();

    setResponseMessage("");

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/DeleteTraining/${training.id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        }
      });

      if (response.ok) {
        toast.success("Training was deleted successfully.");
        if (refreshSignedUpTrainings) 
        {
          refreshSignedUpTrainings();
        };
        if (refreshDayTrainings) 
        {
          refreshDayTrainings();
        }
      } else {
        const data = await response.json();
        toast.error(data.message || 'Training could not be deleted.');
      }
    } catch (error) {
      toast.error('An error occurred during deleting training.');
    }
  }

  const handleUpdate = async (updatedTraining) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/UpdateTraining/`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedTraining),
      });

      const data = await response.json();
      toast.info(response.ok ? "Training updated successfully." : data.message || "Failed to update training.");
      if (response.ok) {
        if (refreshSignedUpTrainings) 
        {
          refreshSignedUpTrainings();
        };
        if (refreshDayTrainings) 
        {
          refreshDayTrainings();
        };
        if (triggerRefresh)
        {
          triggerRefresh();
        };
        return { success: true, message: "Training updated successfully." };
      } else {
        return { success: false, message: data.message || "Failed to update training." };
      }
    } catch (error) {
      return { success: false, message: "An error occured during update." };
    }
  }

  return (
    <div className = "training">
      <h5 className = "training-info col-sm-4">{time}</h5>
      <h5 className = "training-info col-sm-4" onClick={() => showUsersByTraining(training)}>
        {getTrainingTypeLabel(type)}
      </h5>

      {showSignUp ? (
        signedUpTrainingIdsForDay.includes(training.id) ? (
          <button
            className = "btn btn-sm btn-danger col-sm-4"
            onClick = {signOffFromTraining}
            disabled = {isPast}
          >
            Sign Off
          </button>
        ) : (
          <button
            className = "btn btn-sm btn-success col-sm-4"
            onClick = {signUpForTraining}
            disabled = {isDisabled || isPast}
          >
            Sign Up
          </button>
        )
      ) : (
        <div className = "col-sm-4 d-flex gap-2">
          <button
            className = "btn btn-sm btn-primary"
            disabled = {isPast}
            onClick = {() => setEditModalIsOpen(true)}
          >
            Edit
          </button>
          <button
            className = "btn btn-sm btn-danger"
            disabled = {isPast}
            onClick = {(e) => setDeleteModalIsOpen(true)}
          >
            Delete
          </button>
        </div>
      )}

      <UsersByTrainingModal
        training={training}
        trainingTime = {time}
        trainingType = {type}
        usersByTraining = {usersByTraining}
        usersByTrainingModalIsOpen = {usersByTrainingModalIsOpen}
        closeUsersByTrainingModal = {() => setUsersByTrainingModalIsOpen(false)}
      />

      <DeleteModal
        deleteModalIsOpen = {deleteModalIsOpen}
        closeDeleteModal = {() => setDeleteModalIsOpen(false)}
        handleDelete = {handleDelete}
      />

      <EditTrainingModal
        editModalIsOpen = {editModalIsOpen}
        closeEditModal = {() => setEditModalIsOpen(false)}
        training = {training}
        handleUpdate = {handleUpdate}
        isSelectedDateToday = {isSelectedDateToday}
      />

      {responseMessage && (
        <ResponseMessageModal
        responseMessageModalIsOpen = {responseMessageModalIsOpen}
        closeResponseMessageModal = {() => setResponseMessageModalIsOpen(false)}
        responseMessage = {responseMessage}
        />
      )}
    </div>
  );
};

export default Training;