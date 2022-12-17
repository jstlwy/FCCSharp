using Microsoft.EntityFrameworkCore;
using FCCSharp.Models;

namespace FCCSharp.Data;

public class ExerciseDbContext : DbContext
{
	public ExerciseDbContext (DbContextOptions<ExerciseDbContext> options)
 		: base(options)
 	{
 	}
	
	public DbSet<ExerciseUser> Users { get; set; } = null!;

	public DbSet<Exercise> Exercises { get; set; } = null!;
}
