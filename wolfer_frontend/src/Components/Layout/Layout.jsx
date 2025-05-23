import Sidebar from "./Sidebar";

const Layout = ({setSuccessfulLogin}) => {

  const handleLogout = (e) => {
    e.preventDefault();
    setSuccessfulLogin(false);
    localStorage.setItem('token', "");
  }

  return (
      <div className="align-items-start">
          {/* <Sidebar /> */}
          <ul className="list-unstyled">
            <li className="sidebar-item">Trainings</li>
            <li className="sidebar-item">Profile</li>
            <li className="sidebar-item">PRs</li>
            <li className="sidebar-item">Training History</li>
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
