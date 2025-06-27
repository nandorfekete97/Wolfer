import './Layout.css';
import { useNavigate } from 'react-router-dom';

const Layout = ({setSuccessfulLogin}) => {

  const navigate = useNavigate();

  const handleLogout = (e) => {
    e.preventDefault();
    setSuccessfulLogin(false);
    localStorage.setItem('token', "");
  }

  const goToPlanning = () => {
    navigate('/planning');
  }

  const goToTrainings = () => {
    navigate('/trainings');
  }

  const goToTrainingHistory = () => {
    navigate('/training-history');
  }

  const goToProfile = () => {
    navigate('/profile');
  }

  const goToPersonalRecords = () => {
    navigate('/personal-records');
  }

  return (
      <div className="align-items-start">
          <ul className="list-unstyled">
            <li className="sidebar-item" onClick={(goToTrainings)}>Trainings</li>
            <li className="sidebar-item" onClick={(goToProfile)}>Profile</li>
            <li className="sidebar-item" onClick={(goToPersonalRecords)}>PRs</li>
            <li className="sidebar-item" onClick={(goToTrainingHistory)}>Training History</li>
            <li className="sidebar-item" onClick={(goToPlanning)}>Planning</li>            
            <li>
              <button className="sidebar-item" id="logout-button" onClick={(e) => handleLogout(e)}>
                Logout
              </button>
            </li>
          </ul>
      </div>
  );
};

export default Layout;
