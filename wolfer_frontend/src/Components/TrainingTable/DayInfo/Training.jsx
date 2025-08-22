import './Training.css';
import React, { useState, useEffect } from 'react';
import DeleteTrainingModal from '../../Modals/DeleteTrainingModal';
import EditTrainingModal from '../../Modals/EditTrainingModal';
import UsersByTrainingModal from '../../Modals/UsersByTrainingModal';
import { TrainingTypes, getTrainingTypeLabel } from '../../../Utils/TrainingTypes';
import { toast } from 'react-toastify';
import axios from 'axios';

const Training = ({ training, signedUpTrainingIdsForDay, refreshSignedUpTrainings, refreshDayTrainings, triggerRefresh, showSignUp = true, isSelectedDateToday, formattedDate }) => {
  const [time, setTime] = useState(null);
  const [type, setType] = useState("");
  const [isDisabled, setIsDisabled] = useState(false);
  const [deleteModalIsOpen, setDeleteModalIsOpen] = useState(false);
  const [editModalIsOpen, setEditModalIsOpen] = useState(false);
  const [usersByTrainingModalIsOpen, setUsersByTrainingModalIsOpen] = useState(false);
  const [usersByTraining, setUsersByTraining] = useState([]);
  const [currentUsersCount, setCurrentUsersCount] = useState(0);

  const userId = localStorage.getItem("userId");
  const token = localStorage.getItem("token");

  const maxCapacity = 12;
  const isPast = new Date(training.date).getTime() < Date.now();
  const isFull = currentUsersCount >= maxCapacity;

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

  useEffect(() => {
    checkTrainingCapacity();
  }, [training]);

  const checkTrainingCapacity = async () => {
    const token = localStorage.getItem("token");

    try {
      const response = await axios.get(`${import.meta.env.VITE_API_URL}/UserTraining/GetUsersByTrainingId/${training.id}`,
        {
          headers: {
            "Content-Type": "application/json",
              "Authorization": `Bearer ${token}`
        }
        }
      );

      setCurrentUsersCount((response.data.userEntities || []).length);

    } catch (err) {
      if (err.response)
      {
        toast.error(`Failed to check training capacity: ${err.response.status}`);
      } else {
        toast.error(`Network error while checking training capacity: ${err.message}`);
      }
    }
  };

  const signUpForTraining = async (e) =>{
    e.preventDefault();

    try {
      const response = await axios.get(
        `${import.meta.env.VITE_API_URL}/UserTraining/GetUsersByTrainingId/${training.id}`, 
        {
          headers: {
            "Authorization": `Bearer ${token}`
          }
        }
      );
 
      const currentUsers = response.data.userEntities || [];

      if (currentUsers.length >= maxCapacity)
      {
        toast.error("This training is full.");
        setCurrentUsersCount(currentUsers.length);
        return;
      }
      } catch (err) {
        if (err.response)
        {
          toast.error(`Failed to fetch current training signups. Status: ${err.response.status}`);
          return;
        } else {
          toast.error(`An error occurred fetching data for training. Status: ${err.message}`);
          return;
        }        
    }

    try {
        await axios.post(
          `${import.meta.env.VITE_API_URL}/UserTraining/SignUserUpForTraining/users/${userId}/trainings/${training.id}`, 
          {
            userId: userId,
            trainingId: training.id,
          },
          {
            headers: {
            "Content-Type": "application/json",
              Authorization: `Bearer ${token}`
            },
          }
        );

        toast.success("Successfully signed up for training.");

        }
         catch (err) {
          if (err.response)
          {
            toast.error(response.data?.message || 'Failed to sign up for training.');
          } else {
            toast.error('An error occurred during signing up for training.');
          }
      }
      finally {
      refreshSignedUpTrainings();
    }
  }

  const signOffFromTraining = async (e) =>{
    e.preventDefault();

    try {
        await axios.delete(
          `${import.meta.env.VITE_API_URL}/UserTraining/SignUserOffFromTraining/users/${userId}/trainings/${training.id}`, 
          {
            headers: {
            "Content-Type": "application/json",
              "Authorization": `Bearer ${token}`
            },
        });

        toast.success("Successfully signed off from training.");
        refreshSignedUpTrainings();
        }
        catch (err)
        {
          if (err.response) 
          {
            toast.error(`Failed to sign off from training. Status: ${err.response.status}`);
          } else {
            toast.error(`Network error while signing off from training. Status: ${err.message}`);
          }
        }
    finally {
      refreshSignedUpTrainings();
    }
  }
  
  const showUsersByTraining = async (training) => {
    const token = localStorage.getItem("token");

    try{
      const response = await axios.get(
        `${import.meta.env.VITE_API_URL}/UserTraining/GetUsersByTrainingId/${training.id}`, 
        {
          headers: {
            "Content-Type": "application/json",
              Authorization: `Bearer ${token}`
          }
        }
      );

      setUsersByTraining(response.data.userEntities);
      setUsersByTrainingModalIsOpen(true);

    } catch (err) {
      if (err.response) 
        {
          toast.error(`Failed to show users for training. Status: ${err.response.status}`);
        } else {
          toast.error(`Network error while showing users for training. Status: ${err.message}`);
        }
    }
  }

  const handleDelete = async (e) => {
    e.preventDefault();

    try {
      await axios.delete(
        `${import.meta.env.VITE_API_URL}/Training/DeleteTraining/${training.id}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
          }
        }
      );

      if (refreshSignedUpTrainings) {
        refreshSignedUpTrainings();
      }

      if (refreshDayTrainings) {
        refreshDayTrainings();
      }

      toast.success("Training deleted successfully.");

    } catch (err) {
      if (err.response) {
        toast.error(`Failed to delete training. Status: ${err.response.status}`);
      } else {
        toast.error(`Network error while deleting training. Status: ${err.message}`);
      }
    }
  };

  const handleUpdate = async (updatedTraining) => {
    try {
      await axios.put(
        `${import.meta.env.VITE_API_URL}/Training/UpdateTraining/`, 
          updatedTraining,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
          }
        }
      );

      toast.success("Training updated successfully.");

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

    }
      catch (err) 
      {
        if (err.response)
        {
          return { success: false, message: `Failed to update training. Status: ${err.response.status}` };
        } else {
          return { success: false, message: `Network error while updating training. Status: ${err.message}` };
        }
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
            disabled = {isDisabled || isPast || isFull}
            title={isFull ? "Training is full" : ""}
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
        isFull = {isFull}
      />

      <DeleteTrainingModal
        training = {training}
        formattedDate = {formattedDate}
        deleteModalIsOpen = {deleteModalIsOpen}
        closeDeleteModal = {() => setDeleteModalIsOpen(false)}
        handleDelete = {handleDelete}
      />

      <EditTrainingModal
        editModalIsOpen = {editModalIsOpen}
        closeEditModal = {() => setEditModalIsOpen(false)}
        training = {training}
        formattedDate = {formattedDate}
        handleUpdate = {handleUpdate}
        isSelectedDateToday = {isSelectedDateToday}
      />
    </div>
  );
};

export default Training;