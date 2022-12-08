using Microsoft.EntityFrameworkCore;
using FCCSharp.Models;

namespace FCCSharp.Data;

public class ShortUrlDbContext : DbContext
{
	public DbSet<ShortUrlEntry> ShortUrls { get; set; } = null!;
}
