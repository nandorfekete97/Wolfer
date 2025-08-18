import React from "react";
import DefaultProfilePhoto from '../../Images/profile-photo-default.jpg';

const ProfilePhoto = ({ src, alt = "User profile photo" }) => {
    return (
        <div>
            <img
                src={src || DefaultProfilePhoto}
                alt={alt}
                style={{ width: "96px", height: "96px", borderRadius: "50%", objectFit: "cover", marginBottom: "16px"}}
            />
        </div>
    );
};

export default ProfilePhoto;