import { useEffect, useState } from 'react';
import './App.css';
import Layout from './Components/Layout/Layout';
import TrainingsTable from './Components/TrainingTable/TrainingsTable';
import Login from './Components/Login/Login';
import Profile from './Components/Profile/Profile';
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import Planning from './Components/Planning/Planning';
import TrainingHistory from './Components/TrainingHistory/TrainingHistory';
import PersonalRecords from './Components/PersonalRecords/PersonalRecords';
import Register from './Components/Register/Register';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import AdminRoute from './Components/AdminRoute/AdminRoute';

function App() {
  const [successfulLogin, setSuccessfulLogin] = useState(false);

  useEffect(() => {

    const token = localStorage.getItem('token');

    if (token) {
      setSuccessfulLogin(true);
    }
    else {
      setSuccessfulLogin(false);
    }

    const handleTokenChange = (event) => {
      if (event.key === "token")
      {
            console.log("token: ", token);

        if (!localStorage.getItem("token"))
        {
          setSuccessfulLogin(false);
        }
      }
    }

    window.addEventListener('storage', handleTokenChange);
  }, []);

  return (
    <Router>
      <div className = "col-sm-12">
        <h1 className = "gym-name">WOLFER</h1>
      </div>

      <div className = "container col-sm-12">
        {successfulLogin ? (
          <>
            <div className = "col-sm-3">
              <Layout setSuccessfulLogin = {setSuccessfulLogin} />
            </div>
            <div className="col-sm-9">
              <Routes>
                <Route path = "/" element={<Navigate to ="/login" />} />
                <Route path = "*" element={<Navigate to="/trainings" />} />
                <Route path = "/trainings" element={<TrainingsTable />} />
                <Route path = "/profile" element={<Profile />} />
                <Route path = '/training-history' element={<TrainingHistory/>} />
                <Route path = '/personal-records' element={<PersonalRecords/>}/>
                <Route path='/planning' 
                  element={<AdminRoute>
                            <Planning/>
                          </AdminRoute>}
                />
              </Routes>
            </div>
          </>
        ) : (
          <div className="col-sm-12">
            <Routes>
              <Route path="/login" element={<Login setSuccessfulLogin={setSuccessfulLogin} />} />
              <Route path='/register' element={<Register/>}/>
              <Route path="*" element={<Navigate to="/login" />}/>
            </Routes>
          </div>
        )}
      </div>
      <ToastContainer
          position="bottom-right"
          autoClose={2000}
          hideProgressBar
          closeOnClick
          pauseOnHover
          pauseOnFocusLoss={false}
          draggable
      />
    </Router>
  );
}

export default App;