using Microsoft.AspNetCore.Mvc;

namespace FCCSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class ExerciseController : ControllerBase
{
	[HttpPost("/users")]
	public URLReceipt CreateUser()
	{

	}

	[HttpGet("/users")]
	public GetAll()
	{

	}

	[HttpPost("/users/{id}/exercises")]
	public AddExercise(int id)
	{

	}

	[HttpGet("/users/{id}/logs")]
	public GetLogs(int id)
	{
		
	}
}
