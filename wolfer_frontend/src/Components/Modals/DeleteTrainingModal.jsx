import Modal from 'react-modal';
import './DeleteModal.css';
import { getTrainingTypeLabel } from '../../Utils/TrainingTypes';

const DeleteTrainingModal = ({training, formattedDate, deleteModalIsOpen, closeDeleteModal, handleDelete}) => {

  return (
        <div>
          <Modal
            className="modal-content"
            isOpen={deleteModalIsOpen}
            contentLabel="Training Deletion"
            ariaHideApp={false}
          >
            <h2>DELETE {getTrainingTypeLabel(training.trainingType)} for date: <br></br> {formattedDate} </h2>
            <button className="modal-btn btn btn-danger" onClick={(e) => handleDelete(e)}>DELETE</button>
            <button className="modal-btn btn btn-secondary" onClick={() => closeDeleteModal()}>CANCEL</button>
          </Modal>
        </div>
      );
}

export default DeleteTrainingModal;