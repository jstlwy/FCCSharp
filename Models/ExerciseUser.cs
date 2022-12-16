using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ExerciseUser
{
	[JsonPropertyName("_id")]
	public int Id { get; set; }

	[StringLength(254, MinimumLength = 2)]
	[RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9_]*$")]
	public string Username { get; set; } = null!;
}
