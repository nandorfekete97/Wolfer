import Sidebar from "./Sidebar";

const Layout = ({setSuccessfulLogin}) => {

  const handleLogout = (e) => {
    e.preventDefault();
    setSuccessfulLogin(false);
    localStorage.setItem('token', "");
  }

  return (
      <div className="align-items-start">
          <Sidebar />
          <button onClick={(e) => handleLogout(e)}>
            Logout
          </button>
      </div>
  );
};

export default Layout;
