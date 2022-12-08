using Microsoft.AspNetCore.Mvc;
using FCCSharp.Models;

namespace FCCSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class ShortUrlController : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> Create([FromForm] string url)
	{
		// Check if the format of the URL is valid
		if (!System.Uri.IsWellFormedUriString(url, System.UriKind.Absolute))
		{
			return BadRequest("Invalid URL format.");
		}

		// Check if the web site exists
		// Based on the following:
		// https://www.dotnetperls.com/httpclient
		using (HttpClient client = new HttpClient())
		{
			HttpResponseMessage response = await client.GetAsync(url);
			if (!response.IsSuccessStatusCode)
			{
				return BadRequest("The web site could not be found.");
			}
		}

		ShortUrlEntry newEntry = new ShortUrlEntry()
		{
			OriginalUrl = url
		};
		context..Add(newEntry);
		return Ok(newEntry);
	}

	[HttpGet("{id}")]
	public ActionResult Get(string id)
	{
		if (true)
		{
			return Redirect(originalUrl);
		}
		else
		{
			return NotFound();
		}
	}
}
