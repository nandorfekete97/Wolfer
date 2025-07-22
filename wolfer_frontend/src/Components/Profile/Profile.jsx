import React, { useEffect, useState } from 'react';
import EditUserModal from '../Modals/EditUserModal';
import ChangePasswordModal from '../Modals/ChangePasswordModal';
import './Profile.css';

const Profile = () => {
  const [user, setUser] = useState('');
  const [username, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [changePasswordModalOpen, setChangePasswordModalOpen] = useState(false);

  const getUserInfo = async () => {
    const userId = localStorage.getItem("userId");
    const token = localStorage.getItem("token");

    const res = await fetch(`${import.meta.env.VITE_API_URL}/User/GetUserById/${userId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });

    if (res.ok) {
      const data = await res.json();
      setUser(data.userEntity);
      setUserName(data.userEntity.userName);
      setEmail(data.userEntity.email);
    } else {
      console.error("Failed to fetch user data. Status:", res.status);
    }
  };

  const handleUpdate = async (updatedUser) => {
    const token = localStorage.getItem("token");
    const userId = localStorage.getItem("userId");

    const updatedUserO = {
      id: userId,
      userName: updatedUser.userName,
      email: updatedUser.email
    };

    const res = await fetch(`${import.meta.env.VITE_API_URL}/User/UpdateUser`, {
      method: 'PUT',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(updatedUserO)
    });

    if (res.ok) {
      await getUserInfo();
    } else {
      console.error("Failed to update user. Status:", res.status);
    }
  };

  const handlePasswordChange = async (updatedUser) => {
    const token = localStorage.getItem("token");
    const userId = localStorage.getItem("userId");

    const updatedUserO = {
      userId: userId,
      oldPassword: updatedUser.oldPassword,
      newPassword: updatedUser.newPassword
    }

    const res = await fetch(`${import.meta.env.VITE_API_URL}/User/ChangePassword`, {
      method: 'PUT',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(updatedUserO)
    });

    if (res.ok) {
      await getUserInfo();
    } else {
      console.error("Failed to change password. Status:", res.status);
    } 
  }

  useEffect(() => {
    getUserInfo();
  }, []);

  return (
  <div className="profile-container">
    <h2>Profile</h2>

    <div className='user-info-group'>
      <label>Username: {username}</label>
    </div>

    <div className='user-info-group'>
      <label>Email: {email}</label>
    </div>

    <button className="edit-profile-button" onClick={() => setEditModalOpen(true)}>
      Edit Profile
    </button>

    <EditUserModal
      isOpen={editModalOpen}
      closeModal={() => setEditModalOpen(false)}
      user={user}
      handleUpdate={handleUpdate}
    />

    <button className="change-password-button" onClick={() => setChangePasswordModalOpen(true)}>
      Change Password
    </button>

    <ChangePasswordModal
      isOpen={changePasswordModalOpen}
      closeModal={() => setChangePasswordModalOpen(false)}
      user={user}
      handlePasswordChange={handlePasswordChange}
    />

  </div>
  );
};

export default Profile;