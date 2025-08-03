import { useState, useEffect } from "react";
import { TrainingTypes, getTrainingTypeLabel } from '../../Utils/TrainingTypes';
import { AllHours, AllMinutes } from '../../Utils/AllTimes';
import { toast } from "react-toastify";

const AddTrainings = ({availableHours, today, isSelectedDateToday, triggerRefresh }) => {

    const [trainingType, setTrainingType] = useState('');
    const [trainingDate, setTrainingDate] = useState('');
    const [trainingHour, setTrainingHour] = useState('');
    const [trainingMinute, setTrainingMinute] = useState('');
    const [trainings, setTrainings] = useState([]);

    const handleMultipleSubmit = async (e) => {
        e.preventDefault();

        if (trainingType == null || trainingHour == null || trainingMinute == null || trainings.length === 0) {
        setResponseMessage("Training type, time and at least one date must be selected.");
        setResponseMessageModalIsOpen(true);
        return;
        }

        const trainingList = trainings.map((d) => ({
        Date: d.Date,
        TrainingType: d.TrainingType
        }));

        try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/Training/AddTrainings`, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify(trainingList),
        });

        if (response.ok) {
            toast.success("Trainings were added successfully.");
            setTrainings([]);
            triggerRefresh();
        } else {
            const data = await response.json();
            toast.error(data.message || "Trainings could not be added.");
        }
        }
        catch {
        toast.error("An error occured during adding trainings.");
        } 
    };

    // const isTrainingSlotOccupied = (existingTraining) => {
    //     if (existingTraining.Hour > trainingHour)
    //     {
    //         return (existingTraining.)
    //     }
    // }

    const addTrainingToList = () => {

        // TODO: filtering needed for frontend to not allow users to add trainings to list with conflicting times
        //trainings.filter(training => training.Date == trainingDate).filter(training => Math.abs(training.Hour - trainingHour) <= 1).any(training => )

        setTrainings(([
            ...trainings,
            { Date: `${trainingDate}T${trainingHour.padStart(2, '0')}:${trainingMinute.padStart(2, '0')}:00`,
                TrainingType: trainingType },
        ]));
    }

    return (
        <div>
            <h3>ADD MULTIPLE TRAININGS</h3>
            <form className="trainings-creation-form" onSubmit={handleMultipleSubmit}>
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
                <label>Select Date:</label>
                <div className="add-multiple-date-group">
                <input
                    type="date"
                    min={today.toISOString().split('T')[0]}
                    value={trainingDate}
                    onChange={(e) => setTrainingDate(e.target.value)}
                />
                </div>
            </div>

            <div className="form-group">
                <label>Training Hour:</label>
                <select
                id="hours"
                onChange={(e) => setTrainingHour(e.target.value)}
                value={trainingHour || ''}
                >
                <option value="">-- Select Hour --</option>
                {isSelectedDateToday
                    ? availableHours.map((h) => (
                        <option key={h} value={h}>
                        {h}
                        </option>
                    ))
                    : AllHours.map((h) => (
                        <option key={h} value={h}>
                        {h}
                        </option>
                    ))}
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
                {AllMinutes.map((m) => (
                    <option key={m} value={m}>
                    {m}
                    </option>
                ))}
                </select>
            </div>

            <div className="form-group">
                <button
                type="button"
                onClick={() => addTrainingToList()}
                >
                Add Training To List
                </button>
            </div>

            {trainings.length > 0 && (
                <div className="form-group">
                <label>Selected Training Times:</label>
                <ul className="selected-dates-list">
                    {trainings.map((t, index) => (
                    <li key={index}>
                        {t.Date}, {t.TrainingType}
                        <button
                        type="button"
                        onClick={() =>
                            setTrainings(
                            trainings.filter((_, i) => i !== index)
                            )
                        }
                        >
                        Remove
                        </button>
                    </li>
                    ))}
                </ul>
                </div>
            )}

            <button type="submit">Submit Trainings</button>
            </form>
        </div>
    );
};

export default AddTrainings;