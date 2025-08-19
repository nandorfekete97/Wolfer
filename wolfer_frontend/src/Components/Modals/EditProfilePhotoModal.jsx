import { useEffect, useState } from "react";
import ProfilePhoto from "../Profile/ProfilePhoto";
import Modal from 'react-modal';
import { toast } from "react-toastify";

const EditProfilePhotoModal = ({isOpen, closeModal, userId, token, src, refreshUserData}) => {

    const [profilePhoto, setProfilePhoto] = useState("");

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

    return (
        <Modal 
            className='modal-content' 
            isOpen={isOpen}
            onRequestClose={closeModal}
        >
            <div>
                <h2>Edit Profile Photo</h2>
                <ProfilePhoto src={src}/>
                <button className="modal-btn btn btn-danger" onClick={() => deleteProfilePhoto()}> DELETE CURRENT PROFILE PHOTO</button>
                <button onClick={closeModal}>CANCEL</button>
            </div>
        </Modal>
        
    )
}

export default EditProfilePhotoModal;