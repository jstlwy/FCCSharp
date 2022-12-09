using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FCCSharp.Models;
using FCCSharp.Data;

namespace FCCSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class ShortUrlController : ControllerBase
{
	private readonly ShortUrlDbContext _context;

	public ShortUrlController(FCCSharp.Data.ShortUrlDbContext context)
	{
		_context = context;
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromForm] string url)
	{
		if (String.IsNullOrEmpty(url))
			return BadRequest("No URL was provided.");

		// Check if the format of the URL is valid
		if (!System.Uri.IsWellFormedUriString(url, System.UriKind.Absolute))
			return BadRequest("Invalid URL format.");

		// Check if the web site exists
		// Based on the following:
		// https://www.dotnetperls.com/httpclient
		using (HttpClient client = new HttpClient())
		{
			HttpResponseMessage response = await client.GetAsync(url);
			if (!response.IsSuccessStatusCode)
				return BadRequest("The web site could not be found.");
		}

		ShortUrlEntry newEntry = new ShortUrlEntry()
		{
			OriginalUrl = url
		};
		_context.ShortUrls.Add(newEntry);

		try
		{
			_context.SaveChanges();
		}
		catch(DbUpdateException e)
		{
			Console.WriteLine($"Error when attempting to save to ShortUrl database: {e.Message}");
			return BadRequest("Error when attempting to save to database.");
		}

		return Ok(newEntry);
	}

	[HttpGet("{id}")]
	public ActionResult Get(string id)
	{
		string? url = (
			from entry in _context.ShortUrls
			where entry.ShortUrl == id
			select entry.OriginalUrl
		).FirstOrDefault();

		if (String.IsNullOrEmpty(url))
			return NotFound();
		
		return Redirect(url);
	}
}
