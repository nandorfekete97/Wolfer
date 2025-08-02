import { useEffect, useState } from "react";
import Modal from "react-modal";
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';

const UsersByTrainingModal = ({training, trainingTime, trainingType, usersByTraining, usersByTrainingModalIsOpen, closeUsersByTrainingModal}) => {

    const [localUsers, setLocalUsers] = useState(usersByTraining || []);
    const [date, setDate] = useState('');
    const [time, setTime] = useState(trainingTime || '');

    useEffect(() => {
        const date = new Date(training.date).toISOString().split('T')[0];
        setLocalUsers(usersByTraining || []);
        setTime(trainingTime);
        setDate(date);
    }, [usersByTraining]);

    return (
        <Modal 
            className='modal-content' 
            isOpen={usersByTrainingModalIsOpen} 
            onRequestClose={closeUsersByTrainingModal}
            contentLabel="Show Users By Training Modal" 
            ariaHideApp={false}>
            <h2>
                Attending {getTrainingTypeLabel(trainingType)} on {date} at {time}
            </h2>
            <ul>
                {localUsers.map((user, index) => (
                    <li key={index}>{user.userName}</li>
                ))}
            </ul>
            <button onClick={closeUsersByTrainingModal}>Close</button>
        </Modal>
    );
};

export default UsersByTrainingModal;