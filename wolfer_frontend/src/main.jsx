import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.jsx'
import { BrowserRouter as Router, Routes, Route, Navigate, BrowserRouter } from 'react-router-dom';
import Register from './Components/Register/Register.jsx';
import Layout from './Components/Layout/Layout.jsx';
import TrainingsTable from './Components/TrainingTable/TrainingsTable.jsx';
import UserTrainings from './Components/UserTrainings.jsx/UserTrainings.jsx';
import Modal from 'react-modal';

Modal.setAppElement('#root');

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App/>
  </StrictMode>,
)
