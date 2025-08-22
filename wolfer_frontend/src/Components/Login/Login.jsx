import React, { useEffect, useState } from 'react';
import './Login.css'; 
import Register from '../Register/Register';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import axios from 'axios';

const Login = ({ setSuccessfulLogin }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();

  const goToRegister = () => {
    navigate('/register');
  }

  const handleLogin = async (e) => {
    e.preventDefault();

    if (email.length === 0 || password.length === 0) {
      setError("Email and password cannot be empty.");
      return;
    }

    try {
      const response = await axios.post(
        `${import.meta.env.VITE_API_URL}/Auth/Login`, 
        {
          email,
          password,
        },
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );

      localStorage.setItem('token', response.data.token);
      localStorage.setItem('userId', response.data.userId);

      const token = localStorage.getItem("token");

      const payloadBase64 = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payloadBase64));
      const role = decodedPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      localStorage.setItem('role', role);

      setSuccessfulLogin(true);

    } catch (err)
    {
      if (err.response)
      {
        toast.error(`Login failed. Status: ${err.response.status}`);
      } else {
        toast.error(`Network error during login: ${err.message}`);
      }
    }
  };

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleLogin}>
        <div className="form-group">
          <label>Email:</label>
          <input
            type="text"
            className="login-input"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label>Password:</label>
          <input
            type="password"
            className="login-input"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>

        {error && <div className="login-error">{error}</div>}

        <button 
          type="submit" 
          className="login-button">
          LOG IN
        </button>
      </form>

      <div className="login-footer">
        <small>Don't have a profile?</small>
        <p className="register-link" onClick={goToRegister}>Register here</p>
      </div>
    </div>
  );
};

export default Login;
