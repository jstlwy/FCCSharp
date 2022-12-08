using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FCCSharp.Models;

public class ShortUrlEntry
{
	public ShortUrlEntry()
	{
		TimesAccessed = 0;
	}

	public int Id { get; set; }

	[JsonPropertyName("original_url")]
	public string OriginalUrl { get; set; }

	[JsonPropertyName("short_url")]
	public string ShortUrl { get; set; }

	public int TimesAccessed { get; set; }
}
