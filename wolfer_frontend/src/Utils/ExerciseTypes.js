export const ExerciseTypes = {
    Squat: "Squat",
    Deadlift: "Deadlift",
    BenchPress: "Bench Press",
    PullUp: "PullUp",
    Clean: "Clean",
    Snatch: "Snatch",
    ShoulderPress: "Shoulder Press"
}

export const getExerciseTypeLabel = (key) => ExerciseTypes[key] || key;