namespace Wolfer.Data.DTOs;

public class PersonalRecordDTO
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public ExerciseType ExerciseType { get; set; }
    public int Weight { get; set; }
}