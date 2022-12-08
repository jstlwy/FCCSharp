using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ExerciseUser
{
	public int Id { get; set; }

	public string Username { get; set; }

	public string UUId { get; set; }
}
