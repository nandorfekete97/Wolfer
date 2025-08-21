import React, { useEffect, useState } from 'react';
import EditUserModal from '../Modals/EditUserModal';
import ChangePasswordModal from '../Modals/ChangePasswordModal';
import EditProfilePhotoModal from '../Modals/EditProfilePhotoModal';
import ProfilePhoto from './ProfilePhoto';
import { toast } from 'react-toastify';
import './Profile.css';
import axios from "axios";

const Profile = () => {
  const [user, setUser] = useState('');
  const [username, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [changePasswordModalOpen, setChangePasswordModalOpen] = useState(false);
  const [editProfilePhotoModalOpen, setEditProfilePhotoModalOpen] = useState(false);
  const [profilepPhoto, setProfilePhoto] = useState("");

  const userId = localStorage.getItem("userId");
  const token = localStorage.getItem("token");

  const getUserInfo = async () => {

    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/User/GetUserById/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      const data = res.data;
      setUser(data.userEntity);
      setUserName(data.userEntity.userName);
      setEmail(data.userEntity.email);
    }
     catch (err) {
      if (err.response) {
        console.error("Failed to fetch user data. Status:", err.response.status);
      }
    }
  };

  const getUserProfilePhoto = async () => {
    const res = await fetch(`${import.meta.env.VITE_API_URL}/ProfilePhoto/GetByUserId/${userId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    });

    if (res.ok) {
      const blob = await res.blob();
      const imageUrl = URL.createObjectURL(blob);
      setProfilePhoto(imageUrl);
    } else {
      setProfilePhoto("");
      console.error("Failed to fetch user profile photo. Status:", res.status);
    }
  };

  const refreshUserData = async () => {
    getUserInfo();
    getUserProfilePhoto();
  }

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
      toast.success("Profile updated successfully!");
      await getUserInfo();
    } else {
      toast.error("Failed to update user. Status:", res.status);
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

    try {
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
        toast.success("Password changed successfully!");
        return true;
      } else {
        const errorData = await res.json();
        toast.error(errorData.message || "Failed to change password.");
        return false;
      } 
    } catch (err) {
      toast.error("Network error:", err);
      return false;
    }
  };

  useEffect(() => {
    refreshUserData();
  }, []);

  return (
  <div className="profile-container">
    <h2>Profile</h2>
    
    <div>
      <ProfilePhoto 
        src={profilepPhoto} 
        onClick={() => setEditProfilePhotoModalOpen(true)}
      />
    </div>

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

    <EditProfilePhotoModal
      isOpen={editProfilePhotoModalOpen}
      closeModal={() => setEditProfilePhotoModalOpen(false)}
      userId={userId}
      token={token}
      src={profilepPhoto}
      refreshUserData={refreshUserData}
    />

  </div>
  );
};

export default Profile;