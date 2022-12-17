using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ShortUrlEntry
{
    public int Id { get; set; }

	public string OriginalUrl { get; set; } = null!;

	public int TimesAccessed { get; set; }
}
