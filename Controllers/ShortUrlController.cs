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

	public ShortUrlController(ShortUrlDbContext context)
	{
		_context = context;
	}

	[HttpPost("new")]
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
		using (HttpClient client = new())
		{
			HttpResponseMessage response = await client.GetAsync(url);
			if (!response.IsSuccessStatusCode)
				return BadRequest("The web site could not be found.");
		}

		ShortUrlEntry newEntry = new()
		{
			OriginalUrl = url
		};
		_context.ShortUrls.Add(newEntry);

		try
		{
			_context.SaveChanges();
		}
		catch (DbUpdateException e)
		{
			Console.WriteLine($"Error when attempting to save {url} to database: {e.Message}");
			return BadRequest("There was an error when attempting to save your URL to the database. Please try again.");
		}

		return Ok(new
		{
			original_url = url,
			short_url = newEntry.Id
		});
	}

	[HttpGet("go/{id}")]
	public ActionResult Get(int id)
	{
		var result = _context.ShortUrls.Find(id);
		if (result == null)
			return NotFound($"The short URL you provided, {id}, does not exist.");

		result.TimesAccessed++;
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Error when attempting to update times accessed for {result.OriginalUrl}: {e.Message}");
        }

        return Redirect(result.OriginalUrl);
	}
}
