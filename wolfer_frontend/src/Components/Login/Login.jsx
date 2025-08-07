import React, { useEffect, useState } from 'react';
import './Login.css'; 
import Register from '../Register/Register';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

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
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Auth/Login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token);
        localStorage.setItem('userId', data.userId);

        const token = localStorage.getItem("token");

        const payloadBase64 = token.split('.')[1];
        const decodedPayload = JSON.parse(atob(payloadBase64));
        const role = decodedPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        localStorage.setItem('role', role);

        setSuccessfulLogin(true);
      } else {
        const data = await response.json();
        setError(data.message || 'Login failed');
      }
    } catch (error) {
      setError('An error occurred during login');
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
