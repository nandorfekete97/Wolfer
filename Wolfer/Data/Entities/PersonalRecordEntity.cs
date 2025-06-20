namespace Wolfer.Data.Entities;

public class PersonalRecordEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public ExerciseType ExerciseType { get; set; }
    public int Weight { get; set; }
}