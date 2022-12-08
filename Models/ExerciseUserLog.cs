using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ExerciseUserLog
{
	public string Username { get; set; }

	[JsonPropertyName("_id")]
	public string UserId { get; set; }

	public int Count { get; set; }

	public List<Exercise> Log { get; set; }
}
