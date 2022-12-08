using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class Exercise
{
	public int Id { get; set; }

	// Foreign key
	public int ExerciseUserId { get; set; }

	public string Description { get; set; }

	public int Duration { get; set; }

	public Date Date { get; set; }
}
