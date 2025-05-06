import React, { useState } from 'react';

const Profile = () => {
  const [firstName, setFirstName] = useState("John");
  const [lastName, setLastName] = useState("Doe");
  const [userName, setUserName] = useState("johndoe123");
  const [email, setEmail] = useState("john@example.com");

  const [isEditable, setIsEditable] = useState(false);

  const toggleEdit = (e) => {
    e.preventDefault();
    setIsEditable(!isEditable);
    console.log("firstname: ", firstName);
  };

  return (
    <div className="col-sm-9">
      <h2>Profile</h2>
      <form>
        <div className="form-group">
          <label>Firstname:</label>
          <input
            type="text"
            value={firstName}
            disabled={!isEditable}
            onChange={(e) => setFirstName(e.target.value)}
            className="form-control"
          />
        </div>

        <div className="form-group">
          <label>Lastname:</label>
          <input
            type="text"
            value={lastName}
            disabled={!isEditable}
            onChange={(e) => setLastName(e.target.value)}
            className="form-control"
          />
        </div>

        <div className="form-group">
          <label>Username:</label>
          <input
            type="text"
            value={userName}
            disabled={!isEditable}
            onChange={(e) => setUserName(e.target.value)}
            className="form-control"
          />
        </div>

        <div className="form-group">
          <label>Email:</label>
          <input
            type="email"
            value={email}
            disabled={!isEditable}
            onChange={(e) => setEmail(e.target.value)}
            className="form-control"
          />
        </div>

        <button className="btn btn-primary mt-3" onClick={toggleEdit}>
          {isEditable ? 'Save Profile' : 'Edit Profile'}
        </button>
      </form>
    </div>
  );
};

export default Profile;
