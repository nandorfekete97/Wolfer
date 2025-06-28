export const TrainingTypes = {
  FunctionalBodyBuilding: "Functional Body-Building",
  WeightLifting: "Weight Lifting",
  CrossFit: "CrossFit",
  LegDay: "Leg Day",
};

export const getTrainingTypeLabel = (key) => TrainingTypes[key] || key;