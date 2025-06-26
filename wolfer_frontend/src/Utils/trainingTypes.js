export const trainingTypeOptions = {
  FunctionalBodyBuilding: "Functional Body-Building",
  WeightLifting: "Weight Lifting",
  CrossFit: "CrossFit",
  LegDay: "Leg Day",
};

export const getTrainingTypeLabel = (key) => trainingTypeOptions[key] || key;