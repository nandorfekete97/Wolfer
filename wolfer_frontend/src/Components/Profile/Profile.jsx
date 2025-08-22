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

    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/ProfilePhoto/GetByUserId/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        responseType: "blob",
      });

      const imageUrl = URL.createObjectURL(res.data);
      setProfilePhoto(imageUrl);
    }
      catch (err) {
        if (err.response) {
          setProfilePhoto("");
          console.error("Failed to fetch user profile photo. Status:", res.status);
        } else {
          console.error("Network error:", err.message);
        }
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

    try {
      await axios.put(`${import.meta.env.VITE_API_URL}/User/UpdateUser`, 
        updatedUserO,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
        }
      );

      toast.success("Profile updated successfully!");
      await getUserInfo();
    } catch (err) {
      if (err.response)
      {
        toast.error(`Failed to update user. Status: ${err.response.status}`);
      } else {
        toast.error("Network error while updating user.");
      }
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
      await axios.put(`${import.meta.env.VITE_API_URL}/User/ChangePassword`, 
        updatedUserO,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
        }
      );

      toast.success("Password changed successfully!");
      await getUserInfo();
      return true;
    } catch (err) {
      if (err.response)
      {
        toast.error(`Failed to change password. Status: ${err.response.status}`);
      } else {
        toast.error("Network error while changing password.");
      }
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