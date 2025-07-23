import React, { useEffect, useState } from 'react'
import Modal from 'react-modal';

const ChangePasswordModal = ({ isOpen, closeModal, handlePasswordChange }) => {

    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [newPasswordAgain, setNewPasswordAgain] = useState("");
    const [responseMessage, setResponseMessage] = useState("");

    useEffect(() => {
        if (isOpen) {
            setOldPassword("");
            setNewPassword("");
            setNewPasswordAgain("");
            setResponseMessage("");
        }
    }, [isOpen]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!oldPassword || !newPassword || !newPasswordAgain) {
            setResponseMessage("All fields are required.");
            return;
        }

        if (newPassword != newPasswordAgain)
        {
            setResponseMessage("Password inputs differ.");
            return;
        }

        const updatedUser = {
            oldPassword,
            newPassword
        };

        const success = await handlePasswordChange(updatedUser);

        if (success)
        {
            closeModal();
        } else {
            setResponseMessage("Failed to change password. Please check your original password or try again.");
        }
    }

  return (
    <Modal className='modal-content' isOpen={isOpen} contentLabel="Change Password" ariaHideApp={false}>
      <h2>Change Password</h2>
      <form onSubmit={handleSubmit}>
        <div className='form-group'>
            <label>Old Password:</label>
            <input 
                type='password' 
                value={oldPassword} 
                onChange={(e) => setOldPassword(e.target.value)} 
                className='form-control'
            />
        </div>

        <div className='form-group'>
            <label>New Password:</label>
            <input 
                type='password' 
                value={newPassword} 
                onChange={(e) => setNewPassword(e.target.value)} 
                className='form-control'
            />
        </div>

        <div className='form-group'>
            <label>New Password Again:</label>
            <input 
                type='password' 
                value={newPasswordAgain} 
                onChange={(e) => setNewPasswordAgain(e.target.value)} 
                className='form-control'
            />
        </div>

        <button type='submit' className='btn btn-primary'>Execute</button>
        {responseMessage && <p>{responseMessage}</p>}
      </form>

      <button className='btn btn-secondary mt-2' onClick={closeModal}>Cancel</button>
    </Modal>
  );
}

export default ChangePasswordModal;
