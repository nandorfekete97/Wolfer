import { useState } from 'react'
import './App.css'
import Layout from './Components/Layout/Layout';
import TrainingsTable from './Components/TrainingTable/TrainingsTable';
import Login from './Components/Login/Login';

function App() {
  const [successfulLogin, setSuccessfulLogin] = useState(false);

  return (
    <>
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

          <div className="col-sm-9">
            <TrainingsTable/>
          </div>
        </>
          
        :
          <Login setSuccessfulLogin = {setSuccessfulLogin}/>
        }
      </div>
    </>
  )
}

export default App
