import React, { useState } from 'react'

const Login = ({setSuccessfulLogin}) => {

const [email, setEmail] = useState("");
const [password, setPassword] = useState("");
const [error, setError] = useState("");

const handleLogin = async (e) => {
    e.preventDefault();

    if (email.length == 0 || password.length == 0) {
        setError("Email and password cannot be empty.");
        return;
      }
  
    try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/Auth/Login`, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify({
            email: email,
            password: password,
            }),
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem('token', data.token);
            localStorage.setItem('userId', data.userId);
            setSuccessfulLogin(true);
        } else {
            const data = await response.json();
            setError(data.message || 'Login failed');
        }
    } catch (error) {
        setError('An error occurred during login');
    }
}

  return (
    <div className="col-sm-12">
        <form className="login-form">
            <div className="col-sm-12">
                <label>Email:</label>
                <input className="email" type="text" value={email} onChange={(e) => setEmail(e.target.value)}/>
            </div>
            <div>
                <label>Password:</label>
                <input className="password" type="password" value={password} onChange={(e) => setPassword(e.target.value)}/>
            </div>
            <div>
                {error && <p style={{ color: 'red' }}>{error}</p>}
                <button onClick={(e) => handleLogin(e)}>
                    LOGIN
                </button>
            </div> 
        </form>
        Don't have a profile?
        <p>Register here</p>
    </div>
  )
}

export default Login
