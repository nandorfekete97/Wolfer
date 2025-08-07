import { useState, useEffect } from 'react';
import './TrainingHistory.css';
import { getTrainingTypeLabel } from '../../Utils/TrainingTypes';

const TrainingHistory = () => {

const [pastTrainings, setPastTrainings] = useState([]);
const [filteredTrainings, setFilteredTrainings] = useState([]);
const [dataIsLoaded, setDataIsLoaded] = useState(false);
const [trainingTypeStats, setTrainingTypeStats] = useState([]);
const [selectedYear, setSelectedYear] = useState("all");
const [selectedMonth, setSelectedMonth] = useState("all");

const getpastTrainings = async () => {
    const userId = localStorage.getItem("userId");
    const token = localStorage.getItem("token");

    const res = await fetch(`${import.meta.env.VITE_API_URL}/UserTraining/GetPastTrainingsForUser/${userId}`,
        {
            method: "GET",
            headers: {
            "Content-Type": "application/json",
              "Authorization": `Bearer ${token}`
            }
        }
    );
    
    const data = await res.json();
    const trainings = data.trainingEntities;
    setPastTrainings(trainings); 
    setFilteredTrainings(trainings);

    const counts = {};
    data.trainingEntities.forEach(training => {
        counts[training.trainingType] = (counts[training.trainingType] || 0) + 1;
    });
    const total = data.trainingEntities.length;

    const percentages = Object.entries(counts).map(([type, count]) => ({
        trainingType: type,
        percentage: ((count / total) * 100).toFixed(1),
    })).sort((a, b) => b.percentage - a.percentage);

    setTrainingTypeStats(percentages);
    setDataIsLoaded(true);
}

useEffect(() => {
    getpastTrainings();
}, []);

useEffect(() => {
    const filtered = pastTrainings.filter(training => {
        const trainingDate = new Date(training.date);
        const yearMatch = selectedYear === "all" || trainingDate.getFullYear().toString() === selectedYear;
        const monthMatch = selectedMonth === "all" || (trainingDate.getMonth() + 1).toString().padStart(2, '0') === selectedMonth;
        return yearMatch && monthMatch;
    });
    setFilteredTrainings(filtered);
}, [selectedYear, selectedMonth, pastTrainings]);

const getUniqueYears = () => {
    const years = new Set(pastTrainings.map(training => training.date.split('-')[0]));
    return Array.from(years).sort();
};

    return (
        <div className="training-history-container">
            <h1 className="training-history-header">TRAINING HISTORY</h1>
            {dataIsLoaded ? (
            <>
                <div className="training-summary">
                <h3>
                    Number of all trainings:
                    <h1 className="training-counter">{pastTrainings.length}</h1>
                </h3>
                </div>

                <div className="training-filters">
                <div>
                    <label htmlFor="year-select">Year:</label>
                    <select
                    id="year-select"
                    onChange={(e) => setSelectedYear(e.target.value)}
                    value={selectedYear}
                    >
                    <option value="all">All</option>
                    {getUniqueYears().map((year) => (
                        <option key={year} value={year}>
                        {year}
                        </option>
                    ))}
                    </select>
                </div>

                <div>
                    <label htmlFor="month-select">Month:</label>
                    <select
                    id="month-select"
                    onChange={(e) => setSelectedMonth(e.target.value)}
                    value={selectedMonth}
                    >
                    <option value="all">All</option>
                    {Array.from({ length: 12 }, (_, i) => {
                        const monthNum = String(i + 1).padStart(2, "0");
                        const monthName = new Date(0, i).toLocaleString("default", {
                        month: "long",
                        });
                        return (
                        <option key={monthNum} value={monthNum}>
                            {monthName}
                        </option>
                        );
                    })}
                    </select>
                </div>
                </div>

                <div className="past-trainings">
                <ul className="training-list">
                    {filteredTrainings.map((training) => (
                    <li key={training.id}>
                        {getTrainingTypeLabel(training.trainingType)} | {training.date.split("T")[0]}
                    </li>
                    ))}
                </ul>
                </div>

                <div className="training-stats">
                <h3>Your favourite trainings:</h3>
                <ul>
                    {trainingTypeStats.map((stat) => (
                    <li key={getTrainingTypeLabel(stat.trainingType)}>
                        {getTrainingTypeLabel(stat.trainingType)}: {getTrainingTypeLabel(stat.percentage)}%
                    </li>
                    ))}
                </ul>
                </div>
            </>
            ) : (
            "Loading"
            )}
        </div>
        );
}

export default TrainingHistory;
