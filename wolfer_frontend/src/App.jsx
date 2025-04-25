import { useState } from 'react'
import './App.css'
import Layout from './Components/Layout/Layout';
import TrainingsTable from './Components/TrainingTable/TrainingsTable';

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
        <div className="col-sm-12">
          <h1 className="gym-name">WOLFER</h1>
        </div>
        <div className="container col-sm-12">
          <div className="col-sm-3">
            <Layout/>
          </div>

          <div className="col-sm-9">
            <TrainingsTable/>
          </div>
        </div>
    </>
  )
}

export default App
