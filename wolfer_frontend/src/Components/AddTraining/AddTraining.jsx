import { useState, useEffect } from "react";
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';
import { AllHours, AllMinutes } from '../../Utils/AllTimes';
import { toast } from "react-toastify";

const AddTraining = ({availableHours, today, isSelectedDateToday, trainingDate, setTrainingDate, triggerRefresh}) => {

    const [trainingType, setTrainingType] = useState('');
    const [trainingHour, setTrainingHour] = useState('');
    const [trainingMinute, setTrainingMinute] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (trainingType == null || trainingDate == null || trainingHour == null || trainingMinute == null) {
        toast.error("Training type, date and time cannot be empty.");
        return;
        }

        const trainingTime = trainingHour + ":" + trainingMinute;
        const localDateTimeString = `${trainingDate}T${trainingTime}`;
        const token = localStorage.getItem("token");

        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/AddTraining`, {
                method: 'POST',
                headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify({
                Date: localDateTimeString,
                TrainingType: trainingType
                }),
            });

            if (response.ok) {
                toast.success("Training was added successfully.");
                triggerRefresh();
            } else {
                const data = await response.json();
                toast.error(data.message || 'Training could not be added.');
            }
        } catch (error) {
            toast.error('An error occurred during adding training.');
        }
    };

    return (
        <div>
            <h3>ADD NEW TRAINING</h3>
            <form className="training-creation-form" onSubmit={handleSubmit}>
            <div className="form-group">
                <label>Training Type:</label>
                <select
                id="choices"
                onChange={(e) => setTrainingType(e.target.value)}
                value={trainingType || ''}
                >
                <option value="">-- Select Training Type --</option>
                {Object.entries(TrainingTypes).map(([value, label]) => (
                    <option key={value} value={value}>{label}</option>
                ))}
                </select>
            </div>
    
            <div className="form-group">
                <label>Training Date:</label>
                <input
                type="date"
                min={today.toISOString().split('T')[0]}
                className="training-date-input"
                value={trainingDate}
                onChange={(e) => setTrainingDate(e.target.value)}
                />
            </div>
    
            <div className="form-group">
                <label>Training Hour:</label>
                <select
                id="hours"
                onChange={(e) => setTrainingHour(e.target.value)}
                value={trainingHour || ''}
                >
                <option value="">-- Select Hour --</option>
                {isSelectedDateToday ?
                    availableHours.map((h) => (
                    <option key={h} value={h}>{h}</option>
                    )) :
                    AllHours.map((h) => (
                    <option key={h} value={h}>{h}</option>
                    ))
                }
                </select>
            </div>
    
            <div className="form-group">
                <label>Training Minute:</label>
                <select
                id="minutes"
                onChange={(e) => setTrainingMinute(e.target.value)}
                value={trainingMinute || ''}
                >
                <option value="">-- Select Minute --</option>
                { }
                {AllMinutes.map((m) => (
                    <option key={m} value={m}>{m}</option>
                ))}
                </select>
            </div>
    
            <button type="submit">Submit Training</button>
            </form>
        </div>
    );
};

export default AddTraining;