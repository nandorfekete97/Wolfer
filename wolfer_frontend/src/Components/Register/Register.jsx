import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Register.css'; 

const Register = () => {

  const navigate = useNavigate();

  const goToTrainings = () => {
    navigate('/trainings');
  }

  const goToLogin = () => {
    navigate('/login');
  }
  
  const [formData, setFormData] = useState({
    email: '',
    username: '',
    password: '',
    confirmPassword: '',
  });

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (formData.password !== formData.confirmPassword) {
      setError("Passwords don't match");
      return;
    }

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/Auth/Register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: formData.email,
          userName: formData.username,
          password: formData.password,
        }),
      });

      if (response.ok) {
        setSuccess('Registration successful!');
        setError('');
        goToTrainings();
      } else {
        const data = await response.json();
        setError(data.message || 'Registration failed');
        setSuccess('');
      }
    } catch (error) {
      setError('An error occurred during registration');
    }
  };

  return (
    <div className="register-container">
      <h2>Register</h2>
      <form className="register-form" onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Email:</label>
          <input 
            type="email" 
            name="email" 
            className="register-input" 
            value={formData.email} 
            onChange={handleChange} required 
          />
        </div>

        <div className="form-group">
          <label>Username:</label>
          <input 
            type="text" 
            name="username" 
            className="register-input"
            value={formData.username} 
            onChange={handleChange} required 
            />
        </div>

        <div className="form-group">
          <label>Password:</label>
          <input 
            type="password" 
            name="password" 
            className="register-input"
            value={formData.password} 
            onChange={handleChange} required 
          />
        </div>

        <div className="form-group">
          <label>Confirm Password:</label>
          <input 
            type="password" 
            name="confirmPassword" 
            className="register-input"
            value={formData.confirmPassword} 
            onChange={handleChange} required 
          />
        </div>

        {error && <p className="register-error" style={{ color: 'red' }}>{error}</p>}
        {success && <p style={{ color: 'green' }}>{success}</p>}

        <button 
          type="submit" 
          className="register-button">
          REGISTER
        </button>

      </form>

      <div className="register-footer">
        <small>Already have a profile?</small>
        <p className="login-link" onClick={goToLogin}>Login here</p>
      </div>
    </div>
  );
};

export default Register;
