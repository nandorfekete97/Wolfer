import { data } from 'react-router-dom';
import './Training.css'
import React, { useState, useEffect } from 'react';
import DeleteModal from '../../Modals/DeleteTrainingModal';
import EditTrainingModal from '../../Modals/EditTrainingModal';

const Training = ({ training, signedUpTrainingIdsForDay, refreshSignedUpTrainings, refreshDayTrainings, showSignUp = true }) => {
  const [time, setTime] = useState(null);
  const [type, setType] = useState("");
  const [isDisabled, setIsDisabled] = useState(false);
  const [responseMessage, setResponseMessage] = useState("");
  const [deleteModalIsOpen, setDeleteModalIsOpen] = useState(false);
  const [editModalIsOpen, setEditModalIsOpen] = useState(false);

  const today = new Date();

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
            setResponseMessage("Successfully signed up for training.");
            refreshSignedUpTrainings();
        } else {
            const data = await response.json();
            setResponseMessage(data.message || 'Failed to sign up for training.');
        }
    } catch (error) {
        setResponseMessage('An error occurred during signing up for training.');
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
        setResponseMessage("Training was deleted successfully.");
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
        setResponseMessage(data.message || 'Training could not be deleted.');
      }
    } catch (error) {
      setResponseMessage('An error occurred during deleting training.');
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
      setResponseMessage(response.ok ? "Training updated successfully." : data.message || "Failed to update training.");
      if (response.ok) {
        refreshDayTrainings?.();
        refreshSignedUpTrainings?.();
      }
    } catch (error) {
      setResponseMessage("An error occurred during update.");
    }
  }

  return (
    <div className = "training">
      <h5 className = "training-info col-sm-4">{time}</h5>
      <h5 className = "training-info col-sm-4">{type}</h5>

      {showSignUp ? (
        signedUpTrainingIdsForDay.includes(training.id) ? (
          <button
            className = "btn btn-sm btn-danger col-sm-4"
            onClick = {signOffFromTraining}
            disabled = {today.toISOString() > training.date}
          >
            Sign Off
          </button>
        ) : (
          <button
            className = "btn btn-sm btn-success col-sm-4"
            onClick = {signUpForTraining}
            disabled = {isDisabled || today.toISOString() > training.date}
          >
            Sign Up
          </button>
        )
      ) : (
        <div className = "col-sm-4 d-flex gap-2">
          <button
            className = "btn btn-sm btn-primary"
            disabled = {today.toISOString() > training.date}
            onClick = {() => setEditModalIsOpen(true)}
          >
            Edit
          </button>
          <button
            className = "btn btn-sm btn-danger"
            disabled = {today.toISOString() > training.date}
            onClick = {(e) => setDeleteModalIsOpen(true)}
          >
            Delete
          </button>
        </div>
      )}

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
      />

      {responseMessage && (
        <div className = "response-message mt-2 text-info col-12">
          {responseMessage}
        </div>
      )}
    </div>
  );
};

export default Training;