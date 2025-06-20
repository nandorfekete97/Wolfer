import React, { useEffect, useState } from 'react'
import Modal from 'react-modal';

const EditUserModal = ({ isOpen, closeModal, user, handleUpdate }) => {
    const [username, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [responseMessage, setResponseMessage] = useState("");

    useEffect(() => {
        if (user) {
            setUserName(user.userName || "");
            setEmail(user.email || ""); 
        }
    }, [user]);

    useEffect(() => {
        if (isOpen && user) {
            setUserName(user.userName || "");
            setEmail(user.email || "");
            setResponseMessage("");
        }
    }, [isOpen, user]);

    const handleSubmit = (e) => {
        e.preventDefault();

        if (!username || !email) {
            setResponseMessage("Username and Email are required.");
            return;
        }

        const updatedUser = {
            ...user,
            userName: username,
            email: email,
        };

        handleUpdate(updatedUser);
        closeModal();
    }

  return (
    <Modal className='modal-content' isOpen={isOpen} contentLabel="Edit User Profile" ariaHideApp={false}>
      <h2>Edit Profile</h2>
      <form onSubmit={handleSubmit}>
        <div className='form-group'>
            <label>Username:</label>
            <input 
                type='text' 
                value={username} 
                onChange={(e) => setUserName(e.target.value)} 
                className='form-control'
            />
        </div>

        <div className='form-group'>
            <label>Email:</label>
            <input
                type='email'
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className='form-control'
            />
        </div>

        <button type='submit' className='btn btn-primary'>Save</button>
        {responseMessage && <p>{responseMessage}</p>}
      </form>

      <button className='btn btn-secondary mt-2' onClick={closeModal}>Cancel</button>
    </Modal>
  );
}

export default EditUserModal;
