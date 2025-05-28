import { useEffect, useState } from 'react';
import './App.css';
import Layout from './Components/Layout/Layout';
import TrainingsTable from './Components/TrainingTable/TrainingsTable';
import Login from './Components/Login/Login';
import Profile from './Components/Profile/Profile';
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import Planning from './Components/Planning/Planning';

function App() {
  const [successfulLogin, setSuccessfulLogin] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      setSuccessfulLogin(true);
    }
  }, []);

  return (
    <Router>
      <div className="col-sm-12">
        <h1 className="gym-name">WOLFER</h1>
      </div>

      <div className="container col-sm-12">
        {successfulLogin ? (
          <>
            <div className="col-sm-3">
              <Layout setSuccessfulLogin={setSuccessfulLogin} />
            </div>
            <div className="col-sm-9">
              <Routes>
                <Route path="/" element={<Navigate to="/trainings" />} />
                <Route path="*" element={<Navigate to="/trainings" />} />
                <Route path="/trainings" element={<TrainingsTable />} />
                <Route path="/profile" element={<Profile />} />
                <Route path='/planning' element={<Planning/>}/>
              </Routes>
            </div>
          </>
        ) : (
          <div className="col-sm-12">
            <Routes>
              <Route path="*" element={<Login setSuccessfulLogin={setSuccessfulLogin} />} />
            </Routes>
          </div>
        )}
      </div>
    </Router>
  );
}

export default App;