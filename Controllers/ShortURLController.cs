using Microsoft.AspNetCore.Mvc;

namespace FCCSharp.Controllers;

private class URLReceipt
{
	public string original_url { set; get; }
	public string short_url { set; get; }
}

[ApiController]
[Route("[controller]")]
public class ShortURLController : ControllerBase
{
	[HttpPost]
	public URLReceipt Create()
	{
		URLReceipt receipt = new URLReceipt();
		receipt.original_url = ;
		receipt.short_url = ;
		return receipt;
	}

	[HttpGet("{id}")]
	public void Get(int id)
	{
		
	}
}
