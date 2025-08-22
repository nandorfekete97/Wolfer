import { useEffect, useState } from "react";
import ProfilePhoto from "../Profile/ProfilePhoto";
import './EditProfilePhotoModal.css';
import Modal from 'react-modal';
import { toast } from "react-toastify";
import axios from "axios";

const EditProfilePhotoModal = ({isOpen, closeModal, userId, token, src, refreshUserData}) => {

    const [selectedFile, setSelectedFile] = useState(null);

    const deleteProfilePhoto = async () => {
        try {
            await axios.delete(
                `${import.meta.env.VITE_API_URL}/ProfilePhoto/DeleteProfilePhoto/${userId}`, 
                {
                    headers: {
                    "Authorization": `Bearer ${token}`,
                    }
                });
            
            toast.success("Successfully deleted profile photo.");
            closeModal();

        } catch (err) 
        {
            if (err.response) 
            {
                toast.error(`Failed to delete profile photo. Status: ${err.response.status}`);
            } else {
                toast.error(`Network error while deleting photo: ${err.message}`);
            }
        }
    };

    const updateProfilePhoto = async () => {

        if (!selectedFile) {
            toast.error("Please select a file first.");
            return;
        }

        try {
            const formData = new FormData();
            formData.append("UserId", userId);
            formData.append("Photo", selectedFile);
            formData.append("ContentType", selectedFile.type);

            await axios.post(
                `${import.meta.env.VITE_API_URL}/ProfilePhoto/Upload/`, 
                    formData,
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                            "Content-Type": "multipart/form-data",
                        },
                    }
            );

            toast.success("Successfully added new profile photo.");
            closeModal();

        } catch (err)
        {
            if (err.response)
            {
                toast.error(`Failed to add new profile photo. Status: ${err.response.status}`);
            } else {
                toast.error(`Network error while adding profile photo: ${err.message}`);
            }
        }
        finally {
            refreshUserData();
        }
    }

    const handleFileChange = (e) => {
        setSelectedFile(e.target.files[0]);
    }

    return (
        <Modal 
            className='modal-content' 
            isOpen={isOpen}
            onRequestClose={closeModal}
        >
            <div>
                <h2>Edit Profile Photo</h2>
                <ProfilePhoto src={src}/>

                <div className="file-input-wrapper">
                    <input
                        type="file"
                        accept="image/*"
                        onChange={handleFileChange}
                        className="file-input"
                    />
                </div>

                <button 
                    className='btn btn-primary mb-2' 
                    onClick={updateProfilePhoto}
                > 
                    UPLOAD NEW PROFILE PICTURE 
                </button>

                <button 
                    className="modal-btn btn btn-danger" 
                    onClick={async () => {
                        const deleted = await deleteProfilePhoto();
                        if (deleted) closeModal();
                        refreshUserData();
                    }}
                > 
                    DELETE CURRENT PROFILE PHOTO
                </button>

                <button 
                    className='btn btn-secondary mt-2' 
                    onClick={closeModal}
                > CANCEL
                </button>
            </div>
        </Modal>
        
    )
}

export default EditProfilePhotoModal;