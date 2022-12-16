using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ExerciseUserLog
{
	public string Username { get; set; } = null!;

	[JsonPropertyName("_id")]
	public int UserId { get; set; }

	public int Count { get; set; }

	public List<ExerciseLog>? Exercises { get; set; }
}
