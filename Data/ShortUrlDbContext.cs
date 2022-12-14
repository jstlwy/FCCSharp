using Microsoft.EntityFrameworkCore;
using FCCSharp.Models;

namespace FCCSharp.Data;

public class ShortUrlDbContext : DbContext
{
	public ShortUrlDbContext (DbContextOptions<ShortUrlDbContext> options)
 		: base(options)
 	{
 	}
	
	public DbSet<ShortUrlEntry> ShortUrls { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ShortUrlEntry>()
			.Property(s => s.TimesAccessed)
			.HasDefaultValue(0);
	}
}
