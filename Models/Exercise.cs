using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCCSharp.Models;

public class Exercise
{
	// Primary key
	public int Id { get; set; }

	// Foreign key
	public int UserId { get; set; }

	public string Description { get; set; } = null!;

	public uint Duration { get; set; }

	public DateOnly Date { get; set; }
}
