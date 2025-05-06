import { useState } from 'react'
import './App.css'
import Layout from './Components/Layout/Layout';
import TrainingsTable from './Components/TrainingTable/TrainingsTable';
import Login from './Components/Login/Login';
import Profile from './Components/Profile/Profile';
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";

function App() {
  const [successfulLogin, setSuccessfulLogin] = useState(false);

  return (
    <>
    <Router>
      <div className="col-sm-12">
        <h1 className="gym-name">WOLFER</h1>
      </div>

      <div className="container col-sm-12">
        {
        successfulLogin ?
        <>
          <div className="col-sm-3">
            <Layout setSuccessfulLogin={setSuccessfulLogin}/>
          </div>

          {/* // wouldn't it make sense to create routing here? in case of successful login, 
          // we want to be able to switch between sidebar components */}

          <div className="col-sm-9">
            {/* <TrainingsTable/> */}
            <Routes>
                <Route path="/" element={<Navigate to="/trainings" />} />
                <Route path="/trainings" element={<TrainingsTable />} />
                <Route path="/profile" element={<Profile />} />
                {/* Add more routes like PRs, History, etc. */}
              </Routes>
          </div>
        </>
          
        :
          <Login setSuccessfulLogin = {setSuccessfulLogin}/>
        }
      </div>
      </Router>
    </>
  )
}

export default App
