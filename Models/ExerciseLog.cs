//using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ExerciseLog
{
    public string Description { get; set; } = null!;

	public uint Duration { get; set; }

	public DateOnly Date { get; set; }
}
