import Sidebar from "./Sidebar";
import Planning from "../Planning/Planning";
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

  return (
      <div className="align-items-start">
          {/* <Sidebar /> */}
          <ul className="list-unstyled">
            <li className="sidebar-item" onClick={(goToTrainings)}>Trainings</li>
            <li className="sidebar-item">Profile</li>
            <li className="sidebar-item">PRs</li>
            <li className="sidebar-item">Training History</li>
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
