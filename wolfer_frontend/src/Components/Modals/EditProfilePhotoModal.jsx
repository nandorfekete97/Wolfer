import { useEffect, useState } from "react";
import ProfilePhoto from "../Profile/ProfilePhoto";
import Modal from 'react-modal';
import { toast } from "react-toastify";

const EditProfilePhotoModal = ({isOpen, closeModal, userId, token, src, refreshUserData}) => {

    const [selectedFile, setSelectedFile] = useState(null);

    const deleteProfilePhoto = async () => {
        try {
            const res = await fetch(`${import.meta.env.VITE_API_URL}/ProfilePhoto/DeleteProfilePhoto/${userId}`, {
                    method: 'DELETE',
                    headers: {
                    "Authorization": `Bearer ${token}`,
                }
            });

            if (res.ok) {
                toast.success("Successfully deleted profile photo.");
                closeModal();
            } else {
                toast.error("Failed to delete profile photo. Status:", res.status);
            }
        } catch (error) {
            toast.error('An error occurred during deleting profile photo.');
        }
        finally {
            refreshUserData();
        }
    }

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

            const res = await fetch(`${import.meta.env.VITE_API_URL}/ProfilePhoto/Upload/`, {
                    method: 'POST',
                    headers: {
                    "Authorization": `Bearer ${token}`,
                    },
                    body: formData
            });

            if (res.ok) {
                toast.success("Successfully added new profile photo.");
                closeModal();
            } else {
                const error = await res.json();
                toast.error("Failed to upload profile photo: " + (error.message || res.status));
            }
        } catch (error) {
            toast.error('An error occurred during uploading profile photo.');
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

                <input
                    type="file"
                    accept="image/*"
                    onChange={handleFileChange}
                    className="mb-2"
                />

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