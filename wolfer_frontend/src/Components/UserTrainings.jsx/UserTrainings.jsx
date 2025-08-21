import React, { useState, useEffect } from 'react';
import axios from "axios";
import { toast } from 'react-toastify';

const UserTrainings = ({ userId }) => {
  const [trainings, setTrainings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const token = localStorage.getItem("token");

  useEffect(() => {
    const fetchPastTrainings = async () => {

      try {
        const response = await axios.get(`${import.meta.env.VITE_API_URL}/UserTraining/GetPastTrainingsForUser/${userId}`,
          {
            headers: {
            "Content-Type": "application/json",
              "Authorization": `Bearer ${token}`
            }
          }
        );

        setTrainings(response.data);
      } 
        catch (err) {
          if (err.response)
          {
            toast.error("Failed to fetch past trainings. Status:", err.response.status);
          } else {
            toast.error("Network error while fetching past trainings.");
          }
      } finally {
        setLoading(false);
      }
    };

    if (userId) {
      fetchPastTrainings();
    }
  }, [userId]);

  if (loading) return <p>Loading past trainings...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <h2>Past Trainings for User ID: {userId}</h2>
      {trainings.length === 0 ? (
        <p>No past trainings found for this user.</p>
      ) : (
        <ul>
          {trainings.map((training) => (
            <li key={training.time}>
              <h5>{training.type}</h5> <br />
              <h5>Training Date: {new Date(training.date).toLocaleDateString()}</h5>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default UserTrainings;
